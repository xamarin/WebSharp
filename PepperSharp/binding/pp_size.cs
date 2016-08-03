/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_size.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the width and height of a 2D rectangle.
 */


/**
 * @addtogroup Structs
 * @{
 */
/**
 * The <code>PP_Size</code> struct contains the size of a 2D rectangle.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPSize {
  /** This value represents the width of the rectangle. */
  public int width;
  /** This value represents the height of the rectangle. */
  public int height;
}

/**
 * The <code>PP_FloatSize</code> struct contains the size of a 2D rectangle.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPFloatSize {
  /** This value represents the width of the rectangle. */
  public float width;
  /** This value represents the height of the rectangle. */
  public float height;
}
/**
 * @}
 */


}
