using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class MessageLoop : Resource
    {

        Task loopTask;
        internal MessageLoop(Instance instance)
        {
            handle = PPBMessageLoop.Create(instance);
        }

        internal MessageLoop(PPResource resource) : base(resource)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MessageLoop GetForMainThread()
        {
            return new MessageLoop(PPBMessageLoop.GetForMainThread());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MessageLoop GetCurrent()
        {
            return new MessageLoop(PPBMessageLoop.GetCurrent());
        }

        /// <summary>
        /// Sets the given message loop resource as being the associated message loop
        /// for the currently running thread.
        ///
        /// You must call this function exactly once on a thread before making any
        /// PPAPI calls. A message loop can only be attached to one thread, and the
        /// message loop can not be changed later. The message loop will be attached
        /// as long as the thread is running or until you quit with should_destroy
        /// set to PP_TRUE.
        ///
        /// If this function fails, attempting to run the message loop will fail.
        /// Note that you can still post work to the message loop: it will get queued
        /// up should the message loop eventually be successfully attached and run.
        ///
        /// </summary>
        /// <returns>
        ///   - PPError.Ok: The message loop was successfully attached to the thread and is
        ///     ready to use.
        ///   - PPError.Badresource: The given message loop resource is invalid.
        ///   - PPError.Inprogress: The current thread already has a message loop
        ///     attached. This will always be the case for the main thread, which has
        ///     an implicit system-created message loop attached.
        ///   - PPError.WrongThread: The current thread type can not have a message
        ///     loop attached to it. See the interface level discussion about these
        ///     special threads, which include realtime audio threads.
        /// </returns>
        public PPError AttachToCurrentThread()
        {
            return (PPError)PPBMessageLoop.AttachToCurrentThread(this);
        }

        /// <summary>
        /// Runs the thread message loop. Running the message loop is required for
        /// you to get issued completion callbacks on the thread.
        ///
        /// The message loop identified by the argument must have been previously
        /// successfully attached to the current thread.
        ///
        /// You may not run nested message loops. Since the main thread has an
        /// implicit message loop that the system runs, you may not call Run on the
        /// main thread.
        ///
        /// </summary>
        /// <returns>
        ///   - PPError.Ok: The message loop was successfully run. Note that on
        ///     success, the message loop will only exit when you call PostQuit().
        ///   - PPError.Badresource: The given message loop resource is invalid.
        ///   - PPError.WrongThread: You are attempting to run a message loop that
        ///     has not been successfully attached to the current thread. Call
        ///     AttachToCurrentThread().
        ///   - PPError.Inprogress: You are attempting to call Run in a nested
        ///     fashion (Run is already on the stack). This will occur if you attempt
        ///     to call run on the main thread's message loop (see above).
        /// </returns>
        public PPError Run()
        {
            return (PPError)PPBMessageLoop.Run(this);
        }

        ///// <summary>
        ///// Schedules work to run on the given message loop. This may be called from
        ///// any thread. Posted work will be executed in the order it was posted when
        ///// the message loop is Run().
        /////
        ///// The completion callback will be called with PPError.Ok as the "result"
        ///// parameter if it is run normally. It is good practice to check for PP_OK
        ///// and return early otherwise.
        /////
        ///// The "required" flag on the completion callback is ignored. If there is an
        ///// error posting your callback, the error will be returned from PostWork and
        ///// the callback will never be run (because there is no appropriate place to
        ///// run your callback with an error without causing unexpected threading
        ///// problems). If you associate memory with the completion callback (for
        ///// example, you're using the C++ CompletionCallbackFactory), you will need to
        ///// free this or manually run the callback. See "Desctruction and error
        ///// handling" above.
        /////
        /////
        ///// You can call this function before the message loop has started and the
        ///// work will get queued until the message loop is run. You can also post
        ///// work after the message loop has exited as long as should_destroy was
        ///// false. It will be queued until the next invocation of Run().
        ///// </summary>
        ///// <param name="callback">A pointer to the completion callback to execute from the
        ///// message loop.
        ///// </param>
        ///// <param name="delay">The number of milliseconds to delay execution of the given
        ///// completion callback. Passing 0 means it will get queued normally and
        ///// executed in order.
        ///// </param>
        ///// <returns></returns>
        //public PPError PostWork(PPCompletionCallback callback, long delay = 0)
        //{
        //    return (PPError)PPBMessageLoop.PostWork(this, callback, delay);
        //}

        /// <summary>
        /// Schedules work to run on the given message loop. This may be called from
        /// any thread. Posted work will be executed in the order it was posted when
        /// the message loop is Run().
        ///
        /// The completion callback will be called with PPError.Ok as the "result"
        /// parameter if it is run normally. It is good practice to check for PP_OK
        /// and return early otherwise.
        ///
        /// The "required" flag on the completion callback is ignored. If there is an
        /// error posting your callback, the error will be returned from PostWork and
        /// the callback will never be run (because there is no appropriate place to
        /// run your callback with an error without causing unexpected threading
        /// problems). If you associate memory with the completion callback (for
        /// example, you're using the C++ CompletionCallbackFactory), you will need to
        /// free this or manually run the callback. See "Desctruction and error
        /// handling" above.
        ///
        ///
        /// You can call this function before the message loop has started and the
        /// work will get queued until the message loop is run. You can also post
        /// work after the message loop has exited as long as should_destroy was
        /// false. It will be queued until the next invocation of Run(). 
        /// </summary>
        /// <param name="action">An Action that serves as completion callback to execute from the
        /// message loop.
        /// </param>
        /// <param name="delay">The number of milliseconds to delay execution of the given
        /// completion callback. Passing 0 means it will get queued normally and
        /// executed in order.
        /// </param>
        /// <returns></returns>
        public PPError PostWork(Action<PPError> action, long delay = 0)
        {
            TaskCompletionSource<PPError> tcs = new TaskCompletionSource<PPError>();
            var fun = new CompletionCallbackFunc(action);
            var callback = new CompletionCallback(fun);

            var res = (PPError)PPBMessageLoop.PostWork(this, callback, delay);
            return res;
        }

        /// <summary>
        /// Schedules work to run on the given message loop. This may be called from
        /// any thread. Posted work will be executed in the order it was posted when
        /// the message loop is Run().
        ///
        /// The completion callback will be called with PPError.Ok as the "result"
        /// parameter if it is run normally. It is good practice to check for PP_OK
        /// and return early otherwise.
        ///
        /// The "required" flag on the completion callback is ignored. If there is an
        /// error posting your callback, the error will be returned from PostWork and
        /// the callback will never be run (because there is no appropriate place to
        /// run your callback with an error without causing unexpected threading
        /// problems). If you associate memory with the completion callback (for
        /// example, you're using the C++ CompletionCallbackFactory), you will need to
        /// free this or manually run the callback. See "Desctruction and error
        /// handling" above.
        ///
        ///
        /// You can call this function before the message loop has started and the
        /// work will get queued until the message loop is run. You can also post
        /// work after the message loop has exited as long as should_destroy was
        /// false. It will be queued until the next invocation of Run(). 
        /// </summary>
        /// <param name="action">An Action<PPError, T> that serves as completion callback to execute from the
        /// message loop that can be passed an object to serve as user datas to be passed to the delegate.
        /// </param>
        /// <param name="delay">The number of milliseconds to delay execution of the given
        /// completion callback. Passing 0 means it will get queued normally and
        /// executed in order.
        /// </param>
        /// <returns></returns>
        public PPError PostWork<T>(Action<PPError, T> action, T userData, long delay = 0)
        {
            TaskCompletionSource<PPError> tcs = new TaskCompletionSource<PPError>();
            var fun = new CompletionCallbackFunc<T>(action);
            var callback = new CompletionCallback<T>(fun, userData);

            var res = (PPError)PPBMessageLoop.PostWork(this, callback, delay);
            return res;
        }

        /// <summary>
        /// Schedules work to run on the given message loop. This may be called from
        /// any thread. Posted work will be executed in the order it was posted when
        /// the message loop is Run().
        ///
        /// The completion callback will be called with PPError.Ok as the "result"
        /// parameter if it is run normally. It is good practice to check for PP_OK
        /// and return early otherwise.
        ///
        /// The "required" flag on the completion callback is ignored. If there is an
        /// error posting your callback, the error will be returned from PostWork and
        /// the callback will never be run (because there is no appropriate place to
        /// run your callback with an error without causing unexpected threading
        /// problems). If you associate memory with the completion callback (for
        /// example, you're using the C++ CompletionCallbackFactory), you will need to
        /// free this or manually run the callback. See "Desctruction and error
        /// handling" above.
        ///
        ///
        /// You can call this function before the message loop has started and the
        /// work will get queued until the message loop is run. You can also post
        /// work after the message loop has exited as long as should_destroy was
        /// false. It will be queued until the next invocation of Run(). 
        /// </summary>
        /// <param name="action">An Action<PPError, T, U> that serves as completion callback to execute from the
        /// message loop that can be passed two objects to serve as user datas to be passed to the delegate.
        /// </param>
        /// <param name="delay">The number of milliseconds to delay execution of the given
        /// completion callback. Passing 0 means it will get queued normally and
        /// executed in order.
        /// </param>
        /// <returns></returns>
        public PPError PostWork<T,U>(Action<PPError, T, U> action, T userData1, U userData2, long delay = 0)
        {
            TaskCompletionSource<PPError> tcs = new TaskCompletionSource<PPError>();
            var fun = new CompletionCallbackFunc<T, U>(action);
            var callback = new CompletionCallback<T, U>(fun, userData1, userData2);

            var res = (PPError)PPBMessageLoop.PostWork(this, callback, delay);
            return res;
        }

        /// <summary>
        /// Posts a quit message to the given message loop's work queue. Work posted
        /// before that point will be processed before quitting.
        ///
        /// This may be called on the message loop registered for the current thread,
        /// or it may be called on the message loop registered for another thread. It
        /// is an error to attempt to quit the main thread loop.
        ///
        /// If you quit a message loop without setting shouldDestroy, it will still
        /// be attached to the thread and you can still run it again by calling Run()
        /// again. If you destroy it, it will be detached from the current thread.
        /// </summary>
        /// <param name="shouldDestroy">Marks the message loop as being in a destroyed
        /// state and prevents further posting of messages.</param>
        /// <returns></returns>
        public PPError PostQuit(bool shouldDestroy)
        {
            return (PPError)PPBMessageLoop.PostQuit(this, shouldDestroy ? PPBool.True : PPBool.False);
        }

        public Task<PPError> Start()
        {
            var tcs = new TaskCompletionSource<PPError>();
            loopTask = Task.Run(() =>
            {

                // Try to attach to the current thread
                var messageLoopResult = AttachToCurrentThread();
                if (messageLoopResult != PPError.Ok)
                    tcs.SetResult(messageLoopResult);

                // Start the message loop on the thread
                messageLoopResult = Run();
                if (messageLoopResult != PPError.Ok)
                    tcs.SetResult(messageLoopResult);
            });

            return tcs.Task;
        }

    }
}
