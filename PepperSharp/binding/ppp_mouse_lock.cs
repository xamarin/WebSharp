/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppp_mouse_lock.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPP_MouseLock</code> interface containing a
 * function that you must implement to receive mouse lock events from the
 * browser.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPP_MouseLock</code> interface contains a function that you must
 * implement to receive mouse lock events from the browser.
 */
public static partial class PPPMouseLock {
  [DllImport("PepperPlugin", EntryPoint = "PPP_MouseLock_MouseLockLost")]
  extern static void _MouseLockLost ( PPInstance instance);

  /**
   * MouseLockLost() is called when the instance loses the mouse lock, such as
   * when the user presses the ESC key.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance
   * of a module.
   */
  public static void MouseLockLost ( PPInstance instance)
  {
  	 _MouseLockLost (instance);
  }


}
/**
 * @}
 */


}
