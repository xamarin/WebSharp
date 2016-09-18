/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_var_array.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_VarArray</code> struct providing
 * a way to interact with array vars.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
internal static partial class PPBVarArray {
  [DllImport("PepperPlugin", EntryPoint = "PPB_VarArray_Create")]
  extern static PPVar _Create ();

  /**
   * Creates an array var, i.e., a <code>PP_Var</code> with type set to
   * <code>PP_VARTYPE_ARRAY</code>. The array length is set to 0.
   *
   * @return An empty array var, whose reference count is set to 1 on behalf of
   * the caller.
   */
  public static PPVar Create ()
  {
  	return _Create ();
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarArray_Get")]
  extern static PPVar _Get ( PPVar array,  uint index);

  /**
   * Gets an element from the array.
   *
   * @param[in] array An array var.
   * @param[in] index An index indicating which element to return.
   *
   * @return The element at the specified position. The reference count of the
   * element returned is incremented on behalf of the caller. If
   * <code>index</code> is larger than or equal to the array length, an
   * undefined var is returned.
   */
  public static PPVar Get ( PPVar array,  uint index)
  {
  	return _Get (array, index);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarArray_Set")]
  extern static PPBool _Set ( PPVar array,  uint index,  PPVar value);

  /**
   * Sets the value of an element in the array.
   *
   * @param[in] array An array var.
   * @param[in] index An index indicating which element to modify. If
   * <code>index</code> is larger than or equal to the array length, the length
   * is updated to be <code>index</code> + 1. Any position in the array that
   * hasn't been set before is set to undefined, i.e., <code>PP_Var</code> of
   * type <code>PP_VARTYPE_UNDEFINED</code>.
   * @param[in] value The value to set. The array holds a reference to it on
   * success.
   *
   * @return A <code>PP_Bool</code> indicating whether the operation succeeds.
   */
  public static PPBool Set ( PPVar array,  uint index,  PPVar value)
  {
  	return _Set (array, index, value);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarArray_GetLength")]
  extern static uint _GetLength ( PPVar array);

  /**
   * Gets the array length.
   *
   * @param[in] array An array var.
   *
   * @return The array length.
   */
  public static uint GetLength ( PPVar array)
  {
  	return _GetLength (array);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarArray_SetLength")]
  extern static PPBool _SetLength ( PPVar array,  uint length);

  /**
   * Sets the array length.
   *
   * @param[in] array An array var.
   * @param[in] length The new array length. If <code>length</code> is smaller
   * than its current value, the array is truncated to the new length; any
   * elements that no longer fit are removed and the references to them will be
   * released. If <code>length</code> is larger than its current value,
   * undefined vars are appended to increase the array to the specified length.
   *
   * @return A <code>PP_Bool</code> indicating whether the operation succeeds.
   */
  public static PPBool SetLength ( PPVar array,  uint length)
  {
  	return _SetLength (array, length);
  }


}
/**
 * @}
 */


}
