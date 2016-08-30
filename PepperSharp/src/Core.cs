using System;

namespace PepperSharp
{
    public static class Core
    {
        /// <summary>
        /// AddRefResource() increments the reference count for the provided
        /// <code>resource</code>.
        /// </summary>
        /// <param name="resource">A <code>Resource</code> corresponding to a
        /// resource.</param>
        public static void AddRefResource(Resource resource)
        {
            PPBCore.AddRefResource(resource);
        }

        /// <summary>
        /// ReleaseResource() decrements the reference count for the provided
        /// <code>resource</code>. The resource will be deallocated if the
        /// reference count reaches zero.
        /// </summary>
        /// <param name="resource">A <code>Resource</code> corresponding to a
        /// resource.</param>
        public static void ReleaseResource(Resource resource)
        {
            PPBCore.ReleaseResource(resource);
        }

        /// <summary>
        /// Property that returns the "wall clock time" according to the
        /// browser.
        /// </summary>
        public static double Time
        {
            get
            {
                return PPBCore.GetTime();
            }
        }

        /// <summary>
        /// Property that returns the "tick time" according to the browser.
        /// This clock is used by the browser when passing some event times to the
        /// module (for example, using the
        /// <code>PInputEvent.TimestampSeconds</code> field). It is not
        /// correlated to any actual wall clock time (like Core.Time). Because
        /// of this, it will not change if the user changes their computer clock.
        /// </summary>
        public static double TimeTicks
        {
            get
            {
                return PPBCore.GetTimeTicks();
            }
        }

        /// <summary>
        /// CallOnMainThread() schedules work to be executed on the main pepper
        /// thread after the specified delay. The delay may be 0 to specify a call
        /// back as soon as possible.
        ///
        /// The |result| parameter will just be passed as the second argument to the
        /// callback. Many applications won't need this, but it allows a module to
        /// emulate calls of some callbacks which do use this value.
        ///
        /// <strong>Note:</strong> CallOnMainThread(), even when used from the main
        /// thread with a delay of 0 milliseconds, will never directly invoke the
        /// callback.  Even in this case, the callback will be scheduled
        /// asynchronously.
        ///
        /// <strong>Note:</strong> If the browser is shutting down or if the module
        /// has no instances, then the callback function may not be called.
        ///
        /// </summary>
        /// <param name="callback">An <code>Action<PPError></code> callback function
        /// that the browser will call after the specified delay.
        /// </param>
        /// <param name="delay_in_milliseconds">An int delay in milliseconds.  Default 0</param>
        /// <param name="result">An int that the browser will pass to the given
        /// <code>Action<PPError></PPError></code>.  Default 0</param>
        public static void CallOnMainThread(Action<PPError> action,
                        int delay_in_milliseconds = 0,
                        int result = 0)
        {
            PPBCore.CallOnMainThread(delay_in_milliseconds, new CompletionCallback(new CompletionCallbackFunc(action)), result);
        }

        /// <summary>
        /// Property that returns true if the current thread is the main pepper
        /// thread.
        ///
        /// This function is useful for implementing sanity checks, and deciding if
        /// dispatching using CallOnMainThread() is required.
        /// </summary>
        public static bool IsMainThread
        {
            get
            {
                return PPBCore.IsMainThread() == PPBool.True ? true : false;
            }
        }
    }
}
