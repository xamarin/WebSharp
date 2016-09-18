/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_var_dictionary.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_VarDictionary</code> struct providing
 * a way to interact with dictionary vars.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * A dictionary var contains key-value pairs with unique keys. The keys are
 * strings while the values can be arbitrary vars. Key comparison is always
 * done by value instead of by reference.
 */
internal static partial class PPBVarDictionary {
  [DllImport("PepperPlugin", EntryPoint = "PPB_VarDictionary_Create")]
  extern static PPVar _Create ();

  /**
   * Creates a dictionary var, i.e., a <code>PP_Var</code> with type set to
   * <code>PP_VARTYPE_DICTIONARY</code>.
   *
   * @return An empty dictionary var, whose reference count is set to 1 on
   * behalf of the caller.
   */
  public static PPVar Create ()
  {
  	return _Create ();
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarDictionary_Get")]
  extern static PPVar _Get ( PPVar dict,  PPVar key);

  /**
   * Gets the value associated with the specified key.
   *
   * @param[in] dict A dictionary var.
   * @param[in] key A string var.
   *
   * @return The value that is associated with <code>key</code>. The reference
   * count of the element returned is incremented on behalf of the caller. If
   * <code>key</code> is not a string var, or it doesn't exist in
   * <code>dict</code>, an undefined var is returned.
   */
  public static PPVar Get ( PPVar dict,  PPVar key)
  {
  	return _Get (dict, key);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarDictionary_Set")]
  extern static PPBool _Set ( PPVar dict,  PPVar key,  PPVar value);

  /**
   * Sets the value associated with the specified key.
   *
   * @param[in] dict A dictionary var.
   * @param[in] key A string var. If this key hasn't existed in
   * <code>dict</code>, it is added and associated with <code>value</code>;
   * otherwise, the previous value is replaced with <code>value</code>.
   * @param[in] value The value to set. The dictionary holds a reference to it
   * on success.
   *
   * @return A <code>PP_Bool</code> indicating whether the operation succeeds.
   */
  public static PPBool Set ( PPVar dict,  PPVar key,  PPVar value)
  {
  	return _Set (dict, key, value);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarDictionary_Delete")]
  extern static void _Delete ( PPVar dict,  PPVar key);

  /**
   * Deletes the specified key and its associated value, if the key exists. The
   * reference to the element will be released.
   *
   * @param[in] dict A dictionary var.
   * @param[in] key A string var.
   */
  public static void Delete ( PPVar dict,  PPVar key)
  {
  	 _Delete (dict, key);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarDictionary_HasKey")]
  extern static PPBool _HasKey ( PPVar dict,  PPVar key);

  /**
   * Checks whether a key exists.
   *
   * @param[in] dict A dictionary var.
   * @param[in] key A string var.
   *
   * @return A <code>PP_Bool</code> indicating whether the key exists.
   */
  public static PPBool HasKey ( PPVar dict,  PPVar key)
  {
  	return _HasKey (dict, key);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarDictionary_GetKeys")]
  extern static PPVar _GetKeys ( PPVar dict);

  /**
   * Gets all the keys in a dictionary. Please note that for each key that you
   * set into the dictionary, a string var with the same contents is returned;
   * but it may not be the same string var (i.e., <code>value.as_id</code> may
   * be different).
   *
   * @param[in] dict A dictionary var.
   *
   * @return An array var which contains all the keys of <code>dict</code>. Its
   * reference count is incremented on behalf of the caller. The elements are
   * string vars. Returns a null var if failed.
   */
  public static PPVar GetKeys ( PPVar dict)
  {
  	return _GetKeys (dict);
  }


}
/**
 * @}
 */


}
