/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_tcp_socket.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_TCPSocket</code> interface.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * Option names used by <code>SetOption()</code>.
 */
public enum PPTCPSocketOption {
  /**
   * Disables coalescing of small writes to make TCP segments, and instead
   * delivers data immediately. Value's type is <code>PP_VARTYPE_BOOL</code>.
   * On version 1.1 or earlier, this option can only be set after a successful
   * <code>Connect()</code> call. On version 1.2 or later, there is no such
   * limitation.
   */
  NoDelay = 0,
  /**
   * Specifies the total per-socket buffer space reserved for sends. Value's
   * type should be <code>PP_VARTYPE_INT32</code>.
   * On version 1.1 or earlier, this option can only be set after a successful
   * <code>Connect()</code> call. On version 1.2 or later, there is no such
   * limitation.
   *
   * Note: This is only treated as a hint for the browser to set the buffer
   * size. Even if <code>SetOption()</code> succeeds, the browser doesn't
   * guarantee it will conform to the size.
   */
  SendBufferSize = 1,
  /**
   * Specifies the total per-socket buffer space reserved for receives. Value's
   * type should be <code>PP_VARTYPE_INT32</code>.
   * On version 1.1 or earlier, this option can only be set after a successful
   * <code>Connect()</code> call. On version 1.2 or later, there is no such
   * limitation.
   *
   * Note: This is only treated as a hint for the browser to set the buffer
   * size. Even if <code>SetOption()</code> succeeds, the browser doesn't
   * guarantee it will conform to the size.
   */
  RecvBufferSize = 2
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_TCPSocket</code> interface provides TCP socket operations.
 *
 * Permissions: Apps permission <code>socket</code> with subrule
 * <code>tcp-connect</code> is required for <code>Connect()</code>; subrule
 * <code>tcp-listen</code> is required for <code>Listen()</code>.
 * For more details about network communication permissions, please see:
 * http://developer.chrome.com/apps/app_network.html
 */
public static partial class PPBTCPSocket {
  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_Create")]
  extern static PPResource _Create ( PPInstance instance);

  /**
   * Creates a TCP socket resource.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance of
   * a module.
   *
   * @return A <code>PP_Resource</code> corresponding to a TCP socket or 0
   * on failure.
   */
  public static PPResource Create ( PPInstance instance)
  {
  	return _Create (instance);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_IsTCPSocket")]
  extern static PPBool _IsTCPSocket ( PPResource resource);

  /**
   * Determines if a given resource is a TCP socket.
   *
   * @param[in] resource A <code>PP_Resource</code> to check.
   *
   * @return <code>PP_TRUE</code> if the input is a
   * <code>PPB_TCPSocket</code> resource; <code>PP_FALSE</code> otherwise.
   */
  public static PPBool IsTCPSocket ( PPResource resource)
  {
  	return _IsTCPSocket (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_Bind")]
  extern static int _Bind ( PPResource tcp_socket,
                            PPResource addr,
                            PPCompletionCallback callback);

  /**
   * Binds the socket to the given address. The socket must not be bound.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   * @param[in] addr A <code>PPB_NetAddress</code> resource.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>,
   * including (but not limited to):
   * - <code>PP_ERROR_ADDRESS_IN_USE</code>: the address is already in use.
   * - <code>PP_ERROR_ADDRESS_INVALID</code>: the address is invalid.
   */
  public static int Bind ( PPResource tcp_socket,
                           PPResource addr,
                           PPCompletionCallback callback)
  {
  	return _Bind (tcp_socket, addr, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_Connect")]
  extern static int _Connect ( PPResource tcp_socket,
                               PPResource addr,
                               PPCompletionCallback callback);

  /**
   * Connects the socket to the given address. The socket must not be listening.
   * Binding the socket beforehand is optional.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   * @param[in] addr A <code>PPB_NetAddress</code> resource.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>,
   * including (but not limited to):
   * - <code>PP_ERROR_NOACCESS</code>: the caller doesn't have required
   *   permissions.
   * - <code>PP_ERROR_ADDRESS_UNREACHABLE</code>: <code>addr</code> is
   *   unreachable.
   * - <code>PP_ERROR_CONNECTION_REFUSED</code>: the connection attempt was
   *   refused.
   * - <code>PP_ERROR_CONNECTION_FAILED</code>: the connection attempt failed.
   * - <code>PP_ERROR_CONNECTION_TIMEDOUT</code>: the connection attempt timed
   *   out.
   *
   * Since version 1.1, if the socket is listening/connected or has a pending
   * listen/connect request, <code>Connect()</code> will fail without starting a
   * connection attempt; otherwise, any failure during the connection attempt
   * will cause the socket to be closed.
   */
  public static int Connect ( PPResource tcp_socket,
                              PPResource addr,
                              PPCompletionCallback callback)
  {
  	return _Connect (tcp_socket, addr, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_GetLocalAddress")]
  extern static PPResource _GetLocalAddress ( PPResource tcp_socket);

  /**
   * Gets the local address of the socket, if it is bound.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   *
   * @return A <code>PPB_NetAddress</code> resource on success or 0 on failure.
   */
  public static PPResource GetLocalAddress ( PPResource tcp_socket)
  {
  	return _GetLocalAddress (tcp_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_GetRemoteAddress")]
  extern static PPResource _GetRemoteAddress ( PPResource tcp_socket);

  /**
   * Gets the remote address of the socket, if it is connected.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   *
   * @return A <code>PPB_NetAddress</code> resource on success or 0 on failure.
   */
  public static PPResource GetRemoteAddress ( PPResource tcp_socket)
  {
  	return _GetRemoteAddress (tcp_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_Read")]
  extern static int _Read ( PPResource tcp_socket,
                           IntPtr buffer,
                            int bytes_to_read,
                            PPCompletionCallback callback);

  /**
   * Reads data from the socket. The socket must be connected. It may perform a
   * partial read.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   * @param[out] buffer The buffer to store the received data on success. It
   * must be at least as large as <code>bytes_to_read</code>.
   * @param[in] bytes_to_read The number of bytes to read.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return A non-negative number on success to indicate how many bytes have
   * been read, 0 means that end-of-file was reached; otherwise, an error code
   * from <code>pp_errors.h</code>.
   */
  public static int Read ( PPResource tcp_socket,
                          byte[] buffer,
                           int bytes_to_read,
                           PPCompletionCallback callback)
  {
  	if (buffer == null)
  		throw new ArgumentNullException ("buffer");

  	unsafe
  	{
  		fixed (byte* buffer_ = &buffer[0])
  		{
  			return _Read (tcp_socket, (IntPtr) buffer_, bytes_to_read, callback);
  		}
  	}
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_Write")]
  extern static int _Write ( PPResource tcp_socket,
                            IntPtr buffer,
                             int bytes_to_write,
                             PPCompletionCallback callback);

  /**
   * Writes data to the socket. The socket must be connected. It may perform a
   * partial write.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   * @param[in] buffer The buffer containing the data to write.
   * @param[in] bytes_to_write The number of bytes to write.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return A non-negative number on success to indicate how many bytes have
   * been written; otherwise, an error code from <code>pp_errors.h</code>.
   */
  public static int Write ( PPResource tcp_socket,
                           byte[] buffer,
                            int bytes_to_write,
                            PPCompletionCallback callback)
  {
  	if (buffer == null)
  		throw new ArgumentNullException ("buffer");

  	unsafe
  	{
  		fixed (byte* buffer_ = &buffer[0])
  		{
  			return _Write (tcp_socket, (IntPtr) buffer_, bytes_to_write, callback);
  		}
  	}
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_Listen")]
  extern static int _Listen ( PPResource tcp_socket,
                              int backlog,
                              PPCompletionCallback callback);

  /**
   * Starts listening. The socket must be bound and not connected.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   * @param[in] backlog A hint to determine the maximum length to which the
   * queue of pending connections may grow.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>,
   * including (but not limited to):
   * - <code>PP_ERROR_NOACCESS</code>: the caller doesn't have required
   *   permissions.
   * - <code>PP_ERROR_ADDRESS_IN_USE</code>: Another socket is already listening
   *   on the same port.
   */
  public static int Listen ( PPResource tcp_socket,
                             int backlog,
                             PPCompletionCallback callback)
  {
  	return _Listen (tcp_socket, backlog, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_Accept")]
  extern static int _Accept ( PPResource tcp_socket,
                             out PPResource accepted_tcp_socket,
                              PPCompletionCallback callback);

  /**
   * Accepts a connection. The socket must be listening.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   * @param[out] accepted_tcp_socket Stores the accepted TCP socket on success.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>,
   * including (but not limited to):
   * - <code>PP_ERROR_CONNECTION_ABORTED</code>: A connection has been aborted.
   */
  public static int Accept ( PPResource tcp_socket,
                            out PPResource accepted_tcp_socket,
                             PPCompletionCallback callback)
  {
  	return _Accept (tcp_socket, out accepted_tcp_socket, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_Close")]
  extern static void _Close ( PPResource tcp_socket);

  /**
   * Cancels all pending operations and closes the socket. Any pending callbacks
   * will still run, reporting <code>PP_ERROR_ABORTED</code> if pending IO was
   * interrupted. After a call to this method, no output buffer pointers passed
   * into previous <code>Read()</code> or <code>Accept()</code> calls will be
   * accessed. It is not valid to call <code>Connect()</code> or
   * <code>Listen()</code> again.
   *
   * The socket is implicitly closed if it is destroyed, so you are not required
   * to call this method.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   */
  public static void Close ( PPResource tcp_socket)
  {
  	 _Close (tcp_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TCPSocket_SetOption")]
  extern static int _SetOption ( PPResource tcp_socket,
                                 PPTCPSocketOption name,
                                 PPVar value,
                                 PPCompletionCallback callback);

  /**
   * Sets a socket option on the TCP socket.
   * Please see the <code>PP_TCPSocket_Option</code> description for option
   * names, value types and allowed values.
   *
   * @param[in] tcp_socket A <code>PP_Resource</code> corresponding to a TCP
   * socket.
   * @param[in] name The option to set.
   * @param[in] value The option value to set.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int SetOption ( PPResource tcp_socket,
                                PPTCPSocketOption name,
                                PPVar value,
                                PPCompletionCallback callback)
  {
  	return _SetOption (tcp_socket, name, value, callback);
  }


}
/**
 * @}
 */


}
