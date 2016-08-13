using System;
using System.ComponentModel;

namespace PepperSharp
{
    public partial class Instance : NativeInstance
    {
        protected Instance() { throw new PlatformNotSupportedException("Can not create an instace of PPInstance"); }
        protected Instance(IntPtr handle) : base(handle) { }

        /// <summary>
        /// Event when the view information for the Instance has changed.
        /// </summary>
        public event EventHandler<View> ViewChanged;

        /// <summary>
        /// Event when an instance has gained or lost focus.
        /// </summary>
        public event EventHandler<bool> FocusChanged;

        /// <summary>
        /// Event when the browser calls PostMessage() on the DOM element for the instance in JavaScript.
        /// </summary>
        public event EventHandler<Var> ReceiveMessage;

        // Define a class to hold custom Cancelable Event event info
        public class InitializeEventArgs : CancelEventArgs
        {
            internal InitializeEventArgs(int argc, string[] argn, string[] argv)
            {
                this.argc = argc;
                this.argn = argn;
                this.argv = argv;
                Cancel = false;
            }
            private int argc;
            private string[] argn;
            private string[] argv;

            public int Count
            {
                get { return argc; }
            }

            public string[] Names
            {
                get { return argn;  }
            }

            public string[] Values
            {
                get { return argv;  }
            }
        }

        /// <summary>
        /// Event when the browser calls PostMessage() on the DOM element for the instance in JavaScript.
        /// </summary>
        public delegate void InitializeDelegateAndHandler(object sender, InitializeEventArgs args);
        public event InitializeDelegateAndHandler Initialize;

        /// <summary>
        /// Raise event to intialize the instance with the provided arguments. This
        /// event will be raised immediately after the instance object is constructed.
        /// </summary>
        /// <remarks>
        /// The OnInitialize method also enables derived classes to handle the event without attaching 
        /// a delegate. This is the preferred technique for handling the event in a derived class.
        /// </remarks>
        /// <param name="args">An instance of InitializeEventArgs that is cancelable</param>
        /// <returns>true if the event was not canceled and falce if the event was canceled for some reason.</returns>
        protected virtual bool OnInitialize(InitializeEventArgs args)
        {
            var handler = Initialize;
            if (handler != null)
            {
                bool cancel = false;
                foreach (InitializeDelegateAndHandler subscriber in handler.GetInvocationList())
                {
                    subscriber.Invoke(this, args);
                    if (args.Cancel)
                    {
                        cancel = true;
                        break;
                    }
                }

                return !cancel;
            }

            return true;
        }

        bool Init(int argc, string[] argn, string[] argv)
        {

            return OnInitialize(new InitializeEventArgs(argc, argn, argv));
        }

        /// <summary>
        /// Raises the ViewChange event when the view information for the Instance
        /// has changed. See the <code>View</code> object for information.
        ///
        /// Most implementations will want to check if the size and user visibility
        /// changed, and either resize themselves or start/stop generating updates.
        /// </summary>
        /// <remarks>
        /// The OnViewChanged method also enables derived classes to handle the event without attaching 
        /// a delegate. This is the preferred technique for handling the event in a derived class.
        /// </remarks>
        /// <param name="view">The view object that contains the new view properties</param>
        protected virtual void OnViewChanged(View view)
        {
            ViewChanged?.Invoke(this, view);
        }

        void DidChangeView(PPResource view)
        {
            OnViewChanged(new View(view));
        }

        /// <summary>
        /// Raises the FocusChanged event when an instance has gained or lost focus.
        ///
        /// Having focus means that keyboard events will be sent to the instance.  An instance's default 
        /// condition is that it will not have focus.
        ///
        /// The focus flag takes into account both browser tab and window focus as well as focus of the plugin 
        /// element on the page. In order to be deemed to have focus, the browser window must be topmost, 
        /// the tab must be selected in the window, and the instance must be the focused element on the page.
        /// 
        /// Note:Clicks on instances will give focus only if you handle the click event. 
        /// Return true from HandleInputEvent in InputEvent (or use unfiltered events) to signal that the 
        /// click event was handled. Otherwise, the browser will bubble the event and give focus to the element 
        /// on the page that actually did end up consuming it.If you're not getting focus, check to make sure
        /// you're either requesting them via RequestInputEvents() (which implicitly marks all input events as 
        /// consumed) or via RequestFilteringInputEvents() and returning true from your event handler.
        /// </summary>
        /// <remarks>
        /// The OnFocusChanged method also enables derived classes to handle the event without attaching 
        /// a delegate. This is the preferred technique for handling the event in a derived class.
        /// </remarks>
        /// <param name="hasFocus">Indicates the new focused state of the instance.</param>
        /// 
        protected virtual void OnFocusChanged(bool hasFocus)
        {
            FocusChanged?.Invoke(this, hasFocus);
        }

        void DidChangeFocus(bool hasFocus)
        {
            OnFocusChanged(hasFocus);
        }

        /// <summary>
        /// HandleInputEvent() handles input events from the browser.
        ///
        /// The default implementation does nothing and returns false.
        /// 
        /// In order to receive input events, you must register for them by calling RequestInputEvents() or 
        /// RequestFilteringInputEvents(). By default, no events are delivered.
        /// 
        /// If the event was handled, it will not be forwarded to any default handlers.If it was not handled, 
        /// it may be dispatched to a default handler.So it is important that an instance respond accurately 
        /// with whether event propagation should continue.
        /// 
        /// Event propagation also controls focus.If you handle an event like a mouse event, typically the 
        /// instance will be given focus. Returning false from a filtered event handler or not registering for an event type means that the click will be given to a lower part of the page and your instance will not receive focus.This allows an instance to be partially transparent, where clicks on the transparent areas will behave like clicks to the underlying page.
        /// 
        /// In general, you should try to keep input event handling short. Especially for filtered input events,
        /// the browser or page may be blocked waiting for you to respond.
        /// 
        /// The caller of this function will maintain a reference to the input event resource during this call.
        /// Unless you take a reference to the resource to hold it for later, you don't need to release it.
        /// 
        /// Note: If you're not receiving input events, make sure you register for the event classes you want 
        /// by calling RequestInputEvents or RequestFilteringInputEvents. If you're still not receiving 
        /// keyboard input events, make sure you're returning true (or using a non-filtered event handler) for
        /// mouse events. Otherwise, the instance will not receive focus and keyboard events will not be sent.
        /// 
        /// Refer to RequestInputEvents and RequestFilteringInputEvents for further information.
        /// </summary>
        /// <param name="inputEvent">The event to handle.</param>
        /// <returns>true if the event was handled, false if not. If you have registered to filter this class of events by calling RequestFilteringInputEvents, and you return false, the event will be forwarded to the page (and eventually the browser) for the default handling. For non-filtered events, the return value will be ignored.</returns>
        public virtual bool HandleInputEvent(PPResource inputEvent)
        {
            return false;
        }

        public virtual bool HandleDocumentLoad(PPResource urlLoader)
        {
            return false;
        }

        /// <summary>
        /// Raises the RecieveMessage event when the browser calls PostMessage() on the DOM element for 
        /// the instance in JavaScript.
        /// 
        /// Note that PostMessage() in the JavaScript interface is asynchronous, meaning JavaScript execution 
        /// will not be blocked while OnRecieveMessage is processing the message.
        /// 
        /// When converting JavaScript arrays, any object properties whose name is not an array index are 
        /// ignored.  When passing arrays and objects, the entire reference graph will be converted and 
        /// transferred.  If the reference graph has cycles, the message will not be sent and an error will 
        /// be logged to the console.
        /// </summary>
        /// <remarks>
        /// The OnReceiveMessage method also enables derived classes to handle the event without attaching 
        /// a delegate. This is the preferred technique for handling the event in a derived class.
        /// </remarks>
        /// <param name="message">A Var which has been converted from a JavaScript value. JavaScript 
        /// array/object types are supported from Chrome M29 onward. All JavaScript values are copied 
        /// when passing them to the plugin.
        /// </param>
        protected virtual void OnReceiveMessage(Var message)
        {
            ReceiveMessage?.Invoke(this, message);
        }


        void HandleMessage (PPVar message)
        {
            OnReceiveMessage(new Var(message));
        }

        /// <summary>
        /// BindGraphics() binds the given graphics as the current display surface.
        /// 
        /// The contents of this device is what will be displayed in the instance's area on the web page. 
        /// The device must be a 2D or a 3D device.
        ///
        /// You can pass an is_null() (default constructed) Graphics2D as the device parameter to unbind all 
        /// devices from the given instance. The instance will then appear transparent. Re-binding the same 
        /// device will return true and will do nothing.
        /// 
        /// Any previously-bound device will be released. It is an error to bind a device when it is already
        /// bound to another instance. If you want to move a device between instances, first unbind it from 
        /// the old one, and then rebind it to the new one.
        ///
        /// Binding a device will invalidate that portion of the web page to flush the contents of the new 
        /// device to the screen.
        /// </summary>
        /// <param name="graphics2d">A Graphics2D to bind</param>
        /// <returns>true if bind was successful or false if the device was not the correct type. On success, a reference to the device will be held by the instance, so the caller can release its reference if it chooses.</returns>
        public bool BindGraphics(PPResource graphics2d)
        {
            return PPBInstance.BindGraphics(this, graphics2d) == PPBool.True ? true : false;
        }

        /// <summary>
        /// asynchronously invokes any listeners for message events on the DOM element for the given instance.
        /// </summary>
        /// <param name="message"></param>
        public void PostMessage(object message)
        {
            if (message is Var)
                PPBMessaging.PostMessage(this, (Var)message);
            else
                PPBMessaging.PostMessage(this, new Var(message));
        }

        PPInstance instance = new PPInstance();
        public PPInstance PPInstance
        {
            get
            {
                if (instance.ppinstance == 0)
                {
                    instance.ppinstance = this.Handle.ToInt32();
                }
                return instance;
            }
        }

        public static implicit operator PPInstance(Instance instance)
        {
            return instance.PPInstance;
        }

        public void LogToConsole(PPLogLevel level, object value)
        {
            if (value is Var)
                PPBConsole.Log(this, level, (Var)value);
            else
                PPBConsole.Log(this, level, new Var(value));
        }

        public void LogToConsoleWithSource(PPLogLevel level,
                                          string source,
                                          object value)
        {
            if (value is Var)
                PPBConsole.LogWithSource(this, level, new Var(source), (Var)value);
            else
                PPBConsole.LogWithSource(this, level, new Var(source), new Var(value));
        }

        /// <summary>
        /// RequestInputEvents() requests that input events corresponding to the given input events are delivered to the instance.
        /// 
        /// By default, no input events are delivered.Call this function with the classes of events you are
        /// interested in to have them be delivered to the instance. Calling this function will override any 
        /// previous setting for each specified class of input events(for example, if you previously called 
        /// RequestFilteringInputEvents(), this function will set those events to non-filtering mode).
        /// 
        /// Input events may have high overhead, so you should only request input events that your plugin will 
        /// actually handle.For example, the browser may do optimizations for scroll or touch events that can
        /// be processed substantially faster if it knows there are no non-default receivers for that message. 
        /// Requesting that such messages be delivered, even if they are processed very quickly, may have a 
        /// noticeable effect on the performance of the page.
        /// 
        /// When requesting input events through this function, the events will be delivered and not bubbled 
        /// to the page.This means that even if you aren't interested in the message, no other parts of the 
        /// page will get the message.
        /// </summary>
        /// <param name="eventClasses">A combination of flags from PP_InputEvent_Class that identifies the classes of events the instance is requesting. The flags are combined by logically ORing their values.</param>
        /// <returns>PP_OK if the operation succeeded, PP_ERROR_BADARGUMENT if instance is invalid, or PP_ERROR_NOTSUPPORTED if one of the event class bits were illegal. In the case of an invalid bit, all valid bits will be applied and only the illegal bits will be ignored.</returns>
        public PPError RequestInputEvents(PPInputEventClass eventClasses)
        {
            return (PPError)PPBInputEvent.RequestInputEvents(this, (uint)eventClasses);
        }

        /// <summary>
        /// RequestFilteringInputEvents() requests that input events corresponding to the given input events 
        /// are delivered to the instance for filtering.
        /// 
        /// By default, no input events are delivered.In most cases you would register to receive events by 
        /// calling RequestInputEvents(). In some cases, however, you may wish to filter events such that 
        /// they can be bubbled up to the DOM.In this case, register for those classes of events using this 
        /// function instead of RequestInputEvents(). Keyboard events must always be registered in filtering 
        /// mode.
        /// 
        /// Filtering input events requires significantly more overhead than just delivering them to the 
        /// instance. As such, you should only request filtering in those cases where it's absolutely
        /// necessary. The reason is that it requires the browser to stop and block for the instance to 
        /// handle the input event, rather than sending the input event asynchronously. This can have 
        /// significant overhead.
        /// </summary>
        /// <param name="eventClasses">A combination of flags from PP_InputEvent_Class that identifies the classes of events the instance is requesting. The flags are combined by logically ORing their values.</param>
        /// <returns>OK if the operation succeeded, BADARGUMENT if instance is invalid, or NOTSUPPORTED if 
        /// one of the event class bits were illegal. In the case of an invalid bit, all valid bits will be 
        /// applied and only the illegal bits will be ignored.
        /// </returns>
        public PPError RequestFilteringInputEvents(PPInputEventClass eventClasses)
        {
            return (PPError)PPBInputEvent.RequestFilteringInputEvents(this, (uint)eventClasses);
        }

        /// <summary>
        /// IsFullFrame() determines if the instance is full-frame (repr).
        /// 
        ///Such an instance represents the entire document in a frame rather than an embedded resource.
        ///This can happen if the user does a top-level navigation or the page specifies an iframe to a 
        ///resource with a MIME type registered by the module.
        /// </summary>
        public bool IsFullFrame
        {
            get { return PPBInstance.IsFullFrame(this) == PPBool.True ? true : false; }
        }

        /// <summary>
        /// ClearInputEventRequest() requests that input events corresponding to the given input classes no longer be delivered to the instance.
        /// 
        /// By default, no input events are delivered.If you have previously requested input events using 
        /// RequestInputEvents() or RequestFilteringInputEvents(), this function will unregister handling 
        /// for the given instance.This will allow greater browser performance for those events.
        /// 
        /// Note: You may still get some input events after clearing the flag if they were dispatched before the request was cleared. For example, if there are 3 mouse move events waiting to be delivered, and you clear the mouse event class during the processing of the first one, you'll still receive the next two. You just won't get more events generated.
        /// </summary>
        /// <param name="eventClasses">A combination of flags from PP_InputEvent_Class that identifies the classes of events the instance is no longer interested in.</param>
        public void ClearInputEventRequest(PPInputEventClass eventClasses)
        {
            PPBInputEvent.ClearInputEventRequest(this, (uint)eventClasses);
        }


    }
}
