//
// Enums that are used to interop with Pepper
//
// Authors:
//   Kenneth Pouncey (kenneth.pouncey@xamarin.com)
//
// Copyright 2016 Xamarin Inc
//

using System;
using System.Runtime.InteropServices;

namespace Pepper
{
    public enum PP_LogLevel : int
    {
        PP_LOGLEVEL_TIP = 0,
        PP_LOGLEVEL_LOG = 1,
        PP_LOGLEVEL_WARNING = 2,
        PP_LOGLEVEL_ERROR = 3
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
        PP_INPUTEVENT_CLASS_MOUSE = 1 << 0,
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
        PP_INPUTEVENT_CLASS_KEYBOARD = 1 << 1,
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
        PP_INPUTEVENT_CLASS_WHEEL = 1 << 2,
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
        PP_INPUTEVENT_CLASS_TOUCH = 1 << 3,
        /**
         * Identifies IME composition input events.
         *
         * Request this input event class if you allow on-the-spot IME input.
         */
        PP_INPUTEVENT_CLASS_IME = 1 << 4
    }
   

}
