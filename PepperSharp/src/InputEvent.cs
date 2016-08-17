using System;

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

        /// GetPosition() returns the pixel location of a mouse input event. When
        /// the mouse is locked, it returns the last known mouse position just as
        /// mouse lock was entered.
        ///
        /// @return The point associated with the mouse event, relative to the upper-
        /// left of the instance receiving the event. These values can be negative for
        /// mouse drags. The return value will be (0, 0) for non-mouse events.
        public PPPoint Position
        {
            get { return PPBMouseInputEvent.GetPosition(this);  }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ClickCount
        {
            get { return PPBMouseInputEvent.GetClickCount(this); }
        }

        /// Returns the change in position of the mouse. When the mouse is locked,
        /// although the mouse position doesn't actually change, this function
        /// still provides movement information, which indicates what the change in
        /// position would be had the mouse not been locked.
        ///
        /// @return The change in position of the mouse, relative to the previous
        /// position.
        public PPPoint Movement
        {
            get { return PPBMouseInputEvent.GetMovement(this); }
        }
    }
}
