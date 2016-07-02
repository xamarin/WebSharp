/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_errors.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines an enumeration of all PPAPI error codes.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * This enumeration contains enumerators of all PPAPI error codes.
 *
 * Errors are negative valued. Callers should treat all negative values as a
 * failure, even if it's not in the list, since the possible errors are likely
 * to expand and change over time.
 */
public enum PPError {
  /**
   * This value is returned by a function on successful synchronous completion
   * or is passed as a result to a PP_CompletionCallback_Func on successful
   * asynchronous completion.
   */
  Ok = 0,
  /**
   * This value is returned by a function that accepts a PP_CompletionCallback
   * and cannot complete synchronously. This code indicates that the given
   * callback will be asynchronously notified of the final result once it is
   * available.
   */
  OkCompletionpending = -1,
  /**This value indicates failure for unspecified reasons. */
  Failed = -2,
  /**
   * This value indicates failure due to an asynchronous operation being
   * interrupted. The most common cause of this error code is destroying a
   * resource that still has a callback pending. All callbacks are guaranteed
   * to execute, so any callbacks pending on a destroyed resource will be
   * issued with PP_ERROR_ABORTED.
   *
   * If you get an aborted notification that you aren't expecting, check to
   * make sure that the resource you're using is still in scope. A common
   * mistake is to create a resource on the stack, which will destroy the
   * resource as soon as the function returns.
   */
  Aborted = -3,
  /** This value indicates failure due to an invalid argument. */
  Badargument = -4,
  /** This value indicates failure due to an invalid PP_Resource. */
  Badresource = -5,
  /** This value indicates failure due to an unavailable PPAPI interface. */
  Nointerface = -6,
  /** This value indicates failure due to insufficient privileges. */
  Noaccess = -7,
  /** This value indicates failure due to insufficient memory. */
  Nomemory = -8,
  /** This value indicates failure due to insufficient storage space. */
  Nospace = -9,
  /** This value indicates failure due to insufficient storage quota. */
  Noquota = -10,
  /**
   * This value indicates failure due to an action already being in
   * progress.
   */
  Inprogress = -11,
  /**
   * The requested command is not supported by the browser.
   */
  Notsupported = -12,
  /**
   * Returned if you try to use a null completion callback to "block until
   * complete" on the main thread. Blocking the main thread is not permitted
   * to keep the browser responsive (otherwise, you may not be able to handle
   * input events, and there are reentrancy and deadlock issues).
   */
  BlocksMainThread = -13,
  /**
   * This value indicates that the plugin sent bad input data to a resource,
   * leaving it in an invalid state. The resource can't be used after returning
   * this error and should be released.
   */
  MalformedInput = -14,
  /**
   * This value indicates that a resource has failed.  The resource can't be
   * used after returning this error and should be released.
   */
  ResourceFailed = -15,
  /** This value indicates failure due to a file that does not exist. */
  Filenotfound = -20,
  /** This value indicates failure due to a file that already exists. */
  Fileexists = -21,
  /** This value indicates failure due to a file that is too big. */
  Filetoobig = -22,
  /**
   * This value indicates failure due to a file having been modified
   * unexpectedly.
   */
  Filechanged = -23,
  /** This value indicates that the pathname does not reference a file. */
  Notafile = -24,
  /** This value indicates failure due to a time limit being exceeded. */
  Timedout = -30,
  /**
   * This value indicates that the user cancelled rather than providing
   * expected input.
   */
  Usercancel = -40,
  /**
   * This value indicates failure due to lack of a user gesture such as a
   * mouse click or key input event. Examples of actions requiring a user
   * gesture are showing the file chooser dialog and going into fullscreen
   * mode.
   */
  NoUserGesture = -41,
  /**
   * This value indicates that the graphics context was lost due to a
   * power management event.
   */
  ContextLost = -50,
  /**
   * Indicates an attempt to make a PPAPI call on a thread without previously
   * registering a message loop via PPB_MessageLoop.AttachToCurrentThread.
   * Without this registration step, no PPAPI calls are supported.
   */
  NoMessageLoop = -51,
  /**
   * Indicates that the requested operation is not permitted on the current
   * thread.
   */
  WrongThread = -52,
  /**
   * Indicates that a null completion callback was used on a thread handling a
   * blocking message from JavaScript. Null completion callbacks "block until
   * complete", which could cause the main JavaScript thread to be blocked
   * excessively.
   */
  WouldBlockThread = -53,
  /**
   * This value indicates that the connection was closed. For TCP sockets, it
   * corresponds to a TCP FIN.
   */
  ConnectionClosed = -100,
  /**
   * This value indicates that the connection was reset. For TCP sockets, it
   * corresponds to a TCP RST.
   */
  ConnectionReset = -101,
  /**
   * This value indicates that the connection attempt was refused.
   */
  ConnectionRefused = -102,
  /**
   * This value indicates that the connection was aborted. For TCP sockets, it
   * means the connection timed out as a result of not receiving an ACK for data
   * sent. This can include a FIN packet that did not get ACK'd.
   */
  ConnectionAborted = -103,
  /**
   * This value indicates that the connection attempt failed.
   */
  ConnectionFailed = -104,
  /**
   * This value indicates that the connection attempt timed out.
   */
  ConnectionTimedout = -105,
  /**
   * This value indicates that the IP address or port number is invalid.
   */
  AddressInvalid = -106,
  /**
   * This value indicates that the IP address is unreachable. This usually means
   * that there is no route to the specified host or network.
   */
  AddressUnreachable = -107,
  /**
   * This value is returned when attempting to bind an address that is already
   * in use.
   */
  AddressInUse = -108,
  /**
   * This value indicates that the message was too large for the transport.
   */
  MessageTooBig = -109,
  /**
   * This value indicates that the host name could not be resolved.
   */
  NameNotResolved = -110
}
/**
 * @}
 */


}
