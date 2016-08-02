#!/usr/bin/env python
# Copyright (c) 2012 The Chromium Authors. All rights reserved.
# Use of this source code is governed by a BSD-style license that can be
# found in the LICENSE file.

""" Generator for C-Sharp style binding definitions """

import glob
import os
import sys
import string

from idl_log import ErrOut, InfoOut, WarnOut
from idl_node import IDLNode
from idl_ast import IDLAst
from idl_option import GetOption, Option, ParseOptions
from idl_parser import ParseFiles

Option('csgen_debug', 'Debug generate.')
Option('cs-enum_prefix', 'Suppress the enum prefix when generating.')

class CGenError(Exception):
  def __init__(self, msg):
    self.value = msg

  def __str__(self):
    return repr(self.value)


def CommentLines(lines, tabs=0):
  # Generate a C style comment block by prepending the block with '<tab>/*'
  # and adding a '<tab> *' per line.
  tab = '  ' * tabs

  out = '%s/*' % tab + ('\n%s *' % tab).join(lines)

  # Add a terminating ' */' unless the last line is blank which would mean it
  # already has ' *'
  if not lines[-1]:
    out += '/\n'
  else:
    out += ' */\n'
  return out

def Comment(node, prefix=None, tabs=0):
  # Generate a comment block from the provided Comment node.
  comment = node.GetName()
  lines = comment.split('\n')

  # If an option prefix is provided, then prepend that to the comment
  # for this node.
  if prefix:
    prefix_lines = prefix.split('\n')
    # If both the prefix and comment start with a blank line ('*') remove
    # the extra one.
    if prefix_lines[0] == '*' and lines[0] == '*':
      lines = prefix_lines + lines[1:]
    else:
      lines = prefix_lines + lines;
  return CommentLines(lines, tabs)

def GetNodeComments(node, tabs=0):
  # Generate a comment block joining all comment nodes which are children of
  # the provided node.
  comment_txt = ''
  for doc in node.GetListOf('Comment'):
    comment_txt += Comment(doc, tabs=tabs)
  return comment_txt


class CSGen(object):
  # TypeMap
  #
  # TypeMap modifies how an object is stored or passed, for example pointers
  # are passed as 'const' if they are 'in' parameters, and structures are
  # preceeded by the keyword 'struct' as well as using a pointer.
  #
  TypeMap = {
    'Array': {
      'in': '%s[]',
      'inout': '%s[]',
      'out': '%s[]',
      'store': '%s',
      'return': '%s',
      'ref': '%s*'
    },
    'Callspec': {
      'in': '%s',
      'inout': '%s',
      'out': '%s',
      'store': '%s',
      'return': '%s'
    },
    'Enum': {
      'in': '%s',
      'inout': '%s',
      'out': '%s',
      'store': '%s',
      'return': '%s'
    },
    'Interface': {
      'in': 'const %s*',
      'inout': '%s*',
      'out': '%s**',
      'return': '%s*',
      'store': '%s*'
    },
    'Struct': {
      'in': '%s',
      'inout': '%s',
      'out': '%s',
      'return': ' %s',
      'store': '%s',
      'ref': '%s*'
    },
    'blob_t': {
      'in': 'const %s',
      'inout': '%s',
      'out': '%s',
      'return': '%s',
      'store': '%s'
    },
    'mem_t': {
      'in': 'const %s',
      'inout': '%s',
      'out': '%s',
      'return': '%s',
      'store': '%s'
    },
    'mem_ptr_t': {
      'in': 'const %s',
      'inout': '%s',
      'out': '%s',
      'return': '%s',
      'store': '%s'
    },
    'str_t': {
      'in': 'const %s',
      'inout': 'ref %s',
      'out': '%s',
      'return': 'string%.0s%s',
      'store': '%s'
    },
    'cstr_t': {
      'in': '%s',
      'inout': '%s*',
      'out': '%s*',
      'return': '%s',
      'store': '%s'
    },
    'TypeValue': {
      'in': '%s',
      'constptr_in': '%s',  # So we can use const* for PP_Var sometimes.
      'inout': '%s',
      'out': '%s',
      'return': '%s',
      'store': '%s'
    },
  }


  #
  # RemapName
  #
  # A diction array of PPAPI types that are converted to language specific
  # types before being returned by by the C generator
  #
  RemapName = {
  'blob_t': 'IntPtr',
  'float_t': 'float',
  'double_t': 'double',
  'int32_t': 'int',
  'uint32_t': 'uint',
  'int64_t': 'long',
  'uint64_t': 'ulong',
  'int16_t': 'short',
  'uint16_t': 'ushort',
  'handle_t': 'int',
  'int8_t': 'sbyte',
  'uint8_t': 'byte',
  'mem_t': 'IntPtr',
  'mem_ptr_t': 'mem_ptr_IntPtr',
  'str_t': 'str_t',
  'cstr_t': 'string',
  'interface_t' : 'const void*'
  }

  #
  # BlackListed
  #
  # A dictionary of methods that we should not generate code for.  The will be manually coded.
  #
  BlackListed = [
  'PPB_Var_VarToUtf8',
  ]

  #
  # RemapSizeToType
  #
  # A dictionary of C# sizes are converted to language specific
  # types before being returned by by the C generator
  #
  RemapSizeToType = {
  #'4': 'byte'
  }

  #
  # RemapMode
  #
  # A diction array of PInvoke marshalling types that are converted to language specific
  # types before being returned by the CS generator
  #
  RemapMode = {
  'in': '',
  'out': 'out',
  'inout': '',
  'return': '',
  'constptr_in': '',
  }

  #
  # RemapParameter
  #
  # A diction array of parameter types that need to be remapped
  #
  RemapParameter = {
  'PP_Time': 'double',  # PP_TimeXXXX is better mapped to a double
  'PP_TimeTicks': 'double',
  'PP_TimeDelta': 'double'
  }

  #
  # RemapArgument
  #
  # A dictionary of argument names that need to be remapped
  #
  RemapArgument = {
  'event': 'eventArg'
  }

  #
  # RemapEnum
  #
  # A dictionary of enum names that can not be mapped automatically 
  #
  RemapEnum = {
  '16Bits': '_16Bits',
  }

  # Tell how to handle pointers to GL types.
  for gltype in ['GLbitfield', 'GLboolean', 'GLbyte', 'GLclampf',
                 'GLclampx', 'GLenum', 'GLfixed', 'GLfloat', 'GLint',
                 'GLintptr', 'GLshort', 'GLsizei', 'GLsizeiptr',
                 'GLubyte', 'GLuint', 'GLushort']:
    ptrtype = gltype + '_ptr_t'
    TypeMap[ptrtype] = {
      'in': 'const %s',
      'inout': '%s',
      'out': '%s',
      'return': 'const %s',
      'store': '%s'
    }
    RemapName[ptrtype] = '%s*' % gltype

  def __init__(self):
    self.dbg_depth = 0

  #
  # Debug Logging functions
  #
  def Log(self, txt):
    if not GetOption('csgen_debug'): return
    tabs = '  ' * self.dbg_depth
    print '%s%s' % (tabs, txt)

  def LogEnter(self, txt):
    if txt: self.Log(txt)
    self.dbg_depth += 1

  def LogExit(self, txt):
    self.dbg_depth -= 1
    if txt: self.Log(txt)

  #
  # Interface strings
  #
  def GetInterfaceString(self, node, version = None):
    # If an interface name is specified, use that
    name = node.GetProperty('iname')
    if not name:
      # Otherwise, the interface name is the object's name
      # With '_Dev' replaced by '(Dev)' if it's a Dev interface.
      name = node.GetName()
      if name.endswith('_Dev'):
        name = '%s(Dev)' % name[:-4]
    if version is None:
      return name
    return "%s;%s" % (name, version)


  #
  # Return the array specification of the object.
  #
  def GetArraySpec(self, node):
    assert(node.cls == 'Array')
    fixed = node.GetProperty('FIXED')
    if fixed:
      return '[%s]' % fixed
    else:
      return '[]'

  #
  # GetTypeName
  #
  # For any valid 'typed' object such as Member or Typedef
  # the typenode object contains the typename
  #
  # For a given node return the type name by passing mode.
  #
  def GetTypeName(self, node, release, prefix=''):
    self.LogEnter('GetTypeName of %s rel=%s' % (node, release))

    # For Members, Params, and Typedefs get the type it refers to otherwise
    # the node in question is it's own type (struct, union etc...)
    if node.IsA('Member', 'Param', 'Typedef'):
      typeref = node.GetType(release)
    else:
      typeref = node

    if typeref is None:
      node.Error('No type at release %s.' % release)
      raise CGenError('No type for %s' % node)

    # If the type is a (BuiltIn) Type then return it's name
    # remapping as needed
    if typeref.IsA('Type'):
      name = CSGen.RemapName.get(typeref.GetName(), None)
      if name is None: name = typeref.GetName()
      name = '%s%s' % (prefix, name)

    # For Interfaces, use the name + version
    elif typeref.IsA('Interface'):
      rel = typeref.first_release[release]
      name = 'struct %s%s' % (prefix, self.GetStructName(typeref, rel, True))

    # For structures, preceed with 'struct' or 'union' as appropriate
    elif typeref.IsA('Struct'):
      if typeref.GetProperty('union'):
        name = '%s%s' % (prefix, typeref.GetName())
      else:
        name = '%s%s' % (prefix, typeref.GetName())

    # If it's an enum, or typedef then return the Enum's name
    elif typeref.IsA('Enum', 'Typedef'):
      if not typeref.LastRelease(release):
        first = node.first_release[release]
        ver = '_' + node.GetVersion(first).replace('.','_')
      else:
        ver = ''
      name = '%s%s%s' % (prefix, typeref.GetName(), ver)

    else:
      raise RuntimeError('Getting name of non-type %s.' % node)
    self.LogExit('GetTypeName %s is %s' % (node, name))
    return name


  #
  # GetRootType
  #
  # For a given node return basic type of that object.  This is
  # either a 'Type', 'Callspec', or 'Array'
  #
  def GetRootTypeMode(self, node, release, mode):
    self.LogEnter('GetRootType of %s' % node)
    # If it has an array spec, then treat it as an array regardless of type
    if node.GetOneOf('Array'):
      rootType = 'Array'
    # Or if it has a callspec, treat it as a function
    elif node.GetOneOf('Callspec'):
      rootType, mode = self.GetRootTypeMode(node.GetType(release), release,
                                            'return')

    # If it's a plain typedef, try that object's root type
    elif node.IsA('Member', 'Param', 'Typedef'):
      rootType, mode = self.GetRootTypeMode(node.GetType(release),
                                            release, mode)

    # If it's an Enum, then it's normal passing rules
    elif node.IsA('Enum'):
      rootType = node.cls

    # If it's an Interface or Struct, we may be passing by value
    elif node.IsA('Interface', 'Struct'):
      if mode == 'return':
        if node.GetProperty('returnByValue'):
          rootType = 'TypeValue'
        else:
          rootType = node.cls
      else:
        if node.GetProperty('passByValue'):
          rootType = 'TypeValue'
        else:
          rootType = node.cls

    # If it's an Basic Type, check if it's a special type
    elif node.IsA('Type'):
      if node.GetName() in CSGen.TypeMap:
        rootType = node.GetName()
      else:
        rootType = 'TypeValue'
    else:
      raise RuntimeError('Getting root type of non-type %s.' % node)
    self.LogExit('RootType is "%s"' % rootType)
    return rootType, mode


  def GetTypeByMode(self, node, release, mode):
    self.LogEnter('GetTypeByMode of %s mode=%s release=%s' %
                  (node, mode, release))
    name = self.GetTypeName(node, release)
    ntype, mode = self.GetRootTypeMode(node, release, mode)
    out = CSGen.TypeMap[ntype][mode] % name
    self.LogExit('GetTypeByMode %s = %s' % (node, out))
    return out


  # Get the passing mode of the object (in, out, inout).
  def GetParamMode(self, node):
    self.Log('GetParamMode for %s' % node)
    if node.GetProperty('in'): return 'in'
    if node.GetProperty('out'): return 'out'
    if node.GetProperty('inout'): return 'inout'
    if node.GetProperty('constptr_in'): return 'constptr_in'
    return 'return'

  #
  # GetComponents
  #
  # Returns the signature components of an object as a tuple of
  # (rtype, name, arrays, callspec) where:
  #   rtype - The store or return type of the object.
  #   name - The name of the object.
  #   arrays - A list of array dimensions as [] or [<fixed_num>].
  #   args -  None if not a function, otherwise a list of parameters.
  #
  def GetComponents(self, node, release, mode, compose_mode=''):
    self.LogEnter('GetComponents mode %s for %s %s' % (mode, node, release))

    # Generate passing type by modifying root type
    rtype = self.GetTypeByMode(node, release, mode)

    # If this is an array output, change it from type* foo[] to type** foo.
    # type* foo[] means an array of pointers to type, which is confusing.
    arrayspec = [self.GetArraySpec(array) for array in node.GetListOf('Array')]
    
    # If this is a char array then we want to remap it to a string
    if len(arrayspec) > 0 and rtype == 'char':
        rtype = 'string'
        arrayspec = []
    if mode == 'out' and len(arrayspec) == 1 and arrayspec[0] == '[]':
      #rtype += '*'
      del arrayspec[0]

    if node.IsA('Enum', 'Interface', 'Struct'):
      rname = node.GetName()
    else:
      rname = node.GetType(release).GetName()

    if rname in CSGen.RemapName:
      rname = CSGen.RemapName[rname]
    if '%' in rtype:
      rtype = rtype % rname
    if rtype in CSGen.RemapParameter:
        rtype = CSGen.RemapParameter[rtype]

    rtype = self.FormatName(rtype)
    name = self.FormatName(node.GetName())

    callnode = node.GetOneOf('Callspec')
    if callnode:
      callspec = []
      for param in callnode.GetListOf('Param'):
        if not param.IsRelease(release):
          continue
        mode = self.GetParamMode(param)
        ptype, pname, parray, pspec = self.GetComponents(param, release, mode)
        if ptype in CSGen.RemapParameter:
            ptype = CSGen.RemapParameter[ptype]

        if compose_mode:
          ptype = '%s %s' % (CSGen.RemapMode[mode], ptype)
          callspec.append((ptype, pname, parray, pspec))
        else:
          callspec.append((ptype, pname, parray, pspec))
    else:
      callspec = None

    self.LogExit('GetComponents: %s, %s, %s, %s' %
                 (rtype, name, arrayspec, callspec))
    return (rtype, name, arrayspec, callspec)

  def StripUnderScores(self, strip) :
    return "".join(strip.split('_')) 

  def FormatName(self, name) :
    if name.startswith('PP_') or name.startswith('PB_') or name.startswith('PPP_') or name.startswith('PPB'):
        name = self.StripUnderScores(name)

    return name

  def FormatArgs(self, c_operator, args_spec):
    args = []
    for rtype, name, array_dims, more_args in args_spec:
        if name in CSGen.RemapArgument:
            name = CSGen.RemapArgument[name]

        # special for unsafe - The need to be converted to IntPtr
        if rtype.startswith('out IntPtr') \
            or rtype.startswith('out str_t') \
            or rtype.startswith(' const IntPtr') \
            or rtype.startswith(' const str_t') \
            or rtype.startswith(' ref str_t'):  
                args.append('(IntPtr) %s_' % name)

        elif rtype.startswith('out ') or rtype.startswith('ref '):
            args.append('%s%s' % (rtype[0:4], name))
        elif rtype.startswith(' ref '): # TODO: look into why there is a space
            args.append('%s%s' % (rtype[0:5], name))
        else:
            args.append('%s' % name)
    return ', '.join(args)

  def Compose(self, rtype, name, arrayspec, callspec, prefix, func_as_ptr,
              include_name, unsized_as_ptr, compose_mode=''):

    self.LogEnter('Compose: %s %s' % (rtype, name))
    arrayspec = ''.join(arrayspec)

    
    # Switch unsized array to a ptr. NOTE: Only last element can be unsized.
    if unsized_as_ptr and arrayspec[-2:] == '[]':
      prefix +=  '*'
      arrayspec=arrayspec[:-2]

    if not include_name:
      name = prefix + arrayspec
    else:
      if not compose_mode:
        if arrayspec:
            name = prefix + name
        else:
            name = prefix + name + arrayspec
    if name in CSGen.RemapArgument:
        name = CSGen.RemapArgument[name]
    if callspec is None:
        if rtype in CSGen.RemapParameter:
            out = '%s %s' % (CSGen.RemapParameter[rtype], name)
        else:
            if compose_mode:
                out = '%s %s' % (rtype, name) 
            else:
                if arrayspec:
                    out = 'public unsafe fixed %s %s%s' % (rtype, name, arrayspec)
                else:
                    out = 'public %s %s' % (rtype, name)
    else:
      params = []
      for ptype, pname, parray, pspec in callspec:
        params.append(self.Compose(ptype, pname, parray, pspec, '', True,
                                   include_name=True,
                                   unsized_as_ptr=unsized_as_ptr,
                                   compose_mode=compose_mode))
      
      if func_as_ptr:
        name = '%s' % name
      if compose_mode.startswith('EntryPoint_'): 
        out = '[DllImport("PepperPlugin", EntryPoint = "%s_%s")]\n' % (compose_mode[len('EntryPoint_'):], name)
        # We now check for unsafe parameters
        entryParams = []
        for eparm in params:
            if eparm.startswith('out IntPtr'):
                eparm = eparm[len('out '):]
            elif eparm.startswith(' const IntPtr'):
                eparm = eparm[len(' const '):]
            elif eparm.startswith('out str_t'):
                eparm = 'IntPtr' + eparm[len('out str_t'):]
            elif eparm.startswith(' const str_t'):
                eparm = 'IntPtr' + eparm[len(' const str_t'):]
            elif eparm.startswith(' ref str_t'):
                eparm = 'IntPtr' + eparm[len(' ref str_t'):]
            entryParams.append(eparm)
        out += 'extern static %s _%s (%s);\n' % (rtype, name, ', '.join(entryParams))
      else:
        # We now check for unsafe parameters
        entryParams = []
        unsafeParams = []
        for eparm in params:
            if eparm.startswith('out IntPtr'):
                unsafeParams.append(eparm[len('out IntPtr '):])
                eparm = 'byte[]' + eparm[len('out IntPtr'):]
            elif eparm.startswith(' const IntPtr'):
                unsafeParams.append(eparm[len(' const IntPtr '):])
                eparm = 'byte[]' + eparm[len(' const IntPtr'):]
            elif eparm.startswith('out str_t'):
                unsafeParams.append(eparm[len('out str_t '):])
                eparm = 'byte[]' + eparm[len('out str_t'):]
            elif eparm.startswith(' const str_t'):
                unsafeParams.append(eparm[len(' const str_t '):])
                eparm = 'byte[]' + eparm[len(' const str_t'):]
            elif eparm.startswith(' ref str_t'):
                unsafeParams.append(eparm[len(' ref str_t '):])
                eparm = 'byte[]' + eparm[len(' ref str_t'):]
                
            entryParams.append(eparm)
        out = 'public static %s %s (%s) \n{\n' % (rtype, name, ', '.join(entryParams))

        needsReturn = ''
        if rtype != 'void':
            needsReturn = 'return'

        tabs = '\t'
        if unsafeParams:
            # Generate some sanity checks for the unsafe parameters
            for cparm in unsafeParams:
                out += '%sif (%s == null)\n%s\tthrow new ArgumentNullException ("%s");\n\n' % (tabs,cparm,tabs,cparm)
            out += '%sunsafe\n%s{\n' % (tabs,tabs)
            for uparm in unsafeParams:
                tabs += '\t'
                out += '%sfixed (byte* %s_ = &%s[0])\n%s{\n' % (tabs,uparm, uparm[0:len(uparm)], tabs)
            tabs += '\t'

        out += '%s%s _%s (%s);\n' % (tabs,needsReturn, name, self.FormatArgs('', callspec))
            

        if unsafeParams:
            tabs = tabs[0:len(tabs) - 1]
            for uparm in unsafeParams:
                out += '%s}\n' % tabs
                tabs = tabs[0:len(tabs) - 1]
            out += '%s}\n' % tabs
        out += '}\n\n'
    self.LogExit('Exit Compose: %s' % out)
    return out

  #
  # GetSignature
  #
  # Returns the 'C' style signature of the object
  #  prefix - A prefix for the object's name
  #  func_as_ptr - Formats a function as a function pointer
  #  include_name - If true, include member name in the signature.
  #                 If false, leave it out. In any case, prefix is always
  #                 included.
  #  include_version - if True, include version in the member name
  #
  def GetSignature(self, node, release, mode, prefix='', func_as_ptr=True,
                   include_name=True, include_version=False, compose_mode=''):
    self.LogEnter('GetSignature %s %s as func=%s' %
                  (node, mode, func_as_ptr))

    rtype, name, arrayspec, callspec = self.GetComponents(node, release, mode, compose_mode)
 
    # Member name can not be the same as enclosing type
    if node.cls == 'Typedef':
        if name == self.GetStructName(node, release, False):
            name = name.lower()

    if include_version:
      name = self.GetStructName(node, release, True)

    # If not a callspec (such as a struct) use a ptr instead of []
    unsized_as_ptr = not callspec

    out = self.Compose(rtype, name, arrayspec, callspec, prefix,
                       func_as_ptr, include_name, unsized_as_ptr, compose_mode)

    self.LogExit('Exit GetSignature: %s' % out)
    return out

  # Define a Typedef.
  def DefineTypedef(self, node, releases, prefix='', comment=False, compose_mode=''):
    __pychecker__ = 'unusednames=comment'
    build_list = node.GetUniqueReleases(releases)

    rtype, name, arrayspec, callspec = self.GetComponents(node, build_list[-1], 'return')

    #if this is a Typedef that has a callspec we will not handle that right now
    #example being a callback function.
    if callspec:
        params = []
        for ptype, pname, parray, pspec in callspec:
            params.append(self.Compose(ptype, pname, parray, pspec, '', True,
                                        include_name=True,
                                        unsized_as_ptr=True,
                                        compose_mode='Params'))
        
        entryParams = []
        for eparm in params:
            if eparm.startswith('out IntPtr'):
                eparm = eparm[len('out '):]
            elif eparm.startswith('const IntPtr'):
                eparm = eparm[len('const '):]
            entryParams.append(eparm)

        out = '[UnmanagedFunctionPointer(CallingConvention.Cdecl)]\npublic delegate %s %s (%s);\n\n' % (rtype, name,
                ', '.join(entryParams))
    else:
        out = '[StructLayout(LayoutKind.Sequential)]\npublic partial struct %s {\n\t%s;\n}\n' % (self.FormatStructName(node.GetName()), 
                self.GetSignature(node, build_list[-1], 'return',
                                              prefix, True,
                                              include_version=False))

    # We will not support version mangling right now
    # Version mangle any other versions
    #for index, rel in enumerate(build_list[:-1]):
    #  out += '\n'
    #  out += 'typedef %s;\n' % self.GetSignature(node, rel, 'return',
    #                                             prefix, True,
    #                                             include_version=True)
    self.Log('DefineTypedef: %s' % out)
    return out

  def is_number(self, s):
      try:
          complex(s) # for int, long, float and complex
      except ValueError:
          return False

      return True

  def GetItemName(self, enum, item):
    suppress_prefix = GetOption('cs-enum_prefix')
    if suppress_prefix:
        enumeration = item
        if item.upper().startswith(enum.upper()+"_"):
            enumeration = item[len(enum+"_"):]
        elif item.upper().startswith(enum.upper()):
            enumeration = item[len(enum):]

        if self.is_number(enumeration):    
            enumeration = "_" + enumeration

        if enumeration.startswith('PP_'):
            enumeration = enumeration[3:]
        if enumeration.startswith('PPB_'):
            enumeration = enumeration[4:]
        enumeration = enumeration.lower()
        enumeration = enumeration.replace('_', ' ')
        enumeration = string.capwords(enumeration)
        enumeration = "".join(enumeration.split(' '))

        if self.is_number(enumeration):    
            enumeration = "_" + enumeration

        if enumeration in self.RemapEnum:
            enumeration = self.RemapEnum[enumeration]

        item = enumeration
    return item

  def GetEnumValue(self, enum, value):
    suppress_prefix = GetOption('cs-enum_prefix')
    if suppress_prefix:
        enumValue = value
        if value.upper().startswith(enum.upper()+"_"):
            enumValue = value[len(enum+"_"):]
        elif value.upper().startswith(enum.upper()):
            enumValue = value[len(enum):]

        enumValue = enumValue.lower()
        enumValue = enumValue.replace('_', ' ')
        enumValue = string.capwords(enumValue)
        enumValue = "".join(enumValue.split(' '))

        value = enumValue
    return value

  def GetEnumName(self, enum):
    return self.StripUnderScores(enum)

  # Define an Enum.
  def DefineEnum(self, node, releases, prefix='', comment=False, compose_mode=''):
    __pychecker__ = 'unusednames=comment,releases'
    self.LogEnter('DefineEnum %s' % node)
    name = '%s%s' % (prefix, node.GetName())
    notypedef = node.GetProperty('notypedef')
    unnamed = node.GetProperty('unnamed')
    asize = node.GetProperty('assert_size()')
    out = 'public enum %s {' % self.GetEnumName(name)

    enumlist = []
    for child in node.GetListOf('EnumItem'):
      value = child.GetProperty('VALUE')
      comment_txt = GetNodeComments(child, tabs=1)
      if value:
        item_txt = '%s%s = %s' % (prefix, self.GetItemName(name, child.GetName()), self.GetEnumValue(name, value))
      else:
        item_txt = '%s%s' % (prefix, self.GetItemName(name, child.GetName()))
      enumlist.append('%s  %s' % (comment_txt, item_txt))

    out = '%s\n%s\n}\n' % (out, ',\n'.join(enumlist))

    self.LogExit('Exit DefineEnum')
    return out

  def DefineMember(self, node, releases, prefix='', comment=False, compose_mode=''):
    __pychecker__ = 'unusednames=prefix,comment'
    release = releases[0]
    self.LogEnter('DefineMember %s' % node)
    if node.GetProperty('ref'):
      out = '%s;' % self.GetSignature(node, release, 'ref', '', True, compose_mode=compose_mode)
    else:
      if compose_mode:
        # Check for blacklisted methods that need to be manually handled.
        black = "%s_%s" % (node.parent.GetName(),node.GetName())
        if black in CSGen.BlackListed:
            print ('/* Not generating entry point methods for %s */\n\n' % black)
            out = '/* Not generating entry point methods for %s */\n\n' % black
        else:
            out = '%s' % self.GetSignature(node, release, 'store', '', True, compose_mode=compose_mode)
      else:
        out = '%s;' % self.GetSignature(node, release, 'store', '', True, compose_mode=compose_mode)
    self.LogExit('Exit DefineMember')
    return out

  def GetStructName(self, node, release, include_version=False):
    suffix = ''
    if include_version:
      ver_num = node.GetVersion(release)
      suffix = ('_%s' % ver_num).replace('.', '_')
    structureName = node.GetName() + suffix
    return self.FormatStructName(structureName)

  def FormatStructName(self, structureName):
    return self.StripUnderScores(structureName)

  def DefineStructInternals(self, node, release,
                            include_version=False, comment=True):
    channel = node.GetProperty('FILE').release_map.GetChannel(release)
    if channel == 'dev':
      channel_comment = ' /* dev */'
    else:
      channel_comment = ''
    out = ''
    structName = self.GetStructName(node, release, include_version)
    if node.GetProperty('union'):
      out += 'union %s {%s\n' % (
          structName, channel_comment)
    else:
      out += '[StructLayout(LayoutKind.Sequential)]\npublic partial struct %s {%s\n' % (
          structName, channel_comment)
    channel = node.GetProperty('FILE').release_map.GetChannel(release)
    # Generate Member Functions
    members = []
    for child in node.GetListOf('Member'):
      if channel == 'stable' and child.NodeIsDevOnly():
        continue
      member = self.Define(child, [release], tabs=1, comment=comment)
      if not member:
        continue
      members.append(member)
    out += '%s\n};\n' % '\n'.join(members)
    return out

  def GetInterfaceName(self, node, release, include_version=False):
    suffix = ''
    if include_version:
      ver_num = node.GetVersion(release)
      suffix = ('_%s' % ver_num).replace('.', '_')
    return node.GetName() + suffix

  def DefineInterfaceInternals(self, node, release,
                            include_version=False, comment=True):
    channel = node.GetProperty('FILE').release_map.GetChannel(release)
    if channel == 'dev':
      channel_comment = ' /* dev */'
    else:
      channel_comment = ''
    out = ''
    interfaceName = ''
    if node.GetProperty('union'):
      out += 'union %s {%s\n' % (
          self.GetStructName(node, release, include_version), channel_comment)
    else:
      interfaceName = self.GetInterfaceName(node, release, include_version)
      out += 'public static partial class %s {%s\n' % (
          self.StripUnderScores(interfaceName), channel_comment)

    channel = node.GetProperty('FILE').release_map.GetChannel(release)
    # Generate Member Functions for PInvoke
    members = []
    for child in node.GetListOf('Member'):
      if channel == 'stable' and child.NodeIsDevOnly():
        continue
      member = self.Define(child, [release], tabs=1, comment=comment, compose_mode='EntryPoint_%s' % interfaceName)
      if not member:
        continue
      members.append(member)
      member = self.Define(child, [release], tabs=1, comment=comment, compose_mode='Binding')
      if not member:
        continue
      members.append(member)
    out += '%s\n}\n' % '\n'.join(members)


    return out

  def DefineUnversionedInterface(self, node, rel):
    out = '\n'
    if node.GetProperty('force_struct_namespace'):
      # Duplicate the definition to put it in struct namespace. This
      # attribute is only for legacy APIs like OpenGLES2 and new APIs
      # must not use this. See http://crbug.com/411799
      out += self.DefineStructInternals(node, rel,
                                        include_version=False, comment=True)
    else:
      # Define an unversioned typedef for the most recent version
      out += 'typedef struct %s %s;\n' % (
        self.GetStructName(node, rel, include_version=True),
        self.GetStructName(node, rel, include_version=False))
    return out


  def DefineStruct(self, node, releases, prefix='', comment=False, compose_mode=''):
    __pychecker__ = 'unusednames=comment,prefix'
    self.LogEnter('DefineStruct %s' % node)
    out = ''
    build_list = node.GetUniqueReleases(releases)

    newest_stable = None
    newest_dev = None
    for rel in build_list:
      channel = node.GetProperty('FILE').release_map.GetChannel(rel)
      if channel == 'stable':
        newest_stable = rel
      if channel == 'dev':
        newest_dev = rel
    last_rel = build_list[-1]

    # TODO(bradnelson) : Bug 157017 finish multiversion support
    if node.IsA('Struct'):
      if len(build_list) != 1:
        node.Error('Can not support multiple versions of node.')
      assert len(build_list) == 1
      # Build the most recent one versioned, with comments
      if node.GetProperty('union'):
        # We do not handle generating for unions automatically right now
        print ("Skipping generation of << union %s >>." % node.GetName())
        out = "/* Skipping generation of << union %s >>. */" % node.GetName()

      else:
        out = self.DefineStructInternals(node, last_rel,
                                         include_version=False, comment=True)

    self.LogExit('Exit DefineStruct')
    return out

  def DefineInterface(self, node, releases, prefix='', comment=False, compose_mode=''):
    __pychecker__ = 'unusednames=comment,prefix'
    self.LogEnter('DefineInterface %s' % node)
    out = ''
    build_list = node.GetUniqueReleases(releases)

    newest_stable = None
    newest_dev = None
    for rel in build_list:
      channel = node.GetProperty('FILE').release_map.GetChannel(rel)
      if channel == 'stable':
        newest_stable = rel
      if channel == 'dev':
        newest_dev = rel
    last_rel = build_list[-1]

    if node.IsA('Interface'):
      # Build the most recent one versioned, with comments
      out = self.DefineInterfaceInternals(node, last_rel,
                                       include_version=False, comment=True)

    self.LogExit('Exit DefineInterface')
    return out

  #
  # Copyright and Comment
  #
  # Generate a comment or copyright block
  #
  def Copyright(self, node, cpp_style=False):
    lines = node.GetName().split('\n')
    if cpp_style:
      return '//' + '\n//'.join(filter(lambda f: f != '', lines)) + '\n'
    return CommentLines(lines)


  def Indent(self, data, tabs=0):
    """Handles indentation and 80-column line wrapping."""
    tab = '  ' * tabs
    lines = []
    for line in data.split('\n'):
      # Add indentation
      line = tab + line
      space_break = line.rfind(' ', 0, 80)
      if len(line) <= 80 or 'http://' in line:
        # Ignore normal line and URLs permitted by the style guide.
        lines.append(line.rstrip())
      elif not '(' in line and space_break >= 0:
        # Break long typedefs on nearest space.
        lines.append(line[0:space_break])
        lines.append('    ' + line[space_break + 1:])
      else:
        left = line.rfind('(') + 1
        args = line[left:].split(',')
        orig_args = args
        orig_left = left
        # Try to split on '(arg1)' or '(arg1, arg2)', not '()'
        while args[0][0] == ')':
          left = line.rfind('(', 0, left - 1) + 1
          if left == 0:  # No more parens, take the original option
            args = orig_args
            left = orig_left
            break
          args = line[left:].split(',')

        line_max = 0
        for arg in args:
          if len(arg) > line_max: line_max = len(arg)

        if left + line_max >= 80:
          indent = '%s    ' % tab
          args =  (',\n%s' % indent).join([arg.strip() for arg in args])
          lines.append('%s\n%s%s' % (line[:left], indent, args))
        else:
          indent = ' ' * (left - 1)
          args =  (',\n%s' % indent).join(args)
          lines.append('%s%s' % (line[:left], args))
    return '\n'.join(lines)


  # Define a top level object.
  def Define(self, node, releases, tabs=0, prefix='', comment=False, compose_mode=''):
    # If this request does not match unique release, or if the release is not
    # available (possibly deprecated) then skip.
    unique = node.GetUniqueReleases(releases)
    if not unique or not node.InReleases(releases):
      return ''

    self.LogEnter('Define %s tab=%d prefix="%s"' % (node,tabs,prefix))
    declmap = dict({
      'Enum': CSGen.DefineEnum,
      'Function': CSGen.DefineMember,
      'Interface': CSGen.DefineInterface,
      'Member': CSGen.DefineMember,
      'Struct': CSGen.DefineStruct,
      'Typedef': CSGen.DefineTypedef
    })

    out = ''
    func = declmap.get(node.cls, None)
    if not func:
      ErrOut.Log('Failed to define %s named %s' % (node.cls, node.GetName()))
    define_txt = func(self, node, releases, prefix=prefix, comment=comment, compose_mode=compose_mode)

    comment_txt = GetNodeComments(node, tabs=0)
    if comment_txt and comment and not compose_mode.startswith('EntryPoint'):
      out += comment_txt
    out += define_txt

    indented_out = self.Indent(out, tabs)
    self.LogExit('Exit Define')
    return indented_out


# Clean a string representing an object definition and return then string
# as a single space delimited set of tokens.
def CleanString(instr):
  instr = instr.strip()
  instr = instr.split()
  return ' '.join(instr)


# Test a file, by comparing all it's objects, with their comments.
def TestFile(filenode):
  csgen = CSGen()

  errors = 0
  for node in filenode.GetChildren()[2:]:
    instr = node.GetOneOf('Comment')
    if not instr: continue
    instr.Dump()
    instr = CleanString(instr.GetName())

    outstr = csgen.Define(node, releases=['M14'])
    if GetOption('verbose'):
      print outstr + '\n'
    outstr = CleanString(outstr)

    if instr != outstr:
      ErrOut.Log('Failed match of\n>>%s<<\nto:\n>>%s<<\nFor:\n' %
                 (instr, outstr))
      node.Dump(1, comments=True)
      errors += 1
  return errors


# Build and resolve the AST and compare each file individual.
def TestFiles(filenames):
  if not filenames:
    idldir = os.path.split(sys.argv[0])[0]
    idldir = os.path.join(idldir, 'test_cgen', '*.idl')
    filenames = glob.glob(idldir)

  filenames = sorted(filenames)
  ast = ParseFiles(filenames)

  total_errs = 0
  for filenode in ast.GetListOf('File'):
    errs = TestFile(filenode)
    if errs:
      ErrOut.Log('%s test failed with %d error(s).' %
                 (filenode.GetName(), errs))
      total_errs += errs

  if total_errs:
    ErrOut.Log('Failed generator test.')
  else:
    InfoOut.Log('Passed generator test.')
  return total_errs

def main(args):
  filenames = ParseOptions(args)
  if GetOption('test'):
    return TestFiles(filenames)
  ast = ParseFiles(filenames)
  cgen = CSGen()
  for f in ast.GetListOf('File'):
    if f.GetProperty('ERRORS') > 0:
      print 'Skipping %s' % f.GetName()
      continue
    for node in f.GetChildren()[2:]:
      print cgen.Define(node, ast.releases, comment=True, prefix='tst_')


if __name__ == '__main__':
  sys.exit(main(sys.argv[1:]))
