/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_rect.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the APIs for creating a 2 dimensional rectangle.
 */


/**
 * @addtogroup Structs
 * @{
 */
/**
 * The <code>PP_Rect</code> struct contains the size and location of a 2D
 * rectangle.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPRect {
  /**
   * This value represents the x and y coordinates of the upper-left corner of
   * the rectangle.
   */
  public PPPoint point;
  /** This value represents the width and height of the rectangle. */
  public PPSize size;
};

/**
 * The <code>PP_FloatRect</code> struct contains the size and location of a 2D
 * rectangle.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPFloatRect {
  /**
   * This value represents the x and y coordinates of the upper-left corner of
   * the rectangle.
   */
  public PPFloatPoint point;
  /** This value represents the width and height of the rectangle. */
  public PPFloatSize size;
};
/**
 * @}
 */


}
