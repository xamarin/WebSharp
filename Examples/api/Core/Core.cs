using System;

using PepperSharp;

namespace Core
{
    public class Core : Instance
    {
        PPCompletionCallback callback;
        double last_receive_time_;

        /// Handler for messages coming in from the browser via postMessage().  The
        /// @a var_message will contain the requested delay time.
        ///
        /// @param[in] var_message The message posted by the browser.
        public override void HandleMessage(PPVar message)
        {
            int delay = ((Var)message).AsInt();
            if (delay > 0)
            {
                // If a delay is requested, issue a callback after delay ms.
                last_receive_time_ = PPBCore.GetTimeTicks();
                callback = new PPCompletionCallback();
                callback.func = DelayedPost;
                PPBCore.CallOnMainThread(delay, callback, 0);
            }
            else
            {
                // If no delay is requested, reply immediately with zero time elapsed.
                var msg = new Var(0);
                PostMessage(msg);
            }
        }

        void DelayedPost(IntPtr userData, int result)
        {
            // Send the time elapsed until the callbacked fired.
            var msg = new Var(PPBCore.GetTimeTicks() - last_receive_time_);
            PostMessage(msg);
        }
    }
}
