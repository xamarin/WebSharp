# PepperSharp.Instance Class Reference
## Description

An instance is a rectangle on a web page that is managed by a PepperPlugin module.</br>             
- An instance may have a dimension of width = 0 and height = 0, meaning that the instance does not have any visible component on the web page.
- An instance is created by including an `<embed></embed>` element in a web page.
   - The `<embed></embed>` element references a Dot Net class that implements the PepperSharp API and loads the appropriate version of the PepperPlugin module.
   - A PepperPlugin module may be included in a web page multiple times by using multiple `<embed></embed>` elements that refer to the class implementation; in this case the Native Client runtime system loads the module once and creates multiple instances that are managed by the module.

_C# code_

``` c#
    using System;
    
    using PepperSharp;
    
    namespace HelloWorld
    {
       public class HelloWorld : Instance
       {
           public HelloWorld(IntPtr handle) : base(handle)
           {
               Initialize += OnInitialize;
           }
   
           private void OnInitialize(object sender, InitializeEventArgs args)
           {
               LogToConsoleWithSource(PPLogLevel.Log, "HelloWorld.HelloWorld", "Hello from C#");
           }
       }
    }
    
```



---


## Constructors
| Name | Description |
| --- | ----------- |
| [#ctor](#mpeppersharpinstancector) | Default constructor not supported - Throws error if called</br>             |
| [#ctor (System.IntPtr)](#mpeppersharpinstancectorsystemintptr) | A constructor used when creating managed representations of unmanaged objects; Called by the PepperPlugin Native Client implementation.</br>             |


---


## Properties
| Name | Description |
| --- | ----------- |
| [PPInstance](#ppeppersharpinstanceppinstance) | Getter that returnes the unmanaged pointer (Handle) of this instance.</br>             |
| [IsFullFrame](#ppeppersharpinstanceisfullframe) | Getter that returns if the instance is full-frame (repr).</br>            </br>Such an instance represents the entire document in a frame rather than an embedded resource.</br>This can happen if the user does a top-level navigation or the page specifies an iframe to a </br>resource with a MIME type registered by the module.</br>             |
| [IsFullScreen](#ppeppersharpinstanceisfullscreen) | Getter and Setter that checks whether the module instance is currently in</br>fullscreen mode or sets the instance to full screen mode.</br>             </br>The setter switches the module instance to and from fullscreen</br>mode.</br>             </br>The transition to and from fullscreen mode is asynchronous. During the</br>transition, IsFullscreen() will return the previous value and</br>no 2D or 3D device can be bound. The transition ends at DidChangeView()</br>when IsFullscreen() returns the new value. You might receive other</br>DidChangeView() calls while in transition.</br>            </br>The transition to fullscreen mode can only occur while the browser is</br>processing a user gesture, even if `true` is returned.</br>             </br>              |
| [ScreenSize](#ppeppersharpinstancescreensize) | Gets the size of the screen in pixels. The module instance</br>will be resized to this size when IsFullscreen is set to true to enter</br>fullscreen mode.</br>             |


---


## Methods
| Name | Description |
| --- | ----------- |
| [OnInitialize (PepperSharp.Instance.InitializeEventArgs)](#mpeppersharpinstanceoninitializepeppersharpinstanceinitializeeventargs) | Raise event to intialize the instance with the provided arguments. This</br>event will be raised immediately after the instance object is constructed.</br>             |
| [Init (System.Int32, System.String[], System.String[])](#mpeppersharpinstanceinitsystemint32systemstringsystemstring) | This is the entry point of the Init call that just passes the information on to the event handler</br>             |
| [OnViewChanged (PepperSharp.View)](#mpeppersharpinstanceonviewchangedpeppersharpview) | Raises the ViewChange event when the view information for the Instance</br>has changed. See the `View` object for information.</br>            </br>             Most implementations will want to check if the size and user visibility</br>             changed, and either resize themselves or start/stop generating updates.</br>              |
| [OnFocusChanged (System.Boolean)](#mpeppersharpinstanceonfocuschangedsystemboolean) | Raises the FocusChanged event when an instance has gained or lost focus.</br>            </br>Having focus means that keyboard events will be sent to the instance.  An instance's default </br>condition is that it will not have focus.</br>            </br>The focus flag takes into account both browser tab and window focus as well as focus of the plugin </br>element on the page. In order to be deemed to have focus, the browser window must be topmost, </br>the tab must be selected in the window, and the instance must be the focused element on the page.</br>              |
| [OnInputEvents (PepperSharp.InputEvent)](#mpeppersharpinstanceoninputeventspeppersharpinputevent) | Raises the HandleInput event from the browser.</br>            </br>The default implementation does nothing and returns false.</br>             </br>In order to receive input events, you must register for them by calling RequestInputEvents() or </br>RequestFilteringInputEvents(). By default, no events are delivered.</br>             </br>If the event was handled, it will not be forwarded to any default handlers.If it was not handled, </br>it may be dispatched to a default handler.So it is important that an instance respond accurately </br>with whether event propagation should continue.</br>             </br>Event propagation also controls focus.If you handle an event like a mouse event, typically the </br>instance will be given focus. Returning false from a filtered event handler or not registering for </br>an event type means that the click will be given to a lower part of the page and your instance </br>will not receive focus.This allows an instance to be partially transparent, where clicks on the </br>transparent areas will behave like clicks to the underlying page.</br>             </br>In general, you should try to keep input event handling short. Especially for filtered input events,</br>the browser or page may be blocked waiting for you to respond.</br>             </br>The caller of this function will maintain a reference to the input event resource during this call.</br>Unless you take a reference to the resource to hold it for later, you don't need to release it.</br>              |
| [OnMouseDown (PepperSharp.Instance.MouseEventArgs)](#mpeppersharpinstanceonmousedownpeppersharpinstancemouseeventargs) | Notification that a mouse button was pressed.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>              |
| [OnMouseUp (PepperSharp.Instance.MouseEventArgs)](#mpeppersharpinstanceonmouseuppeppersharpinstancemouseeventargs) | Notification that a mouse button was released.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>              |
| [OnMouseEnter (PepperSharp.Instance.MouseEventArgs)](#mpeppersharpinstanceonmouseenterpeppersharpinstancemouseeventargs) | Notification that the mouse entered the instance's bounds.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>              |
| [OnMouseLeave (PepperSharp.Instance.MouseEventArgs)](#mpeppersharpinstanceonmouseleavepeppersharpinstancemouseeventargs) | Notification that a mouse left the instance's bounds.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>              |
| [OnMouseMove (PepperSharp.Instance.MouseEventArgs)](#mpeppersharpinstanceonmousemovepeppersharpinstancemouseeventargs) | Notification that a mouse button was moved when it is over the instance or dragged out of it.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>              |
| [OnContextMenu (PepperSharp.Instance.MouseEventArgs)](#mpeppersharpinstanceoncontextmenupeppersharpinstancemouseeventargs) | Notification that a context menu should be shown.</br>             </br>This message will be sent when the user right-clicks or performs another OS-specific mouse command that should open a context menu.When this event is delivered depends on the system, on some systems (Mac) it will delivered after the mouse down event, and on others (Windows) it will be delivered after the mouse up event.</br>            </br>You will always get the normal mouse events. For example, you may see OnMouseDown, OnContextMenu, OnMouseUp or OnMouseDown, OnMouseUp, OnContextMenu.</br>             </br>The return value from the event handler determines if the context menu event will be passed to the page when you are using filtered input events(via RequestFilteringInputEvents()). In non-filtering mode the event will never be propagated and no context menu will be displayed.If you are handling mouse events in filtering mode, you may want to return true from this event even if you do not support a context menu to suppress the default one.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>              |
| [OnWheel (PepperSharp.Instance.WheelEventArgs)](#mpeppersharpinstanceonwheelpeppersharpinstancewheeleventargs) | Notification that the scroll wheel was used.</br>            </br>Register for this event using the PPInputEventClass.Wheel class.</br>              |
| [OnKeyUp (PepperSharp.Instance.KeyboardEventArgs)](#mpeppersharpinstanceonkeyuppeppersharpinstancekeyboardeventargs) | Notification that a key was released.</br>             </br>Register for this event using the PPInputEventClass.Keyboard class.</br>              |
| [OnKeyDown (PepperSharp.Instance.KeyboardEventArgs)](#mpeppersharpinstanceonkeydownpeppersharpinstancekeyboardeventargs) | Notification that a key was pressed.</br>             </br>This does not necessarily correspond to a character depending on the key and language.  Use the OnKeyChar for character input.</br>            </br>Register for this event using the PPInputEventClass.Keyboard class.</br>              |
| [OnKeyChar (PepperSharp.Instance.KeyboardEventArgs)](#mpeppersharpinstanceonkeycharpeppersharpinstancekeyboardeventargs) | Notification that a character was typed.</br>             </br>Use this for text input. Key down events may generate 0, 1, or more than one character event depending on the key, locale, and operating system.</br>            </br>Register for this event using the PPInputEventClass.Keyboard class.</br>              |
| [OnRawKeyDown (PepperSharp.Instance.KeyboardEventArgs)](#mpeppersharpinstanceonrawkeydownpeppersharpinstancekeyboardeventargs) | Notification that a key transitioned from "up" to "down".</br>             </br>Register for this event using the PPInputEventClass.Keyboard class.</br>              |
| [OnTouchStart (PepperSharp.TouchInputEvent)](#mpeppersharpinstanceontouchstartpeppersharptouchinputevent) | Notification that a finger was placed on a touch-enabled device.</br>             </br>Register for this event using the PPInputEventClass.Touch class.</br>              |
| [OnTouchEnd (PepperSharp.TouchInputEvent)](#mpeppersharpinstanceontouchendpeppersharptouchinputevent) | Notification that a finger was released on a touch-enabled device.</br>             </br>Register for this event using the PPInputEventClass.Touch class.</br>              |
| [OnTouchCancel (PepperSharp.TouchInputEvent)](#mpeppersharpinstanceontouchcancelpeppersharptouchinputevent) | Notification that a touch event was canceled.</br>             </br>Register for this event using the PPInputEventClass.Touch class.</br>              |
| [OnTouchMove (PepperSharp.TouchInputEvent)](#mpeppersharpinstanceontouchmovepeppersharptouchinputevent) | Notification that a finger was moved on a touch-enabled device.</br>             </br>Register for this event using the PPInputEventClass.Touch class.</br>              |
| [OnIMECompositionStart (PepperSharp.IMEInputEvent)](#mpeppersharpinstanceonimecompositionstartpeppersharpimeinputevent) | Notification that an input method composition process has just started.</br>             </br>Register for this event using the PPInputEventClass.IME class.</br>              |
| [OnIMECompositionEnd (PepperSharp.IMEInputEvent)](#mpeppersharpinstanceonimecompositionendpeppersharpimeinputevent) | Notification that an input method composition process has completed.</br>             </br>Register for this event using the PPInputEventClass.IME class.</br>              |
| [OnIMECompositionUpdate (PepperSharp.IMEInputEvent)](#mpeppersharpinstanceonimecompositionupdatepeppersharpimeinputevent) | Notification that the input method composition string is updated.</br>             </br>Register for this event using the PPInputEventClass.IME class.</br>              |
| [OnIMEText (PepperSharp.IMEInputEvent)](#mpeppersharpinstanceonimetextpeppersharpimeinputevent) | Notification that an input method committed a string.</br>             </br>Register for this event using the PPInputEventClass.IME class.</br>              |
| [OnHandleMessage (PepperSharp.Var)](#mpeppersharpinstanceonhandlemessagepeppersharpvar) | Raises the HandleMessage event when the browser calls PostMessage() on the DOM element for </br>the instance in JavaScript.</br>             |
| [BindGraphics (PepperSharp.PPResource)](#mpeppersharpinstancebindgraphicspeppersharpppresource) | BindGraphics() binds the given graphics as the current display surface.</br>             </br>The contents of this device is what will be displayed in the instance's area on the web page. </br>The device must be a 2D or a 3D device.</br>            </br>You can pass an is_null() (default constructed) Graphics2D as the device parameter to unbind all </br>devices from the given instance. The instance will then appear transparent. Re-binding the same </br>device will return true and will do nothing.</br>             </br>Any previously-bound device will be released. It is an error to bind a device when it is already</br>bound to another instance. If you want to move a device between instances, first unbind it from </br>the old one, and then rebind it to the new one.</br>            </br>Binding a device will invalidate that portion of the web page to flush the contents of the new </br>device to the screen.</br>              |
| [PostMessage (System.Object)](#mpeppersharpinstancepostmessagesystemobject) | Asynchronously invokes any listeners for message events on the DOM element for the given instance.</br>             |
| [op_Implicit (PepperSharp.Instance)~PepperSharp.PPInstance](#mpeppersharpinstanceopimplicitpeppersharpinstance~peppersharpppinstance) | Implicit conversion to unmanaged pointer (Handle).  Used by PInvoke.</br>             |
| [LogToConsole (PepperSharp.PPLogLevel, System.Object)](#mpeppersharpinstancelogtoconsolepeppersharppploglevelsystemobject) | Logs the given message to the JavaScript console associated with the given plugin instance with the given logging level.</br>            </br>The name of the plugin issuing the log message will be automatically prepended to the message.The value may be any type of Var.</br>             |
| [LogToConsoleWithSource (PepperSharp.PPLogLevel, System.String, System.Object)](#mpeppersharpinstancelogtoconsolewithsourcepeppersharppploglevelsystemstringsystemobject) | Logs a message to the console with the given source information rather than using the internal PPAPI plugin name.</br>            </br>The regular log function will automatically prepend the name of your plugin to the message as the "source" of the message.  </br>Some plugins may wish to override this. </br>             |
| [RequestInputEvents (PepperSharp.PPInputEventClass)](#mpeppersharpinstancerequestinputeventspeppersharpppinputeventclass) | Requests that input events corresponding to the given input events are delivered to the instance.</br>            </br>By default, no input events are delivered.Call this function with the classes of events you are</br>interested in to have them be delivered to the instance. Calling this function will override any </br>previous setting for each specified class of input events(for example, if you previously called </br>RequestFilteringInputEvents(), this function will set those events to non-filtering mode).</br>            </br>Input events may have high overhead, so you should only request input events that your plugin will </br>actually handle.For example, the browser may do optimizations for scroll or touch events that can</br>be processed substantially faster if it knows there are no non-default receivers for that message. </br>Requesting that such messages be delivered, even if they are processed very quickly, may have a </br>noticeable effect on the performance of the page.</br>            </br>When requesting input events through this function, the events will be delivered and not bubbled </br>to the page.This means that even if you aren't interested in the message, no other parts of the </br>page will get the message.</br>             |
| [RequestFilteringInputEvents (PepperSharp.PPInputEventClass)](#mpeppersharpinstancerequestfilteringinputeventspeppersharpppinputeventclass) | Requests that input events corresponding to the given input events </br>are delivered to the instance for filtering.</br>            </br>By default, no input events are delivered.In most cases you would register to receive events by </br>calling RequestInputEvents(). In some cases, however, you may wish to filter events such that </br>they can be bubbled up to the DOM.In this case, register for those classes of events using this </br>function instead of RequestInputEvents(). Keyboard events must always be registered in filtering </br>mode.</br>            </br>Filtering input events requires significantly more overhead than just delivering them to the </br>instance. As such, you should only request filtering in those cases where it's absolutely</br>necessary. The reason is that it requires the browser to stop and block for the instance to </br>handle the input event, rather than sending the input event asynchronously. This can have </br>significant overhead.</br>             |
| [ClearInputEventRequest (PepperSharp.PPInputEventClass)](#mpeppersharpinstanceclearinputeventrequestpeppersharpppinputeventclass) | ClearInputEventRequest() requests that input events corresponding to the given input classes no longer be delivered to the instance.</br>            </br>By default, no input events are delivered.If you have previously requested input events using </br>RequestInputEvents() or RequestFilteringInputEvents(), this function will unregister handling </br>for the given instance.This will allow greater browser performance for those events.</br>             |
| [SetCursor (PepperSharp.PPMouseCursorType, PepperSharp.ImageData, System.Nullable{PepperSharp.PPPoint})](#mpeppersharpinstancesetcursorpeppersharpppmousecursortypepeppersharpimagedatasystemnullablepeppersharppppoint) | Sets the given mouse cursor. The mouse cursor will be in effect whenever</br>the mouse is over the given instance until it is set again by another</br>call.</br>            This function allows setting both system defined mouse cursors and</br>custom cursors. To set a system-defined cursor, pass the type you want</br>and set the custom image to a default-constructor ImageData object.</br>To set a custom cursor, set the type to</br>            `PPMouseCursorType.CUSTOM` and specify your image and hot</br>            spot.</br>             |
| [GamepadsSample (PepperSharp.GamepadsSampleData@)](#mpeppersharpinstancegamepadssamplepeppersharpgamepadssampledata) | Allows retrieving data from gamepad/joystick devices that are connected to the system.</br>             |


---


## Events
| Event | Description |
| --- | ----------- |
| [ViewChanged](#epeppersharpinstanceviewchanged) | Event raised when the view information for the Instance has changed.</br>             |
| [FocusChanged](#epeppersharpinstancefocuschanged) | Event raised when an instance has gained or lost focus.</br>             |
| [HandleMessage](#epeppersharpinstancehandlemessage) | Event when the browser calls PostMessage() on the DOM element for the instance in JavaScript.</br>             |
| [InputEvents](#epeppersharpinstanceinputevents) | Event when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript.</br>             |
| [MouseDown](#epeppersharpinstancemousedown) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseDown InputEvent.</br>             |
| [MouseUp](#epeppersharpinstancemouseup) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseUp InputEvent.</br>             |
| [MouseEnter](#epeppersharpinstancemouseenter) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseEnter InputEvent.</br>             |
| [MouseLeave](#epeppersharpinstancemouseleave) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseLeave InputEvent.</br>             |
| [MouseMove](#epeppersharpinstancemousemove) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseMove InputEvent.</br>             |
| [ContextMenu](#epeppersharpinstancecontextmenu) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a ContextMenu InputEvent.</br>             |
| [Wheel](#epeppersharpinstancewheel) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a Wheel InputEvent.</br>             |
| [KeyUp](#epeppersharpinstancekeyup) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a KeyboardInputEvent for KeyUp.</br>             |
| [KeyDown](#epeppersharpinstancekeydown) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a KeyboardInputEvent for KeyDown.</br>             |
| [KeyChar](#epeppersharpinstancekeychar) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a KeyboardInputEvent for Char.</br>             |
| [RawKeyDown](#epeppersharpinstancerawkeydown) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a KeyboardInputEvent for RawKeydown.</br>             |
| [TouchStart](#epeppersharpinstancetouchstart) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a TouchInputEvent for TouchStart.</br>             |
| [TouchEnd](#epeppersharpinstancetouchend) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a TouchInputEvent for TouchEnd.</br>             |
| [TouchCancel](#epeppersharpinstancetouchcancel) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a TouchInputEvent for TouchCancel.</br>             |
| [TouchMove](#epeppersharpinstancetouchmove) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a TouchInputEvent for TouchMove.</br>             |
| [IMECompositionEnd](#epeppersharpinstanceimecompositionend) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a IMEInputEvent for CompositionEnd.</br>             |
| [IMECompositionStart](#epeppersharpinstanceimecompositionstart) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a IMEInputEvent for CompositionStart.</br>             |
| [IMECompositionUpdate](#epeppersharpinstanceimecompositionupdate) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a IMEInputEvent for CompositionUpdate.</br>             |
| [IMEText](#epeppersharpinstanceimetext) | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a IMEInputEvent for Text.</br>             |
| [Initialize](#epeppersharpinstanceinitialize) | Handler for Initialize </br>             |


---

# T:PepperSharp.Instance

An instance is a rectangle on a web page that is managed by a PepperPlugin module.</br>             
- An instance may have a dimension of width = 0 and height = 0, meaning that the instance does not have any visible component on the web page.
- An instance is created by including an `<embed></embed>` element in a web page.
   - The `<embed></embed>` element references a Dot Net class that implements the PepperSharp API and loads the appropriate version of the PepperPlugin module.
   - A PepperPlugin module may be included in a web page multiple times by using multiple `<embed></embed>` elements that refer to the class implementation; in this case the Native Client runtime system loads the module once and creates multiple instances that are managed by the module.

_C# code_

``` c#
    using System;
    
    using PepperSharp;
    
    namespace HelloWorld
    {
       public class HelloWorld : Instance
       {
           public HelloWorld(IntPtr handle) : base(handle)
           {
               Initialize += OnInitialize;
           }
   
           private void OnInitialize(object sender, InitializeEventArgs args)
           {
               LogToConsoleWithSource(PPLogLevel.Log, "HelloWorld.HelloWorld", "Hello from C#");
           }
       }
    }
    
```



---
##### M:PepperSharp.Instance.#ctor

Default constructor not supported - Throws error if called</br>            

[[T:System.PlatformNotSupportedException|T:System.PlatformNotSupportedException]]: 



---
##### M:PepperSharp.Instance.#ctor(System.IntPtr)

A constructor used when creating managed representations of unmanaged objects; Called by the PepperPlugin Native Client implementation.</br>            



>This constructor is invoked by the Native Client runtime infrastructure to create a new managed representation for a pointer to an unmanaged pp:Instance object. You should not invoke this method directly</br>            

|Name | Description |
|-----|------|
|handle: |Pointer (handle) to the unmanaged Native Client instance|


---
##### E:PepperSharp.Instance.ViewChanged

Event raised when the view information for the Instance has changed.</br>            



---
##### E:PepperSharp.Instance.FocusChanged

Event raised when an instance has gained or lost focus.</br>            



---
##### E:PepperSharp.Instance.HandleMessage

Event when the browser calls PostMessage() on the DOM element for the instance in JavaScript.</br>            



---
##### E:PepperSharp.Instance.InputEvents

Event when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript.</br>            



---
##### E:PepperSharp.Instance.MouseDown

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseDown InputEvent.</br>            



---
##### E:PepperSharp.Instance.MouseUp

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseUp InputEvent.</br>            



---
##### E:PepperSharp.Instance.MouseEnter

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseEnter InputEvent.</br>            



---
##### E:PepperSharp.Instance.MouseLeave

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseLeave InputEvent.</br>            



---
##### E:PepperSharp.Instance.MouseMove

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a MouseMove InputEvent.</br>            



---
##### E:PepperSharp.Instance.ContextMenu

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a ContextMenu InputEvent.</br>            



---
##### E:PepperSharp.Instance.Wheel

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a Wheel InputEvent.</br>            



---
##### E:PepperSharp.Instance.KeyUp

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a KeyboardInputEvent for KeyUp.</br>            



---
##### E:PepperSharp.Instance.KeyDown

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a KeyboardInputEvent for KeyDown.</br>            



---
##### E:PepperSharp.Instance.KeyChar

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a KeyboardInputEvent for Char.</br>            



---
##### E:PepperSharp.Instance.RawKeyDown

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a KeyboardInputEvent for RawKeydown.</br>            



---
##### E:PepperSharp.Instance.TouchStart

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a TouchInputEvent for TouchStart.</br>            



---
##### E:PepperSharp.Instance.TouchEnd

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a TouchInputEvent for TouchEnd.</br>            



---
##### E:PepperSharp.Instance.TouchCancel

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a TouchInputEvent for TouchCancel.</br>            



---
##### E:PepperSharp.Instance.TouchMove

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a TouchInputEvent for TouchMove.</br>            



---
##### E:PepperSharp.Instance.IMECompositionEnd

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a IMEInputEvent for CompositionEnd.</br>            



---
##### E:PepperSharp.Instance.IMECompositionStart

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a IMEInputEvent for CompositionStart.</br>            



---
##### E:PepperSharp.Instance.IMECompositionUpdate

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a IMEInputEvent for CompositionUpdate.</br>            



---
##### E:PepperSharp.Instance.IMEText

Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript </br>that represents a IMEInputEvent for Text.</br>            



---
# T:PepperSharp.Instance.InitializeDelegateAndHandler

Event raised to initialize this instance with the provided arguments..</br>            



---
##### E:PepperSharp.Instance.Initialize

Handler for Initialize </br>            



---
##### M:PepperSharp.Instance.OnInitialize(PepperSharp.Instance.InitializeEventArgs)

Raise event to intialize the instance with the provided arguments. This</br>event will be raised immediately after the instance object is constructed.</br>            



>The OnInitialize method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>            

|Name | Description |
|-----|------|
|args: |An instance of InitializeEventArgs that is cancelable|
Returns: true if the event was not canceled and falce if the event was canceled for some reason.



---
##### M:PepperSharp.Instance.Init(System.Int32,System.String[],System.String[])

This is the entry point of the Init call that just passes the information on to the event handler</br>            



---
##### M:PepperSharp.Instance.OnViewChanged(PepperSharp.View)

Raises the ViewChange event when the view information for the Instance</br>has changed. See the `View` object for information.</br>            </br>             Most implementations will want to check if the size and user visibility</br>             changed, and either resize themselves or start/stop generating updates.</br>             



>The OnViewChanged method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

|Name | Description |
|-----|------|
|view: |The view object that contains the new view properties|


---
##### M:PepperSharp.Instance.OnFocusChanged(System.Boolean)

Raises the FocusChanged event when an instance has gained or lost focus.</br>            </br>Having focus means that keyboard events will be sent to the instance.  An instance's default </br>condition is that it will not have focus.</br>            </br>The focus flag takes into account both browser tab and window focus as well as focus of the plugin </br>element on the page. In order to be deemed to have focus, the browser window must be topmost, </br>the tab must be selected in the window, and the instance must be the focused element on the page.</br>             



>The OnFocusChanged method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             



> :bulb: Clicks on instances will give focus only if you handle the click event. </br>Return true from HandleInputEvent in InputEvent (or use unfiltered events) to signal that the </br>click event was handled. Otherwise, the browser will bubble the event and give focus to the element </br>on the page that actually did end up consuming it.If you're not getting focus, check to make sure</br>you're either requesting them via RequestInputEvents() (which implicitly marks all input events as </br>consumed) or via RequestFilteringInputEvents() and returning true from your event handler.</br>             

|Name | Description |
|-----|------|
|hasFocus: |Indicates the new focused state of the instance.|


---
##### M:PepperSharp.Instance.OnInputEvents(PepperSharp.InputEvent)

Raises the HandleInput event from the browser.</br>            </br>The default implementation does nothing and returns false.</br>             </br>In order to receive input events, you must register for them by calling RequestInputEvents() or </br>RequestFilteringInputEvents(). By default, no events are delivered.</br>             </br>If the event was handled, it will not be forwarded to any default handlers.If it was not handled, </br>it may be dispatched to a default handler.So it is important that an instance respond accurately </br>with whether event propagation should continue.</br>             </br>Event propagation also controls focus.If you handle an event like a mouse event, typically the </br>instance will be given focus. Returning false from a filtered event handler or not registering for </br>an event type means that the click will be given to a lower part of the page and your instance </br>will not receive focus.This allows an instance to be partially transparent, where clicks on the </br>transparent areas will behave like clicks to the underlying page.</br>             </br>In general, you should try to keep input event handling short. Especially for filtered input events,</br>the browser or page may be blocked waiting for you to respond.</br>             </br>The caller of this function will maintain a reference to the input event resource during this call.</br>Unless you take a reference to the resource to hold it for later, you don't need to release it.</br>             



>The OnInputEvents method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             



> :bulb: If you're not receiving input events, make sure you register for the event classes you want </br>by calling RequestInputEvents or RequestFilteringInputEvents. If you're still not receiving </br>keyboard input events, make sure you're returning true (or using a non-filtered event handler) for</br>mouse events. Otherwise, the instance will not receive focus and keyboard events will not be sent.</br>             </br>Refer to RequestInputEvents and RequestFilteringInputEvents for further information.</br>             

|Name | Description |
|-----|------|
|inputEvent: |The event to handle.|
Returns: true if the event was handled, false if not. If you have registered to filter this class of events by calling RequestFilteringInputEvents, and you return false, the event will be forwarded to the page (and eventually the browser) for the default handling. For non-filtered events, the return value will be ignored.



---
##### M:PepperSharp.Instance.OnMouseDown(PepperSharp.Instance.MouseEventArgs)

Notification that a mouse button was pressed.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>             



>The OnMouseDown method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       MouseDown += HandleMouseEvents;
       MouseEnter += HandleMouseEvents;
       MouseLeave += HandleMouseEvents;
       MouseMove += HandleMouseEvents;
       MouseUp += HandleMouseEvents;
       ContextMenu += HandleMouseEvents;
       RequestInputEvents(PPInputEventClass.Mouse);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: true if handled, false otherwise.



---
##### M:PepperSharp.Instance.OnMouseUp(PepperSharp.Instance.MouseEventArgs)

Notification that a mouse button was released.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>             



>The OnMouseUp method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       MouseDown += HandleMouseEvents;
       MouseEnter += HandleMouseEvents;
       MouseLeave += HandleMouseEvents;
       MouseMove += HandleMouseEvents;
       MouseUp += HandleMouseEvents;
       ContextMenu += HandleMouseEvents;
       RequestInputEvents(PPInputEventClass.Mouse);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: true if handled, false otherwise.



---
##### M:PepperSharp.Instance.OnMouseEnter(PepperSharp.Instance.MouseEventArgs)

Notification that the mouse entered the instance's bounds.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>             



>The OnMouseEnter method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       MouseDown += HandleMouseEvents;
       MouseEnter += HandleMouseEvents;
       MouseLeave += HandleMouseEvents;
       MouseMove += HandleMouseEvents;
       MouseUp += HandleMouseEvents;
       ContextMenu += HandleMouseEvents;
       RequestInputEvents(PPInputEventClass.Mouse);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: true if handled, false otherwise.



---
##### M:PepperSharp.Instance.OnMouseLeave(PepperSharp.Instance.MouseEventArgs)

Notification that a mouse left the instance's bounds.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>             



>The OnMouseLeave method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       MouseDown += HandleMouseEvents;
       MouseEnter += HandleMouseEvents;
       MouseLeave += HandleMouseEvents;
       MouseMove += HandleMouseEvents;
       MouseUp += HandleMouseEvents;
       ContextMenu += HandleMouseEvents;
       RequestInputEvents(PPInputEventClass.Mouse);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: true if handled, false otherwise.



---
##### M:PepperSharp.Instance.OnMouseMove(PepperSharp.Instance.MouseEventArgs)

Notification that a mouse button was moved when it is over the instance or dragged out of it.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>             



>The OnMouseMove method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       MouseDown += HandleMouseEvents;
       MouseEnter += HandleMouseEvents;
       MouseLeave += HandleMouseEvents;
       MouseMove += HandleMouseEvents;
       MouseUp += HandleMouseEvents;
       ContextMenu += HandleMouseEvents;
       RequestInputEvents(PPInputEventClass.Mouse);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: true if handled, false otherwise.



---
##### M:PepperSharp.Instance.OnContextMenu(PepperSharp.Instance.MouseEventArgs)

Notification that a context menu should be shown.</br>             </br>This message will be sent when the user right-clicks or performs another OS-specific mouse command that should open a context menu.When this event is delivered depends on the system, on some systems (Mac) it will delivered after the mouse down event, and on others (Windows) it will be delivered after the mouse up event.</br>            </br>You will always get the normal mouse events. For example, you may see OnMouseDown, OnContextMenu, OnMouseUp or OnMouseDown, OnMouseUp, OnContextMenu.</br>             </br>The return value from the event handler determines if the context menu event will be passed to the page when you are using filtered input events(via RequestFilteringInputEvents()). In non-filtering mode the event will never be propagated and no context menu will be displayed.If you are handling mouse events in filtering mode, you may want to return true from this event even if you do not support a context menu to suppress the default one.</br>             </br>Register for this event using the PPInputEventClass.Mouse class.</br>             



>The OnContextMenu method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       MouseDown += HandleMouseEvents;
       MouseEnter += HandleMouseEvents;
       MouseLeave += HandleMouseEvents;
       MouseMove += HandleMouseEvents;
       MouseUp += HandleMouseEvents;
       ContextMenu += HandleMouseEvents;
       RequestInputEvents(PPInputEventClass.Mouse);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: true if handled, false otherwise.



---
##### M:PepperSharp.Instance.OnWheel(PepperSharp.Instance.WheelEventArgs)

Notification that the scroll wheel was used.</br>            </br>Register for this event using the PPInputEventClass.Wheel class.</br>             



>The OnWheel method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
       Wheel += HandleWheel;
       RequestInputEvents(PPInputEventClass.Wheel);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: true if handled, false otherwise.



---
##### M:PepperSharp.Instance.OnKeyUp(PepperSharp.Instance.KeyboardEventArgs)

Notification that a key was released.</br>             </br>Register for this event using the PPInputEventClass.Keyboard class.</br>             



>The OnKeyUp method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       KeyUp += HandleKeyboardEvents;
       KeyDown += HandleKeyboardEvents;
       KeyChar += HandleKeyboardEvents;
       RawKeyDown += HandleKeyboardEvents;
       RequestFilteringInputEvents(PPInputEventClass.Keyboard);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnKeyDown(PepperSharp.Instance.KeyboardEventArgs)

Notification that a key was pressed.</br>             </br>This does not necessarily correspond to a character depending on the key and language.  Use the OnKeyChar for character input.</br>            </br>Register for this event using the PPInputEventClass.Keyboard class.</br>             



>The OnKeyDown method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       KeyUp += HandleKeyboardEvents;
       KeyDown += HandleKeyboardEvents;
       KeyChar += HandleKeyboardEvents;
       RawKeyDown += HandleKeyboardEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.Keyboard);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnKeyChar(PepperSharp.Instance.KeyboardEventArgs)

Notification that a character was typed.</br>             </br>Use this for text input. Key down events may generate 0, 1, or more than one character event depending on the key, locale, and operating system.</br>            </br>Register for this event using the PPInputEventClass.Keyboard class.</br>             



>The OnKeyChar method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       KeyUp += HandleKeyboardEvents;
       KeyDown += HandleKeyboardEvents;
       KeyChar += HandleKeyboardEvents;
       RawKeyDown += HandleKeyboardEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.Keyboard);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnRawKeyDown(PepperSharp.Instance.KeyboardEventArgs)

Notification that a key transitioned from "up" to "down".</br>             </br>Register for this event using the PPInputEventClass.Keyboard class.</br>             



>The OnRawKeyDown method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       KeyUp += HandleKeyboardEvents;
       KeyDown += HandleKeyboardEvents;
       KeyChar += HandleKeyboardEvents;
       RawKeyDown += HandleKeyboardEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.Keyboard);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnTouchStart(PepperSharp.TouchInputEvent)

Notification that a finger was placed on a touch-enabled device.</br>             </br>Register for this event using the PPInputEventClass.Touch class.</br>             



>The OnTouchStart method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       TouchStart += HandleTouchEvents;
       TouchEnd += HandleTouchEvents;
       TouchMove += HandleTouchEvents;
       TouchCancel += HandleTouchEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.Touch);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnTouchEnd(PepperSharp.TouchInputEvent)

Notification that a finger was released on a touch-enabled device.</br>             </br>Register for this event using the PPInputEventClass.Touch class.</br>             



>The OnTouchEnd method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       TouchStart += HandleTouchEvents;
       TouchEnd += HandleTouchEvents;
       TouchMove += HandleTouchEvents;
       TouchCancel += HandleTouchEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.Touch);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnTouchCancel(PepperSharp.TouchInputEvent)

Notification that a touch event was canceled.</br>             </br>Register for this event using the PPInputEventClass.Touch class.</br>             



>The OnTouchCancel method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       TouchStart += HandleTouchEvents;
       TouchEnd += HandleTouchEvents;
       TouchMove += HandleTouchEvents;
       TouchCancel += HandleTouchEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.Touch);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnTouchMove(PepperSharp.TouchInputEvent)

Notification that a finger was moved on a touch-enabled device.</br>             </br>Register for this event using the PPInputEventClass.Touch class.</br>             



>The OnTouchMove method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       TouchStart += HandleTouchEvents;
       TouchEnd += HandleTouchEvents;
       TouchMove += HandleTouchEvents;
       TouchCancel += HandleTouchEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.Touch);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnIMECompositionStart(PepperSharp.IMEInputEvent)

Notification that an input method composition process has just started.</br>             </br>Register for this event using the PPInputEventClass.IME class.</br>             



>The OnIMECompositionStart method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       IMECompositionEnd += HandleIMEEvents;
       IMECompositionStart += HandleIMEEvents;
       IMECompositionUpdate += HandleIMEEvents;
       IMEText += HandleIMEEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.IME);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnIMECompositionEnd(PepperSharp.IMEInputEvent)

Notification that an input method composition process has completed.</br>             </br>Register for this event using the PPInputEventClass.IME class.</br>             



>The OnIMECompositionEnd method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       IMECompositionEnd += HandleIMEEvents;
       IMECompositionStart += HandleIMEEvents;
       IMECompositionUpdate += HandleIMEEvents;
       IMEText += HandleIMEEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.IME);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnIMECompositionUpdate(PepperSharp.IMEInputEvent)

Notification that the input method composition string is updated.</br>             </br>Register for this event using the PPInputEventClass.IME class.</br>             



>The OnIMECompositionUpdate method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       IMECompositionEnd += HandleIMEEvents;
       IMECompositionStart += HandleIMEEvents;
       IMECompositionUpdate += HandleIMEEvents;
       IMEText += HandleIMEEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.IME);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnIMEText(PepperSharp.IMEInputEvent)

Notification that an input method committed a string.</br>             </br>Register for this event using the PPInputEventClass.IME class.</br>             



>The OnIMEText method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>             

_C# code_

``` c#
    public InputEventInstance(IntPtr handle) : base(handle)
    {
   
       IMECompositionEnd += HandleIMEEvents;
       IMECompositionStart += HandleIMEEvents;
       IMECompositionUpdate += HandleIMEEvents;
       IMEText += HandleIMEEvents;
       
       RequestFilteringInputEvents(PPInputEventClass.IME);
    }
    
```

|Name | Description |
|-----|------|
|e: |Event args|
Returns: 



---
##### M:PepperSharp.Instance.OnHandleMessage(PepperSharp.Var)

Raises the HandleMessage event when the browser calls PostMessage() on the DOM element for </br>the instance in JavaScript.</br>            



>The OnHandleMessage method also enables derived classes to handle the event without attaching </br>a delegate. This is the preferred technique for handling the event in a derived class.</br>            



> :bulb: PostMessage() in the JavaScript interface is asynchronous, meaning JavaScript execution </br>will not be blocked while OnRecieveMessage is processing the message.</br>            </br>When converting JavaScript arrays, any object properties whose name is not an array index are </br>ignored.  When passing arrays and objects, the entire reference graph will be converted and </br>transferred.  If the reference graph has cycles, the message will not be sent and an error will </br>be logged to the console.</br>            

|Name | Description |
|-----|------|
|message: |A Var which has been converted from a JavaScript value. JavaScript </br>            array/object types are supported from Chrome M29 onward. All JavaScript values are copied </br>            when passing them to the plugin.</br>            |


---
##### M:PepperSharp.Instance.BindGraphics(PepperSharp.PPResource)

BindGraphics() binds the given graphics as the current display surface.</br>             </br>The contents of this device is what will be displayed in the instance's area on the web page. </br>The device must be a 2D or a 3D device.</br>            </br>You can pass an is_null() (default constructed) Graphics2D as the device parameter to unbind all </br>devices from the given instance. The instance will then appear transparent. Re-binding the same </br>device will return true and will do nothing.</br>             </br>Any previously-bound device will be released. It is an error to bind a device when it is already</br>bound to another instance. If you want to move a device between instances, first unbind it from </br>the old one, and then rebind it to the new one.</br>            </br>Binding a device will invalidate that portion of the web page to flush the contents of the new </br>device to the screen.</br>             

|Name | Description |
|-----|------|
|graphics2d: |A Graphics2D to bind|
Returns: true if bind was successful or false if the device was not the correct type. On success, a reference to the device will be held by the instance, so the caller can release its reference if it chooses.



---
##### M:PepperSharp.Instance.PostMessage(System.Object)

Asynchronously invokes any listeners for message events on the DOM element for the given instance.</br>            

|Name | Description |
|-----|------|
|message: |A manage object that will be converted to a JavaScript value.  JavaScript </br>            array/object types are supported from Chrome M29 onward. All JavaScript values are copied </br>            when passing them to the plugin.</br>            |


---
##### P:PepperSharp.Instance.PPInstance

Getter that returnes the unmanaged pointer (Handle) of this instance.</br>            



---
##### M:PepperSharp.Instance.op_Implicit(PepperSharp.Instance)~PepperSharp.PPInstance

Implicit conversion to unmanaged pointer (Handle).  Used by PInvoke.</br>            

|Name | Description |
|-----|------|
|instance: ||


---
##### M:PepperSharp.Instance.LogToConsole(PepperSharp.PPLogLevel,System.Object)

Logs the given message to the JavaScript console associated with the given plugin instance with the given logging level.</br>            </br>The name of the plugin issuing the log message will be automatically prepended to the message.The value may be any type of Var.</br>            

|Name | Description |
|-----|------|
|level: |The log level|
|Name | Description |
|-----|------|
|value: |The managed object to log.|


---
##### M:PepperSharp.Instance.LogToConsoleWithSource(PepperSharp.PPLogLevel,System.String,System.Object)

Logs a message to the console with the given source information rather than using the internal PPAPI plugin name.</br>            </br>The regular log function will automatically prepend the name of your plugin to the message as the "source" of the message.  </br>Some plugins may wish to override this. </br>            

|Name | Description |
|-----|------|
|level: |The log level|
|Name | Description |
|-----|------|
|source: |Must be a string value|
|Name | Description |
|-----|------|
|value: |The managed object to log.|


---
##### M:PepperSharp.Instance.RequestInputEvents(PepperSharp.PPInputEventClass)

Requests that input events corresponding to the given input events are delivered to the instance.</br>            </br>By default, no input events are delivered.Call this function with the classes of events you are</br>interested in to have them be delivered to the instance. Calling this function will override any </br>previous setting for each specified class of input events(for example, if you previously called </br>RequestFilteringInputEvents(), this function will set those events to non-filtering mode).</br>            </br>Input events may have high overhead, so you should only request input events that your plugin will </br>actually handle.For example, the browser may do optimizations for scroll or touch events that can</br>be processed substantially faster if it knows there are no non-default receivers for that message. </br>Requesting that such messages be delivered, even if they are processed very quickly, may have a </br>noticeable effect on the performance of the page.</br>            </br>When requesting input events through this function, the events will be delivered and not bubbled </br>to the page.This means that even if you aren't interested in the message, no other parts of the </br>page will get the message.</br>            

|Name | Description |
|-----|------|
|eventClasses: |A combination of flags from PP_InputEvent_Class that identifies the classes of events the instance is requesting. The flags are combined by logically ORing their values.|
Returns: PP_OK if the operation succeeded, PP_ERROR_BADARGUMENT if instance is invalid, or PP_ERROR_NOTSUPPORTED if one of the event class bits were illegal. In the case of an invalid bit, all valid bits will be applied and only the illegal bits will be ignored.



---
##### M:PepperSharp.Instance.RequestFilteringInputEvents(PepperSharp.PPInputEventClass)

Requests that input events corresponding to the given input events </br>are delivered to the instance for filtering.</br>            </br>By default, no input events are delivered.In most cases you would register to receive events by </br>calling RequestInputEvents(). In some cases, however, you may wish to filter events such that </br>they can be bubbled up to the DOM.In this case, register for those classes of events using this </br>function instead of RequestInputEvents(). Keyboard events must always be registered in filtering </br>mode.</br>            </br>Filtering input events requires significantly more overhead than just delivering them to the </br>instance. As such, you should only request filtering in those cases where it's absolutely</br>necessary. The reason is that it requires the browser to stop and block for the instance to </br>handle the input event, rather than sending the input event asynchronously. This can have </br>significant overhead.</br>            

|Name | Description |
|-----|------|
|eventClasses: |A combination of flags from PP_InputEvent_Class that identifies the classes of events the instance is requesting. The flags are combined by logically ORing their values.|
Returns: OK if the operation succeeded, BADARGUMENT if instance is invalid, or NOTSUPPORTED if </br>            one of the event class bits were illegal. In the case of an invalid bit, all valid bits will be </br>            applied and only the illegal bits will be ignored.</br>            



---
##### P:PepperSharp.Instance.IsFullFrame

Getter that returns if the instance is full-frame (repr).</br>            </br>Such an instance represents the entire document in a frame rather than an embedded resource.</br>This can happen if the user does a top-level navigation or the page specifies an iframe to a </br>resource with a MIME type registered by the module.</br>            



---
##### M:PepperSharp.Instance.ClearInputEventRequest(PepperSharp.PPInputEventClass)

ClearInputEventRequest() requests that input events corresponding to the given input classes no longer be delivered to the instance.</br>            </br>By default, no input events are delivered.If you have previously requested input events using </br>RequestInputEvents() or RequestFilteringInputEvents(), this function will unregister handling </br>for the given instance.This will allow greater browser performance for those events.</br>            

> :bulb: You may still get some input events after clearing the flag if they were dispatched before the request was cleared. For example, if there are 3 mouse move events waiting to be delivered, and you clear the mouse event class during the processing of the first one, you'll still receive the next two. You just won't get more events generated.</br>            



|Name | Description |
|-----|------|
|eventClasses: |A combination of flags from PP_InputEvent_Class that identifies the classes of events the instance is no longer interested in.|


---
##### M:PepperSharp.Instance.SetCursor(PepperSharp.PPMouseCursorType,PepperSharp.ImageData,System.Nullable{PepperSharp.PPPoint})

Sets the given mouse cursor. The mouse cursor will be in effect whenever</br>the mouse is over the given instance until it is set again by another</br>call.</br>            

> :bulb: You can hide the mouse cursor by setting it to the</br>            `PPMouseCursorType` type.</br>            

This function allows setting both system defined mouse cursors and</br>custom cursors. To set a system-defined cursor, pass the type you want</br>and set the custom image to a default-constructor ImageData object.</br>To set a custom cursor, set the type to</br>            `PPMouseCursorType.CUSTOM` and specify your image and hot</br>            spot.</br>            

|Name | Description |
|-----|------|
|type: |A `PPMouseCursorType` identifying the type</br>            of mouse cursor to show.</br>            |
|Name | Description |
|-----|------|
|imageData: |A `ImageData` object identifying the</br>            custom image to set when the type is</br>            `PPMouseCursorType.CUSTOM`. The image must be less than 32</br>            pixels in each direction and must be of the system's native image format.</br>            When you are specifying a predefined cursor, this parameter should be a</br>            default-constructed ImageData.</br>            |
|Name | Description |
|-----|------|
|hotSpot: |When setting a custom cursor, this identifies the</br>            pixel position within the given image of the "hot spot" of the cursor.</br>            When specifying a stock cursor, this parameter is ignored.</br>            |
Returns: true on success, or false if the instance or cursor type</br>            was invalid or if the image was too large.



---
##### P:PepperSharp.Instance.IsFullScreen

Getter and Setter that checks whether the module instance is currently in</br>fullscreen mode or sets the instance to full screen mode.</br>             </br>The setter switches the module instance to and from fullscreen</br>mode.</br>             </br>The transition to and from fullscreen mode is asynchronous. During the</br>transition, IsFullscreen() will return the previous value and</br>no 2D or 3D device can be bound. The transition ends at DidChangeView()</br>when IsFullscreen() returns the new value. You might receive other</br>DidChangeView() calls while in transition.</br>            </br>The transition to fullscreen mode can only occur while the browser is</br>processing a user gesture, even if `true` is returned.</br>             </br>             



>Throws InvalidOperationException if there was an error transitioning to or from</br>fullscreen mode.</br>             

[[T:System.InvalidOperationException|T:System.InvalidOperationException]]: 



---
##### P:PepperSharp.Instance.ScreenSize

Gets the size of the screen in pixels. The module instance</br>will be resized to this size when IsFullscreen is set to true to enter</br>fullscreen mode.</br>            



>Throws InvalidOperationException if there was an error obtaining the screen size for</br>some reason.</br>            

[[T:System.InvalidOperationException|T:System.InvalidOperationException]]: 



---
##### M:PepperSharp.Instance.GamepadsSample(PepperSharp.GamepadsSampleData@)

Allows retrieving data from gamepad/joystick devices that are connected to the system.</br>            

|Name | Description |
|-----|------|
|sampleData: |The data for all gamepads connected to the system.|


---

