/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_udp_socket.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_UDPSocket</code> interface.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * Option names used by <code>SetOption()</code>.
 */
public enum PPUDPSocketOption {
  /**
   * Allows the socket to share the local address to which it will be bound with
   * other processes. Value's type should be <code>PP_VARTYPE_BOOL</code>.
   * This option can only be set before calling <code>Bind()</code>.
   */
  AddressReuse = 0,
  /**
   * Allows sending and receiving packets to and from broadcast addresses.
   * Value's type should be <code>PP_VARTYPE_BOOL</code>.
   * On version 1.0, this option can only be set before calling
   * <code>Bind()</code>. On version 1.1 or later, there is no such limitation.
   */
  Broadcast = 1,
  /**
   * Specifies the total per-socket buffer space reserved for sends. Value's
   * type should be <code>PP_VARTYPE_INT32</code>.
   * On version 1.0, this option can only be set after a successful
   * <code>Bind()</code> call. On version 1.1 or later, there is no such
   * limitation.
   *
   * Note: This is only treated as a hint for the browser to set the buffer
   * size. Even if <code>SetOption()</code> succeeds, the browser doesn't
   * guarantee it will conform to the size.
   */
  SendBufferSize = 2,
  /**
   * Specifies the total per-socket buffer space reserved for receives. Value's
   * type should be <code>PP_VARTYPE_INT32</code>.
   * On version 1.0, this option can only be set after a successful
   * <code>Bind()</code> call. On version 1.1 or later, there is no such
   * limitation.
   *
   * Note: This is only treated as a hint for the browser to set the buffer
   * size. Even if <code>SetOption()</code> succeeds, the browser doesn't
   * guarantee it will conform to the size.
   */
  RecvBufferSize = 3,
  /**
   * Specifies whether the packets sent from the host to the multicast group
   * should be looped back to the host or not. Value's type should be
   * <code>PP_VARTYPE_BOOL</code>.
   * This option can only be set before calling <code>Bind()</code>.
   *
   * This is only supported in version 1.2 of the API (Chrome 43) and later.
   */
  MulticastLoop = 4,
  /**
   * Specifies the time-to-live for packets sent to the multicast group. The
   * value should be within 0 to 255 range. The default value is 1 and means
   * that packets will not be routed beyond the local network. Value's type
   * should be <code>PP_VARTYPE_INT32</code>.
   * This option can only be set before calling <code>Bind()</code>.
   *
   * This is only supported in version 1.2 of the API (Chrome 43) and later.
   */
  MulticastTtl = 5
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_UDPSocket</code> interface provides UDP socket operations.
 *
 * Permissions: Apps permission <code>socket</code> with subrule
 * <code>udp-bind</code> is required for <code>Bind()</code>; subrule
 * <code>udp-send-to</code> is required for <code>SendTo()</code>.
 * For more details about network communication permissions, please see:
 * http://developer.chrome.com/apps/app_network.html
 */
internal static partial class PPBUDPSocket {
  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_Create")]
  extern static PPResource _Create ( PPInstance instance);

  /**
   * Creates a UDP socket resource.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance of
   * a module.
   *
   * @return A <code>PP_Resource</code> corresponding to a UDP socket or 0
   * on failure.
   */
  public static PPResource Create ( PPInstance instance)
  {
  	return _Create (instance);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_IsUDPSocket")]
  extern static PPBool _IsUDPSocket ( PPResource resource);

  /**
   * Determines if a given resource is a UDP socket.
   *
   * @param[in] resource A <code>PP_Resource</code> to check.
   *
   * @return <code>PP_TRUE</code> if the input is a <code>PPB_UDPSocket</code>
   * resource; <code>PP_FALSE</code> otherwise.
   */
  public static PPBool IsUDPSocket ( PPResource resource)
  {
  	return _IsUDPSocket (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_Bind")]
  extern static int _Bind ( PPResource udp_socket,
                            PPResource addr,
                            PPCompletionCallback callback);

  /**
   * Binds the socket to the given address.
   *
   * @param[in] udp_socket A <code>PP_Resource</code> corresponding to a UDP
   * socket.
   * @param[in] addr A <code>PPB_NetAddress</code> resource.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * <code>PP_ERROR_NOACCESS</code> will be returned if the caller doesn't have
   * required permissions. <code>PP_ERROR_ADDRESS_IN_USE</code> will be returned
   * if the address is already in use.
   */
  public static int Bind ( PPResource udp_socket,
                           PPResource addr,
                           PPCompletionCallback callback)
  {
  	return _Bind (udp_socket, addr, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_GetBoundAddress")]
  extern static PPResource _GetBoundAddress ( PPResource udp_socket);

  /**
   * Gets the address that the socket is bound to. The socket must be bound.
   *
   * @param[in] udp_socket A <code>PP_Resource</code> corresponding to a UDP
   * socket.
   *
   * @return A <code>PPB_NetAddress</code> resource on success or 0 on failure.
   */
  public static PPResource GetBoundAddress ( PPResource udp_socket)
  {
  	return _GetBoundAddress (udp_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_RecvFrom")]
  extern static int _RecvFrom ( PPResource udp_socket,
                               IntPtr buffer,
                                int num_bytes,
                               out PPResource addr,
                                PPCompletionCallback callback);

  /**
   * Receives data from the socket and stores the source address. The socket
   * must be bound.
   *
   * @param[in] udp_socket A <code>PP_Resource</code> corresponding to a UDP
   * socket.
   * @param[out] buffer The buffer to store the received data on success. It
   * must be at least as large as <code>num_bytes</code>.
   * @param[in] num_bytes The number of bytes to receive.
   * @param[out] addr A <code>PPB_NetAddress</code> resource to store the source
   * address on success.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return A non-negative number on success to indicate how many bytes have
   * been received; otherwise, an error code from <code>pp_errors.h</code>.
   */
  public static int RecvFrom ( PPResource udp_socket,
                              byte[] buffer,
                               int num_bytes,
                              out PPResource addr,
                               PPCompletionCallback callback)
  {
  	if (buffer == null)
  		throw new ArgumentNullException ("buffer");

  	unsafe
  	{
  		fixed (byte* buffer_ = &buffer[0])
  		{
  			return _RecvFrom (udp_socket, (IntPtr) buffer_,
                                    num_bytes,
                                    out addr,
                                    callback);
  		}
  	}
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_SendTo")]
  extern static int _SendTo ( PPResource udp_socket,
                             IntPtr buffer,
                              int num_bytes,
                              PPResource addr,
                              PPCompletionCallback callback);

  /**
   * Sends data to a specific destination. The socket must be bound.
   *
   * @param[in] udp_socket A <code>PP_Resource</code> corresponding to a UDP
   * socket.
   * @param[in] buffer The buffer containing the data to send.
   * @param[in] num_bytes The number of bytes to send.
   * @param[in] addr A <code>PPB_NetAddress</code> resource holding the
   * destination address.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return A non-negative number on success to indicate how many bytes have
   * been sent; otherwise, an error code from <code>pp_errors.h</code>.
   * <code>PP_ERROR_NOACCESS</code> will be returned if the caller doesn't have
   * required permissions.
   * <code>PP_ERROR_INPROGRESS</code> will be returned if the socket is busy
   * sending. The caller should wait until a pending send completes before
   * retrying.
   */
  public static int SendTo ( PPResource udp_socket,
                            byte[] buffer,
                             int num_bytes,
                             PPResource addr,
                             PPCompletionCallback callback)
  {
  	if (buffer == null)
  		throw new ArgumentNullException ("buffer");

  	unsafe
  	{
  		fixed (byte* buffer_ = &buffer[0])
  		{
  			return _SendTo (udp_socket, (IntPtr) buffer_, num_bytes, addr, callback);
  		}
  	}
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_Close")]
  extern static void _Close ( PPResource udp_socket);

  /**
   * Cancels all pending reads and writes, and closes the socket. Any pending
   * callbacks will still run, reporting <code>PP_ERROR_ABORTED</code> if
   * pending IO was interrupted. After a call to this method, no output
   * parameters passed into previous <code>RecvFrom()</code> calls will be
   * accessed. It is not valid to call <code>Bind()</code> again.
   *
   * The socket is implicitly closed if it is destroyed, so you are not
   * required to call this method.
   *
   * @param[in] udp_socket A <code>PP_Resource</code> corresponding to a UDP
   * socket.
   */
  public static void Close ( PPResource udp_socket)
  {
  	 _Close (udp_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_SetOption")]
  extern static int _SetOption ( PPResource udp_socket,
                                 PPUDPSocketOption name,
                                 PPVar value,
                                 PPCompletionCallback callback);

  /**
   * Sets a socket option on the UDP socket.
   * Please see the <code>PP_UDPSocket_Option</code> description for option
   * names, value types and allowed values.
   *
   * @param[in] udp_socket A <code>PP_Resource</code> corresponding to a UDP
   * socket.
   * @param[in] name The option to set.
   * @param[in] value The option value to set.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int SetOption ( PPResource udp_socket,
                                PPUDPSocketOption name,
                                PPVar value,
                                PPCompletionCallback callback)
  {
  	return _SetOption (udp_socket, name, value, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_JoinGroup")]
  extern static int _JoinGroup ( PPResource udp_socket,
                                 PPResource group,
                                 PPCompletionCallback callback);

  /**
   * Joins the multicast group with address specified by <code>group</code>
   * parameter, which is expected to be a <code>PPB_NetAddress</code> object.
   *
   * @param[in] udp_socket A <code>PP_Resource</code> corresponding to a UDP
   * socket.
   * @param[in] group A <code>PP_Resource</code> corresponding to the network
   * address of the multicast group.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int JoinGroup ( PPResource udp_socket,
                                PPResource group,
                                PPCompletionCallback callback)
  {
  	return _JoinGroup (udp_socket, group, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_UDPSocket_LeaveGroup")]
  extern static int _LeaveGroup ( PPResource udp_socket,
                                  PPResource group,
                                  PPCompletionCallback callback);

  /**
   * Leaves the multicast group with address specified by <code>group</code>
   * parameter, which is expected to be a <code>PPB_NetAddress</code> object.
   *
   * @param[in] udp_socket A <code>PP_Resource</code> corresponding to a UDP
   * socket.
   * @param[in] group A <code>PP_Resource</code> corresponding to the network
   * address of the multicast group.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int LeaveGroup ( PPResource udp_socket,
                                 PPResource group,
                                 PPCompletionCallback callback)
  {
  	return _LeaveGroup (udp_socket, group, callback);
  }


}
/**
 * @}
 */


}
