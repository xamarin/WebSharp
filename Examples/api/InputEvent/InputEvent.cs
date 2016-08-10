using System;

using PepperSharp;

namespace InputEvent
{
    public class InputEvent : Instance
    {
        public InputEvent(IntPtr handle) : base(handle)
        {
            RequestInputEvents(PPInputEventClass.Mouse | PPInputEventClass.Wheel |
                   PPInputEventClass.Touch);
            RequestFilteringInputEvents(PPInputEventClass.Keyboard);
        }

        ~InputEvent() { System.Console.WriteLine("InputEvent destructed"); }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            PPBConsole.Log(this, PPLogLevel.Log, new Var("Hello from InputEvent using C#").AsPPVar());
 
            return true;
        }

        /// Clicking outside of the instance's bounding box
        /// will create a DidChangeFocus event (the NaCl instance is
        /// out of focus). Clicking back inside the instance's
        /// bounding box will create another DidChangeFocus event
        /// (the NaCl instance is back in focus). The default is
        /// that the instance is out of focus.
        public override void DidChangeFocus(bool hasFocus)
        {
            PostMessage($"DidChangeFocus: {hasFocus}");
        }

        /// Scrolling the mouse wheel causes a DidChangeView event.
        public override void DidChangeView(PPResource vview)
        {
            var view = new View(vview);
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

        public override bool HandleInputEvent(PPResource inputEvent)
        {

            var eventType = PPBInputEvent.GetType(inputEvent);
            switch (eventType)
            {
                case PPInputEventType.ImeCompositionStart:
                case PPInputEventType.ImeCompositionUpdate:
                case PPInputEventType.ImeCompositionEnd:
                case PPInputEventType.ImeText:
                case PPInputEventType.Undefined:
                    // these cases are not handled.
                    break;

                case PPInputEventType.Mousedown:
                case PPInputEventType.Mousemove:
                case PPInputEventType.Mouseup:
                case PPInputEventType.Mouseenter:
                case PPInputEventType.Mouseleave:
                case PPInputEventType.Contextmenu:
                    if (PPBMouseInputEvent.IsMouseInputEvent(inputEvent) == PPBool.True)
                    {
                        var mouse = $"Mouse event:" +
                            $" modifier: {ModifierToString(PPBInputEvent.GetModifiers(inputEvent))}" +
                            $" button: {PPBMouseInputEvent.GetButton(inputEvent)}" +
                            $" x: {PPBMouseInputEvent.GetPosition(inputEvent).x}" +
                            $" y: {PPBMouseInputEvent.GetPosition(inputEvent).y}" +
                            $" click_count: {PPBMouseInputEvent.GetClickCount(inputEvent)}" +
                            $" time: {PPBInputEvent.GetTimeStamp(inputEvent)}" +
                            $" is_context_menu: {eventType == PPInputEventType.Contextmenu}";
                        PostMessage(mouse);
                    }


                    break;

                case PPInputEventType.Wheel:
                    
                    if (PPBWheelInputEvent.IsWheelInputEvent(inputEvent) == PPBool.True)
                    {
                        var wheel = "Wheel event:" +
                            $" modifier: {ModifierToString(PPBInputEvent.GetModifiers(inputEvent))}" +
                            $" deltax: {PPBWheelInputEvent.GetDelta(inputEvent).x}" +
                            $" deltay: {PPBWheelInputEvent.GetDelta(inputEvent).y}" +
                            $" wheel_ticks_x: {PPBWheelInputEvent.GetTicks(inputEvent).x}" +
                            $" wheel_ticks_y: {PPBWheelInputEvent.GetTicks(inputEvent).y}" +
                            $" scroll_by_page: {PPBWheelInputEvent.GetScrollByPage(inputEvent) == PPBool.True}" +
                            $" time: {PPBInputEvent.GetTimeStamp(inputEvent)}";

                        PostMessage(wheel);
                    }
                    break;

                case PPInputEventType.Rawkeydown:
                case PPInputEventType.Keyup:
                case PPInputEventType.Char:
                case PPInputEventType.Keydown:

                    if (PPBKeyboardInputEvent.IsKeyboardInputEvent(inputEvent) == PPBool.True)
                    {

                        var keyboard = "Key event:" +
                            $" modifier: {ModifierToString(PPBInputEvent.GetModifiers(inputEvent))}" +
                            $" key_code: {((Var)PPBKeyboardInputEvent.GetCode(inputEvent)).AsString()}" +
                            $" time: {PPBInputEvent.GetTimeStamp(inputEvent)}" +
                        $" text: {((Var)PPBKeyboardInputEvent.GetCharacterText(inputEvent)).DebugString()}";
                        
                        PostMessage(keyboard);
                    }
                    break;


                case PPInputEventType.Touchstart:
                case PPInputEventType.Touchmove:
                case PPInputEventType.Touchend:
                case PPInputEventType.Touchcancel:
                    break;
                default:
                    // For any unhandled events, send a message to the browser
                    // so that the user is aware of these and can investigate.
                    var message = $"Default (unhandled) event, type={eventType}";
                    PostMessage(message);
                    break;
            }

            return true;
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
