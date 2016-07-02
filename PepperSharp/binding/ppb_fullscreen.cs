/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_fullscreen.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_Fullscreen</code> interface for
 * handling transitions of a module instance to and from fullscreen mode.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_Fullscreen</code> interface is implemented by the browser.
 * This interface provides a way of checking the current screen mode and
 * toggling fullscreen mode.
 */
public static partial class PPBFullscreen {
  [DllImport("PepperPlugin", EntryPoint = "PPB_Fullscreen_IsFullscreen")]
  extern static PP_Bool _IsFullscreen ( PP_Instance instance);

  /**
   * IsFullscreen() checks whether the module instance is currently in
   * fullscreen mode.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance
   * of a module.
   *
   * @return <code>PP_TRUE</code> if the module instance is in fullscreen mode,
   * <code>PP_FALSE</code> if the module instance is not in fullscreen mode.
   */
  public static PP_Bool IsFullscreen ( PP_Instance instance)
  {
  	return _IsFullscreen (instance);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Fullscreen_SetFullscreen")]
  extern static PP_Bool _SetFullscreen ( PP_Instance instance,
                                         PP_Bool fullscreen);

  /**
   * SetFullscreen() switches the module instance to and from fullscreen
   * mode.
   *
   * The transition to and from fullscreen mode is asynchronous. During the
   * transition, IsFullscreen() will return the previous value and
   * no 2D or 3D device can be bound. The transition ends at DidChangeView()
   * when IsFullscreen() returns the new value. You might receive other
   * DidChangeView() calls while in transition.
   *
   * The transition to fullscreen mode can only occur while the browser is
   * processing a user gesture, even if <code>PP_TRUE</code> is returned.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance
   * of a module.
   * @param[in] fullscreen <code>PP_TRUE</code> to enter fullscreen mode, or
   * <code>PP_FALSE</code> to exit fullscreen mode.
   *
   * @return <code>PP_TRUE</code> on success or <code>PP_FALSE</code> on
   * failure.
   */
  public static PP_Bool SetFullscreen ( PP_Instance instance,
                                        PP_Bool fullscreen)
  {
  	return _SetFullscreen (instance, fullscreen);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Fullscreen_GetScreenSize")]
  extern static PP_Bool _GetScreenSize ( PP_Instance instance,
                                        out PP_Size size);

  /**
   * GetScreenSize() gets the size of the screen in pixels. The module instance
   * will be resized to this size when SetFullscreen() is called to enter
   * fullscreen mode.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance
   * of a module.
   * @param[out] size The size of the entire screen in pixels.
   *
   * @return <code>PP_TRUE</code> on success or <code>PP_FALSE</code> on
   * failure.
   */
  public static PP_Bool GetScreenSize ( PP_Instance instance, out PP_Size size)
  {
  	return _GetScreenSize (instance, out size);
  }


}
/**
 * @}
 */


}
