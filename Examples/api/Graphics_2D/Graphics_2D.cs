using System;
using Pepper;

namespace Graphics_2D
{
    public class Graphics_2D : PPInstance
    {
        ~Graphics_2D () { System.Console.WriteLine("Graphics_2D destructed"); }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            LogToConsoleWithSource(PP_LogLevel.PP_LOGLEVEL_LOG, "Graphics_2D.dll", "Hello from PepperSharp using C#");
            //RequestInputEvents(PP_InputEvent_Class.PP_INPUTEVENT_CLASS_MOUSE);
            return true;
        }

        public override void DidChangeView(PPView view)
        {
            Console.WriteLine($"Graphics_2D DidChangeView: {view.Handle}");
        }

        public override void DidChangeFocus(bool hasFocus)
        {
            Console.WriteLine($"Graphics_2D DidChangeFocus: {hasFocus}");
        }

        public override bool HandleInputEvent(PPInputEvent inputEvent)
        {
            Console.WriteLine("Graphics_2D HandleInputEvent: ");
            return true;
        }
    }
}
