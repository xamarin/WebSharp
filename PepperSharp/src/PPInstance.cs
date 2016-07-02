using System;


namespace PepperSharp
{
    public partial class PPInstance : PPResource
    {
        protected PPInstance() { throw new PlatformNotSupportedException("Can not create an instace of PPInstance"); }
        protected PPInstance(IntPtr handle) : base(handle) { }

        public virtual bool Init(int argc, string[] argn, string[] argv)
        {
            return true;
        }

        public virtual void DidChangeView(PP_Resource view)
        { }

        public virtual void DidChangeFocus(bool hasFocus)
        { }

        public virtual bool HandleInputEvent(PP_Resource inputEvent)
        {
            return false;
        }

        public virtual bool HandleDocumentLoad(PP_Resource urlLoader)
        {
            return false;
        }

        public virtual void HandleMessage (PP_Var message)
        { }

        public bool BindGraphics(PP_Resource graphics2d)
        {
            if (PPBInstance.BindGraphics(Instance, graphics2d) == PPBool.True)
                return true;
            else
                return false;
        }

        // Called by the browser when mouselock is lost.  This happens when the NaCl
        // module exits fullscreen mode.
        public virtual void MouseLockLost() { }

        /// <summary>
        /// asynchronously invokes any listeners for message events on the DOM element for the given instance.
        /// </summary>
        /// <param name="message"></param>
        public void PostMessage(object message)
        {
            PPBMessaging.PostMessage(Instance, new PPVar(message).AsPP_Var());
        }

        PP_Instance instance = new PP_Instance();
        public PP_Instance Instance
        {
            get
            {
                if (instance.pp_instance == 0)
                {
                    instance.pp_instance = this.Handle.ToInt32();
                }
                return instance;
            }
        }

        public void LogToConsole(PPLogLevel level, object value)
        {
            PPBConsole.Log(Instance, level, new PPVar(value));
        }

        public void LogToConsoleWithSource(PPLogLevel level,
                                          string source,
                                          object value)
        {
            PPBConsole.LogWithSource(Instance, level, new PPVar(source), new PPVar(value));
        }

    }
}
