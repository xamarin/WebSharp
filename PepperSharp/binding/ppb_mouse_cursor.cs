/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_mouse_cursor.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_MouseCursor</code> interface for setting
 * the mouse cursor.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * The <code>PP_MouseCursor_Type</code> enumeration lists the available stock
 * cursor types.
 */
public enum PPMouseCursorType {
  Custom = -1,
  Pointer = 0,
  Cross = 1,
  Hand = 2,
  Ibeam = 3,
  Wait = 4,
  Help = 5,
  Eastresize = 6,
  Northresize = 7,
  Northeastresize = 8,
  Northwestresize = 9,
  Southresize = 10,
  Southeastresize = 11,
  Southwestresize = 12,
  Westresize = 13,
  Northsouthresize = 14,
  Eastwestresize = 15,
  Northeastsouthwestresize = 16,
  Northwestsoutheastresize = 17,
  Columnresize = 18,
  Rowresize = 19,
  Middlepanning = 20,
  Eastpanning = 21,
  Northpanning = 22,
  Northeastpanning = 23,
  Northwestpanning = 24,
  Southpanning = 25,
  Southeastpanning = 26,
  Southwestpanning = 27,
  Westpanning = 28,
  Move = 29,
  Verticaltext = 30,
  Cell = 31,
  Contextmenu = 32,
  Alias = 33,
  Progress = 34,
  Nodrop = 35,
  Copy = 36,
  None = 37,
  Notallowed = 38,
  Zoomin = 39,
  Zoomout = 40,
  Grab = 41,
  Grabbing = 42
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_MouseCursor</code> allows setting the mouse cursor.
 */
public static partial class PPBMouseCursor {
  [DllImport("PepperPlugin", EntryPoint = "PPB_MouseCursor_SetCursor")]
  extern static PPBool _SetCursor ( PPInstance instance,
                                    PPMouseCursorType type,
                                    PPResource image,
                                    PPPoint hot_spot);

  /**
   * Sets the given mouse cursor. The mouse cursor will be in effect whenever
   * the mouse is over the given instance until it is set again by another
   * call. Note that you can hide the mouse cursor by setting it to the
   * <code>PP_MOUSECURSOR_TYPE_NONE</code> type.
   *
   * This function allows setting both system defined mouse cursors and
   * custom cursors. To set a system-defined cursor, pass the type you want
   * and set the custom image to 0 and the hot spot to NULL. To set a custom
   * cursor, set the type to <code>PP_MOUSECURSOR_TYPE_CUSTOM</code> and
   * specify your image and hot spot.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying the instance
   * that the mouse cursor will affect.
   *
   * @param[in] type A <code>PP_MouseCursor_Type</code> identifying the type of
   * mouse cursor to show.
   *
   * @param[in] image A <code>PPB_ImageData</code> resource identifying the
   * custom image to set when the type is
   * <code>PP_MOUSECURSOR_TYPE_CUSTOM</code>. The image must be less than 32
   * pixels in each direction and must be of the system's native image format.
   * When you are specifying a predefined cursor, this parameter must be 0.
   *
   * @param[in] hot_spot When setting a custom cursor, this identifies the
   * pixel position within the given image of the "hot spot" of the cursor.
   * When specifying a stock cursor, this parameter is ignored.
   *
   * @return PP_TRUE on success, or PP_FALSE if the instance or cursor type
   * is invalid, or if the image is too large.
   */
  public static PPBool SetCursor ( PPInstance instance,
                                   PPMouseCursorType type,
                                   PPResource image,
                                   PPPoint hot_spot)
  {
  	return _SetCursor (instance, type, image, hot_spot);
  }


}
/**
 * @}
 */


}
