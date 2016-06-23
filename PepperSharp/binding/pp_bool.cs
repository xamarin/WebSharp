/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_bool.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the <code>PP_Bool</code> enumeration for use in PPAPI C
 * headers.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * The <code>PP_Bool</code> enum is a boolean value for use in PPAPI C headers.
 * The standard bool type is not available to pre-C99 compilers, and is not
 * guaranteed to be compatible between C and C++, whereas the PPAPI C headers
 * can be included from C or C++ code.
 */
public enum PP_Bool {
  PP_FALSE = 0,
  PP_TRUE = 1
}
/**
 * @}
 */


}
