//
// Runtime.cs: Basic features for the Pepper C# runtime
//
// Authors:
//   Kenneth Pouncey (kenneth.pouncey@xamarin.com)
//
// Copyright 2016 Xamarin Inc
//
using System;
using System.Collections.Concurrent;

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
        static readonly int releaseLoopTime = (int)((1.0f / 30.0f) * 10000);

        // internal ConcurrentQueue that holds PPResource's that need to be released on the Main Thread
        internal static ConcurrentQueue<PPResource> resourceReleaseQueue = new ConcurrentQueue<PPResource>();

        public NativeInstance(IntPtr handle)
        {
            this.handle = handle;
            Console.WriteLine($"PPInstance constructed {handle}");

            // Start our release pump
            StartReleasePump();
        }

        /// <summary>
        /// The method is called to start a call back process on the Browsers main thread
        /// </summary>
        void StartReleasePump()
        {
            PPBCore.CallOnMainThread(releaseLoopTime, new CompletionCallback(ReleasePump), releaseLoopTime);
        }

        /// <summary>
        /// Method that will release all the PPResource's that have been placed on the queue to be released.
        /// </summary>
        /// <param name="result"></param>
        void ReleasePump(PPError result)
        {
            if (!resourceReleaseQueue.IsEmpty)
            {
                PPResource resourceToBeReleased;
                while (resourceReleaseQueue.TryDequeue(out resourceToBeReleased))
                {
                    PPBCore.ReleaseResource(resourceToBeReleased);
                }
            }
            PPBCore.CallOnMainThread(releaseLoopTime, new CompletionCallback(ReleasePump), releaseLoopTime);
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

        internal PPResource handle = PPResource.Empty;
        public PPResource Handle => handle;

        protected Resource() { }

        protected Resource(int resourceId)
        {
            handle.ppresource = resourceId;
        }

        public Resource(PPResource resource) : this(resource.ppresource)
        {
            if (!resource.IsEmpty)
                PPBCore.AddRefResource(this);
        }

        public Resource(PassRef passRef, PPResource resource) : this(resource.ppresource)
        { }

        public override string ToString()
        {
            return handle.ToString();
        }

        #region Equality

        public override bool Equals(object obj)
        {
            if (!(obj is Resource))
                return false;

            Resource comp = (Resource)obj;
            return (comp.handle == this.handle);
        }

        public override int GetHashCode()
        {
            return handle.ppresource;
        }

        public static bool operator ==(Resource resource1, Resource resource2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(resource1, resource2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)resource1 == null) || ((object)resource2 == null))
            {
                return false;
            }

            return resource1.handle == resource2.handle;
        }

        public static bool operator !=(Resource resource1, Resource resource2)
        {
            return !(resource1 == resource2);
        }

        #endregion

        /// <summary>
        /// Sets this resource to null. This releases ownership of the resource.
        /// Same as calling Dispose()
        /// </summary>
        void Clear()
        {
            Dispose();
        }

        /// <summary>
        /// This function releases ownership of this resource and returns it to the caller.
        /// 
        /// Note that the reference count on the resource is unchanged and the caller
        /// needs to release the resource.
        /// </summary>
        /// <returns>The detached <code>PPResource</code>.</returns>
        public Resource Detach()
        {
            var ret = new Resource(handle);
            Dispose();
            return ret;
        }

        #region Implement IDisposable.

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    // de-reference the managed resource.
                    NativeInstance.resourceReleaseQueue.Enqueue(Handle);
                }
                handle.ppresource = 0; // set ourselves to empty
            }
        }

        ~Resource ()
        {
            // This will call the Dispose method with true so that the resource can be
            // added to a queue to be released on the main thread.
            Dispose(true);
        }

        #endregion


        public bool IsEmpty
        {
            get { return handle.IsEmpty;  }
        }

        public static implicit operator PPResource(Resource resource)
        {
            return resource.Handle;
        }

    }

    public enum PassRef
    {
        PassRef = 0
    }

}