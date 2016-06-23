/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_instance.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the PP_Instance type which uniquely identifies one module
 * instance.
 */


/**
 * @addtogroup Typedefs
 * @{
 */
/**
 * The <code>PP_Instance</code> value uniquely identifies one instance of a
 * module (.nexe/PP_Module). There will be one module instance for every
 * \<embed> tag on a page.
 *
 * This identifier is an opaque handle assigned by the browser to the module.
 * It is guaranteed never to be 0, so a module can initialize it to 0 to
 * indicate a "NULL handle."
 */
[StructLayout(LayoutKind.Sequential)]
public struct PP_Instance {
	public int pp_instance;
}
/**
 * @}
 */


}
