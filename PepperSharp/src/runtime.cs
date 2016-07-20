//
// Runtime.cs: Basic features for the Pepper C# runtime
//
// Authors:
//   Kenneth Pouncey (kenneth.pouncey@xamarin.com)
//
// Copyright 2016 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    public interface INativeObject
    {
        IntPtr Handle { get; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class Resource : INativeObject, IDisposable
    {
        internal IntPtr handle;
        public IntPtr Handle => handle;

        public Resource(IntPtr handle)
        {
            this.handle = handle;
            Console.WriteLine($"PPResource constructed {handle}");
        }

        protected Resource() { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // TODO: do something here
        }

        ~Resource()
        {
            Dispose(false);
        }

    }

    public enum PassRef
    {
        PassRef = 0
    }
 

}