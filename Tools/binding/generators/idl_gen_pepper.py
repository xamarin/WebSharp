#!/usr/bin/env python

"""Generator for Pepper functions that defines the DLL PInvoke entry points
between C# and the C++ PepperPlugin interface.  """

from datetime import datetime
import difflib
import glob
import os
import sys

from idl_c_proto import CGen
from idl_gen_wrapper import Interface, WrapperGen
from idl_log import ErrOut, InfoOut, WarnOut
from idl_option import GetOption, Option, ParseOptions
from idl_parser import ParseFiles

Option('pepperfile', 'Name of the pepper file.',
       default='../pepper/pepper_entrypoints.cpp ')

Option('disable_pepper_opt', 'Turn off optimization of pepper.')
Option('disable_region_gen', 'Turn off #pragma region generation.')

# Remap default values
RemapDefaultValue = {
'PP_Bool': 'PP_FromBool(FALSE)',
'void': '',
'PP_ImageDataFormat': 'PP_IMAGEDATAFORMAT_BGRA_PREMUL',
'PP_InputEvent_MouseButton' : 'PP_INPUTEVENT_MOUSEBUTTON_NONE',
'PP_InputEvent_Type' : 'PP_INPUTEVENT_TYPE_UNDEFINED',
'PP_FloatPoint': 'PP_MakeFloatPoint(0,0)',
'struct PP_FloatPoint': 'PP_MakeFloatPoint(0,0)',
'PP_Point': 'PP_MakePoint(0,0)',
'struct PP_Point': 'PP_MakePoint(0,0)',
'PP_TouchPoint': 'PP_MakeTouchPoint()',
'struct PP_TouchPoint': 'PP_MakeTouchPoint()',
'struct PP_Var': 'PP_MakeNull()',
'PP_Var': 'PP_MakeNull()',
'PP_WebSocketReadyState': 'PP_WEBSOCKETREADYSTATE_INVALID',
'PP_AudioSampleRate': 'PP_AUDIOSAMPLERATE_NONE', 
'PP_AudioBuffer_SampleRate': 'PP_AUDIOBUFFER_SAMPLERATE_UNKNOWN',
'PP_AudioBuffer_SampleSize': 'PP_AUDIOBUFFER_SAMPLESIZE_UNKNOWN',
'PP_VideoFrame_Format' : 'PP_VIDEOFRAME_FORMAT_UNKNOWN',
'PP_FileSystemType' : 'PP_FILESYSTEMTYPE_EXTERNAL'
}

class PPKind(object):
  @staticmethod
  def ChoosePPFunc(iface, ppb_func, ppp_func):
    name = iface.node.GetName()
    if name.startswith("PPP"):
      return ppp_func
    elif name.startswith("PPB"):
      return ppb_func
    else:
      raise Exception('Unknown PPKind for ' + name)

class PepperGen(WrapperGen):
  """PepperGen generates code entry points for C# PInvoking API.

  This subclass of WrapperGenerator takes the IDL sources and
  generates the DLL PInvoke entryp points between C# and the 
  the Native C++ PepperPlugin.
  """

  def __init__(self):
    WrapperGen.__init__(self,
                        'Pepper',
                        'Pepper Gen',
                        'pepper',
                        'Generate the Pepper DLL export.')
    self.cgen = CGen()
    self._skip_opt = False
    self._skip_region_gen = False

  #
  # RemapParameter
  #
  # A diction array of parameter types that need to be remappted
  #
  RemapParameter = {
 # 'struct PP_Var*': 'const char*',  
  }

  #
  # WrapArgument
  #
  # A diction array of parameter types that need to be wrapped
  #
  WrapArgument = {
  'struct PP_Var': '%s',
  'struct PP_CompletionCallback': '%s',
  'struct PP_ArrayOutput': '%s'
  }

  #
  # WrapReturn
  #
  # A diction array of parameter types that need to be wrapped
  #
  WrapReturn = {
#  'struct PP_Var': 'Var(%s).AsString().c_str()'
  }

  #
  # ExcludeInterface
  #
  # A dictionary of interfaces that we will not generate for
  #
  ExcludeInterface = [
  'PPB_Audio_1_0'
  ]

  ############################################################

  def OwnHeaderFile(self):
    """Return the header file that specifies the API of this wrapper.
    We do not generate the header files.  """
    return 'pepper_entrypoints.h'

  def WriteCopyright(self, out):
    now = datetime.now()
    c = """/* Copyright (c) %s Xamarin. */
#include "stdafx.h"
/* NOTE: this is auto-generated from IDL */
""" % now.year
    out.Write(c)

  def GenerateHelperFunctions(self, out):
    """Generate helper functions.
    """
    out.Write("""#ifndef PEPPER_EXPORT
#define PEPPER_EXPORT __declspec(dllexport)
#endif

using namespace pp;

namespace Pepper {

	namespace {
		// Specialize this function to return the interface string corresponding to the
		// PP?_XXX structure.
		template <typename T> const char* interface_name() {
			return NULL;
		}

		template <typename T> inline T const* get_interface() {
			static T const* funcs = reinterpret_cast<T const*>(
				pp::Module::Get()->GetBrowserInterface(interface_name<T>()));
			return funcs;
		}

		template <typename T> inline bool has_interface() {
			return get_interface<T>() != NULL;
		}

	}
}
""")

  def GenerateFixedFunctions(self, out):
    """Write out the set of constant functions (those that do not depend on
    the current Pepper IDL).
    """

  def InterfaceVersionNeedsWrapping(self, iface, version):
    """Return true if the interface+version has ANY methods that
    need wrapping.
    """
    if self._skip_opt:
      return True
    if iface.GetName().endswith('Trusted'):
      return False
    # TODO(dmichael): We have no way to wrap PPP_ interfaces without an
    # interface string. If any ever need wrapping, we'll need to figure out a
    # way to get the plugin-side of the Pepper proxy (within the IRT) to access
    # and use the wrapper.
    if iface.GetProperty("no_interface_string"):
      return False
    for member in iface.GetListOf('Member'):
      release = member.GetRelease(version)
      if self.MemberNeedsWrapping(member, release):
        return True
    return False


  def MemberNeedsWrapping(self, member, release):
    """Return true if a particular member function at a particular
    release needs wrapping.
    """
    if self._skip_opt:
      return True
    if not member.InReleases([release]):
      return False
    ret, name, array, args_spec = self.cgen.GetComponents(member,
                                                          release,
                                                          'store')
    return self.TypeNeedsWrapping(ret, []) or self.ArgsNeedWrapping(args_spec)


  def ArgsNeedWrapping(self, args):
    """Return true if any parameter in the list needs wrapping.
    """
    for arg in args:
      (type_str, name, array_dims, more_args) = arg
      if self.TypeNeedsWrapping(type_str, array_dims):
        return True
    # we always return true here so all interfaces are generated
    return True


  def TypeNeedsWrapping(self, type_node, array_dims):
    """Return true if a parameter type needs wrapping.
    Currently, this is true for byval aggregates.
    """
    #is_aggregate = type_node.startswith('struct') or \
    #    type_node.startswith('union')
    is_aggregate = type_node.startswith('union') 

    is_reference = (type_node.find('*') != -1 or array_dims != [])
    return is_aggregate and not is_reference

  ############################################################


  def ConvertByValueReturnType(self, ret, args_spec):
    if self.TypeNeedsWrapping(ret, array_dims=[]):
      args_spec = [(ret, '_struct_result', [], None)] + args_spec
      ret2 = 'void'
      wrap_return = True
    else:
      ret2 = ret
      wrap_return = False
    return wrap_return, ret2, args_spec


  def ConvertByValueArguments(self, args_spec):
    args = []
    for type_str, name, array_dims, more_args in args_spec:
      if self.TypeNeedsWrapping(type_str, array_dims):
        type_str += '*'
      args.append((type_str, name, array_dims, more_args))
    return args


  def FormatArgs(self, c_operator, args_spec):
    args = []
    for type_str, name, array_dims, more_args in args_spec:
      if self.TypeNeedsWrapping(type_str, array_dims):
        if type_str in PepperGen.WrapArgument:
            args.append(PepperGen.WrapArgument[type_str] % name)
        else:
            args.append(c_operator + name)
      else:
        if type_str in PepperGen.WrapArgument:
            args.append(PepperGen.WrapArgument[type_str] % name)
        else:
            if type_str.startswith('struct ') and type_str.rfind('*') < 0:
                args.append('&%s' % name)
            else:
                args.append(name)
    return ', '.join(args)

  def GenerateWrapperForMethodGroup(self, iface_releases, comments=True):
   
    result = []
    
    if len(iface_releases) == 0:
        return ''
    
    iface = iface_releases[len(iface_releases)-1]

    if not iface.needs_wrapping:
        if comments:
          result.append('/* Not generating entry point methods for %s */\n\n' %
                        iface.struct_name)
        return ''.join(result)
    if self._skip_region_gen:
        result.append('\t\t/* Begin entry point methods for %s */\n\n' % iface.node.GetName())
    else:
        result.append('\t\t#pragma region /* Begin entry point methods for %s */\n\n' % iface.node.GetName())

    generator =  PPKind.ChoosePPFunc(iface,
                                self.GenerateWrapperForPPBMethodReleases,
                                self.GenerateWrapperForPPPMethodReleases)

    for member in iface.node.GetListOf('Member'):
        # Skip the method if it's not actually in the release.
        if not member.InReleases([iface.release]):
          continue
        result.extend(generator(iface, member, iface_releases))

    if comments:
        if self._skip_region_gen:
            result.append('\t\t/* End entry point generation for %s */\n\n' % iface.node.GetName())
        else:
            result.append('\t\t#pragma endregion /* End entry point generation for %s */\n\n' % iface.node.GetName())

    return ''.join(result)

  def GenerateWrapperForMethods(self, iface_releases, comments=True):
    """Return a string representing the code for each wrapper method
    (using a string rather than writing to the file directly for testing.)
    """
    result = []

    result.append("""namespace Pepper {


	/* We don't want name mangling for these external functions.  We only need
	* 'extern "C"' if we're compiling with a C++ compiler.
	*/
#ifdef __cplusplus
	extern "C" {
#endif
	namespace {\n\n""" )
    
    iface_release_group = []
    rangeName = iface_releases[0].node.GetName()
    for x in range(len(iface_releases)):
        if iface_releases[x].node.GetName() == rangeName:
            iface_release_group.append(iface_releases[x])
        else:
            print ("Generating entry points for %s for %d release(s)." % (rangeName, len(iface_release_group)))
            result.append(self.GenerateWrapperForMethodGroup(iface_release_group, comments))
            rangeName = iface_releases[x].node.GetName()
            iface_release_group = []
            iface_release_group.append(iface_releases[x])

    print ("Generating entry points for %s for %d release(s)." % (rangeName, len(iface_release_group)))
    if len(iface_release_group) > 0:
        result.append(self.GenerateWrapperForMethodGroup(iface_release_group, comments))
    
    result.append("""	}
#ifdef __cplusplus
	}  /* extern "C" */
#endif
}\n""")
    return ''.join(result )




  def WrapperMethodPrefix(self, iface, release):
    return '%s_' % (iface.GetName() )

  def Compose(self, rtype, name, arrayspec, callspec, prefix, func_as_ptr,
              include_name, unsized_as_ptr):
    self.cgen.LogEnter('Compose: %s %s' % (rtype, name))
    arrayspec = ''.join(arrayspec)

    # Switch unsized array to a ptr. NOTE: Only last element can be unsized.
    if unsized_as_ptr and arrayspec[-2:] == '[]':
      prefix +=  '*'
      arrayspec=arrayspec[:-2]

    if not include_name:
      name = prefix + arrayspec
    else:
      name = prefix + name + arrayspec
    if callspec is None:
      if rtype in PepperGen.RemapParameter:
          rtype = PepperGen.RemapParameter[rtype]
      out = '%s %s' % (rtype, name)
    else:
      params = []
      for ptype, pname, parray, pspec in callspec:
        params.append(self.Compose(ptype, pname, parray, pspec, '', True,
                                   include_name=True,
                                   unsized_as_ptr=unsized_as_ptr))
      if func_as_ptr:
        name = '(*%s)' % name
      if not params:
        params = ['void']
      out = '%s %s(%s)' % (rtype, name, ', '.join(params))
    self.cgen.LogExit('Exit Compose: %s' % out)
    return out

  def GenerateWrapperForPPBMethodReleases(self, iface, member, iface_releases):
    result = []
    func_prefix = self.WrapperMethodPrefix(iface.node, iface.release)
    if func_prefix == 'PPB_Graphics2D_':
        func_prefix = func_prefix
    ret, name, array, cspec = self.cgen.GetComponents(member,
                                                      iface.release,
                                                      'store')
    wrap_return, ret2, cspec2 = self.ConvertByValueReturnType(ret, cspec)
    cspec2 = self.ConvertByValueArguments(cspec2)
    
    if ret2 in PepperGen.RemapParameter:
        ret2 = PepperGen.RemapParameter[ret2]
    sig = self.Compose(ret2, name, array, cspec2,
                            prefix=func_prefix,
                            func_as_ptr=False,
                            include_name=True,
                            unsized_as_ptr=False)
    result.append('\t\tPEPPER_EXPORT %s {\n' % sig)

    return_prefix = ''
    if wrap_return:
      return_prefix = ''
    elif ret != 'void':
      return_prefix = 'return '

    first = True
    
    for iface_release in reversed(iface_releases):
        # Skip the method if it's not actually in the release.
        if not member.InReleases([iface_release.release]):
          continue
        
        if self.GetWrapperInfoName(iface_release) in PepperGen.ExcludeInterface:
            continue

        if first:
            result.append('\t\t\tif (has_interface<%s>()) {\n' % self.GetWrapperInfoName(iface_release))
            result.append('\t\t\t\t%sget_interface<%s>()->%s(%s);\n' % (return_prefix, 
                                                    self.GetWrapperInfoName(iface_release),
                                                    member.GetName(),
                                                    self.FormatArgs('*', cspec)))

            result.append('\t\t\t}\n')
            first = False
        else:
            result.append('\t\t\telse if (has_interface<%s>()) {\n' % self.GetWrapperInfoName(iface_release))

            result.append('\t\t\t\t%sget_interface<%s>()->%s(%s);\n' % (return_prefix,
                                                    self.GetWrapperInfoName(iface_release),
                                                    member.GetName(),
                                                    self.FormatArgs('*', cspec)))

            result.append('\t\t\t}\n')
    
    dfltValue = ''
    if ret2 in RemapDefaultValue:
        dfltValue = RemapDefaultValue[ret2]
    else:
        dfltValue = "NULL";

    result.append('\t\t\treturn %s;\n\t\t}\n\n' % dfltValue)

    return result


  def GenerateWrapperForPPPMethodReleases(self, iface, member, iface_releases):
      # We just call the PPB code from here unless something changes.
      return self.GenerateWrapperForPPBMethodReleases(iface, member, iface_releases);

  def DeclareWrapperInfos(self, iface_releases, out):
    """The wrapper methods usually need access to the real_iface, so we must
    declare these wrapper infos ahead of time (there is a circular dependency).
    """
    out.Write('/* BEGIN Declarations for all Interface Definitions. */\n\n')
    out.Write('namespace Pepper {\n\tnamespace {\n')

    for iface in iface_releases:
      if iface.needs_wrapping:
        out.Write("""\t\ttemplate <> const char*	interface_name<%s>() {\n\t\t\treturn %s;\n\t\t}\n"""  % (self.GetWrapperInfoName(iface),
        self.cgen.GetInterfaceMacro(iface.node, iface.version)))

    out.Write('\t}\n}\n')
    out.Write('/* END Declarations for all Interface Definitions. */\n\n')
    
  def GetWrapperInfoName(self, iface):
    return '%s' % (iface.struct_name)

  def GetWrapperMetadataName(self):
    return '__%sWrapperInfo' % self.wrapper_prefix

  def GenerateWrapperInfoAndCollection(self, iface_releases, out):
    """Do not generate this as it is not needed"""

  def GenerateWrapperInterfaces(self, iface_releases, out):
    """Do not generate this as it is not needed"""

  def GenerateRange(self, ast, releases, options):
    """Generate entry point code for a range of releases.
    """
    self._skip_opt = GetOption('disable_pepper_opt')
    self._skip_region_gen = GetOption('disable_region_gen')
    self.SetOutputFile(GetOption('pepperfile'))
    return WrapperGen.GenerateRange(self, ast, releases, options)

peppergen = PepperGen()

######################################################################
# Tests.

# Clean a string representing an object definition and return then string
# as a single space delimited set of tokens.
def CleanString(instr):
  instr = instr.strip()
  instr = instr.split()
  return ' '.join(instr)


def PrintErrorDiff(old, new):
  oldlines = old.split(';')
  newlines = new.split(';')
  d = difflib.Differ()
  diff = d.compare(oldlines, newlines)
  ErrOut.Log('Diff is:\n%s' % '\n'.join(diff))


def GetOldTestOutput(ast):
  # Scan the top-level comments in the IDL file for comparison.
  old = []
  for filenode in ast.GetListOf('File'):
    for node in filenode.GetChildren():
      instr = node.GetOneOf('Comment')
      if not instr: continue
      instr.Dump()
      old.append(instr.GetName())
  return CleanString(''.join(old))


def TestFiles(filenames, test_releases):
  ast = ParseFiles(filenames)
  iface_releases = peppergen.DetermineInterfaces(ast, test_releases)
  new_output = CleanString(peppergen.GenerateWrapperForMethods(
      iface_releases, comments=False))
  old_output = GetOldTestOutput(ast)
  if new_output != old_output:
    PrintErrorDiff(old_output, new_output)
    ErrOut.Log('Failed pepper generator test.')
    return 1
  else:
    InfoOut.Log('Passed pepper generator test.')
    return 0


def Main(args):
  filenames = ParseOptions(args)
  test_releases = ['M13', 'M14', 'M15']
  if not filenames:
    idldir = os.path.split(sys.argv[0])[0]
    idldir = os.path.join(idldir, 'test_gen_pepper', '*.idl')
    filenames = glob.glob(idldir)
  filenames = sorted(filenames)
  if GetOption('test'):
    # Run the tests.
    return TestFiles(filenames, test_releases)

  # Otherwise, generate the output file (for potential use as golden file).
  ast = ParseFiles(filenames)
  return peppergen.GenerateRange(ast, test_releases, filenames)


if __name__ == '__main__':
  retval = Main(sys.argv[1:])
  sys.exit(retval)
