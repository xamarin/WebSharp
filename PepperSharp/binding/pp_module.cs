/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_module.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the PP_Module type which uniquely identifies the module
 * or .nexe.
 */


/**
 * @addtogroup Typedefs
 * @{
 */
/**
 * The PP_Module value uniquely identifies the module or .nexe.
 *
 * This identifier is an opaque handle assigned by the browser to the module. It
 * is guaranteed never to be 0, so a module can initialize it to 0 to
 * indicate a "NULL handle."
 */
[StructLayout(LayoutKind.Sequential)]
public struct PP_Module {
	public int pp_module;
}
/**
 * @}
 */


}
