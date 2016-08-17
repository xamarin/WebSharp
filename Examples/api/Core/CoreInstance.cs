using System;

using PepperSharp;

namespace CoreInstance
{
    public class CoreInstance : Instance
    {
        CompletionCallback callback;
        double last_receive_time_;

        public CoreInstance (IntPtr handle) : base(handle)
        {
            HandleMessage += OnReceiveMessage;
        }

        /// Handler for messages coming in from the browser via postMessage().  The
        /// @a var_message will contain the requested delay time.
        ///
        /// @param[in] var_message The message posted by the browser.
        private void OnReceiveMessage(object sender, Var message)
        {
            int delay = message.AsInt();
            if (delay > 0)
            {
                
                // If a delay is requested, issue a callback after delay ms.
                last_receive_time_ = Core.TimeTicks;
                callback = new CompletionCallback(DelayedPost);
                Core.CallOnMainThread(callback, delay);
            }
            else
            {
                // If no delay is requested, reply immediately with zero time elapsed.
                var msg = new Var(0);
                PostMessage(msg);
            }
        }

        void DelayedPost(PPError result)
        {
            // Send the time elapsed until the callbacked fired.
            var msg = new Var(Core.TimeTicks - last_receive_time_);
            PostMessage(msg);
        }
    }
}
