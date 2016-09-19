/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_input_event.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the API used to handle mouse and keyboard input events.
 */


/**
 * @addtogroup Structs
 * @{
 */
/**
 * The <code>PP_InputEvent_Key</code> struct represents a key up or key down
 * event.
 *
 * Key up and key down events correspond to physical keys on the keyboard. The
 * actual character that the user typed (if any) will be delivered in a
 * "character" event.
 *
 * If the user loses focus on the module while a key is down, a key up
 * event might not occur. For example, if the module has focus and the user
 * presses and holds the shift key, the module will see a "shift down" message.
 * Then if the user clicks elsewhere on the web page, the module's focus will
 * be lost and no more input events will be delivered.
 *
 * If your module depends on receiving key up events, it should also handle
 * "lost focus" as the equivalent of "all keys up."
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPInputEventKey {
  /** This value is a bit field combination of the EVENT_MODIFIER flags. */
  internal uint modifier;
  /**
   * This value reflects the DOM KeyboardEvent <code>keyCode</code> field.
   * Chrome populates this with the Windows-style Virtual Key code of the key.
   */
  internal uint key_code;
}

/**
 * The <code>PP_InputEvent_Character</code> struct represents a typed character
 * event.
 *
 * Normally, the program will receive a key down event, followed by a character
 * event, followed by a key up event. The character event will have any
 * modifier keys applied. Obvious examples are symbols, where Shift-5 gives you
 * a '%'. The key down and up events will give you the scan code for the "5"
 * key, and the character event will give you the '%' character.
 *
 * You may not get a character event for all key down events if the key doesn't
 * generate a character. Likewise, you may actually get multiple character
 * events in a row. For example, some locales have an accent key that modifies
 * the next character typed. You might get this stream of events: accent down,
 * accent up (it didn't generate a character), letter key down, letter with
 * accent character event (it was modified by the previous accent key), letter
 * key up.  If the letter can't be combined with the accent, like an umlaut and
 * an 'R', the system might send umlaut down, umlaut up, 'R' key down, umlaut
 * character (can't combine it with 'R', so just send the raw umlaut so it
 * isn't lost"), 'R' character event, 'R' key up.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPInputEventCharacter {
  /** A combination of the <code>PP_InputEvent_Modifier</code> flags. */
  internal uint modifier;
  /**
   * This value represents the typed character as a single null-terminated UTF-8
   * character. Any unused bytes will be filled with null bytes. Since the
   * maximum UTF-8 character is 4 bytes, there will always be at least one null
   * at the end so you can treat this as a null-terminated UTF-8 string.
   */
  internal unsafe fixed byte text[5];
}

/**
 * The <code>PP_InputEvent_Mouse</code> struct represents all mouse events
 * except mouse wheel events.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPInputEventMouse {
  /**
   * This value is a bit field combination of the
   * <code>PP_InputEvent_Modifier</code> flags.
   */
  internal uint modifier;
  /**
   * This value represents the button that changed for mouse down or up events.
   * This value will be <code>PP_EVENT_MOUSEBUTTON_NONE</code> for mouse move,
   * enter, and leave events.
   */
  internal PPInputEventMouseButton button;
  /**
   * This values represents the x coordinate of the mouse when the event
   * occurred.
   *
   * In most, but not all, cases these coordinates will just be integers.
   * For example, the plugin element might be arbitrarily scaled or transformed
   * in the DOM, and translating a mouse event into the coordinate space of the
   * plugin will give non-integer values.
   */
  internal float x;
  /**
   * This values represents the y coordinate of the mouse when the event
   * occurred.
   *
   * In most, but not all, cases these coordinates will just be integers.
   * For example, the plugin element might be arbitrarily scaled or transformed
   * in the DOM, and translating a mouse event into the coordinate space of the
   * plugin will give non-integer values.
   */
  internal float y;
  /* TODO(brettw) figure out exactly what this means. */
  internal int click_count;
}

/**
 * The <code>PP_InputEvent_Wheel</code> struct represents all mouse wheel
 * events.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPInputEventWheel {
  /**
   * This value represents a combination of the <code>EVENT_MODIFIER</code>
   * flags.
   */
  internal uint modifier;
  /**
   * The mouse wheel's horizontal scroll amount. A scroll to the right
   * (where the content moves left) is represented as positive values,
   * and a scroll to the left (where the content moves right) is
   * represented as negative values.
   *
   * The units are either in pixels (when scroll_by_page is false) or pages
   * (when scroll_by_page is true). For example, delta_y = -3 means scroll up 3
   * pixels when scroll_by_page is false, and scroll up 3 pages when
   * scroll_by_page is true.
   *
   * This amount is system dependent and will take into account the user's
   * preferred scroll sensitivity and potentially also nonlinear acceleration
   * based on the speed of the scrolling.
   *
   * Devices will be of varying resolution. Some mice with large detents will
   * only generate integer scroll amounts. But fractional values are also
   * possible, for example, on some trackpads and newer mice that don't have
   * "clicks".
   */
  internal float delta_x;
  /**
   * The mouse wheel's vertical scroll amount. A scroll down (where the
   * content moves up) is represented as positive values, and a scroll up
   * (where the content moves down) is represented as negative values.
   *
   * The units are either in pixels (when scroll_by_page is false) or pages
   * (when scroll_by_page is true). For example, delta_y = -3 means scroll up 3
   * pixels when scroll_by_page is false, and scroll up 3 pages when
   * scroll_by_page is true.
   *
   * This amount is system dependent and will take into account the user's
   * preferred scroll sensitivity and potentially also nonlinear acceleration
   * based on the speed of the scrolling.
   *
   * Devices will be of varying resolution. Some mice with large detents will
   * only generate integer scroll amounts. But fractional values are also
   * possible, for example, on some trackpads and newer mice that don't have
   * "clicks".
   */
  internal float delta_y;
  /**
   * The number of "clicks" of the scroll wheel that have produced the
   * event. The value may have system-specific acceleration applied to it,
   * depending on the device. The positive and negative meanings are the same
   * as for <code>delta_x</code> and <code>delta_y</code>.
   *
   * If you are scrolling, you probably want to use the delta values above.
   * These tick events can be useful if you aren't doing actual scrolling and
   * don't want or pixel values. An example may be cycling between different
   * items in a game.
   *
   * You may receive fractional values for the wheel ticks if the mouse wheel
   * is high resolution or doesn't have "clicks". If your program wants
   * discrete events (as in the "picking items" example) you should accumulate
   * fractional click values from multiple messages until the total value
   * reaches positive or negative one. This should represent a similar amount
   * of scrolling as for a mouse that has a discrete mouse wheel.
   */
  internal float wheel_ticks_x;
  /** This value represents */
  internal float wheel_ticks_y;
  /**
   * Indicates if the scroll <code>delta_x</code>/<code>delta_y</code>
   * indicates pages or lines to scroll by. When true, the user is requesting
   * to scroll by pages.
   */
  internal PPBool scroll_by_page;
}
/**
 * @}
 */


}
