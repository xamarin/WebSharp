using System;


namespace PepperSharp
{
    public partial class Instance : Resource
    {
        protected Instance() { throw new PlatformNotSupportedException("Can not create an instace of PPInstance"); }
        protected Instance(IntPtr handle) : base(handle) { }

        public virtual bool Init(int argc, string[] argn, string[] argv)
        {
            return true;
        }

        public virtual void DidChangeView(PPResource view)
        { }

        public virtual void DidChangeFocus(bool hasFocus)
        { }

        public virtual bool HandleInputEvent(PPResource inputEvent)
        {
            return false;
        }

        public virtual bool HandleDocumentLoad(PPResource urlLoader)
        {
            return false;
        }

        public virtual void HandleMessage (PPVar message)
        { }

        public bool BindGraphics(PPResource graphics2d)
        {
            if (PPBInstance.BindGraphics(this, graphics2d) == PPBool.True)
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
            PPBMessaging.PostMessage(this, new Var(message));
        }

        PPInstance instance = new PPInstance();
        public PPInstance PPInstance
        {
            get
            {
                if (instance.ppinstance == 0)
                {
                    instance.ppinstance = this.Handle.ToInt32();
                }
                return instance;
            }
        }

        public static implicit operator PPInstance(Instance instance)
        {
            return instance.PPInstance;
        }

        public void LogToConsole(PPLogLevel level, object value)
        {
            PPBConsole.Log(this, level, new Var(value));
        }

        public void LogToConsoleWithSource(PPLogLevel level,
                                          string source,
                                          object value)
        {
            PPBConsole.LogWithSource(this, level, new Var(source), new Var(value));
        }

    }
}
