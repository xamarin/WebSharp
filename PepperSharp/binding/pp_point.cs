/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_point.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the API to create a 2 dimensional point.
 * 0,0 is the upper-left starting coordinate.
 */


/**
 * @addtogroup Structs
 * @{
 */
/**
 * The PP_Point structure defines the integer x and y coordinates of a point.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPPoint {
  /**
   * This value represents the horizontal coordinate of a point, starting with 0
   * as the left-most coordinate.
   */
  public int x;
  /**
   * This value represents the vertical coordinate of a point, starting with 0
   * as the top-most coordinate.
   */
  public int y;
}

/**
 * The PP_FloatPoint structure defines the floating-point x and y coordinates
 * of a point.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPFloatPoint {
  public float x;
  public float y;
}
/**
 * @}
 */


}
