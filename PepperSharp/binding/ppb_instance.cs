/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_instance.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_Instance</code> interface implemented by the
 * browser and containing pointers to functions related to
 * the module instance on a web page.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The PPB_Instance interface contains pointers to functions
 * related to the module instance on a web page.
 */
public static partial class PPBInstance {
  [DllImport("PepperPlugin", EntryPoint = "PPB_Instance_BindGraphics")]
  extern static PPBool _BindGraphics ( PP_Instance instance,
                                       PP_Resource device);

  /**
   * BindGraphics() binds the given graphics as the current display surface.
   * The contents of this device is what will be displayed in the instance's
   * area on the web page. The device must be a 2D or a 3D device.
   *
   * You can pass a <code>NULL</code> resource as the device parameter to
   * unbind all devices from the given instance. The instance will then appear
   * transparent. Re-binding the same device will return <code>PP_TRUE</code>
   * and will do nothing.
   *
   * Any previously-bound device will be released. It is an error to bind
   * a device when it is already bound to another instance. If you want
   * to move a device between instances, first unbind it from the old one, and
   * then rebind it to the new one.
   *
   * Binding a device will invalidate that portion of the web page to flush the
   * contents of the new device to the screen.
   *
   * @param[in] instance A PP_Instance identifying one instance of a module.
   * @param[in] device A PP_Resource corresponding to a graphics device.
   *
   * @return <code>PP_Bool</code> containing <code>PP_TRUE</code> if bind was
   * successful or <code>PP_FALSE</code> if the device was not the correct
   * type. On success, a reference to the device will be held by the
   * instance, so the caller can release its reference if it chooses.
   */
  public static PPBool BindGraphics ( PP_Instance instance,
                                      PP_Resource device)
  {
  	return _BindGraphics (instance, device);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Instance_IsFullFrame")]
  extern static PPBool _IsFullFrame ( PP_Instance instance);

  /**
   * IsFullFrame() determines if the instance is full-frame. Such an instance
   * represents the entire document in a frame rather than an embedded
   * resource. This can happen if the user does a top-level navigation or the
   * page specifies an iframe to a resource with a MIME type registered by the
   * module.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance
   * of a module.
   *
   * @return A <code>PP_Bool</code> containing <code>PP_TRUE</code> if the
   * instance is full-frame.
   */
  public static PPBool IsFullFrame ( PP_Instance instance)
  {
  	return _IsFullFrame (instance);
  }


}
/**
 * @}
 */


}
