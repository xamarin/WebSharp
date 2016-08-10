using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PepperSharp
{

    /// <summary>
    /// Provides the C# implementation of a PP_ArrayOutput whose callback function is implemented 
    /// as a virtual call on a derived class. Do not use directly, use one of the
    /// derived classes below.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayOutputAdapterBase<T> : OutputAdapterBase<T>
    {
        private PPArrayOutput arrayOutput = new PPArrayOutput();

        public ArrayOutputAdapterBase() : base()
        {
            arrayOutput.GetDataBuffer = GetDataBufferThunk;
            arrayOutput.user_data = (IntPtr)GCHandle.Alloc(this);
        }

        internal virtual IntPtr GetDataBuffer(uint element_count,
                              uint element_size)
        {
            throw new NotImplementedException();
        }

        public PPArrayOutput PPArrayOutput
        {
            get { return arrayOutput; }
        }

        internal static IntPtr GetDataBufferThunk(IntPtr buffer, uint count, uint size)
        {
            GCHandle userDataHandle = (GCHandle)buffer;
            var buff = (ArrayOutputAdapterBase<T>)userDataHandle.Target;
            return buff.GetDataBuffer(count, size);
        }
    }

    /// <summary>
    /// This adapter provides functionality for implementing a PP_ArrayOutput
    /// structure as writing to a given vector object.
    ///
    /// This is generally used internally in the C++ wrapper objects to
    /// write into an output parameter supplied by the plugin. If the element size
    /// that the browser is writing does not match the size of the type we're using
    /// this will assert and return NULL (which will cause the browser to fail the
    /// call).
    ///
    /// Example that allows the browser to write into a given vector:
    ///   void DoFoo(std::vector<int>* results) {
    ///     ArrayOutputAdapter<int> adapter(results);
    ///     ppb_foo->DoFoo(adapter.pp_array_output());
    ///   }
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayOutputAdapter<T> : ArrayOutputAdapterBase<T>
    {

        protected IntPtr dataStorageHandle;

        protected ArrayOutputAdapter(T output) : base()
        {
            this.output = output;
        }

        protected ArrayOutputAdapter() : base()
        { }

        internal override IntPtr GetDataBuffer(uint count, uint size)
        {
            if (count == 0)
                return IntPtr.Zero;

            Type elementType;

            if (typeof(T).IsArray)
            {
                elementType = typeof(T).GetElementType();
            }
            else
            {
                elementType = typeof(T);
            }

            System.Diagnostics.Debug.Assert(size == Marshal.SizeOf(elementType), $"GetDataBuffer: requested Size {size} Size of Type {Marshal.SizeOf(elementType)}");

            if (size != Marshal.SizeOf(elementType))
                return IntPtr.Zero;

            output = (T)((object)Array.CreateInstance(elementType, count)); // Allocate space for our array

            dataStorageHandle = Marshal.AllocHGlobal((int)(count * size));  // Allocate unmanaged memory to be written into

            return dataStorageHandle;
        }

        public override T Output
        {
            get { return output; }
            set { output = value; }
        }

        public override object Adapter
        {
            get { return PPArrayOutput; }
        }
    }

    /// <summary>
    ///  This is used by the CompletionCallbackFactory system to collect the output
    /// parameters from an async function call. The collected data is then passed to
    /// the plugins callback function.
    ///
    /// You can also use it directly if you want to have an array output and aren't
    /// using the CompletionCallbackFactory. For example, if you're calling a
    /// PPAPI function DoFoo that takes a PP_OutputArray that is supposed to be
    /// writing integers, do this:
    ///
    ///    ArrayOutputAdapterWithStorage<int> adapter;
    ///    ppb_foo->DoFoo(adapter.pp_output_array());
    ///    const std::vector<int>& result = adapter.output();
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayOutputAdapterWithStorage<T> : ArrayOutputAdapter<T>
    {
        public ArrayOutputAdapterWithStorage() : base()
        { }

        public override T Output
        {
            get
            {
                if (dataStorageHandle == IntPtr.Zero)
                    return output;

                var ptrHandle = dataStorageHandle;
                var array = ((Array)(object)output);
                Type elementType;
                if (typeof(T).IsArray)
                {
                    elementType = typeof(T).GetElementType();
                }
                else
                {
                    elementType = typeof(T);
                }
                var ptrOffset = Marshal.SizeOf(elementType);

                for (int i = 0; i < array.Length; i++)
                {
                    array.SetValue(Marshal.PtrToStructure(ptrHandle, elementType), i);
                    ptrHandle += ptrOffset;
                }

                // let's clean ourselves up
                Marshal.FreeHGlobal(dataStorageHandle);
                ptrHandle = IntPtr.Zero;
                dataStorageHandle = IntPtr.Zero;

                return output;
            }
        }
    }

}
