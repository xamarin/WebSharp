using System;

using PepperSharp;

namespace InputEventInstance
{
    public class InputEventInstance : Instance
    {
        public InputEventInstance(IntPtr handle) : base(handle)
        {
            ViewChanged += OnViewChanged;
            FocusChanged += OnFocusChanged;
            Initialize += OnInitialize;
            InputEvents += OnInputEvents;

            MouseDown += HandleMouseEvents;
            MouseEnter += HandleMouseEvents;
            MouseLeave += HandleMouseEvents;
            MouseMove += HandleMouseEvents;
            MouseUp += HandleMouseEvents;
            ContextMenu += HandleMouseEvents;

            Wheel += HandleWheel;

            KeyUp += HandleKeyboardEvents;
            KeyDown += HandleKeyboardEvents;
            KeyChar += HandleKeyboardEvents;
            RawKeyDown += HandleKeyboardEvents;

            TouchStart += HandleTouchEvents;
            TouchEnd += HandleTouchEvents;
            TouchMove += HandleTouchEvents;
            TouchCancel += HandleTouchEvents;

            IMECompositionEnd += HandleIMEEvents;
            IMECompositionStart += HandleIMEEvents;
            IMECompositionUpdate += HandleIMEEvents;
            IMEText += HandleIMEEvents;

            RequestInputEvents(PPInputEventClass.Mouse | PPInputEventClass.Wheel |
                   PPInputEventClass.Touch);
            RequestFilteringInputEvents(PPInputEventClass.Keyboard);
        }

        /// Scrolling the mouse wheel causes a DidChangeView event.
        private void OnViewChanged(object sender, View view)
        {
            var viewRect = view.Rect;
            string message = $"DidChangeView: x:{viewRect.Origin.X}" +
                            $" y: {viewRect.Origin.Y}" +
                            $" width: {viewRect.Size.Width}" +
                            $" height: {viewRect.Size.Height} \n" +
                            $" IsFullScreen: {view.IsFullScreen}" +
                            $" IsVisible: {view.IsVisible}" +
                            $" IsPageVisible: {view.IsPageVisible}" +
                            $" GetDeviceScale: {view.DeviceScale}" +
                            $" GetCSSScale: {view.CSSScale}";
            PostMessage(message);
        }

        ~InputEventInstance() { System.Console.WriteLine("InputEvent destructed"); }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsole(PPLogLevel.Log, "Hello from InputEvent using C#");
 
        }

        /// Clicking outside of the instance's bounding box
        /// will create a DidChangeFocus event (the NaCl instance is
        /// out of focus). Clicking back inside the instance's
        /// bounding box will create another DidChangeFocus event
        /// (the NaCl instance is back in focus). The default is
        /// that the instance is out of focus.
        private void OnFocusChanged(object sender, bool hasFocus)
        {
            PostMessage($"DidChangeFocus: {hasFocus}");
        }

        private void HandleMouseEvents(object sender, MouseEventArgs mouseEvent)
        {
            var mouse = $"Mouse event:" +
                 $" modifier: {ModifierToString((uint)mouseEvent.Modifiers)}" +
                 $" button: {mouseEvent.Buttons}" +
                 $" x: {mouseEvent.Position.X}" +
                 $" y: {mouseEvent.Position.Y}" +
                 $" click_count: {mouseEvent.Clicks}" +
                 $" time: {mouseEvent.TimeStamp}" +
                 $" is_context_menu: {mouseEvent.IsContextMenu}";
            PostMessage(mouse);
            mouseEvent.Handled = true;
        }

        private void HandleWheel(object sender, WheelEventArgs inputEvent)
        {
            var wheel = "Wheel event:" +
                 $" modifier: {ModifierToString((uint)inputEvent.Modifiers)}" +
                 $" deltax: {inputEvent.Delta.X}" +
                 $" deltay: {inputEvent.Delta.Y}" +
                 $" wheel_ticks_x: {inputEvent.Ticks.X}" +
                 $" wheel_ticks_y: {inputEvent.Ticks.Y}" +
                 $" scroll_by_page: {inputEvent.IsScrollByPage}" +
                 $" time: {inputEvent.TimeStamp}";

            PostMessage(wheel);
            inputEvent.Handled = true;
        }

        private void HandleKeyboardEvents(object sender, KeyboardEventArgs inputEvent)
        {
            var keyboard = "Key event:" +
                $" modifier: {ModifierToString((uint)inputEvent.Modifiers)}" +
                $" key_code: {inputEvent.Code}" +
                $" time: {inputEvent.TimeStamp}" +
            $" text: {inputEvent.CharacterText}";

            PostMessage(keyboard);
            inputEvent.Handled = true;
        }

        private void HandleTouchEvents(object sender, TouchInputEvent e)
        {
            // These cases are not handled
        }

        private void HandleIMEEvents(object sender, IMEInputEvent e)
        {
            // These cases are not handled
        }

        private void OnInputEvents(object sender, InputEvent inputEvent)
        {
            if (inputEvent.Handled)
                return;

            // For any unhandled events, send a message to the browser
            // so that the user is aware of these and can investigate.
            var message = $"Default (unhandled) event, type={inputEvent.EventType}";
            PostMessage(message);
            inputEvent.Handled = true;
        }

        static string ModifierToString(uint inputEventModifier)
        {
            string s = string.Empty;
            var modifier = (PPInputEventModifier)inputEventModifier;
            if ((modifier & PPInputEventModifier.Shiftkey) == PPInputEventModifier.Shiftkey)
            {
                s += "shift ";
            }
            if ((modifier & PPInputEventModifier.Controlkey) == PPInputEventModifier.Controlkey)
            {
                s += "ctrl ";
            }
            if ((modifier & PPInputEventModifier.Altkey) == PPInputEventModifier.Altkey)
            {
                s += "alt ";
            }
            if ((modifier & PPInputEventModifier.Metakey) == PPInputEventModifier.Metakey)
            {
                s += "meta ";
            }
            if ((modifier & PPInputEventModifier.Iskeypad) == PPInputEventModifier.Iskeypad)
            {
                s += "keypad ";
            }
            if ((modifier & PPInputEventModifier.Isautorepeat) == PPInputEventModifier.Isautorepeat)
            {
                s += "autorepeat ";
            }
            if ((modifier & PPInputEventModifier.Leftbuttondown) == PPInputEventModifier.Leftbuttondown)
            {
                s += "left-button-down ";
            }
            if ((modifier & PPInputEventModifier.Middlebuttondown) == PPInputEventModifier.Middlebuttondown)
            {
                s += "middle-button-down ";
            }
            if ((modifier & PPInputEventModifier.Rightbuttondown) == PPInputEventModifier.Rightbuttondown)
            {
                s += "right-button-down ";
            }
            if ((modifier & PPInputEventModifier.Capslockkey) == PPInputEventModifier.Capslockkey)
            {
                s += "caps-lock ";
            }
            if ((modifier & PPInputEventModifier.Numlockkey) == PPInputEventModifier.Numlockkey)
            {
                s += "num-lock ";
            }
            return s;
        }
    }
}
