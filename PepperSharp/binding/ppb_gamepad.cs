/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_gamepad.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_Gamepad</code> interface, which
 * provides access to gamepad devices.
 */


/**
 * @addtogroup Structs
 * @{
 */
/**
 * The data for one gamepad device.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPGamepadSampleData {
  /**
   * Number of valid elements in the |axes| array.
   */
  internal uint axes_length;
  /**
   * Normalized values for the axes, indices valid up to |axes_length|-1. Axis
   * values range from -1..1, and are in order of "importance".
   */
  internal unsafe fixed float axes[16];
  /**
   * Number of valid elements in the |buttons| array.
   */
  internal uint buttons_length;
  /**
   * Normalized values for the buttons, indices valid up to |buttons_length|
   * - 1. Button values range from 0..1, and are in order of importance.
   */
  internal unsafe fixed float buttons[32];
  /**
   * Monotonically increasing value that is incremented when the data have
   * been updated.
   */
  internal double timestamp;
  /**
   * Identifier for the type of device/manufacturer.
   */
  internal unsafe fixed ushort id[128];
  /**
   * Is there a gamepad connected at this index? If this is false, no other
   * data in this structure is valid.
   */
  internal PPBool connected;
  /* Padding to make the struct the same size between 64 and 32. */
  internal unsafe fixed byte unused_pad_[4];
}

/**
 * The data for all gamepads connected to the system.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPGamepadsSampleData {
  /**
   * Number of valid elements in the |items| array.
   */
  internal uint length;
  /* Padding to make the struct the same size between 64 and 32. */
  internal unsafe fixed byte unused_pad_[4];
  /**
   * Data for an individual gamepad device connected to the system.
   */
  internal PPGamepadSampleData items_1;
  internal PPGamepadSampleData items_2;
  internal PPGamepadSampleData items_3;
  internal PPGamepadSampleData items_4;
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_Gamepad</code> interface allows retrieving data from
 * gamepad/joystick devices that are connected to the system.
 */
internal static partial class PPBGamepad {
  [DllImport("PepperPlugin", EntryPoint = "PPB_Gamepad_Sample")]
  extern static void _Sample ( PPInstance instance,
                              out PPGamepadsSampleData data);

  /**
   * Samples the current state of the available gamepads.
   */
  public static void Sample ( PPInstance instance,
                             out PPGamepadsSampleData data)
  {
  	 _Sample (instance, out data);
  }


}
/**
 * @}
 */


}
