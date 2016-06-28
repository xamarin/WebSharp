/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_input_event.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {







/**
 * @file
 * This file defines the Input Event interfaces.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * This enumeration contains the types of input events.
 */
public enum PP_InputEvent_Type {
  Undefined = -1,
  /**
   * Notification that a mouse button was pressed.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_MOUSE class.
   */
  Mousedown = 0,
  /**
   * Notification that a mouse button was released.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_MOUSE class.
   */
  Mouseup = 1,
  /**
   * Notification that a mouse button was moved when it is over the instance
   * or dragged out of it.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_MOUSE class.
   */
  Mousemove = 2,
  /**
   * Notification that the mouse entered the instance's bounds.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_MOUSE class.
   */
  Mouseenter = 3,
  /**
   * Notification that a mouse left the instance's bounds.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_MOUSE class.
   */
  Mouseleave = 4,
  /**
   * Notification that the scroll wheel was used.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_WHEEL class.
   */
  Wheel = 5,
  /**
   * Notification that a key transitioned from "up" to "down".
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_KEYBOARD class.
   */
  /*
   * TODO(brettw) differentiate from KEYDOWN.
   */
  Rawkeydown = 6,
  /**
   * Notification that a key was pressed. This does not necessarily correspond
   * to a character depending on the key and language. Use the
   * PP_INPUTEVENT_TYPE_CHAR for character input.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_KEYBOARD class.
   */
  Keydown = 7,
  /**
   * Notification that a key was released.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_KEYBOARD class.
   */
  Keyup = 8,
  /**
   * Notification that a character was typed. Use this for text input. Key
   * down events may generate 0, 1, or more than one character event depending
   * on the key, locale, and operating system.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_KEYBOARD class.
   */
  Char = 9,
  /**
   * Notification that a context menu should be shown.
   *
   * This message will be sent when the user right-clicks or performs another
   * OS-specific mouse command that should open a context menu. When this event
   * is delivered depends on the system, on some systems (Mac) it will
   * delivered after the mouse down event, and on others (Windows) it will be
   * delivered after the mouse up event.
   *
   * You will always get the normal mouse events. For example, you may see
   * MOUSEDOWN,CONTEXTMENU,MOUSEUP or MOUSEDOWN,MOUSEUP,CONTEXTMENU.
   *
   * The return value from the event handler determines if the context menu
   * event will be passed to the page when you are using filtered input events
   * (via RequestFilteringInputEvents()). In non-filtering mode the event will
   * never be propagated and no context menu will be displayed. If you are
   * handling mouse events in filtering mode, you may want to return true from
   * this event even if you do not support a context menu to suppress the
   * default one.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_MOUSE class.
   */
  Contextmenu = 10,
  /**
   * Notification that an input method composition process has just started.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_IME class.
   */
  Ime_composition_start = 11,
  /**
   * Notification that the input method composition string is updated.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_IME class.
   */
  Ime_composition_update = 12,
  /**
   * Notification that an input method composition process has completed.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_IME class.
   */
  Ime_composition_end = 13,
  /**
   * Notification that an input method committed a string.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_IME class.
   */
  Ime_text = 14,
  /**
   * Notification that a finger was placed on a touch-enabled device.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_TOUCH class.
   */
  Touchstart = 15,
  /**
   * Notification that a finger was moved on a touch-enabled device.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_TOUCH class.
   */
  Touchmove = 16,
  /**
   * Notification that a finger was released on a touch-enabled device.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_TOUCH class.
   */
  Touchend = 17,
  /**
   * Notification that a touch event was canceled.
   *
   * Register for this event using the PP_INPUTEVENT_CLASS_TOUCH class.
   */
  Touchcancel = 18
}

/**
 * This enumeration contains event modifier constants. Each modifier is one
 * bit. Retrieve the modifiers from an input event using the GetEventModifiers
 * function on PPB_InputEvent.
 */
public enum PP_InputEvent_Modifier {
  Shiftkey = 1 << 0,
  Controlkey = 1 << 1,
  Altkey = 1 << 2,
  Metakey = 1 << 3,
  Iskeypad = 1 << 4,
  Isautorepeat = 1 << 5,
  Leftbuttondown = 1 << 6,
  Middlebuttondown = 1 << 7,
  Rightbuttondown = 1 << 8,
  Capslockkey = 1 << 9,
  Numlockkey = 1 << 10,
  Isleft = 1 << 11,
  Isright = 1 << 12
}

/**
 * This enumeration contains constants representing each mouse button. To get
 * the mouse button for a mouse down or up event, use GetMouseButton on
 * PPB_InputEvent.
 */
public enum PP_InputEvent_MouseButton {
  None = -1,
  Left = 0,
  Middle = 1,
  Right = 2
}

public enum PP_InputEvent_Class {
  /**
   * Request mouse input events.
   *
   * Normally you will request mouse events by calling RequestInputEvents().
   * The only use case for filtered events (via RequestFilteringInputEvents())
   * is for instances that have irregular outlines and you want to perform hit
   * testing, which is very uncommon. Requesting non-filtered mouse events will
   * lead to higher performance.
   */
  Mouse = 1 << 0,
  /**
   * Requests keyboard events. Often you will want to request filtered mode
   * (via RequestFilteringInputEvents) for keyboard events so you can pass on
   * events (by returning false) that you don't handle. For example, if you
   * don't request filtered mode and the user pressed "Page Down" when your
   * instance has focus, the page won't scroll which will be a poor experience.
   *
   * A small number of tab and window management commands like Alt-F4 are never
   * sent to the page. You can not request these keyboard commands since it
   * would allow pages to trap users on a page.
   */
  Keyboard = 1 << 1,
  /**
   * Identifies scroll wheel input event. Wheel events must be requested in
   * filtering mode via RequestFilteringInputEvents(). This is because many
   * wheel commands should be forwarded to the page.
   *
   * Most instances will not need this event. Consuming wheel events by
   * returning true from your filtered event handler will prevent the user from
   * scrolling the page when the mouse is over the instance which can be very
   * annoying.
   *
   * If you handle wheel events (for example, you have a document viewer which
   * the user can scroll), the recommended behavior is to return false only if
   * the wheel event actually causes your document to scroll. When the user
   * reaches the end of the document, return false to indicating that the event
   * was not handled. This will then forward the event to the containing page
   * for scrolling, producing the nested scrolling behavior users expect from
   * frames in a page.
   */
  Wheel = 1 << 2,
  /**
   * Identifies touch input events.
   *
   * Request touch events only if you intend to handle them. If the browser
   * knows you do not need to handle touch events, it can handle them at a
   * higher level and achieve higher performance. If the plugin does not
   * register for touch-events, then it will receive synthetic mouse events that
   * are generated from the touch events (e.g. mouse-down for touch-start,
   * mouse-move for touch-move (with left-button down), and mouse-up for
   * touch-end. If the plugin does register for touch events, then the synthetic
   * mouse events are not created.
   */
  Touch = 1 << 3,
  /**
   * Identifies IME composition input events.
   *
   * Request this input event class if you allow on-the-spot IME input.
   */
  Ime = 1 << 4
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_InputEvent</code> interface contains pointers to several
 * functions related to generic input events on the browser.
 */
public static partial class PPB_InputEvent {
  [DllImport("PepperPlugin", EntryPoint = "PPB_InputEvent_RequestInputEvents")]
  extern static int _RequestInputEvents ( PP_Instance instance,
                                          uint event_classes);

  /**
   * RequestInputEvent() requests that input events corresponding to the given
   * input events are delivered to the instance.
   *
   * It's recommended that you use RequestFilteringInputEvents() for keyboard
   * events instead of this function so that you don't interfere with normal
   * browser accelerators.
   *
   * By default, no input events are delivered. Call this function with the
   * classes of events you are interested in to have them be delivered to
   * the instance. Calling this function will override any previous setting for
   * each specified class of input events (for example, if you previously
   * called RequestFilteringInputEvents(), this function will set those events
   * to non-filtering mode).
   *
   * Input events may have high overhead, so you should only request input
   * events that your plugin will actually handle. For example, the browser may
   * do optimizations for scroll or touch events that can be processed
   * substantially faster if it knows there are no non-default receivers for
   * that message. Requesting that such messages be delivered, even if they are
   * processed very quickly, may have a noticeable effect on the performance of
   * the page.
   *
   * Note that synthetic mouse events will be generated from touch events if
   * (and only if) you do not request touch events.
   *
   * When requesting input events through this function, the events will be
   * delivered and <i>not</i> bubbled to the default handlers.
   *
   * <strong>Example:</strong>
   * @code
   *   RequestInputEvents(instance, PP_INPUTEVENT_CLASS_MOUSE);
   *   RequestFilteringInputEvents(instance,
   *       PP_INPUTEVENT_CLASS_WHEEL | PP_INPUTEVENT_CLASS_KEYBOARD);
   * @endcode
   *
   * @param instance The <code>PP_Instance</code> of the instance requesting
   * the given events.
   *
   * @param event_classes A combination of flags from
   * <code>PP_InputEvent_Class</code> that identifies the classes of events the
   * instance is requesting. The flags are combined by logically ORing their
   * values.
   *
   * @return <code>PP_OK</code> if the operation succeeded,
   * <code>PP_ERROR_BADARGUMENT</code> if instance is invalid, or
   * <code>PP_ERROR_NOTSUPPORTED</code> if one of the event class bits were
   * illegal. In the case of an invalid bit, all valid bits will be applied
   * and only the illegal bits will be ignored. The most common cause of a
   * <code>PP_ERROR_NOTSUPPORTED</code> return value is requesting keyboard
   * events, these must use RequestFilteringInputEvents().
   */
  public static int RequestInputEvents ( PP_Instance instance,
                                         uint event_classes)
  {
  	return _RequestInputEvents (instance, event_classes);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_InputEvent_RequestFilteringInputEvents")]
  extern static int _RequestFilteringInputEvents ( PP_Instance instance,
                                                   uint event_classes);

  /**
   * RequestFilteringInputEvents() requests that input events corresponding to
   * the given input events are delivered to the instance for filtering.
   *
   * By default, no input events are delivered. In most cases you would
   * register to receive events by calling RequestInputEvents(). In some cases,
   * however, you may wish to filter events such that they can be bubbled up
   * to the default handlers. In this case, register for those classes of
   * events using this function instead of RequestInputEvents().
   *
   * Filtering input events requires significantly more overhead than just
   * delivering them to the instance. As such, you should only request
   * filtering in those cases where it's absolutely necessary. The reason is
   * that it requires the browser to stop and block for the instance to handle
   * the input event, rather than sending the input event asynchronously. This
   * can have significant overhead.
   *
   * <strong>Example:</strong>
   * @code
   *   RequestInputEvents(instance, PP_INPUTEVENT_CLASS_MOUSE);
   *   RequestFilteringInputEvents(instance,
   *       PP_INPUTEVENT_CLASS_WHEEL | PP_INPUTEVENT_CLASS_KEYBOARD);
   * @endcode
   *
   * @return <code>PP_OK</code> if the operation succeeded,
   * <code>PP_ERROR_BADARGUMENT</code> if instance is invalid, or
   * <code>PP_ERROR_NOTSUPPORTED</code> if one of the event class bits were
   * illegal. In the case of an invalid bit, all valid bits will be applied
   * and only the illegal bits will be ignored.
   */
  public static int RequestFilteringInputEvents ( PP_Instance instance,
                                                  uint event_classes)
  {
  	return _RequestFilteringInputEvents (instance, event_classes);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_InputEvent_ClearInputEventRequest")]
  extern static void _ClearInputEventRequest ( PP_Instance instance,
                                               uint event_classes);

  /**
   * ClearInputEventRequest() requests that input events corresponding to the
   * given input classes no longer be delivered to the instance.
   *
   * By default, no input events are delivered. If you have previously
   * requested input events via RequestInputEvents() or
   * RequestFilteringInputEvents(), this function will unregister handling
   * for the given instance. This will allow greater browser performance for
   * those events.
   *
   * Note that you may still get some input events after clearing the flag if
   * they were dispatched before the request was cleared. For example, if
   * there are 3 mouse move events waiting to be delivered, and you clear the
   * mouse event class during the processing of the first one, you'll still
   * receive the next two. You just won't get more events generated.
   *
   * @param instance The <code>PP_Instance</code> of the instance requesting
   * to no longer receive the given events.
   *
   * @param event_classes A combination of flags from
   * <code>PP_InputEvent_Class</code> that identify the classes of events the
   * instance is no longer interested in.
   */
  public static void ClearInputEventRequest ( PP_Instance instance,
                                              uint event_classes)
  {
  	_ClearInputEventRequest (instance, event_classes);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_InputEvent_IsInputEvent")]
  extern static PP_Bool _IsInputEvent ( PP_Resource resource);

  /**
   * IsInputEvent() returns true if the given resource is a valid input event
   * resource.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a generic
   * resource.
   *
   * @return <code>PP_TRUE</code> if the given resource is a valid input event
   * resource.
   */
  public static PP_Bool IsInputEvent ( PP_Resource resource)
  {
  	return _IsInputEvent (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_InputEvent_GetType")]
  extern static PP_InputEvent_Type _GetType ( PP_Resource eventArg);

  /**
   * GetType() returns the type of input event for the given input event
   * resource.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to an input
   * event.
   *
   * @return A <code>PP_InputEvent_Type</code> if its a valid input event or
   * <code>PP_INPUTEVENT_TYPE_UNDEFINED</code> if the resource is invalid.
   */
  public static PP_InputEvent_Type GetType ( PP_Resource eventArg)
  {
  	return _GetType (eventArg);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_InputEvent_GetTimeStamp")]
  extern static PP_TimeTicks _GetTimeStamp ( PP_Resource eventArg);

  /**
   * GetTimeStamp() Returns the time that the event was generated. This will be
   *  before the current time since processing and dispatching the event has
   * some overhead. Use this value to compare the times the user generated two
   * events without being sensitive to variable processing time.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to the event.
   *
   * @return The return value is in time ticks, which is a monotonically
   * increasing clock not related to the wall clock time. It will not change
   * if the user changes their clock or daylight savings time starts, so can
   * be reliably used to compare events. This means, however, that you can't
   * correlate event times to a particular time of day on the system clock.
   */
  public static PP_TimeTicks GetTimeStamp ( PP_Resource eventArg)
  {
  	return _GetTimeStamp (eventArg);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_InputEvent_GetModifiers")]
  extern static uint _GetModifiers ( PP_Resource eventArg);

  /**
   * GetModifiers() returns a bitfield indicating which modifiers were down
   * at the time of the event. This is a combination of the flags in the
   * <code>PP_InputEvent_Modifier</code> enum.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to an input
   * event.
   *
   * @return The modifiers associated with the event, or 0 if the given
   * resource is not a valid event resource.
   */
  public static uint GetModifiers ( PP_Resource eventArg)
  {
  	return _GetModifiers (eventArg);
  }


}

/**
 * The <code>PPB_MouseInputEvent</code> interface contains pointers to several
 * functions related to mouse input events.
 */
public static partial class PPB_MouseInputEvent {
  [DllImport("PepperPlugin", EntryPoint = "PPB_MouseInputEvent_Create")]
  extern static PP_Resource _Create ( PP_Instance instance,
                                      PP_InputEvent_Type type,
                                      PP_TimeTicks time_stamp,
                                      uint modifiers,
                                      PP_InputEvent_MouseButton mouse_button,
                                      PP_Point mouse_position,
                                      int click_count,
                                      PP_Point mouse_movement);

  /**
   * Create() creates a mouse input event with the given parameters. Normally
   * you will get a mouse event passed through the
   * <code>HandleInputEvent</code> and will not need to create them, but some
   * applications may want to create their own for internal use. The type must
   * be one of the mouse event types.
   *
   * @param[in] instance The instance for which this event occurred.
   *
   * @param[in] type A <code>PP_InputEvent_Type</code> identifying the type of
   * input event.
   *
   * @param[in] time_stamp A <code>PP_TimeTicks</code> indicating the time
   * when the event occurred.
   *
   * @param[in] modifiers A bit field combination of the
   * <code>PP_InputEvent_Modifier</code> flags.
   *
   * @param[in] mouse_button The button that changed for mouse down or up
   * events. This value will be <code>PP_EVENT_MOUSEBUTTON_NONE</code> for
   * mouse move, enter, and leave events.
   *
   * @param[in] mouse_position A <code>Point</code> containing the x and y
   * position of the mouse when the event occurred.
   *
   * @param[in] mouse_movement The change in position of the mouse.
   *
   * @return A <code>PP_Resource</code> containing the new mouse input event.
   */
  public static PP_Resource Create ( PP_Instance instance,
                                     PP_InputEvent_Type type,
                                     PP_TimeTicks time_stamp,
                                     uint modifiers,
                                     PP_InputEvent_MouseButton mouse_button,
                                     PP_Point mouse_position,
                                     int click_count,
                                     PP_Point mouse_movement)
  {
  	return _Create (instance,
                   type,
                   time_stamp,
                   modifiers,
                   mouse_button,
                   mouse_position,
                   click_count,
                   mouse_movement);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_MouseInputEvent_IsMouseInputEvent")]
  extern static PP_Bool _IsMouseInputEvent ( PP_Resource resource);

  /**
   * IsMouseInputEvent() determines if a resource is a mouse event.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to an event.
   *
   * @return <code>PP_TRUE</code> if the given resource is a valid mouse input
   * event, otherwise <code>PP_FALSE</code>.
   */
  public static PP_Bool IsMouseInputEvent ( PP_Resource resource)
  {
  	return _IsMouseInputEvent (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_MouseInputEvent_GetButton")]
  extern static PP_InputEvent_MouseButton _GetButton ( PP_Resource mouse_event);

  /**
   * GetButton() returns the mouse button that generated a mouse down or up
   * event.
   *
   * @param[in] mouse_event A <code>PP_Resource</code> corresponding to a
   * mouse event.
   *
   * @return The mouse button associated with mouse down and up events. This
   * value will be <code>PP_EVENT_MOUSEBUTTON_NONE</code> for mouse move,
   * enter, and leave events, and for all non-mouse events.
   */
  public static PP_InputEvent_MouseButton GetButton ( PP_Resource mouse_event)
  {
  	return _GetButton (mouse_event);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_MouseInputEvent_GetPosition")]
  extern static PP_Point _GetPosition ( PP_Resource mouse_event);

  /**
   * GetPosition() returns the pixel location of a mouse input event. When
   * the mouse is locked, it returns the last known mouse position just as
   * mouse lock was entered.
   *
   * @param[in] mouse_event A <code>PP_Resource</code> corresponding to a
   * mouse event.
   *
   * @return The point associated with the mouse event, relative to the upper-
   * left of the instance receiving the event. These values can be negative for
   * mouse drags. The return value will be (0, 0) for non-mouse events.
   */
  public static PP_Point GetPosition ( PP_Resource mouse_event)
  {
  	return _GetPosition (mouse_event);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_MouseInputEvent_GetClickCount")]
  extern static int _GetClickCount ( PP_Resource mouse_event);

  /*
   * TODO(brettw) figure out exactly what this means.
   */
  public static int GetClickCount ( PP_Resource mouse_event)
  {
  	return _GetClickCount (mouse_event);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_MouseInputEvent_GetMovement")]
  extern static PP_Point _GetMovement ( PP_Resource mouse_event);

  /**
   * Returns the change in position of the mouse. When the mouse is locked,
   * although the mouse position doesn't actually change, this function
   * still provides movement information, which indicates what the change in
   * position would be had the mouse not been locked.
   *
   * @param[in] mouse_event A <code>PP_Resource</code> corresponding to a
   * mouse event.
   *
   * @return The change in position of the mouse, relative to the previous
   * position.
   */
  public static PP_Point GetMovement ( PP_Resource mouse_event)
  {
  	return _GetMovement (mouse_event);
  }


}

/**
 * The <code>PPB_WheelIputEvent</code> interface contains pointers to several
 * functions related to wheel input events.
 */
public static partial class PPB_WheelInputEvent {
  [DllImport("PepperPlugin", EntryPoint = "PPB_WheelInputEvent_Create")]
  extern static PP_Resource _Create ( PP_Instance instance,
                                      PP_TimeTicks time_stamp,
                                      uint modifiers,
                                      PP_FloatPoint wheel_delta,
                                      PP_FloatPoint wheel_ticks,
                                      PP_Bool scroll_by_page);

  /**
   * Create() creates a wheel input event with the given parameters. Normally
   * you will get a wheel event passed through the
   * <code>HandleInputEvent</code> and will not need to create them, but some
   * applications may want to create their own for internal use.
   *
   * @param[in] instance The instance for which this event occurred.
   *
   * @param[in] time_stamp A <code>PP_TimeTicks</code> indicating the time
   * when the event occurred.
   *
   * @param[in] modifiers A bit field combination of the
   * <code>PP_InputEvent_Modifier</code> flags.
   *
   * @param[in] wheel_delta The scroll wheel's horizontal and vertical scroll
   * amounts.
   *
   * @param[in] wheel_ticks The number of "clicks" of the scroll wheel that
   * have produced the event.
   *
   * @param[in] scroll_by_page When true, the user is requesting to scroll
   * by pages. When false, the user is requesting to scroll by lines.
   *
   * @return A <code>PP_Resource</code> containing the new wheel input event.
   */
  public static PP_Resource Create ( PP_Instance instance,
                                     PP_TimeTicks time_stamp,
                                     uint modifiers,
                                     PP_FloatPoint wheel_delta,
                                     PP_FloatPoint wheel_ticks,
                                     PP_Bool scroll_by_page)
  {
  	return _Create (instance,
                   time_stamp,
                   modifiers,
                   wheel_delta,
                   wheel_ticks,
                   scroll_by_page);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_WheelInputEvent_IsWheelInputEvent")]
  extern static PP_Bool _IsWheelInputEvent ( PP_Resource resource);

  /**
   * IsWheelInputEvent() determines if a resource is a wheel event.
   *
   * @param[in] wheel_event A <code>PP_Resource</code> corresponding to an
   * event.
   *
   * @return <code>PP_TRUE</code> if the given resource is a valid wheel input
   * event.
   */
  public static PP_Bool IsWheelInputEvent ( PP_Resource resource)
  {
  	return _IsWheelInputEvent (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WheelInputEvent_GetDelta")]
  extern static PP_FloatPoint _GetDelta ( PP_Resource wheel_event);

  /**
   * GetDelta() returns the amount vertically and horizontally the user has
   * requested to scroll by with their mouse wheel. A scroll down or to the
   * right (where the content moves up or left) is represented as positive
   * values, and a scroll up or to the left (where the content moves down or
   * right) is represented as negative values.
   *
   * This amount is system dependent and will take into account the user's
   * preferred scroll sensitivity and potentially also nonlinear acceleration
   * based on the speed of the scrolling.
   *
   * Devices will be of varying resolution. Some mice with large detents will
   * only generate integer scroll amounts. But fractional values are also
   * possible, for example, on some trackpads and newer mice that don't have
   * "clicks".
   *
   * @param[in] wheel_event A <code>PP_Resource</code> corresponding to a wheel
   * event.
   *
   * @return The vertical and horizontal scroll values. The units are either in
   * pixels (when scroll_by_page is false) or pages (when scroll_by_page is
   * true). For example, y = -3 means scroll up 3 pixels when scroll_by_page
   * is false, and scroll up 3 pages when scroll_by_page is true.
   */
  public static PP_FloatPoint GetDelta ( PP_Resource wheel_event)
  {
  	return _GetDelta (wheel_event);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_WheelInputEvent_GetTicks")]
  extern static PP_FloatPoint _GetTicks ( PP_Resource wheel_event);

  /**
   * GetTicks() returns the number of "clicks" of the scroll wheel
   * that have produced the event. The value may have system-specific
   * acceleration applied to it, depending on the device. The positive and
   * negative meanings are the same as for GetDelta().
   *
   * If you are scrolling, you probably want to use the delta values.  These
   * tick events can be useful if you aren't doing actual scrolling and don't
   * want or pixel values. An example may be cycling between different items in
   * a game.
   *
   * @param[in] wheel_event A <code>PP_Resource</code> corresponding to a wheel
   * event.
   *
   * @return The number of "clicks" of the scroll wheel. You may receive
   * fractional values for the wheel ticks if the mouse wheel is high
   * resolution or doesn't have "clicks". If your program wants discrete
   * events (as in the "picking items" example) you should accumulate
   * fractional click values from multiple messages until the total value
   * reaches positive or negative one. This should represent a similar amount
   * of scrolling as for a mouse that has a discrete mouse wheel.
   */
  public static PP_FloatPoint GetTicks ( PP_Resource wheel_event)
  {
  	return _GetTicks (wheel_event);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_WheelInputEvent_GetScrollByPage")]
  extern static PP_Bool _GetScrollByPage ( PP_Resource wheel_event);

  /**
   * GetScrollByPage() indicates if the scroll delta x/y indicates pages or
   * lines to scroll by.
   *
   * @param[in] wheel_event A <code>PP_Resource</code> corresponding to a wheel
   * event.
   *
   * @return <code>PP_TRUE</code> if the event is a wheel event and the user is
   * scrolling by pages. <code>PP_FALSE</code> if not or if the resource is not
   * a wheel event.
   */
  public static PP_Bool GetScrollByPage ( PP_Resource wheel_event)
  {
  	return _GetScrollByPage (wheel_event);
  }


}

/**
 * The <code>PPB_KeyboardInputEvent</code> interface contains pointers to
 * several functions related to keyboard input events.
 */
public static partial class PPB_KeyboardInputEvent {
  [DllImport("PepperPlugin", EntryPoint = "PPB_KeyboardInputEvent_Create")]
  extern static PP_Resource _Create ( PP_Instance instance,
                                      PP_InputEvent_Type type,
                                      PP_TimeTicks time_stamp,
                                      uint modifiers,
                                      uint key_code,
                                      string character_text,
                                      string code);

  /**
   * Creates a keyboard input event with the given parameters. Normally you
   * will get a keyboard event passed through the HandleInputEvent and will not
   * need to create them, but some applications may want to create their own
   * for internal use. The type must be one of the keyboard event types.
   *
   * @param[in] instance The instance for which this event occurred.
   *
   * @param[in] type A <code>PP_InputEvent_Type</code> identifying the type of
   * input event.
   *
   * @param[in] time_stamp A <code>PP_TimeTicks</code> indicating the time
   * when the event occurred.
   *
   * @param[in] modifiers A bit field combination of the
   * <code>PP_InputEvent_Modifier</code> flags.
   *
   * @param[in] key_code This value reflects the DOM KeyboardEvent
   * <code>keyCode</code> field, which is the Windows-style Virtual Key
   * code of the key.
   *
   * @param[in] character_text This value represents the typed character as a
   * UTF-8 string.
   *
   * @param[in] code This value represents the DOM3 |code| string that
   * corresponds to the physical key being pressed.
   *
   * @return A <code>PP_Resource</code> containing the new keyboard input
   * event.
   */
  public static PP_Resource Create ( PP_Instance instance,
                                     PP_InputEvent_Type type,
                                     PP_TimeTicks time_stamp,
                                     uint modifiers,
                                     uint key_code,
                                     string character_text,
                                     string code)
  {
  	return _Create (instance,
                   type,
                   time_stamp,
                   modifiers,
                   key_code,
                   character_text,
                   code);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_KeyboardInputEvent_IsKeyboardInputEvent")]
  extern static PP_Bool _IsKeyboardInputEvent ( PP_Resource resource);

  /**
   * IsKeyboardInputEvent() determines if a resource is a keyboard event.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to an event.
   *
   * @return <code>PP_TRUE</code> if the given resource is a valid input event.
   */
  public static PP_Bool IsKeyboardInputEvent ( PP_Resource resource)
  {
  	return _IsKeyboardInputEvent (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_KeyboardInputEvent_GetKeyCode")]
  extern static uint _GetKeyCode ( PP_Resource key_event);

  /**
   * GetKeyCode() returns the DOM keyCode field for the keyboard event.
   * Chrome populates this with the Windows-style Virtual Key code of the key.
   *
   * @param[in] key_event A <code>PP_Resource</code> corresponding to a
   * keyboard event.
   *
   * @return The DOM keyCode field for the keyboard event.
   */
  public static uint GetKeyCode ( PP_Resource key_event)
  {
  	return _GetKeyCode (key_event);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_KeyboardInputEvent_GetCharacterText")]
  extern static string _GetCharacterText ( PP_Resource character_event);

  /**
   * GetCharacterText() returns the typed character as a UTF-8 string for the
   * given character event.
   *
   * @param[in] character_event A <code>PP_Resource</code> corresponding to a
   * keyboard event.
   *
   * @return A string var representing a single typed character for character
   * input events. For non-character input events the return value will be an
   * undefined var.
   */
  public static string GetCharacterText ( PP_Resource character_event)
  {
  	return _GetCharacterText (character_event);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_KeyboardInputEvent_GetCode")]
  extern static string _GetCode ( PP_Resource key_event);

  /**
   * GetCode() returns the DOM |code| field for this keyboard event, as
   * defined in the DOM3 Events spec:
   * http://www.w3.org/TR/DOM-Level-3-Events/
   *
   * @param[in] key_event The key event for which to return the key code.
   *
   * @return The string that contains the DOM |code| for the keyboard event.
   */
  public static string GetCode ( PP_Resource key_event)
  {
  	return _GetCode (key_event);
  }


}
/**
 * @}
 */

/**
 * @addtogroup Enums
 * @{
 */
public enum PP_TouchListType {
  /**
   * The list of all TouchPoints which are currently down.
   */
  PP_TOUCHLIST_TYPE_TOUCHES = 0,
  /**
   * The list of all TouchPoints whose state has changed since the last
   * TouchInputEvent.
   */
  PP_TOUCHLIST_TYPE_CHANGEDTOUCHES = 1,
  /**
   * The list of all TouchPoints which are targeting this plugin.  This is a
   * subset of Touches.
   */
  PP_TOUCHLIST_TYPE_TARGETTOUCHES = 2
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_TouchInputEvent</code> interface contains pointers to several
 * functions related to touch events.
 */
public static partial class PPB_TouchInputEvent {
  [DllImport("PepperPlugin", EntryPoint = "PPB_TouchInputEvent_Create")]
  extern static PP_Resource _Create ( PP_Instance instance,
                                      PP_InputEvent_Type type,
                                      PP_TimeTicks time_stamp,
                                      uint modifiers);

  /**
   * Creates a touch input event with the given parameters. Normally you
   * will get a touch event passed through the HandleInputEvent and will not
   * need to create them, but some applications may want to create their own
   * for internal use. The type must be one of the touch event types.
   * This newly created touch input event does not have any touch point in any
   * of the touch-point lists. <code>AddTouchPoint</code> should be called to
   * add the touch-points.
   *
   * @param[in] instance The instance for which this event occurred.
   *
   * @param[in] type A <code>PP_InputEvent_Type</code> identifying the type of
   * input event.
   *
   * @param[in] time_stamp A <code>PP_TimeTicks</code> indicating the time
   * when the event occurred.
   *
   * @param[in]  modifiers A bit field combination of the
   * <code>PP_InputEvent_Modifier</code> flags.
   *
   * @return A <code>PP_Resource</code> containing the new touch input event.
   */
  public static PP_Resource Create ( PP_Instance instance,
                                     PP_InputEvent_Type type,
                                     PP_TimeTicks time_stamp,
                                     uint modifiers)
  {
  	return _Create (instance, type, time_stamp, modifiers);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TouchInputEvent_AddTouchPoint")]
  extern static void _AddTouchPoint ( PP_Resource touch_event,
                                      PP_TouchListType list,
                                      PP_TouchPoint point);

  /**
   * Adds a touch point to the touch event in the specified touch-list.
   *
   * @param[in] touch_event A <code>PP_Resource</code> corresponding to a touch
   * event.
   *
   * @param[in] list The list to add the touch point to.
   *
   * @param[in] point The point to add to the list.
   */
  public static void AddTouchPoint ( PP_Resource touch_event,
                                     PP_TouchListType list,
                                     PP_TouchPoint point)
  {
  	_AddTouchPoint (touch_event, list, point);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_TouchInputEvent_IsTouchInputEvent")]
  extern static PP_Bool _IsTouchInputEvent ( PP_Resource resource);

  /**
   * IsTouchInputEvent() determines if a resource is a touch event.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to an event.
   *
   * @return <code>PP_TRUE</code> if the given resource is a valid touch input
   * event, otherwise <code>PP_FALSE</code>.
   */
  public static PP_Bool IsTouchInputEvent ( PP_Resource resource)
  {
  	return _IsTouchInputEvent (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TouchInputEvent_GetTouchCount")]
  extern static uint _GetTouchCount ( PP_Resource resource,
                                      PP_TouchListType list);

  /**
   * Returns the number of touch-points in the specified list.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a touch
   * event.
   *
   * @param[in] list The list.
   *
   * @return The number of touch-points in the specified list.
   */
  public static uint GetTouchCount ( PP_Resource resource,
                                     PP_TouchListType list)
  {
  	return _GetTouchCount (resource, list);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_TouchInputEvent_GetTouchByIndex")]
  extern static PP_TouchPoint _GetTouchByIndex ( PP_Resource resource,
                                                 PP_TouchListType list,
                                                 uint index);

  /**
   * Returns the touch-point at the specified index from the specified list.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a touch
   * event.
   *
   * @param[in] list The list.
   *
   * @param[in] index The index.
   *
   * @return A <code>PP_TouchPoint</code> representing the touch-point.
   */
  public static PP_TouchPoint GetTouchByIndex ( PP_Resource resource,
                                                PP_TouchListType list,
                                                uint index)
  {
  	return _GetTouchByIndex (resource, list, index);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_TouchInputEvent_GetTouchById")]
  extern static PP_TouchPoint _GetTouchById ( PP_Resource resource,
                                              PP_TouchListType list,
                                              uint touch_id);

  /**
   * Returns the touch-point with the specified touch-id in the specified list.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a touch
   * event.
   *
   * @param[in] list The list.
   *
   * @param[in] touch_id The id of the touch-point.
   *
   * @return A <code>PP_TouchPoint</code> representing the touch-point.
   */
  public static PP_TouchPoint GetTouchById ( PP_Resource resource,
                                             PP_TouchListType list,
                                             uint touch_id)
  {
  	return _GetTouchById (resource, list, touch_id);
  }


}

public static partial class PPB_IMEInputEvent {
  [DllImport("PepperPlugin", EntryPoint = "PPB_IMEInputEvent_Create")]
  extern static PP_Resource _Create ( PP_Instance instance,
                                      PP_InputEvent_Type type,
                                      PP_TimeTicks time_stamp,
                                      string text,
                                      uint segment_number,
                                      uint[] segment_offsets,
                                      int target_segment,
                                      uint selection_start,
                                      uint selection_end);

  /**
   * Create() creates an IME input event with the given parameters. Normally
   * you will get an IME event passed through the <code>HandleInputEvent</code>
   * and will not need to create them, but some applications may want to create
   * their own for internal use.
   *
   * @param[in] instance The instance for which this event occurred.
   *
   * @param[in] type A <code>PP_InputEvent_Type</code> identifying the type of
   * input event. The type must be one of the IME event types.
   *
   * @param[in] time_stamp A <code>PP_TimeTicks</code> indicating the time
   * when the event occurred.
   *
   * @param[in] text The string returned by <code>GetText</code>.
   *
   * @param[in] segment_number The number returned by
   * <code>GetSegmentNumber</code>.
   *
   * @param[in] segment_offsets The array of numbers returned by
   * <code>GetSegmentOffset</code>. If <code>segment_number</code> is zero,
   * the number of elements of the array should be zero. If
   * <code>segment_number</code> is non-zero, the length of the array must be
   * <code>segment_number</code> + 1.
   *
   * @param[in] target_segment The number returned by
   * <code>GetTargetSegment</code>.
   *
   * @param[in] selection_start The start index returned by
   * <code>GetSelection</code>.
   *
   * @param[in] selection_end The end index returned by
   * <code>GetSelection</code>.
   *
   * @return A <code>PP_Resource</code> containing the new IME input event.
   */
  public static PP_Resource Create ( PP_Instance instance,
                                     PP_InputEvent_Type type,
                                     PP_TimeTicks time_stamp,
                                     string text,
                                     uint segment_number,
                                     uint[] segment_offsets,
                                     int target_segment,
                                     uint selection_start,
                                     uint selection_end)
  {
  	return _Create (instance,
                   type,
                   time_stamp,
                   text,
                   segment_number,
                   segment_offsets,
                   target_segment,
                   selection_start,
                   selection_end);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_IMEInputEvent_IsIMEInputEvent")]
  extern static PP_Bool _IsIMEInputEvent ( PP_Resource resource);

  /**
   * IsIMEInputEvent() determines if a resource is an IME event.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to an event.
   *
   * @return <code>PP_TRUE</code> if the given resource is a valid input event.
   */
  public static PP_Bool IsIMEInputEvent ( PP_Resource resource)
  {
  	return _IsIMEInputEvent (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_IMEInputEvent_GetText")]
  extern static string _GetText ( PP_Resource ime_event);

  /**
   * GetText() returns the composition text as a UTF-8 string for the given IME
   * event.
   *
   * @param[in] ime_event A <code>PP_Resource</code> corresponding to an IME
   * event.
   *
   * @return A string var representing the composition text. For non-IME input
   * events the return value will be an undefined var.
   */
  public static string GetText ( PP_Resource ime_event)
  {
  	return _GetText (ime_event);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_IMEInputEvent_GetSegmentNumber")]
  extern static uint _GetSegmentNumber ( PP_Resource ime_event);

  /**
   * GetSegmentNumber() returns the number of segments in the composition text.
   *
   * @param[in] ime_event A <code>PP_Resource</code> corresponding to an IME
   * event.
   *
   * @return The number of segments. For events other than COMPOSITION_UPDATE,
   * returns 0.
   */
  public static uint GetSegmentNumber ( PP_Resource ime_event)
  {
  	return _GetSegmentNumber (ime_event);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_IMEInputEvent_GetSegmentOffset")]
  extern static uint _GetSegmentOffset ( PP_Resource ime_event,  uint index);

  /**
   * GetSegmentOffset() returns the position of the index-th segmentation point
   * in the composition text. The position is given by a byte-offset (not a
   * character-offset) of the string returned by GetText(). It always satisfies
   * 0=GetSegmentOffset(0) < ... < GetSegmentOffset(i) < GetSegmentOffset(i+1)
   * < ... < GetSegmentOffset(GetSegmentNumber())=(byte-length of GetText()).
   * Note that [GetSegmentOffset(i), GetSegmentOffset(i+1)) represents the range
   * of the i-th segment, and hence GetSegmentNumber() can be a valid argument
   * to this function instead of an off-by-1 error.
   *
   * @param[in] ime_event A <code>PP_Resource</code> corresponding to an IME
   * event.
   *
   * @param[in] index An integer indicating a segment.
   *
   * @return The byte-offset of the segmentation point. If the event is not
   * COMPOSITION_UPDATE or index is out of range, returns 0.
   */
  public static uint GetSegmentOffset ( PP_Resource ime_event,  uint index)
  {
  	return _GetSegmentOffset (ime_event, index);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_IMEInputEvent_GetTargetSegment")]
  extern static int _GetTargetSegment ( PP_Resource ime_event);

  /**
   * GetTargetSegment() returns the index of the current target segment of
   * composition.
   *
   * @param[in] ime_event A <code>PP_Resource</code> corresponding to an IME
   * event.
   *
   * @return An integer indicating the index of the target segment. When there
   * is no active target segment, or the event is not COMPOSITION_UPDATE,
   * returns -1.
   */
  public static int GetTargetSegment ( PP_Resource ime_event)
  {
  	return _GetTargetSegment (ime_event);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_IMEInputEvent_GetSelection")]
  extern static void _GetSelection ( PP_Resource ime_event,
                                    out uint start,
                                    out uint end);

  /**
   * GetSelection() returns the range selected by caret in the composition text.
   *
   * @param[in] ime_event A <code>PP_Resource</code> corresponding to an IME
   * event.
   *
   * @param[out] start The start position of the current selection.
   *
   * @param[out] end The end position of the current selection.
   */
  public static void GetSelection ( PP_Resource ime_event,
                                   out uint start,
                                   out uint end)
  {
  	_GetSelection (ime_event, out start, out end);
  }


}
/**
 * @}
 */


}
