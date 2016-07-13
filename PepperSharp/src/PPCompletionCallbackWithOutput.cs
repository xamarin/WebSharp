using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PepperSharp
{

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void PPCompletionCallbackWithOutputFunc<T>(int result, T[] output);

    public struct PPCompletionCallbackWithOutput<T> 
    {

        public PPCompletionCallback callback;

        PPCompletionCallbackWithOutputFunc<T> func;
        public ArrayOutputAdapter<T> OutputAdapter { get; private set; }

        public PPCompletionCallbackWithOutput (PPCompletionCallbackWithOutputFunc<T> callbackFunction, ArrayOutputAdapter<T> outputAdapter = null, IntPtr? userData = null, int flags = 0)
        {
            this.func = callbackFunction;
            if (outputAdapter == null)
                this.OutputAdapter = new ArrayOutputAdapterWithStorage<T>();
            else
                this.OutputAdapter = outputAdapter;

            callback = new PPCompletionCallback();
            callback.func = OnCallBack;
            GCHandle userHandle = GCHandle.Alloc(this);
            callback.user_data = (IntPtr)userHandle;
            callback.flags = flags;

        }

        void OnCallBack(IntPtr userData, int result)
        {
            GCHandle userDataHandle = (GCHandle)userData;
            var instance = (PPCompletionCallbackWithOutput<T>)userDataHandle.Target;
            instance.func(result, instance.OutputAdapter.Output);
            userDataHandle.Free();
        }


    }
}
