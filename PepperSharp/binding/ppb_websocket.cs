/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_websocket.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_WebSocket</code> interface providing
 * bi-directional, full-duplex, communications over a single TCP socket.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * This enumeration contains the types representing the WebSocket ready state
 * and these states are based on the JavaScript WebSocket API specification.
 * GetReadyState() returns one of these states.
 */
public enum PPWebSocketReadyState {
  /**
   * Ready state is queried on an invalid resource.
   */
  Invalid = -1,
  /**
   * Ready state that the connection has not yet been established.
   */
  Connecting = 0,
  /**
   * Ready state that the WebSocket connection is established and communication
   * is possible.
   */
  Open = 1,
  /**
   * Ready state that the connection is going through the closing handshake.
   */
  Closing = 2,
  /**
   * Ready state that the connection has been closed or could not be opened.
   */
  Closed = 3
}

/**
 * This enumeration contains status codes. These codes are used in Close() and
 * GetCloseCode(). Refer to RFC 6455, The WebSocket Protocol, for further
 * information.
 * <code>PP_WEBSOCKETSTATUSCODE_NORMAL_CLOSURE</code> and codes in the range
 * <code>PP_WEBSOCKETSTATUSCODE_USER_REGISTERED_MIN</code> to
 * <code>PP_WEBSOCKETSTATUSCODE_USER_REGISTERED_MAX</code>, and
 * <code>PP_WEBSOCKETSTATUSCODE_USER_PRIVATE_MIN</code> to
 * <code>PP_WEBSOCKETSTATUSCODE_USER_PRIVATE_MAX</code> are valid for Close().
 */
public enum PPWebSocketCloseCode {
  /**
   * Indicates to request closing connection without status code and reason.
   *
   * (Note that the code 1005 is forbidden to send in actual close frames by
   * the RFC. PP_WebSocket reuses this code internally and the code will never
   * appear in the actual close frames.)
   */
  WebsocketstatuscodeNotSpecified = 1005,
  /**
   * Status codes in the range 0-999 are not used.
   */
  /**
   * Indicates a normal closure.
   */
  WebsocketstatuscodeNormalClosure = 1000,
  /**
   * Indicates that an endpoint is "going away", such as a server going down.
   */
  WebsocketstatuscodeGoingAway = 1001,
  /**
   * Indicates that an endpoint is terminating the connection due to a protocol
   * error.
   */
  WebsocketstatuscodeProtocolError = 1002,
  /**
   * Indicates that an endpoint is terminating the connection because it has
   * received a type of data it cannot accept.
   */
  WebsocketstatuscodeUnsupportedData = 1003,
  /**
   * Status code 1004 is reserved.
   */
  /**
   * Pseudo code to indicate that receiving close frame doesn't contain any
   * status code.
   */
  WebsocketstatuscodeNoStatusReceived = 1005,
  /**
   * Pseudo code to indicate that connection was closed abnormally, e.g.,
   * without closing handshake.
   */
  WebsocketstatuscodeAbnormalClosure = 1006,
  /**
   * Indicates that an endpoint is terminating the connection because it has
   * received data within a message that was not consistent with the type of
   * the message (e.g., non-UTF-8 data within a text message).
   */
  WebsocketstatuscodeInvalidFramePayloadData = 1007,
  /**
   * Indicates that an endpoint is terminating the connection because it has
   * received a message that violates its policy.
   */
  WebsocketstatuscodePolicyViolation = 1008,
  /**
   * Indicates that an endpoint is terminating the connection because it has
   * received a message that is too big for it to process.
   */
  WebsocketstatuscodeMessageTooBig = 1009,
  /**
   * Indicates that an endpoint (client) is terminating the connection because
   * it has expected the server to negotiate one or more extension, but the
   * server didn't return them in the response message of the WebSocket
   * handshake.
   */
  WebsocketstatuscodeMandatoryExtension = 1010,
  /**
   * Indicates that a server is terminating the connection because it
   * encountered an unexpected condition.
   */
  WebsocketstatuscodeInternalServerError = 1011,
  /**
   * Status codes in the range 1012-1014 are reserved.
   */
  /**
   * Pseudo code to indicate that the connection was closed due to a failure to
   * perform a TLS handshake.
   */
  WebsocketstatuscodeTlsHandshake = 1015,
  /**
   * Status codes in the range 1016-2999 are reserved.
   */
  /**
   * Status codes in the range 3000-3999 are reserved for use by libraries,
   * frameworks, and applications. These codes are registered directly with
   * IANA.
   */
  WebsocketstatuscodeUserRegisteredMin = 3000,
  WebsocketstatuscodeUserRegisteredMax = 3999,
  /**
   * Status codes in the range 4000-4999 are reserved for private use.
   * Application can use these codes for application specific purposes freely.
   */
  WebsocketstatuscodeUserPrivateMin = 4000,
  WebsocketstatuscodeUserPrivateMax = 4999
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_WebSocket</code> interface provides bi-directional,
 * full-duplex, communications over a single TCP socket.
 */
internal static partial class PPBWebSocket {
  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_Create")]
  extern static PPResource _Create ( PPInstance instance);

  /**
   * Create() creates a WebSocket instance.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying the instance
   * with the WebSocket.
   *
   * @return A <code>PP_Resource</code> corresponding to a WebSocket if
   * successful.
   */
  public static PPResource Create ( PPInstance instance)
  {
  	return _Create (instance);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_IsWebSocket")]
  extern static PPBool _IsWebSocket ( PPResource resource);

  /**
   * IsWebSocket() determines if the provided <code>resource</code> is a
   * WebSocket instance.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @return Returns <code>PP_TRUE</code> if <code>resource</code> is a
   * <code>PPB_WebSocket</code>, <code>PP_FALSE</code> if the
   * <code>resource</code> is invalid or some type other than
   * <code>PPB_WebSocket</code>.
   */
  public static PPBool IsWebSocket ( PPResource resource)
  {
  	return _IsWebSocket (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_Connect")]
  extern static int _Connect ( PPResource web_socket,
                               PPVar url,
                               PPVar[] protocols,
                               uint protocol_count,
                               PPCompletionCallback callback);

  /**
   * Connect() connects to the specified WebSocket server. You can call this
   * function once for a <code>web_socket</code>.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @param[in] url A <code>PP_Var</code> representing a WebSocket server URL.
   * The <code>PP_VarType</code> must be <code>PP_VARTYPE_STRING</code>.
   *
   * @param[in] protocols A pointer to an array of <code>PP_Var</code>
   * specifying sub-protocols. Each <code>PP_Var</code> represents one
   * sub-protocol and its <code>PP_VarType</code> must be
   * <code>PP_VARTYPE_STRING</code>. This argument can be null only if
   * <code>protocol_count</code> is 0.
   *
   * @param[in] protocol_count The number of sub-protocols in
   * <code>protocols</code>.
   *
   * @param[in] callback A <code>PP_CompletionCallback</code> called
   * when a connection is established or an error occurs in establishing
   * connection.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns <code>PP_ERROR_BADARGUMENT</code> if the specified
   * <code>url</code>, or <code>protocols</code> contain an invalid string as
   * defined in the WebSocket API specification.
   * <code>PP_ERROR_BADARGUMENT</code> corresponds to a SyntaxError in the
   * WebSocket API specification.
   * Returns <code>PP_ERROR_NOACCESS</code> if the protocol specified in the
   * <code>url</code> is not a secure protocol, but the origin of the caller
   * has a secure scheme. Also returns <code>PP_ERROR_NOACCESS</code> if the
   * port specified in the <code>url</code> is a port that the user agent
   * is configured to block access to because it is a well-known port like
   * SMTP. <code>PP_ERROR_NOACCESS</code> corresponds to a SecurityError of the
   * specification.
   * Returns <code>PP_ERROR_INPROGRESS</code> if this is not the first call to
   * Connect().
   */
  public static int Connect ( PPResource web_socket,
                              PPVar url,
                              PPVar[] protocols,
                              uint protocol_count,
                              PPCompletionCallback callback)
  {
  	return _Connect (web_socket, url, protocols, protocol_count, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_Close")]
  extern static int _Close ( PPResource web_socket,
                             ushort code,
                             PPVar reason,
                             PPCompletionCallback callback);

  /**
   * Close() closes the specified WebSocket connection by specifying
   * <code>code</code> and <code>reason</code>.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @param[in] code The WebSocket close code. This is ignored if it is
   * <code>PP_WEBSOCKETSTATUSCODE_NOT_SPECIFIED</code>.
   * <code>PP_WEBSOCKETSTATUSCODE_NORMAL_CLOSURE</code> must be used for the
   * usual case. To indicate some specific error cases, codes in the range
   * <code>PP_WEBSOCKETSTATUSCODE_USER_REGISTERED_MIN</code> to
   * <code>PP_WEBSOCKETSTATUSCODE_USER_REGISTERED_MAX</code>, and in the range
   * <code>PP_WEBSOCKETSTATUSCODE_USER_PRIVATE_MIN</code> to
   * <code>PP_WEBSOCKETSTATUSCODE_USER_PRIVATE_MAX</code> are available.
   *
   * @param[in] reason A <code>PP_Var</code> representing the WebSocket
   * close reason. This is ignored if it is <code>PP_VARTYPE_UNDEFINED</code>.
   * Otherwise, its <code>PP_VarType</code> must be
   * <code>PP_VARTYPE_STRING</code>.
   *
   * @param[in] callback A <code>PP_CompletionCallback</code> called
   * when the connection is closed or an error occurs in closing the
   * connection.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns <code>PP_ERROR_BADARGUMENT</code> if <code>reason</code> contains
   * an invalid character as a UTF-8 string, or is longer than 123 bytes.
   * <code>PP_ERROR_BADARGUMENT</code> corresponds to a JavaScript SyntaxError
   * in the WebSocket API specification.
   * Returns <code>PP_ERROR_NOACCESS</code> if the code is not an integer
   * equal to 1000 or in the range 3000 to 4999. <code>PP_ERROR_NOACCESS</code>
   * corresponds to an InvalidAccessError in the WebSocket API specification.
   * Returns <code>PP_ERROR_INPROGRESS</code> if a previous call to Close() is
   * not finished.
   */
  public static int Close ( PPResource web_socket,
                            ushort code,
                            PPVar reason,
                            PPCompletionCallback callback)
  {
  	return _Close (web_socket, code, reason, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_ReceiveMessage")]
  extern static int _ReceiveMessage ( PPResource web_socket,
                                     out PPVar message,
                                      PPCompletionCallback callback);

  /**
   * ReceiveMessage() receives a message from the WebSocket server.
   * This interface only returns a single message. That is, this interface must
   * be called at least N times to receive N messages, no matter the size of
   * each message.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @param[out] message The received message is copied to provided
   * <code>message</code>. The <code>message</code> must remain valid until
   * ReceiveMessage() completes. Its received <code>PP_VarType</code> will be
   * <code>PP_VARTYPE_STRING</code> or <code>PP_VARTYPE_ARRAY_BUFFER</code>.
   *
   * @param[in] callback A <code>PP_CompletionCallback</code> called
   * when ReceiveMessage() completes. This callback is ignored if
   * ReceiveMessage() completes synchronously and returns <code>PP_OK</code>.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * If an error is detected or connection is closed, ReceiveMessage() returns
   * <code>PP_ERROR_FAILED</code> after all buffered messages are received.
   * Until buffered message become empty, ReceiveMessage() continues to return
   * <code>PP_OK</code> as if connection is still established without errors.
   */
  public static int ReceiveMessage ( PPResource web_socket,
                                    out PPVar message,
                                     PPCompletionCallback callback)
  {
  	return _ReceiveMessage (web_socket, out message, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_SendMessage")]
  extern static int _SendMessage ( PPResource web_socket,  PPVar message);

  /**
   * SendMessage() sends a message to the WebSocket server.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @param[in] message A message to send. The message is copied to an internal
   * buffer, so the caller can free <code>message</code> safely after returning
   * from the function. Its sent <code>PP_VarType</code> must be
   * <code>PP_VARTYPE_STRING</code> or <code>PP_VARTYPE_ARRAY_BUFFER</code>.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns <code>PP_ERROR_FAILED</code> if the ReadyState is
   * <code>PP_WEBSOCKETREADYSTATE_CONNECTING</code>.
   * <code>PP_ERROR_FAILED</code> corresponds to a JavaScript
   * InvalidStateError in the WebSocket API specification.
   * Returns <code>PP_ERROR_BADARGUMENT</code> if the provided
   * <code>message</code> contains an invalid character as a UTF-8 string.
   * <code>PP_ERROR_BADARGUMENT</code> corresponds to a JavaScript
   * SyntaxError in the WebSocket API specification.
   * Otherwise, returns <code>PP_OK</code>, which doesn't necessarily mean
   * that the server received the message.
   */
  public static int SendMessage ( PPResource web_socket,  PPVar message)
  {
  	return _SendMessage (web_socket, message);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_GetBufferedAmount")]
  extern static ulong _GetBufferedAmount ( PPResource web_socket);

  /**
   * GetBufferedAmount() returns the number of bytes of text and binary
   * messages that have been queued for the WebSocket connection to send, but
   * have not been transmitted to the network yet.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @return Returns the number of bytes.
   */
  public static ulong GetBufferedAmount ( PPResource web_socket)
  {
  	return _GetBufferedAmount (web_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_GetCloseCode")]
  extern static ushort _GetCloseCode ( PPResource web_socket);

  /**
   * GetCloseCode() returns the connection close code for the WebSocket
   * connection.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @return Returns 0 if called before the close code is set.
   */
  public static ushort GetCloseCode ( PPResource web_socket)
  {
  	return _GetCloseCode (web_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_GetCloseReason")]
  extern static PPVar _GetCloseReason ( PPResource web_socket);

  /**
   * GetCloseReason() returns the connection close reason for the WebSocket
   * connection.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @return Returns a <code>PP_VARTYPE_STRING</code> var. If called before the
   * close reason is set, the return value contains an empty string. Returns a
   * <code>PP_VARTYPE_UNDEFINED</code> if called on an invalid resource.
   */
  public static PPVar GetCloseReason ( PPResource web_socket)
  {
  	return _GetCloseReason (web_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_GetCloseWasClean")]
  extern static PPBool _GetCloseWasClean ( PPResource web_socket);

  /**
   * GetCloseWasClean() returns if the connection was closed cleanly for the
   * specified WebSocket connection.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @return Returns <code>PP_FALSE</code> if called before the connection is
   * closed, called on an invalid resource, or closed for abnormal reasons.
   * Otherwise, returns <code>PP_TRUE</code> if the connection was closed
   * cleanly.
   */
  public static PPBool GetCloseWasClean ( PPResource web_socket)
  {
  	return _GetCloseWasClean (web_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_GetExtensions")]
  extern static PPVar _GetExtensions ( PPResource web_socket);

  /**
   * GetExtensions() returns the extensions selected by the server for the
   * specified WebSocket connection.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @return Returns a <code>PP_VARTYPE_STRING</code> var. If called before the
   * connection is established, the var's data is an empty string. Returns a
   * <code>PP_VARTYPE_UNDEFINED</code> if called on an invalid resource.
   */
  public static PPVar GetExtensions ( PPResource web_socket)
  {
  	return _GetExtensions (web_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_GetProtocol")]
  extern static PPVar _GetProtocol ( PPResource web_socket);

  /**
   * GetProtocol() returns the sub-protocol chosen by the server for the
   * specified WebSocket connection.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @return Returns a <code>PP_VARTYPE_STRING</code> var. If called before the
   * connection is established, the var contains the empty string. Returns a
   * <code>PP_VARTYPE_UNDEFINED</code> if called on an invalid resource.
   */
  public static PPVar GetProtocol ( PPResource web_socket)
  {
  	return _GetProtocol (web_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_GetReadyState")]
  extern static PPWebSocketReadyState _GetReadyState ( PPResource web_socket);

  /**
   * GetReadyState() returns the ready state of the specified WebSocket
   * connection.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @return Returns <code>PP_WEBSOCKETREADYSTATE_INVALID</code> if called
   * before Connect() is called, or if this function is called on an
   * invalid resource.
   */
  public static PPWebSocketReadyState GetReadyState ( PPResource web_socket)
  {
  	return _GetReadyState (web_socket);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WebSocket_GetURL")]
  extern static PPVar _GetURL ( PPResource web_socket);

  /**
   * GetURL() returns the URL associated with specified WebSocket connection.
   *
   * @param[in] web_socket A <code>PP_Resource</code> corresponding to a
   * WebSocket.
   *
   * @return Returns a <code>PP_VARTYPE_STRING</code> var. If called before the
   * connection is established, the var contains the empty string. Returns a
   * <code>PP_VARTYPE_UNDEFINED</code> if this function is called on an
   * invalid resource.
   */
  public static PPVar GetURL ( PPResource web_socket)
  {
  	return _GetURL (web_socket);
  }


}
/**
 * @}
 */


}
