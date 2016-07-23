#!/usr/bin/env python
# Copyright (c) 2016 Xamarin, Inc

""" Generator for C# style binding definitions """

from datetime import datetime
import glob
import os
import re
import sys

from idl_log import ErrOut, InfoOut, WarnOut
from idl_node import IDLAttribute, IDLNode
from idl_ast import IDLAst
from idl_option import GetOption, Option, ParseOptions
from idl_outfile import IDLOutFile
from idl_parser import ParseFiles
from idl_cs_proto import CSGen, GetNodeComments, CommentLines, Comment
from idl_generator import Generator, GeneratorByFile
from idl_visitor import IDLVisitor

Option('cs_root', 'Base directory of output', default=os.path.join('..', 'peppersharp'))
Option('cs_guard', 'Include guard prefix', default=os.path.join('PepperSharp', 'c'))
Option('cs_namespace', 'Debug generate.', default='PepperSharp' )

#
# PrototypeResolver
#
# A specialized visitor which traverses the AST, building a mapping of
# Release names to Versions numbers and calculating a min version.
# The mapping is applied to the File nodes within the AST.
#
class ProtoResolver(IDLVisitor):
  def __init__(self):
    IDLVisitor.__init__(self)
    self.struct_map = {}
    self.interface_map = {}

  def Arrive(self, node, ignore):
    if node.IsA('Member') and node.GetProperty('ref'):
      typeref = node.typelist.GetReleases()[0]
      if typeref.IsA('Struct'):
        nodelist = self.struct_map.get(typeref.GetName(), [])
        nodelist.append(node)
        self.struct_map[typeref.GetName()] = nodelist

    if node.IsA('Param'):
      typeref = node.typelist.GetReleases()[0]
      if typeref.IsA('Interface'):
        nodelist = self.struct_map.get(typeref.GetName(), [])
        nodelist.append(node)
        self.interface_map[typeref.GetName()] = nodelist

    return None


def GetPathFromNode(filenode, relpath=None, ext=None):
  path, name = os.path.split(filenode.GetProperty('NAME'))
  if ext: name = os.path.splitext(name)[0] + ext
  if path: name = os.path.join(path, name)
  if relpath: name = os.path.join(relpath, name)
  name = os.path.normpath(name)
  return name


def GetSourceFileFromNode(filenode, relpath=None):
  return GetPathFromNode(filenode, relpath, ext='.cs')


def WriteGroupMarker(out, node, last_group):
  # If we are part of a group comment marker...
  if last_group and last_group != node.cls:
    pre = CommentLines(['*',' @}', '']) + '\n'
  else:
    pre = '\n'

  if node.cls in ['Typedef', 'Interface', 'Struct', 'Enum']:
    if last_group != node.cls:
      pre += CommentLines(['*',' @addtogroup %ss' % node.cls, ' @{', ''])
    last_group = node.cls
  else:
    last_group = None
  out.Write(pre)
  return last_group


def GenerateSourceFile(out, filenode, releases):
  csgen = CSGen()
  pref = ''
  do_comments = True

  # Generate definitions.
  last_group = None
  top_types = ['Typedef', 'Interface', 'Struct', 'Enum', 'Inline']

  for node in filenode.GetListOf(*top_types):
    # Skip if this node is not in this release
    if not node.InReleases(releases):
      print "Skipping %s" % node
      continue

    # End/Start group marker
    if do_comments:
      last_group = WriteGroupMarker(out, node, last_group)

    # Not supporting inline
    if node.IsA('Inline'):
      #item = node.GetProperty('VALUE')
      ## If 'C++' use __cplusplus wrapper
      #if node.GetName() == 'cc':
      #  item = '#if __cplusplus\n%s\n#endif  // __cplusplus \n\n' % item
      ## If not C++ or C, then skip it
      #elif not node.GetName() == 'c':
      #  continue
      #if item: out.Write(item)
      continue

    #
    # Otherwise we are defining a file level object, so generate the
    # correct document notation.
    #
    item = csgen.Define(node, releases, prefix=pref, comment=True)
    if not item: continue

    # TODO: Look into whether or not we need to output a marshalling size or not
    # even if we do we should not ouput this here
    #asize = node.GetProperty('assert_size()')
    #if asize:
    #  name = '%s%s' % (pref, node.GetName())
    #  if node.IsA('Struct'):
    #    form = 'PP_COMPILE_ASSERT_STRUCT_SIZE_IN_BYTES(%s, %s);\n'
    #  elif node.IsA('Enum'):
    #    if node.GetProperty('notypedef'):
    #      form = 'PP_COMPILE_ASSERT_ENUM_SIZE_IN_BYTES(%s, %s);\n'
    #    else:
    #      form = 'PP_COMPILE_ASSERT_SIZE_IN_BYTES(%s, %s);\n'
    #  else:
    #    form = 'PP_COMPILE_ASSERT_SIZE_IN_BYTES(%s, %s);\n'
    #  item += form % (name, asize[0])

    if item: out.Write(item)
  if last_group:
    out.Write(CommentLines(['*',' @}', '']) + '\n')


def CheckTypedefs(filenode, releases):
  """Checks that typedefs don't specify callbacks that take some structs.

  See http://crbug.com/233439 for details.
  """
  csgen = CSGen()
  for node in filenode.GetListOf('Typedef'):
    build_list = node.GetUniqueReleases(releases)
    callnode = node.GetOneOf('Callspec')
    if callnode:
      for param in callnode.GetListOf('Param'):
        if param.GetListOf('Array'):
          continue
        if csgen.GetParamMode(param) != 'in':
          continue
        t = param.GetType(build_list[0])
        while t.IsA('Typedef'):
          t = t.GetType(build_list[0])
        if t.IsA('Struct') and t.GetProperty('passByValue'):
          raise Exception('%s is a struct in callback %s. '
                          'See http://crbug.com/233439' %
                          (t.GetName(), node.GetName()))


def CheckPassByValue(filenode, releases):
  """Checks that new pass-by-value structs are not introduced.

  See http://crbug.com/233439 for details.
  """
  csgen = CSGen()
  # DO NOT add any more entries to this whitelist.
  # http://crbug.com/233439
  type_whitelist = ['PP_ArrayOutput', 'PP_CompletionCallback',
                    'PP_Ext_EventListener', 'PP_FloatPoint',
                    'PP_Point', 'PP_TouchPoint', 'PP_Var']
  nodes_to_check = filenode.GetListOf('Struct')
  nodes_to_check.extend(filenode.GetListOf('Union'))
  for node in nodes_to_check:
    if node.GetName() in type_whitelist:
      continue
    build_list = node.GetUniqueReleases(releases)
    if node.GetProperty('passByValue'):
      raise Exception('%s is a new passByValue struct or union. '
                      'See http://crbug.com/233439' % node.GetName())
    if node.GetProperty('returnByValue'):
      raise Exception('%s is a new returnByValue struct or union. '
                      'See http://crbug.com/233439' % node.GetName())

#
# namespace functions
#
def GetBeginNameSpace():
    if not GetOption('cs_namespace'): return ''
    return '\nnamespace %s {\n' % GetOption('cs_namespace')

def GetEndNameSpace():
    if not GetOption('cs_namespace'): return ''
    return '\n}\n'

class CSharpGen(GeneratorByFile):
  def __init__(self):
    Generator.__init__(self, 'C-Sharp', 'csgen', 'Generate the C# definitions.')

  def GenerateFile(self, filenode, releases, options):


    #
    # BlackListed
    #
    # A dictionary of idl files that we should not generate code for.  Ex. Files in development.
    #
    BlackListed = [
    'ppb_audio_input_dev.idl',
    'pp_video_capture_dev.idl',
    'ppb_device_ref_dev.idl',
    'ppb_video_capture_dev.idl',
    'ppb_device_ref_dev.idl'
    ]

    # make sure we are not generating for certain source files.
    if filenode.GetName() in BlackListed:
        print('Skipping generation of %s' % filenode.GetName())
        return False;

    CheckTypedefs(filenode, releases)
    CheckPassByValue(filenode, releases)
    savename = GetSourceFileFromNode(filenode, GetOption('cs_root'))
    my_min, my_max = filenode.GetMinMax(releases)
    if my_min > releases[-1] or my_max < releases[0]:
      if os.path.isfile(savename):
        print "Removing stale %s for this range." % filenode.GetName()
        os.remove(os.path.realpath(savename))
      return False

    out = IDLOutFile(savename)
    self.GenerateHead(out, filenode, releases, options)
    self.GenerateBody(out, filenode, releases, options)
    self.GenerateTail(out, filenode, releases, options)
    return out.Close()

  def GetCommonUsings (self):
    return {'System','System.Runtime.InteropServices'}

  def WriteCopyright(self, out):
    now = datetime.now()
    c = """/* Copyright (c) %s Xamarin. */

/* NOTE: this is auto-generated from IDL */
""" % now.year
    out.Write(c)

  def GenerateHead(self, out, filenode, releases, options):
    __pychecker__ = 'unusednames=options'

    proto = ProtoResolver()
    proto.Visit(filenode, None)

    csgen = CSGen()
    gpath = GetOption('cs_guard')
    def_guard = GetSourceFileFromNode(filenode, relpath=gpath)
    def_guard = def_guard.replace(os.sep,'_').replace('.','_').upper() + '_'

    cright_node = filenode.GetChildren()[0]
    assert(cright_node.IsA('Copyright'))
    fileinfo = filenode.GetChildren()[1]
    assert(fileinfo.IsA('Comment'))

    self.WriteCopyright(out)

    # Wrap the From ... modified ... comment if it would be >80 characters.
    from_text = 'From %s' % GetPathFromNode(filenode).replace(os.sep, '/')
    modified_text = 'modified %s.' % (
        filenode.GetProperty('DATETIME'))
    if len(from_text) + len(modified_text) < 74:
      out.Write('/* %s %s */\n\n' % (from_text, modified_text))
    else:
      out.Write('/* %s,\n *   %s\n */\n\n' % (from_text, modified_text))

    # Generate set of usings

    deps = set()
    for release in releases:
      deps |= filenode.GetDeps(release)

    # Output our common using statements
    usings = set([])
    for common in self.GetCommonUsings():
        usings.add(common)

    usings = sorted(set(usings))
    cur_include = GetSourceFileFromNode(filenode,
                                    relpath=gpath).replace(os.sep, '/')
    for using in usings:
      if using == cur_include: continue
      out.Write('using %s;\n' % using)

    # write out the namespace
    out.Write(GetBeginNameSpace())

    # Generate Prototypes
    if proto.struct_map:
      out.Write('\n/* Struct prototypes */\n')
      for struct in proto.struct_map:
        out.Write('struct %s;\n' % struct)

    # Create a macro for the highest available release number.
    if filenode.GetProperty('NAME').endswith('pp_macros.idl'):
      releasestr = ' '.join(releases)
      if releasestr:
        release_numbers = re.findall('[\d\_]+', releasestr)
        release = re.findall('\d+', release_numbers[-1])[0]
        if release:
          out.Write('\n#define PPAPI_RELEASE %s\n' % release)

    # Generate all interface defines
    out.Write('\n')
    for node in filenode.GetListOf('Interface'):
      idefs = ''
      unique = node.GetUniqueReleases(releases)

      # Skip this interface if there are no matching versions
      if not unique: continue

      # Skip this interface if it should have no interface string.
      if node.GetProperty('no_interface_string'): continue

      last_stable_ver = None
      last_dev_rel = None
      for rel in unique:
        channel = node.GetProperty('FILE').release_map.GetChannel(rel)
        if channel == 'dev':
          last_dev_rel = rel

      for rel in unique:
        version = node.GetVersion(rel)
        name = csgen.GetInterfaceString(node, version)
        strver = str(version).replace('.', '_')
        channel = node.GetProperty('FILE').release_map.GetChannel(rel)
        if channel == 'dev':
          # Skip dev channel interface versions that are
          #   Not the newest version, and
          #   Don't have an equivalent stable version.
          if rel != last_dev_rel and not node.DevInterfaceMatchesStable(rel):
            continue
          value_string = '"%s" /* dev */' % name
        else:
          value_string = '"%s"' % name
          last_stable_ver = strver
      if last_stable_ver:
        idefs += '\n'

      out.Write(idefs)

    # Generate the @file comment
    out.Write('%s\n' % Comment(fileinfo, prefix='*\n @file'))

  def GenerateBody(self, out, filenode, releases, options):
    __pychecker__ = 'unusednames=options'
    GenerateSourceFile(out, filenode, releases)

  def GenerateTail(self, out, filenode, releases, options):
    __pychecker__ = 'unusednames=options,releases'
    gpath = GetOption('cs_guard')
    def_guard = GetPathFromNode(filenode, relpath=gpath, ext='.h')
    def_guard = def_guard.replace(os.sep,'_').replace('.','_').upper() + '_'
    out.Write(GetEndNameSpace())


csgen = CSharpGen()

def main(args):
  # Default invocation will verify the golden files are unchanged.
  failed = 0
  if not args:
    args = ['--wnone', '--diff', '--test', '--cs_root=.']

  ParseOptions(args)

  idldir = os.path.split(sys.argv[0])[0]
  idldir = os.path.join(idldir, 'test_csgen', '*.idl')
  filenames = glob.glob(idldir)
  ast = ParseFiles(filenames)
  if csgen.GenerateRelease(ast, 'M14', {}):
    print "Golden file for M14 failed."
    failed = 1
  else:
    print "Golden file for M14 passed."


  idldir = os.path.split(sys.argv[0])[0]
  idldir = os.path.join(idldir, 'test_csgen_range', '*.idl')
  filenames = glob.glob(idldir)

  ast = ParseFiles(filenames)
  if csgen.GenerateRange(ast, ['M13', 'M14', 'M15', 'M16', 'M17'], {}):
    print "Golden file for M13-M17 failed."
    failed =1
  else:
    print "Golden file for M13-M17 passed."

  return failed

if __name__ == '__main__':
  sys.exit(main(sys.argv[1:]))

