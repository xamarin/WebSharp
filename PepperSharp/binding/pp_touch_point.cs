/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_touch_point.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the API to create a touch point or position where fingers
 * makes contact with touch screen device.
 */


/**
 * @addtogroup Structs
 * @{
 */
/**
 * The <code>PP_TouchPoint</code> struct represents all information about a
 * single touch point, such as position, id, rotation angle, and pressure.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPTouchPoint {
  /**
   * This value represents the identifier for this TouchPoint. The id
   * corresponds to the order in which the points were pressed. For example,
   * the first point to be pressed has an id of 0, the second has an id of 1,
   * and so on. An id can be reused when a touch point is released.  For
   * example, if two fingers are down, with id 0 and 1, and finger 0 releases,
   * the next finger to be pressed can be assigned to id 0.
   */
  internal uint id;
  /**
   * This value represents the x and y pixel position of this TouchPoint
   * relative to the upper-left of the module instance receiving the event.
   */
  internal PPFloatPoint position;
  /**
   * This value represents the elliptical radii, in screen pixels, in the x
   * and y direction of this TouchPoint.
   */
  internal PPFloatPoint radius;
  /**
   * This value represents the angle of rotation in degrees of the elliptical
   * model of this TouchPoint clockwise from "up."
   */
  internal float rotation_angle;
  /**
   * This value represents the pressure applied to this TouchPoint.  This value
   * is typically between 0 and 1, with 0 indicating no pressure and 1
   * indicating some maximum pressure. Scaling differs depending on the
   * hardware and the value is not guaranteed to stay within that range.
   */
  internal float pressure;
}
/**
 * @}
 */


}
