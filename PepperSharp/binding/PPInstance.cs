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

        public bool BindGraphics(PP_Resource graphics2d)
        {
            if (PPB_Instance.BindGraphics(Instance, graphics2d) == PP_Bool.PP_TRUE)
                return true;
            else
                return false;
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

    }
}
