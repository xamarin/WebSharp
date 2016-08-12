using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class MouseLock : Instance
    {

        public event EventHandler<PPError> MouseLocked;
        public event EventHandler MouseUnLocked;

        protected MouseLock(IntPtr handle) : base(handle) { }

        // Called by the browser when mouselock is lost.  This happens when the NaCl
        // module exits fullscreen mode.
        void MouseLockLost()
        {
            MouseUnLocked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// LockMouse() requests the mouse to be locked.
        ///
        /// While the mouse is locked, the cursor is implicitly hidden from the user.
        /// Any movement of the mouse will generate a
        /// <code>PPINPUTEVENTTYPE.MOUSEMOVE</code> event. The
        /// <code>Position</code> function in <code>InputEvent()</code>
        /// reports the last known mouse position just as mouse lock was
        /// entered. The <code>Movement</code> function provides relative
        /// movement information indicating what the change in position of the mouse
        /// would be had it not been locked.
        ///
        /// The browser may revoke the mouse lock for reasons including (but not
        /// limited to) the user pressing the ESC key, the user activating another
        /// program using a reserved keystroke (e.g. ALT+TAB), or some other system
        /// event.
        /// </summary>
        /// <returns>Error Code</returns>
        public PPError LockMouse() 
        {
            return (PPError)PPBMouseLock.LockMouse(this, new CompletionCallback(
                (result) =>
                {
                    MouseLocked?.Invoke(this, result);
                }
                
                ));
        }

        /// <summary>
        /// UnlockMouse causes the mouse to be unlocked, allowing it to track user
        /// movement again. This is an asynchronous operation. The module instance
        /// will be notified using the EventHandler LostMouseLock interface when it
        /// has lost the mouse lock.
        /// </summary>
        public void UnlockMouse()
        {
            PPBMouseLock.UnlockMouse(this);
        }

    }
}
