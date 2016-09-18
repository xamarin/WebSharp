/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_var_array_buffer.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_VarArrayBuffer</code> struct providing
 * a way to interact with JavaScript ArrayBuffers.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_VarArrayBuffer</code> interface provides a way to interact
 * with JavaScript ArrayBuffers, which represent a contiguous sequence of
 * bytes. Use <code>PPB_Var</code> to manage the reference count for a
 * <code>VarArrayBuffer</code>. Note that these Vars are not part of the
 * embedding page's DOM, and can only be shared with JavaScript using the
 * <code>PostMessage</code> and <code>HandleMessage</code> functions of
 * <code>pp::Instance</code>.
 */
internal static partial class PPBVarArrayBuffer {
  [DllImport("PepperPlugin", EntryPoint = "PPB_VarArrayBuffer_Create")]
  extern static PPVar _Create ( uint size_in_bytes);

  /**
   * Create() creates a zero-initialized <code>VarArrayBuffer</code>.
   *
   * @param[in] size_in_bytes The size of the <code>ArrayBuffer</code> to
   * be created.
   *
   * @return A <code>PP_Var</code> representing a <code>VarArrayBuffer</code>
   * of the requested size and with a reference count of 1.
   */
  public static PPVar Create ( uint size_in_bytes)
  {
  	return _Create (size_in_bytes);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarArrayBuffer_ByteLength")]
  extern static PPBool _ByteLength ( PPVar array, out uint byte_length);

  /**
   * ByteLength() retrieves the length of the <code>VarArrayBuffer</code> in
   * bytes. On success, <code>byte_length</code> is set to the length of the
   * given <code>ArrayBuffer</code> var. On failure, <code>byte_length</code>
   * is unchanged (this could happen, for instance, if the given
   * <code>PP_Var</code> is not of type <code>PP_VARTYPE_ARRAY_BUFFER</code>).
   * Note that ByteLength() will successfully retrieve the size of an
   * <code>ArrayBuffer</code> even if the <code>ArrayBuffer</code> is not
   * currently mapped.
   *
   * @param[in] array The <code>ArrayBuffer</code> whose length should be
   * returned.
   *
   * @param[out] byte_length A variable which is set to the length of the given
   * <code>ArrayBuffer</code> on success.
   *
   * @return <code>PP_TRUE</code> on success, <code>PP_FALSE</code> on failure.
   */
  public static PPBool ByteLength ( PPVar array, out uint byte_length)
  {
  	return _ByteLength (array, out byte_length);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarArrayBuffer_Map")]
  extern static IntPtr _Map ( PPVar array);

  /**
   * Map() maps the <code>ArrayBuffer</code> in to the module's address space
   * and returns a pointer to the beginning of the buffer for the given
   * <code>ArrayBuffer PP_Var</code>. ArrayBuffers are copied when transmitted,
   * so changes to the underlying memory are not automatically available to
   * the embedding page.
   *
   * Note that calling Map() can be a relatively expensive operation. Use care
   * when calling it in performance-critical code. For example, you should call
   * it only once when looping over an <code>ArrayBuffer</code>.
   *
   * <strong>Example:</strong>
   *
   * @code
   * char* data = (char*)(array_buffer_if.Map(array_buffer_var));
   * uint32_t byte_length = 0;
   * PP_Bool ok = array_buffer_if.ByteLength(array_buffer_var, &byte_length);
   * if (!ok)
   *   return DoSomethingBecauseMyVarIsNotAnArrayBuffer();
   * for (uint32_t i = 0; i < byte_length; ++i)
   *   data[i] = 'A';
   * @endcode
   *
   * @param[in] array The <code>ArrayBuffer</code> whose internal buffer should
   * be returned.
   *
   * @return A pointer to the internal buffer for this
   * <code>ArrayBuffer</code>. Returns <code>NULL</code>
   * if the given <code>PP_Var</code> is not of type
   * <code>PP_VARTYPE_ARRAY_BUFFER</code>.
   */
  public static IntPtr Map ( PPVar array)
  {
  	return _Map (array);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VarArrayBuffer_Unmap")]
  extern static void _Unmap ( PPVar array);

  /**
   * Unmap() unmaps the given <code>ArrayBuffer</code> var from the module
   * address space. Use this if you want to save memory but might want to call
   * Map() to map the buffer again later. The <code>PP_Var</code> remains valid
   * and should still be released using <code>PPB_Var</code> when you are done
   * with the <code>ArrayBuffer</code>.
   *
   * @param[in] array The <code>ArrayBuffer</code> to be released.
   */
  public static void Unmap ( PPVar array)
  {
  	 _Unmap (array);
  }


}
/**
 * @}
 */


}
