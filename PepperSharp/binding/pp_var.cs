/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_var.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the API for handling the passing of data types between
 * your module and the page.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * The <code>PP_VarType</code> is an enumeration of the different types that
 * can be contained within a <code>PP_Var</code> structure.
 */
public enum PPVarType {
  /**
   * An undefined value.
   */
  Undefined = 0,
  /**
   * A NULL value. This is similar to undefined, but JavaScript differentiates
   * the two so it is exposed here as well.
   */
  Null = 1,
  /**
   * A boolean value, use the <code>as_bool</code> member of the var.
   */
  Bool = 2,
  /**
   * A 32-bit integer value. Use the <code>as_int</code> member of the var.
   */
  Int32 = 3,
  /**
   * A double-precision floating point value. Use the <code>as_double</code>
   * member of the var.
   */
  Double = 4,
  /**
   * The Var represents a string. The <code>as_id</code> field is used to
   * identify the string, which may be created and retrieved from the
   * <code>PPB_Var</code> interface. These objects are reference counted, so
   * AddRef() and Release() must be used properly to avoid memory leaks.
   */
  String = 5,
  /**
   * Represents a JavaScript object. This vartype is not currently usable
   * from modules, although it is used internally for some tasks. These objects
   * are reference counted, so AddRef() and Release() must be used properly to
   * avoid memory leaks.
   */
  Object = 6,
  /**
   * Represents an array of Vars. The <code>as_id</code> field is used to
   * identify the array, which may be created and manipulated from the
   * <code>PPB_VarArray</code> interface. These objects are reference counted,
   * so AddRef() and Release() must be used properly to avoid memory leaks.
   */
  Array = 7,
  /**
   * Represents a mapping from strings to Vars. The <code>as_id</code> field is
   * used to identify the dictionary, which may be created and manipulated from
   * the <code>PPB_VarDictionary</code> interface. These objects are reference
   * counted, so AddRef() and Release() must be used properly to avoid memory
   * leaks.
   */
  Dictionary = 8,
  /**
   * ArrayBuffer represents a JavaScript ArrayBuffer. This is the type which
   * represents Typed Arrays in JavaScript. Unlike JavaScript 'Array', it is
   * only meant to contain basic numeric types, and is always stored
   * contiguously. See PPB_VarArrayBuffer_Dev for functions special to
   * ArrayBuffer vars. These objects are reference counted, so AddRef() and
   * Release() must be used properly to avoid memory leaks.
   */
  ArrayBuffer = 9,
  /**
   * This type allows the <code>PP_Var</code> to wrap a <code>PP_Resource
   * </code>. This can be useful for sending or receiving some types of
   * <code>PP_Resource</code> using <code>PPB_Messaging</code> or
   * <code>PPP_Messaging</code>.
   *
   * These objects are reference counted, so AddRef() and Release() must be used
   * properly to avoid memory leaks. Under normal circumstances, the
   * <code>PP_Var</code> will implicitly hold a reference count on the
   * <code>PP_Resource</code> on your behalf. For example, if you call
   * VarFromResource(), it implicitly calls PPB_Core::AddRefResource() on the
   * <code>PP_Resource</code>. Likewise, PPB_Var::Release() on a Resource
   * <code>PP_Var</code> will invoke PPB_Core::ReleaseResource() when the Var
   * reference count goes to zero.
   */
  Resource = 10
}
/**
 * @}
 */

/**
 * @addtogroup Structs
 * @{
 */
/**
 * The PP_VarValue union stores the data for any one of the types listed
 * in the PP_VarType enum.
 */
/* Skipping generation of << union PP_VarValue >>. */
/**
 * The <code>PP_VAR</code> struct is a variant data type and can contain any
 * value of one of the types named in the <code>PP_VarType</code> enum. This
 * structure is for passing data between native code which can be strongly
 * typed and the browser (JavaScript) which isn't strongly typed.
 *
 * JavaScript has a "number" type for holding a number, and does not
 * differentiate between floating point and integer numbers. The
 * JavaScript operations will try to optimize operations by using
 * integers when possible, but could end up with doubles. Therefore,
 * you can't assume a numeric <code>PP_Var</code> will be the type you expect.
 * Your code should be capable of handling either int32_t or double for numeric
 * PP_Vars sent from JavaScript.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPVar {
  public PPVarType type;
  /**
   * The <code>padding</code> ensures <code>value</code> is aligned on an
   * 8-byte boundary relative to the start of the struct. Some compilers
   * align doubles on 8-byte boundaries for 32-bit x86, and some align on
   * 4-byte boundaries.
   */
  public int padding;
  /**
   * This <code>value</code> represents the contents of the PP_Var. Only one of
   * the fields of <code>value</code> is valid at a time based upon
   * <code>type</code>.
   */
  public PPVarValue value;
};
/**
 * @}
 */


}
