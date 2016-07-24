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
            // if no callbackfunc is specified then 
            if (callbackFunc != null)
            {
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
        }


        /// <summary>
        /// The default constructor will create a blocking
        /// <code>CompletionCallback</code> that can be passed to a method to
        /// indicate that the calling thread should be blocked until the asynchronous
        /// operation corresponding to the method completes.
        ///
        /// <strong>Note:</strong> Blocking completion callbacks are only allowed from
        /// from background threads.
        /// 
        /// </summary>
        public CompletionCallback() : this(null)
        {  }

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

        public static implicit operator PPCompletionCallback(CompletionCallback completionCallback)
        {
            return completionCallback.Callback;
        }
    }

    public class BlockUntilComplete : CompletionCallback
    {

        public BlockUntilComplete () : base()
        {
            
        }
    }
}
