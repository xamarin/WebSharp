using System;

using PepperSharp;

namespace InputEvent
{
    public class InputEvent : PPInstance
    {
        public InputEvent(IntPtr handle) : base(handle)
        {
            PPBInputEvent.RequestInputEvents(Instance, (int)(PP_InputEvent_Class.Mouse | PP_InputEvent_Class.Wheel |
                   PP_InputEvent_Class.Touch));
            PPBInputEvent.RequestFilteringInputEvents(Instance, (int)PP_InputEvent_Class.Keyboard);
        }

        ~InputEvent() { System.Console.WriteLine("InputEvent destructed"); }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            PPBConsole.Log(Instance, PP_LogLevel.Log, new PPVar("Hello from InputEvent using C#").AsPP_Var());
 
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
        public override void DidChangeView(PP_Resource view)
        {
            var viewRect = new PP_Rect();
            PPBView.GetRect(view, out viewRect);
            string message = $"DidChangeView: x:{viewRect.point.x}" +
                            $" y: {viewRect.point.y}" +
                            $" width: {viewRect.size.width}" +
                            $" height: {viewRect.size.height} \n" +
                            $" IsFullScreen: {(PPBView.IsFullscreen(view) == PP_Bool.PP_TRUE)}" +
                            $" IsVisible: {(PPBView.IsVisible(view) == PP_Bool.PP_TRUE)}" +
                            $" IsPageVisible: {(PPBView.IsPageVisible(view) == PP_Bool.PP_TRUE)}" +
                            $" GetDeviceScale: {PPBView.GetDeviceScale(view)}" +
                            $" GetCSSScale: {PPBView.GetCSSScale(view)}";
            PostMessage(message);
        }

        public override bool HandleInputEvent(PP_Resource inputEvent)
        {

            var eventType = PPBInputEvent.GetType(inputEvent);
            switch (eventType)
            {
                case PP_InputEvent_Type.Ime_composition_start:
                case PP_InputEvent_Type.Ime_composition_update:
                case PP_InputEvent_Type.Ime_composition_end:
                case PP_InputEvent_Type.Ime_text:
                case PP_InputEvent_Type.Undefined:
                    // these cases are not handled.
                    break;

                case PP_InputEvent_Type.Mousedown:
                case PP_InputEvent_Type.Mousemove:
                case PP_InputEvent_Type.Mouseup:
                case PP_InputEvent_Type.Mouseenter:
                case PP_InputEvent_Type.Mouseleave:
                case PP_InputEvent_Type.Contextmenu:
                    if (PPBMouseInputEvent.IsMouseInputEvent(inputEvent) == PP_Bool.PP_TRUE)
                    {
                        var mouse = $"Mouse event:" +
                            $" modifier: {ModifierToString(PPBInputEvent.GetModifiers(inputEvent))}" +
                            $" button: {PPBMouseInputEvent.GetButton(inputEvent)}" +
                            $" x: {PPBMouseInputEvent.GetPosition(inputEvent).x}" +
                            $" y: {PPBMouseInputEvent.GetPosition(inputEvent).y}" +
                            $" click_count: {PPBMouseInputEvent.GetClickCount(inputEvent)}" +
                            $" time: {PPBInputEvent.GetTimeStamp(inputEvent)}" +
                            $" is_context_menu: {eventType == PP_InputEvent_Type.Contextmenu}";
                        PostMessage(mouse);
                    }


                    break;

                case PP_InputEvent_Type.Wheel:
                    
                    if (PPBWheelInputEvent.IsWheelInputEvent(inputEvent) == PP_Bool.PP_TRUE)
                    {
                        var wheel = "Wheel event:" +
                            $" modifier: {ModifierToString(PPBInputEvent.GetModifiers(inputEvent))}" +
                            $" deltax: {PPBWheelInputEvent.GetDelta(inputEvent).x}" +
                            $" deltay: {PPBWheelInputEvent.GetDelta(inputEvent).y}" +
                            $" wheel_ticks_x: {PPBWheelInputEvent.GetTicks(inputEvent).x}" +
                            $" wheel_ticks_y: {PPBWheelInputEvent.GetTicks(inputEvent).y}" +
                            $" scroll_by_page: {PPBWheelInputEvent.GetScrollByPage(inputEvent) == PP_Bool.PP_TRUE}" +
                            $" time: {PPBInputEvent.GetTimeStamp(inputEvent)}";

                        PostMessage(wheel);
                    }
                    break;

                case PP_InputEvent_Type.Rawkeydown:
                case PP_InputEvent_Type.Keyup:
                case PP_InputEvent_Type.Char:
                case PP_InputEvent_Type.Keydown:

                    if (PPBKeyboardInputEvent.IsKeyboardInputEvent(inputEvent) == PP_Bool.PP_TRUE)
                    {

                        var keyboard = "Key event:" +
                            $" modifier: {ModifierToString(PPBInputEvent.GetModifiers(inputEvent))}" +
                            $" key_code: {PPBKeyboardInputEvent.GetCode(inputEvent)}" +
                            $" time: {PPBInputEvent.GetTimeStamp(inputEvent)}" +
                        $" text: {new PPVar(PPBKeyboardInputEvent.GetCharacterText(inputEvent)).DebugString()}";
                        
                        PostMessage(keyboard);
                    }
                    break;


                case PP_InputEvent_Type.Touchstart:
                case PP_InputEvent_Type.Touchmove:
                case PP_InputEvent_Type.Touchend:
                case PP_InputEvent_Type.Touchcancel:
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
            var modifier = (PP_InputEvent_Modifier)inputEventModifier;
            if ((modifier & PP_InputEvent_Modifier.Shiftkey) == PP_InputEvent_Modifier.Shiftkey)
            {
                s += "shift ";
            }
            if ((modifier & PP_InputEvent_Modifier.Controlkey) == PP_InputEvent_Modifier.Controlkey)
            {
                s += "ctrl ";
            }
            if ((modifier & PP_InputEvent_Modifier.Altkey) == PP_InputEvent_Modifier.Altkey)
            {
                s += "alt ";
            }
            if ((modifier & PP_InputEvent_Modifier.Metakey) == PP_InputEvent_Modifier.Metakey)
            {
                s += "meta ";
            }
            if ((modifier & PP_InputEvent_Modifier.Iskeypad) == PP_InputEvent_Modifier.Iskeypad)
            {
                s += "keypad ";
            }
            if ((modifier & PP_InputEvent_Modifier.Isautorepeat) == PP_InputEvent_Modifier.Isautorepeat)
            {
                s += "autorepeat ";
            }
            if ((modifier & PP_InputEvent_Modifier.Leftbuttondown) == PP_InputEvent_Modifier.Leftbuttondown)
            {
                s += "left-button-down ";
            }
            if ((modifier & PP_InputEvent_Modifier.Middlebuttondown) == PP_InputEvent_Modifier.Middlebuttondown)
            {
                s += "middle-button-down ";
            }
            if ((modifier & PP_InputEvent_Modifier.Rightbuttondown) == PP_InputEvent_Modifier.Rightbuttondown)
            {
                s += "right-button-down ";
            }
            if ((modifier & PP_InputEvent_Modifier.Capslockkey) == PP_InputEvent_Modifier.Capslockkey)
            {
                s += "caps-lock ";
            }
            if ((modifier & PP_InputEvent_Modifier.Numlockkey) == PP_InputEvent_Modifier.Numlockkey)
            {
                s += "num-lock ";
            }
            return s;
        }
    }
}
