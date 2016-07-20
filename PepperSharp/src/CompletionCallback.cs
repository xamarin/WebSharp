using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    public class CompletionCallback
    {

        public PPCompletionCallback Callback { get; private set; }
        PPCompletionCallbackFunc callbackFunc;
        IntPtr userData;

        public CompletionCallback(PPCompletionCallbackFunc callbackFunc, object userData = null, PPCompletionCallbackFlag flags = PPCompletionCallbackFlag.None)
        {
            this.callbackFunc = callbackFunc;
            if (userData == null)
                this.userData = IntPtr.Zero;
            else
            {
                GCHandle userDataHandle = GCHandle.Alloc(userData, GCHandleType.Pinned);
                this.userData = (IntPtr)userDataHandle;
            }
            var ourCallback = new PPCompletionCallback();
            ourCallback.func = OnCallBack;
            ourCallback.flags = (int)PPCompletionCallbackFlag.None; 
            GCHandle userHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
            ourCallback.user_data = (IntPtr)userHandle;
            Callback = ourCallback;
        }

        void OnCallBack(IntPtr userData, int result)
        {
            GCHandle userDataHandle = (GCHandle)userData;
            var instance = (CompletionCallback)userDataHandle.Target;
            instance.callbackFunc(instance.userData, result);
            if (instance.userData != IntPtr.Zero)
            {
                ((GCHandle)this.userData).Free();
            }
            userDataHandle.Free();
        }

        //public CompletionCallback(PPCompletionCallbackFunc func, object userData)
        //    : this(func)
        //{
        //    if (userData == null)
        //        this.userData = IntPtr.Zero;
        //    else
        //    {
        //        GCHandle userHandle = GCHandle.Alloc(userData, GCHandleType.Pinned);
        //        this.userData = (IntPtr)userHandle;
        //    }
        //}

        public static object GetUserData(IntPtr userData)
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
