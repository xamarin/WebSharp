using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{

    public delegate void CompletionCallbackFunc(PPError result);
    public delegate void CompletionCallbackFunc<T>(PPError result, T userData);
    public delegate void CompletionCallbackFunc<T, U>(PPError result, T userData1, U userData2);


    public class CompletionCallback
    {

        public PPCompletionCallback Callback { get; private set; }
        PPCompletionCallbackFunc callbackFunc;
        CompletionCallbackFunc compCallbackFunc;
        IntPtr userData;

        internal CompletionCallback(PPCompletionCallbackFunc callbackFunc, object userData = null, PPCompletionCallbackFlag flags = PPCompletionCallbackFlag.None)
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

        public CompletionCallback(CompletionCallbackFunc callbackFunc, PPCompletionCallbackFlag flags = PPCompletionCallbackFlag.None)
        {

            this.compCallbackFunc = callbackFunc;
            // if no callbackfunc is specified then 
            if (callbackFunc != null)
            {
                var ourCallback = new PPCompletionCallback();
                ourCallback.func = OnCallBack;
                ourCallback.flags = (int)flags;
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
        public CompletionCallback() : this((PPCompletionCallbackFunc)null)
        {  }

        void OnCallBack(IntPtr userData, int result)
        {
            GCHandle userDataHandle = (GCHandle)userData;
            var instance = (CompletionCallback)userDataHandle.Target;
            if (instance.compCallbackFunc != null)
            {
                instance.compCallbackFunc((PPError)result);
            }
            else
            {
                instance.callbackFunc(instance.userData, result);
                if (instance.userData != IntPtr.Zero)
                {
                    ((GCHandle)this.userData).Free();
                }
                userDataHandle.Free();
            }
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

    public class CompletionCallback<T>
    {

        public PPCompletionCallback Callback { get; private set; }
        CompletionCallbackFunc<T> compCallbackFunc;
        IntPtr userDataHandle;

        public CompletionCallback(CompletionCallbackFunc<T> callbackFunc, T userData, PPCompletionCallbackFlag flags = PPCompletionCallbackFlag.None)
        {
            this.compCallbackFunc = callbackFunc;
            // if no callbackfunc is specified then 
            if (callbackFunc != null)
            {
                if (userData == null)
                    this.userDataHandle = IntPtr.Zero;
                else
                {
                    GCHandle userDataHandle = GCHandle.Alloc(userData, GCHandleType.Pinned);
                    this.userDataHandle = (IntPtr)userDataHandle;
                }
                var ourCallback = new PPCompletionCallback();
                ourCallback.func = OnCallBack;
                ourCallback.flags = (int)flags;
                GCHandle userHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
                ourCallback.user_data = (IntPtr)userHandle;
                Callback = ourCallback;
            }
        }

        void OnCallBack(IntPtr userData, int result)
        {
            GCHandle instanceHandle = (GCHandle)userData;
            var instance = (CompletionCallback<T>)instanceHandle.Target;

            if (userDataHandle == IntPtr.Zero)
            {
                instance.compCallbackFunc((PPError)result, default(T));
            }
            else
            {
                GCHandle dataHandle = (GCHandle)userDataHandle;

                instance.compCallbackFunc((PPError)result, (T)dataHandle.Target);
                if (instance.userDataHandle != IntPtr.Zero)
                {
                    ((GCHandle)this.userDataHandle).Free();
                }
            }
            instanceHandle.Free();
        }

        public static implicit operator PPCompletionCallback(CompletionCallback<T> completionCallback)
        {
            return completionCallback.Callback;
        }
    }

    public class CompletionCallback<T, U>
    {

        public PPCompletionCallback Callback { get; private set; }
        CompletionCallbackFunc<T, U> compCallbackFunc;
        IntPtr userDataHandle;
        IntPtr userDataHandle2;

        public CompletionCallback(CompletionCallbackFunc<T, U> callbackFunc, T userData1, U userData2, PPCompletionCallbackFlag flags = PPCompletionCallbackFlag.None)
        {
            this.compCallbackFunc = callbackFunc;
            // if no callbackfunc is specified then 
            if (callbackFunc != null)
            {
                if (userData1 == null)
                    this.userDataHandle = IntPtr.Zero;
                else
                {
                    GCHandle userDataHandle = GCHandle.Alloc(userData1, GCHandleType.Pinned);
                    this.userDataHandle = (IntPtr)userDataHandle;
                }
                if (userData2 == null)
                    this.userDataHandle2 = IntPtr.Zero;
                else
                {
                    GCHandle userDataHandle2 = GCHandle.Alloc(userData2, GCHandleType.Pinned);
                    this.userDataHandle2 = (IntPtr)userDataHandle2;
                }
                var ourCallback = new PPCompletionCallback();
                ourCallback.func = OnCallBack;
                ourCallback.flags = (int)PPCompletionCallbackFlag.None;
                GCHandle userHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
                ourCallback.user_data = (IntPtr)userHandle;
                Callback = ourCallback;
            }
        }

        void OnCallBack(IntPtr userData, int result)
        {
            GCHandle instanceHandle = (GCHandle)userData;
            var instance = (CompletionCallback<T, U>)instanceHandle.Target;

            T target1;
            U target2;

            if (userDataHandle != IntPtr.Zero)
            {
                GCHandle dataHandle = (GCHandle)userDataHandle;
                target1 = (T)dataHandle.Target;
            }
            else
            {
                target1 = default(T);
            }
            if (userDataHandle2 != IntPtr.Zero)
            {
                GCHandle dataHandle = (GCHandle)userDataHandle2;
                target2 = (U)dataHandle.Target;
            }
            else
            {
                target2 = default(U);
            }

            instance.compCallbackFunc((PPError)result, target1, target2);

            if (instance.userDataHandle != IntPtr.Zero)
            {
                ((GCHandle)this.userDataHandle).Free();
            }
            if (instance.userDataHandle2 != IntPtr.Zero)
            {
                ((GCHandle)this.userDataHandle2).Free();
            }
            instanceHandle.Free();
        }

        public static implicit operator PPCompletionCallback(CompletionCallback<T, U> completionCallback)
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
