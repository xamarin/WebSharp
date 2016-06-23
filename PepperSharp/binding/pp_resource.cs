/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_resource.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the <code>PP_Resource</code> type which represents data
 * associated with the module.
 */


/**
 * @addtogroup Typedefs
 * @{
 */
/**
 * This typedef represents an opaque handle assigned by the browser to the
 * resource. The handle is guaranteed never to be 0 for a valid resource, so a
 * module can initialize it to 0 to indicate a "NULL handle." Some interfaces
 * may return a NULL resource to indicate failure.
 *
 * While a Var represents something callable to JS or from the module to
 * the DOM, a resource has no meaning or visibility outside of the module
 * interface.
 *
 * Resources are reference counted. Use <code>AddRefResource()</code>
 * and <code>ReleaseResource()</code> in <code>ppb_core.h</code> to manage the
 * reference count of a resource. The data will be automatically destroyed when
 * the internal reference count reaches 0.
 */
[StructLayout(LayoutKind.Sequential)]
public struct PP_Resource {
	public int pp_resource;
}
/**
 * @}
 */


}
