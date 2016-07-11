/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_completion_callback.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the API to create and run a callback.
 */


/**
 * @addtogroup Typedefs
 * @{
 */
/**
 * This typedef defines the signature that you implement to receive callbacks
 * on asynchronous completion of an operation.
 *
 * @param[in] user_data A pointer to user data passed to a callback function.
 * @param[in] result If result is 0 (PP_OK), the operation succeeded.  Negative
 * values (other than -1 or PP_OK_COMPLETE) indicate error and are specified
 * in pp_errors.h. Positive values for result usually indicate success and have
 * some operation-dependent meaning (such as bytes read).
 */
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void PPCompletionCallbackFunc (IntPtr user_data, int result);

/**
 * @}
 */

/**
 * @addtogroup Enums
 * @{
 */
/**
 * This enumeration contains flags used to control how non-NULL callbacks are
 * scheduled by asynchronous methods.
 */
public enum PPCompletionCallbackFlag {
  /**
   * By default any non-NULL callback will always invoked asynchronously,
   * on success or error, even if the operation could complete synchronously
   * without blocking.
   *
   * The method taking such callback will always return PP_OK_COMPLETIONPENDING.
   * The callback will be invoked on the same thread on which the method was
   * invoked.
   *
   * NOTE: If the method taking the callback is invoked on a background
   * thread that has no valid PPB_MessageLoop resource attached, the system has
   * no way to run the callback on the correct thread. In this case, a log
   * message will be emitted and the plugin will be made to crash.
   */
  None = 0<<0,
  /**
   * This flag allows any method taking such callback to complete synchronously
   * and not call the callback if the operation would not block. This is useful
   * when performance is an issue, and the operation bandwidth should not be
   * limited to the processing speed of the message loop.
   *
   * On synchronous method completion, the completion result will be returned
   * by the method itself. Otherwise, the method will return
   * PP_OK_COMPLETIONPENDING, and the callback will be invoked asynchronously on
   * the same thread on which the method was invoked. If there is no valid
   * PPB_MessageLoop attached to that thread, and the callback would normally
   * run asynchronously, the invoked method will return
   * PP_ERROR_NO_MESSAGE_LOOP.
   */
  Optional = 1<<0
}
/**
 * @}
 */

/**
 * @addtogroup Structs
 * @{
 */
/**
 * <code>PP_CompletionCallback</code> is a common mechanism for supporting
 * potentially asynchronous calls in browser interfaces. Any method that takes a
 * <code>PP_CompletionCallback</code> can be used in one of three different
 * ways:
 *   - Required: The callback will always be invoked asynchronously on the
 *               thread where the associated PPB method was invoked. The method
 *               will always return PP_OK_COMPLETIONPENDING when a required
 *               callback, and the callback will be invoked later (barring
 *               system or thread shutdown; see PPB_MessageLoop for details).
 *               Required callbacks are the default.
 *               <br /><br />
 *               NOTE: If you use a required callback on a background thread,
 *               you must have created and attached a PPB_MessageLoop.
 *               Otherwise, the system can not run your callback on that thread,
 *               and will instead emit a log message and crash your plugin to
 *               make the problem more obvious.
 *
 *   - Optional: The callback may be invoked asynchronously, or the PPB method
 *               may complete synchronously if it can do so without blocking.
 *               If the method will complete asynchronously, it will return
 *               PP_OK_COMPLETIONPENDING. Otherwise, it will complete
 *               synchronously and return an appropriate code (see below for
 *               more information on the return code). Optional callbacks are
 *               generally more difficult to use correctly than Required
 *               callbacks, but can provide better performance for some APIs
 *               (especially APIs with buffered reads, such as PPB_URLLoader or
 *               PPB_FileIO).
 *               <br /><br />
 *               NOTE: If you use an optional callback on a background thread,
 *               and you have not created and attached a PPB_MessageLoop, then
 *               the method you invoke will fail without running and return
 *               PP_ERROR_NO_MESSAGE_LOOP.
 *
 *   - Blocking: In this case, the callback's function pointer is NULL, and the
 *               invoked method must complete synchronously. The method will
 *               run to completion and return an appropriate code when finished
 *               (see below for more information). Blocking completion
 *               callbacks are only supported on background threads.
 *               <br /><br />
 *               <code>PP_BlockUntilComplete()</code> provides a convenient way
 *               to specify blocking behavior. Refer to
 *               <code>PP_BlockUntilComplete</code> for more information.
 *
 * When the callback is run asynchronously, the result parameter passed to
 * <code>func</code> is an int32_t that, if negative indicates an error code
 * whose meaning is specific to the calling method (refer to
 * <code>pp_error.h</code> for further information). A positive or 0 value is a
 * return result indicating success whose meaning depends on the calling method
 * (e.g. number of bytes read).
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPCompletionCallback {
  /**
   * This value is a callback function that will be called, or NULL if this is
   * a blocking completion callback.
   */
  public PPCompletionCallbackFunc func;
  /**
   * This value is a pointer to user data passed to a callback function.
   */
  public IntPtr user_data;
  /**
   * Flags used to control how non-NULL callbacks are scheduled by
   * asynchronous methods.
   */
  public int flags;
};
/**
 * @}
 */


}
