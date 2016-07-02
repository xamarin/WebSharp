/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_var.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_Var</code> struct.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * PPB_Var API
 */
public static partial class PPBVar {
  [DllImport("PepperPlugin", EntryPoint = "PPB_Var_AddRef")]
  extern static void _AddRef ( PP_Var var);

  /**
   * AddRef() adds a reference to the given var. If this is not a refcounted
   * object, this function will do nothing so you can always call it no matter
   * what the type.
   *
   * @param[in] var A <code>PP_Var</code> that will have a reference added.
   */
  public static void AddRef ( PP_Var var)
  {
  	 _AddRef (var);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Var_Release")]
  extern static void _Release ( PP_Var var);

  /**
   * Release() removes a reference to given var, deleting it if the internal
   * reference count becomes 0. If the <code>PP_Var</code> is of type
   * <code>PP_VARTYPE_RESOURCE</code>,
   * it will implicitly release a reference count on the
   * <code>PP_Resource</code> (equivalent to PPB_Core::ReleaseResource()).
   *
   * If the given var is not a refcounted object, this function will do nothing
   * so you can always call it no matter what the type.
   *
   * @param[in] var A <code>PP_Var</code> that will have a reference removed.
   */
  public static void Release ( PP_Var var)
  {
  	 _Release (var);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Var_VarFromUtf8")]
  extern static PP_Var _VarFromUtf8 ( string data,  uint len);

  /**
   * VarFromUtf8() creates a string var from a string. The string must be
   * encoded in valid UTF-8 and is NOT NULL-terminated, the length must be
   * specified in <code>len</code>. It is an error if the string is not
   * valid UTF-8.
   *
   * If the length is 0, the <code>*data</code> pointer will not be dereferenced
   * and may be <code>NULL</code>. Note, however if length is 0, the
   * "NULL-ness" will not be preserved, as VarToUtf8() will never return
   * <code>NULL</code> on success, even for empty strings.
   *
   * The resulting object will be a refcounted string object. It will be
   * AddRef'ed for the caller. When the caller is done with it, it should be
   * Released.
   *
   * On error (basically out of memory to allocate the string, or input that
   * is not valid UTF-8), this function will return a Null var.
   *
   * @param[in] data A string
   * @param[in] len The length of the string.
   *
   * @return A <code>PP_Var</code> structure containing a reference counted
   * string object.
   */
  public static PP_Var VarFromUtf8 ( string data,  uint len)
  {
  	return _VarFromUtf8 (data, len);
  }


  /* Not generating entry point methods for PPB_Var_VarToUtf8 */


  /**
   * VarToUtf8() converts a string-type var to a char* encoded in UTF-8. This
   * string is NOT NULL-terminated. The length will be placed in
   * <code>*len</code>. If the string is valid but empty the return value will
   * be non-NULL, but <code>*len</code> will still be 0.
   *
   * If the var is not a string, this function will return NULL and
   * <code>*len</code> will be 0.
   *
   * The returned buffer will be valid as long as the underlying var is alive.
   * If the instance frees its reference, the string will be freed and the
   * pointer will be to arbitrary memory.
   *
   * @param[in] var A PP_Var struct containing a string-type var.
   * @param[in,out] len A pointer to the length of the string-type var.
   *
   * @return A char* encoded in UTF-8.
   */
  /* Not generating entry point methods for PPB_Var_VarToUtf8 */


  [DllImport("PepperPlugin", EntryPoint = "PPB_Var_VarToResource")]
  extern static PP_Resource _VarToResource ( PP_Var var);

  /**
   * Converts a resource-type var to a <code>PP_Resource</code>.
   *
   * @param[in] var A <code>PP_Var</code> struct containing a resource-type var.
   *
   * @return A <code>PP_Resource</code> retrieved from the var, or 0 if the var
   * is not a resource. The reference count of the resource is incremented on
   * behalf of the caller.
   */
  public static PP_Resource VarToResource ( PP_Var var)
  {
  	return _VarToResource (var);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Var_VarFromResource")]
  extern static PP_Var _VarFromResource ( PP_Resource resource);

  /**
   * Creates a new <code>PP_Var</code> from a given resource. Implicitly adds a
   * reference count on the <code>PP_Resource</code> (equivalent to
   * PPB_Core::AddRefResource(resource)).
   *
   * @param[in] resource A <code>PP_Resource</code> to be wrapped in a var.
   *
   * @return A <code>PP_Var</code> created for this resource, with type
   * <code>PP_VARTYPE_RESOURCE</code>. The reference count of the var is set to
   * 1 on behalf of the caller.
   */
  public static PP_Var VarFromResource ( PP_Resource resource)
  {
  	return _VarFromResource (resource);
  }


}
/**
 * @}
 */


}
