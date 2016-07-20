using System;


namespace PepperSharp
{
    public partial struct PPResource : IDisposable
    {
        public static readonly PPResource Empty = new PPResource(0);

        internal PPResource(int resourceId)
        {
            ppresource = resourceId;
        }

        public PPResource(PPResource resource) : this(resource.ppresource)
        {
            if (!resource.IsEmpty)
                PPBCore.AddRefResource(this);
        }

        public PPResource(PassRef passRef, PPResource resource) : this(resource.ppresource)
        { }

        public override string ToString()
        {
            return ppresource.ToString();
        }

        public bool IsEmpty
        {
            get { return ppresource == 0;  }
        }

        public static bool operator == (PPResource s1, PPResource s2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(s1, s2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)s1 == null) || ((object)s2 == null))
            {
                return false;
            }

            // Return true if the fields match:
            return s1.ppresource == s2.ppresource;
        }

        public static bool operator !=(PPResource s1, PPResource s2)
        {
            return !(s1 == s2);
        }

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
        public PPResource Detach()  
        {
            var ret = new PPResource(ppresource);
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
                ppresource = 0; // set ourselves to empty
            }
        }

        #endregion

    }
}
