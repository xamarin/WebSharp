using System;

using PepperSharp;

namespace InputEvent
{
    public class InputEvent : PPInstance
    {
        public InputEvent(IntPtr handle) : base(handle)
        {
            PPB_InputEvent.RequestInputEvents(Instance, (int)(PP_InputEvent_Class.Mouse | PP_InputEvent_Class.Wheel |
                   PP_InputEvent_Class.Touch));
            PPB_InputEvent.RequestFilteringInputEvents(Instance, (int)PP_InputEvent_Class.Keyboard);
        }

        ~InputEvent() { System.Console.WriteLine("InputEvent destructed"); }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            PPB_Console.Log(Instance, PP_LogLevel.Log, new PPVar("Hello from InputEvent using C#").AsPP_Var());
 
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
            PPB_View.GetRect(view, out viewRect);
            string message = $"DidChangeView: x:{viewRect.point.x}" +
                            $" y: {viewRect.point.y}" +
                            $" width: {viewRect.size.width}" +
                            $" height: {viewRect.size.height} \n" +
                            $" IsFullScreen: {(PPB_View.IsFullscreen(view) == PP_Bool.PP_TRUE)}" +
                            $" IsVisible: {(PPB_View.IsVisible(view) == PP_Bool.PP_TRUE)}" +
                            $" IsPageVisible: {(PPB_View.IsPageVisible(view) == PP_Bool.PP_TRUE)}" +
                            $" GetDeviceScale: {PPB_View.GetDeviceScale(view)}" +
                            $" GetCSSScale: {PPB_View.GetCSSScale(view)}";
            PostMessage(message);
        }

        public override bool HandleInputEvent(PP_Resource inputEvent)
        {

            var eventType = PPB_InputEvent.GetType(inputEvent);
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
                    if (PPB_MouseInputEvent.IsMouseInputEvent(inputEvent) == PP_Bool.PP_TRUE)
                    {
                        var mouse = $"Mouse event:" +
                            $" modifier: {ModifierToString(PPB_InputEvent.GetModifiers(inputEvent))}" +
                            $" button: {PPB_MouseInputEvent.GetButton(inputEvent)}" +
                            $" x: {PPB_MouseInputEvent.GetPosition(inputEvent).x}" +
                            $" y: {PPB_MouseInputEvent.GetPosition(inputEvent).y}" +
                            $" click_count: {PPB_MouseInputEvent.GetClickCount(inputEvent)}" +
                            $" time: {PPB_InputEvent.GetTimeStamp(inputEvent)}" +
                            $" is_context_menu: {eventType == PP_InputEvent_Type.Contextmenu}";
                        PostMessage(mouse);
                    }


                    break;

                case PP_InputEvent_Type.Wheel:
                    
                    if (PPB_WheelInputEvent.IsWheelInputEvent(inputEvent) == PP_Bool.PP_TRUE)
                    {
                        var wheel = "Wheel event:" +
                            $" modifier: {ModifierToString(PPB_InputEvent.GetModifiers(inputEvent))}" +
                            $" deltax: {PPB_WheelInputEvent.GetDelta(inputEvent).x}" +
                            $" deltay: {PPB_WheelInputEvent.GetDelta(inputEvent).y}" +
                            $" wheel_ticks_x: {PPB_WheelInputEvent.GetTicks(inputEvent).x}" +
                            $" wheel_ticks_y: {PPB_WheelInputEvent.GetTicks(inputEvent).y}" +
                            $" scroll_by_page: {PPB_WheelInputEvent.GetScrollByPage(inputEvent) == PP_Bool.PP_TRUE}" +
                            $" time: {PPB_InputEvent.GetTimeStamp(inputEvent)}";

                        PostMessage(wheel);
                    }
                    break;

                case PP_InputEvent_Type.Rawkeydown:
                case PP_InputEvent_Type.Keyup:
                case PP_InputEvent_Type.Char:
                case PP_InputEvent_Type.Keydown:

                    if (PPB_KeyboardInputEvent.IsKeyboardInputEvent(inputEvent) == PP_Bool.PP_TRUE)
                    {

                        var keyboard = "Key event:" +
                            $" modifier: {ModifierToString(PPB_InputEvent.GetModifiers(inputEvent))}" +
                            $" key_code: {PPB_KeyboardInputEvent.GetCode(inputEvent)}" +
                            $" time: {PPB_InputEvent.GetTimeStamp(inputEvent)}" +
                        $" text: {new PPVar(PPB_KeyboardInputEvent.GetCharacterText(inputEvent)).DebugString()}";
                        
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
