using System;

using PepperSharp;

namespace CoreInstance
{
    public class CoreInstance : Instance
    {
        CompletionCallback callback;
        double lastReceiveTime;
        Action<PPError> action;

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
                action = DelayedPost;
                //action = delegate (PPError result) { DelayedPost(result); };
                //action = e => DelayedPost(e);
                
                // If a delay is requested, issue a callback after delay ms.
                lastReceiveTime = Core.TimeTicks;
                Core.CallOnMainThread(action, delay);
            }
            else
            {
                // If no delay is requested, reply immediately with zero time elapsed.
                PostMessage(0);
            }
        }

        void DelayedPost(PPError result)
        {
            // Send the time elapsed until the callbacked fired.
            var msg = Core.TimeTicks - lastReceiveTime;
            PostMessage(msg);
        }
    }
}
