﻿using System;

namespace PepperSharp
{
    public class InputEvent : Resource
    {

        /// <summary>
        /// Boolean value that is set to true if the event was handled, false if not. If you have
        /// registered to filter this class of events by calling <code>RequestFilteringInputEvents</code>, 
        /// and you return false, the event will be forwarded to the page (and eventually the browser)
        /// for the default handling. For non-filtered events, the set value of this property
        /// will be ignored.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Returns the type of input event for this input event object.
        /// </summary>
        public PPInputEventType EventType
        {
            get { return PPBInputEvent.GetType(this); }
        }

        /// <summary>
        /// Returns the time that the event was generated. The time
        /// will be before the current time since processing and dispatching the
        /// event has some overhead. Use this value to compare the times the user
        /// generated two events without being sensitive to variable processing time.
        ///
        /// The return value is in time ticks, which is a monotonically increasing
        /// clock not related to the wall clock time. It will not change if the user
        /// changes their clock or daylight savings time starts, so can be reliably
        /// used to compare events. This means, however, that you can't correlate
        /// event times to a particular time of day on the system clock.
        /// </summary>
        public double TimeStamp
        {
            get { return PPBInputEvent.GetTimeStamp(this); }
        }

        /// <summary>
        /// Returns a bitfield indicating which modifiers were down
        /// at the time of the event. This is a combination of the flags in the
        /// <code>PPInputEventModifier</code> enum.
        /// </summary>
        public PPInputEventModifier Modifiers
        {
            get { return (PPInputEventModifier)PPBInputEvent.GetModifiers(this); }
        }
    }

    public class MouseInputEvent : InputEvent
    {
        public MouseInputEvent (InputEvent inputEvent)
        {
            PPBCore.AddRefResource(inputEvent.Handle);
            handle = inputEvent.handle;
        }

        /// <summary>
        /// Gets the mouse button associated with mouse down and up events. This
        /// value will be PPInputEventMouseButton.None for mouse move, enter, and leave
        /// events, and for all non-mouse events.
        /// </summary>
        public PPInputEventMouseButton Button
        {
            get { return PPBMouseInputEvent.GetButton(this);  }
        }

        /// <summary>
        /// Gets the pixel location of a mouse input event. When
        /// the mouse is locked, it returns the last known mouse position just as
        /// mouse lock was entered.
        /// 
        /// The point relative to the upper-left of the instance receiving the event. These values 
        /// can be negative for mouse drags. The return value will be (0, 0) for non-mouse events.
        /// </summary>
        public PPPoint Position
        {
            get { return PPBMouseInputEvent.GetPosition(this);  }
        }

        /// <summary>
        /// Gets the number of clicks of the mouse
        /// </summary>
        public int ClickCount
        {
            get { return PPBMouseInputEvent.GetClickCount(this); }
        }

        /// <summary>
        /// Gets the change in position of the mouse. When the mouse is locked,
        /// although the mouse position doesn't actually change, this function
        /// still provides movement information, which indicates what the change in
        /// position would be had the mouse not been locked.
        ///
        /// The change in position of the mouse, relative to the previous
        /// position.
        /// </summary>
        public PPPoint Movement
        {
            get { return PPBMouseInputEvent.GetMovement(this); }
        }
    }

    public class WheelInputEvent : InputEvent
    {
        public WheelInputEvent(InputEvent inputEvent)
        {
            PPBCore.AddRefResource(inputEvent.Handle);
            handle = inputEvent.handle;
        }

        /// <summary>
        /// Getter that returns the amount vertically and horizontally the user has
        /// requested to scroll by with their mouse wheel. A scroll down or to the
        /// right (where the content moves up or left) is represented as positive
        /// values, and a scroll up or to the left (where the content moves down or
        /// right) is represented as negative values.
        ///
        /// This amount is system dependent and will take into account the user's
        /// preferred scroll sensitivity and potentially also nonlinear acceleration
        /// based on the speed of the scrolling.
        ///
        /// Devices will be of varying resolution. Some mice with large detents will
        /// only generate integer scroll amounts. But fractional values are also
        /// possible, for example, on some trackpads and newer mice that don't have
        /// "clicks".
        ///
        /// The units are either in pixels (when scroll_by_page is false) or pages (when scroll_by_page is
        /// true). For example, y = -3 means scroll up 3 pixels when scroll_by_page
        /// is false, and scroll up 3 pages when scroll_by_page is true.
        /// </summary>
        public PPFloatPoint Delta
        {
            get { return PPBWheelInputEvent.GetDelta(this); }
        }

        /// <summary>
        /// Getter that returns the number of "clicks" of the scroll wheel
        /// that have produced the event. The value may have system-specific
        /// acceleration applied to it, depending on the device. The positive and
        /// negative meanings are the same as for GetDelta().
        ///
        /// If you are scrolling, you probably want to use the delta values.  These
        /// tick events can be useful if you aren't doing actual scrolling and don't
        /// want or pixel values. An example may be cycling between different items in
        /// a game.
        ///
        /// You may receive fractional values for the wheel ticks if the mouse wheel is high
        /// resolution or doesn't have "clicks". If your program wants discrete
        /// events (as in the "picking items" example) you should accumulate
        /// fractional click values from multiple messages until the total value
        /// reaches positive or negative one. This should represent a similar amount
        /// of scrolling as for a mouse that has a discrete mouse wheel.
        /// </summary>
        public PPFloatPoint Ticks
        {
            get { return PPBWheelInputEvent.GetTicks(this); }
        }

        /// <summary>
        /// Getter that indicates if the scroll delta x/y indicates pages or
        /// lines to scroll by.
        ///
        /// True if the event is a wheel event and the user is scrolling
        /// by pages, false if not or if the resource is not a wheel event.
        /// </summary>
        public bool IsScrollByPage
        {
            get { return PPBWheelInputEvent.GetScrollByPage(this) == PPBool.True; }
        }
    }

    public class KeyboardInputEvent : InputEvent
    {
        public KeyboardInputEvent(InputEvent inputEvent)
        {
            PPBCore.AddRefResource(inputEvent.Handle);
            handle = inputEvent.handle;
        }

        /// <summary>
        /// Gets the DOM keyCode field for the keyboard event.
        /// Chrome populates this with the Windows-style Virtual Key code of the key.
        /// </summary>
        public uint KeyCode
        {
            get { return PPBKeyboardInputEvent.GetKeyCode(this); }
        }

        /// <summary>
        /// Gets the typed character for the given character event.
        ///
        /// A string representing a single typed character for character
        /// input events. For non-character input events the return value will be an
        /// empty string.
        /// </summary>
        public string CharacterText
        {
            get
            {
                var ct = (Var)PPBKeyboardInputEvent.GetCharacterText(this);
                if (ct.IsString)
                    return ct.AsString();
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets the DOM |code| for the keyboard event.
        //
        /// A string representing a physical key that was pressed to
        /// generate this event.
        /// </summary>
        public string Code
        {
            get
            {
                var code = (Var)PPBKeyboardInputEvent.GetCode(this);
                if (code.IsString)
                    return code.AsString();
                else
                    return string.Empty;
            }
        }
    }

    public class TouchInputEvent : InputEvent
    {
        public TouchInputEvent(InputEvent inputEvent)
        {
            PPBCore.AddRefResource(inputEvent.Handle);
            handle = inputEvent.handle;
        }

        /// <summary>
        /// Adds the touch-point to the specified TouchList.
        /// </summary>
        /// <param name="list">The list that will be added to</param>
        /// <param name="point">PPTouchPoint to add to the PPTouchList</param>
        void AddTouchPoint(PPTouchListType list, PPTouchPoint point)
        {
            PPBTouchInputEvent.AddTouchPoint(this, list, point);
        }

        /// <summary>
        /// Return the number of touchpoints in the list
        /// </summary>
        /// <param name="list"></param>
        /// <returns>The number of TouchPoints in this TouchList.</returns>
        uint GetTouchCount(PPTouchListType list)
        {
            return PPBTouchInputEvent.GetTouchCount(this, list);
        }

        /// <summary>
        /// Returns the specific PPTouchPoint at the given index of the given list
        /// </summary>
        /// <param name="list">PPTouchListType</param>
        /// <param name="index">Index of the touchpoint in the list</param>
        /// <returns>The TouchPoint at the given index of the given list, or an empty
        /// TouchPoint if the index is out of range.</returns>
        PPTouchPoint GetTouchByIndex(PPTouchListType list, uint index)
        {
            return PPBTouchInputEvent.GetTouchByIndex(this, list, index);
        }

        /// <summary>
        /// Returns the PPTouchPoint in the given list with the given identifier
        /// </summary>
        /// <param name="list">PPTouchList</param>
        /// <param name="id">Id of the touchpoint in the list.</param>
        /// <returns>The TouchPoint in the given list with the given identifier, or an
        /// empty TouchPoint if the list does not contain a TouchPoint with that identifier.
        /// </returns>
        PPTouchPoint GetTouchById(PPTouchListType list, uint id)
        {
            return PPBTouchInputEvent.GetTouchByIndex(this, list, id);
        }
    }

    public class IMEInputEvent : InputEvent
    {
        public IMEInputEvent(InputEvent inputEvent)
        {
            PPBCore.AddRefResource(inputEvent.Handle);
            handle = inputEvent.handle;
        }

        /// <summary>
        /// Getter that returns the composition text as a UTF-8 string for the given IME event.
        /// </summary>
        public string Text
        {
            get
            {
                var txt = (Var)PPBIMEInputEvent.GetText(this);
                if (txt.IsString)
                    return txt.AsString();
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Getter that returns the number of segments in the composition text.
        ///
        /// For events other than COMPOSITION_UPDATE, returns 0.
        /// </summary>
        public uint SegmentNumber
        {
            get { return PPBIMEInputEvent.GetSegmentNumber(this);  }
        }

        /// <summary>
        /// Returns the position of the index-th segmentation point in the composition
        /// text. The position is given by a byte-offset (not a character-offset) of
        /// the string returned by GetText(). It always satisfies
        /// 0=GetSegmentOffset(0) < ... < GetSegmentOffset(i) < GetSegmentOffset(i+1)
        /// < ... < GetSegmentOffset(GetSegmentNumber())=(byte-length of GetText()).
        /// Note that [GetSegmentOffset(i), GetSegmentOffset(i+1)) represents the
        /// range of the i-th segment, and hence GetSegmentNumber() can be a valid
        /// argument to this function instead of an off-by-1 error.
        /// </summary>
        /// <param name="index">An integer indicating a segment.</param>
        /// <returns>
        /// The byte-offset of the segmentation point.If the event is not
        /// COMPOSITION_UPDATE or index is out of range, returns 0. 
        /// </returns>
        public uint GetSegmentOffset(uint index)
        {
            return PPBIMEInputEvent.GetSegmentOffset(this, index);
        }

        /// <summary>
        /// Getter that returns the index of the current target segment of composition.
        ///
        /// When there is no active target segment, or the event is not COMPOSITION_UPDATE,
        /// returns -1.
        /// </summary>
        public int TargetSegment
        {
            get { return PPBIMEInputEvent.GetTargetSegment(this);  }
        }

        /// <summary>
        /// Obtains the range selected by caret in the composition text.
        /// </summary>
        /// <param name="start">An integer indicating a start offset of selection range.</param>
        /// <param name="end">An integer indicating an end offset of selection range.</param>
        public void GetSelection(out uint start, out uint end)
        {
            PPBIMEInputEvent.GetSelection(this, out start, out end);
        }
    }

}
