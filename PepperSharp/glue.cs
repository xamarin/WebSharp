using System.Runtime.CompilerServices;
using System;
using System.Runtime.InteropServices;
namespace Pepper {

	public partial class PPInstance : PPResource {
		internal PPInstance (IntPtr handle) : base (handle) {}

        [DllImport("PepperPlugin", EntryPoint = "PPInstance_LogToConsole")]
        extern static void _LogToConsole(IntPtr handle, PPLogLevel logLevel, string value);
        public void LogToConsole(PPLogLevel logLevel, string value)
        {
            _LogToConsole(handle, logLevel, value);
        }

        [DllImport("PepperPlugin", EntryPoint = "PPInstance_LogToConsoleWithSource")]
        extern static void _LogToConsoleWithSource(IntPtr handle, PPLogLevel logLevel, string source, string value);
        public void LogToConsoleWithSource(PPLogLevel logLevel, string source, string value)
        {
            _LogToConsoleWithSource(handle, logLevel, source, value);
        }

        [DllImport("PepperPlugin", EntryPoint = "PPInstance_RequestInputEvents")]
        extern static void _RequestInputEvents(IntPtr handle, PPInputEvent_Class event_classes);
        public void RequestInputEvents(PPInputEvent_Class eventClasses)
        {
            _RequestInputEvents(handle, eventClasses);
        }


    } /* end class PPInstance*/

    #region class PPBView
    public partial class PPBView : PPResource
    {
        //[MethodImplAttribute(MethodImplOptions.InternalCall)]
        [DllImport("PepperPlugin", EntryPoint = "PPBView_IsView")]
        extern static bool _IsView(IntPtr resource);
        public bool IsView()
        {
            return _IsView(this.Handle);
        }

        [DllImport("PepperPlugin", EntryPoint= "PPBView_GetRect")]
        extern static PPRect _GetRect(IntPtr resource);
        public PPRect GetRect()
        {
            return _GetRect(this.Handle);
        }

        [DllImport("PepperPlugin", EntryPoint = "PPBView_IsFullScreen")]
        extern static bool _IsFullscreen(IntPtr resource);
        public bool IsFullscreen()
        {
            return _IsFullscreen(this.Handle);
        }

        [DllImport("PepperPlugin", EntryPoint = "PPBView_IsVisible")]
        extern static bool _IsVisible(IntPtr resource);
        public bool IsVisible()
        {
            return _IsVisible(this.Handle);
        }

        [DllImport("PepperPlugin", EntryPoint = "PPBView_IsPageVisible")]
        extern static bool _IsPageVisible(IntPtr resource);
        public bool IsPageVisible()
        {
            return _IsPageVisible(this.Handle);
        }

        [DllImport("PepperPlugin", EntryPoint = "PPBView_GetClipRect")]
        extern static bool _GetClipRect(IntPtr resource, PPRect clip);
        public bool GetClipRect(PPRect clip)
        {
            return _GetClipRect(this.Handle, clip);
        }

        [DllImport("PepperPlugin", EntryPoint = "PPBView_GetDeviceScale")]
        extern static float _GetDeviceScale(IntPtr resource);
        public float GetDeviceScale()
        {
            return _GetDeviceScale(this.Handle);
        }

        [DllImport("PepperPlugin", EntryPoint = "PPBView_GetCSSScale")]
        extern static float _GetCSSScale(IntPtr resource);
        public float GetCSSScale()
        {
            return _GetCSSScale(this.Handle);
        }

        [DllImport("PepperPlugin", EntryPoint = "PPBView_GetScrollOffset")]
        extern static bool _GetScrollOffset(IntPtr resource, PPPoint offset);
        public bool GetScrollOffset(PPPoint offset)
        {
            return _GetScrollOffset(this.Handle, offset);
        }

    }
    #endregion class PPBView

    public partial class PPInputEvent : PPResource
    {

    } /* end class PPInputEvent*/

    public partial class PPUrlLoader : PPResource
    {

    } /* end class PPInputEvent*/

    public partial class PPArrayOutput_GetDataBuffer : PPResource
    {

    } /* end class PPArrayOutput_GetDataBuffer*/

    public partial class PPCompletionCallback_Func : PPResource
    {

    } /* end class PPCompletionCallback_Func*/
}
