using System.Runtime.CompilerServices;
using System;
using System.Runtime.InteropServices;
namespace Pepper {

	public partial class PPInstance : PepperBase {
		internal PPInstance (IntPtr handle) : base (handle) {}

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern static void _LogToConsole(IntPtr handle, PP_LogLevel logLevel, string value);
        public void LogToConsole(PP_LogLevel logLevel, string value)
        {
            _LogToConsole(handle, logLevel, value);
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern static void _LogToConsoleWithSource(IntPtr handle, PP_LogLevel logLevel, string source, string value);
        public void LogToConsoleWithSource(PP_LogLevel logLevel, string source, string value)
        {
            _LogToConsoleWithSource(handle, logLevel, source, value);
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern static void _RequestInputEvents(IntPtr handle, PP_InputEvent_Class event_classes);
        public void RequestInputEvents(PP_InputEvent_Class eventClasses)
        {
            _RequestInputEvents(handle, eventClasses);
        }

    } /* end class PPInstance*/

    public partial class PPView : PepperBase
    {

    } /* end class PPView*/

    public partial class PPInputEvent : PepperBase
    {

    } /* end class PPInputEvent*/

    public partial class PPUrlLoader : PepperBase
    {

    } /* end class PPInputEvent*/

}
