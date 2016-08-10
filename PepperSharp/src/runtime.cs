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
    public class NativeInstance : INativeObject, IDisposable
    {
        internal IntPtr handle;
        public IntPtr Handle => handle;

        public NativeInstance(IntPtr handle)
        {
            this.handle = handle;
            Console.WriteLine($"PPInstance constructed {handle}");
        }

        protected NativeInstance() { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // TODO: do something here
        }

        ~NativeInstance()
        {
            Dispose(false);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public class Resource : IDisposable
    {
        PPResource ppResource = PPResource.Empty;
        
        protected Resource(PPResource resource)
        {
            ppResource.ppresource = resource.ppresource;
        }

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

        public static implicit operator PPResource(Resource resource)
        {
            return resource.ppResource;
        }

    }

    public enum PassRef
    {
        PassRef = 0
    }
 

}