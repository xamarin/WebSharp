using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    public partial struct PPCompletionCallback
    {

        public PPCompletionCallback(PPCompletionCallbackFunc func)
        {
            this.func = func;
            this.user_data = IntPtr.Zero;
            this.flags = (int)PPCompletionCallbackFlag.None;
        }

        public PPCompletionCallback(PPCompletionCallbackFunc func, object userData)
        {
            this.func = func;
            if (userData == null)
                this.user_data = IntPtr.Zero;
            else
            {
                GCHandle userHandle = GCHandle.Alloc(userData);
                this.user_data = (IntPtr)userHandle;
            }
            this.flags = (int)PPCompletionCallbackFlag.None;
        }

        public static object GetUserDataAsObject(IntPtr userData)
        {
            if (userData == IntPtr.Zero)
                return null;

            // back to object (in callback function):
            GCHandle userDataHandle = (GCHandle)userData;
            return userDataHandle.Target as object;

        }

        public static T GetUserData<T>(IntPtr userData)
        {
            if (userData == IntPtr.Zero)
                return default(T);

            GCHandle userDataHandle = (GCHandle)userData;
            return (T)userDataHandle.Target;

        }

    }
}
