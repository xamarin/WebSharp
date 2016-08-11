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
        protected PPResource ppResource = PPResource.Empty;

        protected Resource() { }

        protected Resource(int resourceId)
        {
            ppResource.ppresource = resourceId;
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
            return ppResource.ToString();
        }

        #region Equality

        public override bool Equals(object obj)
        {
            if (!(obj is Resource))
                return false;

            Resource comp = (Resource)obj;
            return (comp.ppResource == this.ppResource);
        }

        public override int GetHashCode()
        {
            return ppResource.ppresource;
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

            return resource1.ppResource == resource2.ppResource;
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
            var ret = new Resource(ppResource);
            Dispose();
            return ret;
        }

        #region Implement IDisposable.

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    // de-reference the managed resource.
                    PPBCore.ReleaseResource(this);
                }
                ppResource.ppresource = 0; // set ourselves to empty
            }
        }
        ~Resource ()
        {
            // TODO: Look at releasing in some type of disposal pumping in
            // case we are in threads.
            Dispose(false);
        }

        #endregion


        public bool IsEmpty
        {
            get { return ppResource.IsEmpty;  }
        }

        //        Resource& Resource::operator=(const Resource& other) {
        //  if (!other.is_null())
        //    Module::Get()->core()->AddRefResource(other.pp_resource_);
        //  if (!is_null())
        //    Module::Get()->core()->ReleaseResource(pp_resource_);
        //        pp_resource_ = other.pp_resource_;
        //  return *this;
        //}

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