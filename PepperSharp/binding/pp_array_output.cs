/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_array_output.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * PP_ArrayOutput_GetDataBuffer is a callback function to allocate plugin
 * memory for an array. It returns the allocated memory or null on failure.
 *
 * This function will be called reentrantly. This means that if you call a
 * function PPB_Foo.GetData(&array_output), GetData will call your
 * GetDataBuffer function before it returns.
 *
 * This function will be called even when returning 0-length arrays, so be sure
 * your implementation can support that. You can return NULL for 0 length
 * arrays and it will not be treated as a failure.
 *
 * You should not perform any processing in this callback, including calling
 * other PPAPI functions, outside of allocating memory. You should not throw
 * any exceptions. In C++, this means using "new (nothrow)" or being sure to
 * catch any exceptions before returning.
 *
 * The C++ wrapper provides a convenient templatized implementation around
 * std::vector which you should generally use instead of coding this
 * specifically.
 *
 * @param user_data The pointer provided in the PP_ArrayOutput structure. This
 * has no meaning to the browser, it is intended to be used by the
 * implementation to figure out where to put the data.
 *
 * @param element_count The number of elements in the array. This will be 0
 * if there is no data to return.
 *
 * @param element_size The size of each element in bytes.
 *
 * @return Returns a pointer to the allocated memory. On failure, returns null.
 * You can also return null if the element_count is 0. When a non-null value is
 * returned, the buffer must remain valid until after the callback runs. If used
 * with a blocking callback, the buffer must remain valid until after the
 * function returns. The plugin can then free any memory that it allocated.
 */


/**
 * @addtogroup Typedefs
 * @{
 */
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr PPArrayOutputGetDataBuffer (IntPtr user_data,
                                                   uint element_count,
                                                   uint element_size);

/**
 * @}
 */

/**
 * @addtogroup Structs
 * @{
 */
/**
 * A structure that defines a way for the browser to return arrays of data
 * to the plugin. The browser can not allocate memory on behalf of the plugin
 * because the plugin and browser may have different allocators.
 *
 * Array output works by having the browser call to the plugin to allocate a
 * buffer, and then the browser will copy the contents of the array into that
 * buffer.
 *
 * In C, you would typically implement this as follows:
 *
 * @code
 * struct MyArrayOutput {
 *   void* data;
 *   int element_count;
 * };
 * void* MyGetDataBuffer(void* user_data, uint32_t count, uint32_t size) {
 *   MyArrayOutput* output = (MyArrayOutput*)user_data;
 *   output->element_count = count;
 *   if (size) {
 *     output->data = malloc(count * size);
 *     if (!output->data)  // Be careful to set size properly on malloc failure.
 *       output->element_count = 0;
 *   } else {
 *     output->data = NULL;
 *   }
 *   return output->data;
 * }
 * void MyFunction() {
 *   MyArrayOutput array = { NULL, 0 };
 *   PP_ArrayOutput output = { &MyGetDataBuffer, &array };
 *   ppb_foo->GetData(&output);
 * }
 * @endcode
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPArrayOutput {
  /**
   * A pointer to the allocation function that the browser will call.
   */
  internal PPArrayOutputGetDataBuffer GetDataBuffer;
  /**
   * Data that is passed to the allocation function. Typically, this is used
   * to communicate how the data should be stored.
   */
  internal IntPtr user_data;
}
/**
 * @}
 */


}
