using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PepperSharp
{

    public delegate void CompletionCallbackWithOutputFunc<T>(PPError result, T output);

    public delegate void CompletionCallbackWithOutputFunc<T, U>(PPError result, T output, U userData1);

    public delegate void CompletionCallbackWithOutputFunc<T, U, V>(PPError result, T output, U userData1, V userData2);

    public class CompletionCallbackWithOutput<T> 
    {

        public PPCompletionCallback Callback;

        CompletionCallbackWithOutputFunc<T> CompletionCallbackFunc;
        public OutputAdapterBase<T> OutputAdapter { get; private set; }

        // used internally for when we have to pass the PPArrayOutput as ref
        internal PPArrayOutput ArrayOutput;

        public CompletionCallbackWithOutput (CompletionCallbackWithOutputFunc<T> completionCallback, int flags = 0)
        {
            CompletionCallbackFunc = completionCallback;

            if (typeof(T).IsArray)
            {
                OutputAdapter = new ArrayOutputAdapterWithStorage<T>();
                if (OutputAdapter.Adapter is PPArrayOutput)
                    ArrayOutput = (PPArrayOutput)OutputAdapter.Adapter;
            }
            else
            {
                this.OutputAdapter = new APIArgumentAdapter<T>();
            }

            Callback = new PPCompletionCallback();
            Callback.func = OnCallBack;
            GCHandle userHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
            Callback.user_data = (IntPtr)userHandle;
            
            Callback.flags = flags;

        }

        void OnCallBack(IntPtr userData, int result)
        {
            GCHandle userDataHandle = (GCHandle)userData;
            var instance = (CompletionCallbackWithOutput<T>)userDataHandle.Target;
            if (typeof(T).IsArray)
                instance.CompletionCallbackFunc((PPError)result, instance.OutputAdapter.Output);
            else
            {
                instance.CompletionCallbackFunc((PPError)result, instance.OutputAdapter.output);
            }
            userDataHandle.Free();
        }

        public static implicit operator PPCompletionCallback(CompletionCallbackWithOutput<T> completionCallback)
        {
            return completionCallback.Callback;
        }

        public static implicit operator PPArrayOutput(CompletionCallbackWithOutput<T> completionCallback)
        {
            return (PPArrayOutput)completionCallback.OutputAdapter.Adapter;
        }
    }

    public class CompletionCallbackWithOutput<T, U>
    {

        public PPCompletionCallback Callback;

        CompletionCallbackWithOutputFunc<T, U> CompletionCallbackFunc;
        public OutputAdapterBase<T> OutputAdapter { get; private set; }
        U userData1;

        // used internally for when we have to pass the PPArrayOutput as ref
        internal PPArrayOutput ArrayOutput;

        public CompletionCallbackWithOutput(CompletionCallbackWithOutputFunc<T, U> completionCallback, U userData1, IntPtr? userData = null, int flags = 0)
        {
            CompletionCallbackFunc = completionCallback;
            this.userData1 = userData1;

            if (typeof(T).IsArray)
            {
                OutputAdapter = new ArrayOutputAdapterWithStorage<T>();
                if (OutputAdapter.Adapter is PPArrayOutput)
                    ArrayOutput = (PPArrayOutput)OutputAdapter.Adapter;
            }
            else
            {
                this.OutputAdapter = new APIArgumentAdapter<T>(); 
            }

            Callback = new PPCompletionCallback();
            Callback.func = OnCallBack;
            GCHandle userHandle = GCHandle.Alloc(this);
            Callback.user_data = (IntPtr)userHandle;
            Callback.flags = flags;

        }

        void OnCallBack(IntPtr userData, int result)
        {
            GCHandle userDataHandle = (GCHandle)userData;
            var instance = (CompletionCallbackWithOutput<T,U>)userDataHandle.Target;
            if (typeof(T).IsArray)
                instance.CompletionCallbackFunc((PPError)result, instance.OutputAdapter.Output, instance.userData1);
            else
            {
                instance.CompletionCallbackFunc((PPError)result, instance.OutputAdapter.output, instance.userData1);
            }
            userDataHandle.Free();
        }

        public static implicit operator PPCompletionCallback(CompletionCallbackWithOutput<T, U> completionCallback)
        {
            return completionCallback.Callback;
        }

        public static implicit operator PPArrayOutput(CompletionCallbackWithOutput<T, U> completionCallback)
        {
            return (PPArrayOutput)completionCallback.OutputAdapter.Adapter;
        }
    }

    public class CompletionCallbackWithOutput<T, U, V>
    {

        public PPCompletionCallback Callback;

        CompletionCallbackWithOutputFunc<T, U, V> CompletionCallbackFunc;
        public OutputAdapterBase<T> OutputAdapter { get; private set; }
        U userData1;
        V userData2;

        // used internally for when we have to pass the PPArrayOutput as ref
        internal PPArrayOutput ArrayOutput;

        public CompletionCallbackWithOutput(CompletionCallbackWithOutputFunc<T, U, V> completionCallback, U userData1, V userData2, int flags = 0)
        {
            CompletionCallbackFunc = completionCallback;
            this.userData1 = userData1;
            this.userData2 = userData2;

            if (typeof(T).IsArray)
            {
                OutputAdapter = new ArrayOutputAdapterWithStorage<T>();
                if (OutputAdapter.Adapter is PPArrayOutput)
                    ArrayOutput = (PPArrayOutput)OutputAdapter.Adapter;
            }
            else
            {
                this.OutputAdapter = new APIArgumentAdapter<T>(); 
            }

            Callback = new PPCompletionCallback();
            Callback.func = OnCallBack;
            GCHandle userHandle = GCHandle.Alloc(this);
            Callback.user_data = (IntPtr)userHandle;
            Callback.flags = flags;

        }

        void OnCallBack(IntPtr userData, int result)
        {
            GCHandle userDataHandle = (GCHandle)userData;
            var instance = (CompletionCallbackWithOutput<T, U, V>)userDataHandle.Target;
            if (typeof(T).IsArray)
                instance.CompletionCallbackFunc((PPError)result, instance.OutputAdapter.Output, instance.userData1, instance.userData2);
            else
            {
                instance.CompletionCallbackFunc((PPError)result, instance.OutputAdapter.output, instance.userData1, instance.userData2);
            }
            userDataHandle.Free();
        }

        public static implicit operator PPCompletionCallback(CompletionCallbackWithOutput<T, U, V> completionCallback)
        {
            return completionCallback.Callback;
        }

        public static implicit operator PPArrayOutput(CompletionCallbackWithOutput<T, U, V> completionCallback)
        {
            return (PPArrayOutput)completionCallback.OutputAdapter.Adapter;
        }
    }

    public class APIArgumentAdapter<T> : OutputAdapterBase<T>
    {
        public APIArgumentAdapter()
        {
            output = default(T);
        }
    }

    public abstract class OutputAdapterBase<T> : IOutputAdapter<T>
    {
        public T output;

        public virtual object Adapter
        {
            get { return output; }
        }
        public virtual T Output
        {
            get { return output; }
            set { output = value; }
        }
    }

    internal interface IOutputAdapter<T>
    {
        
        object Adapter { get;  }
        T Output { get; set; }
    }
}
