# Instance Class Reference

## Summary

An instance is a rectangle on a web page that is managed by a PepperPlugin module.

  * An instance may have a dimension of width=0 and height=0, meaning that the instance does not have any visible component on the web page.
  * An instance is created by including an `<embed></embed>` element in a web page. 
    * The `<embed></embed>` element references a Dot Net class that implements the PepperSharp API and loads the appropriate version of the PepperPlugin module. 
    * A PepperPlugin module may be included in a web page multiple times by using multiple `<embed></embed>` elements that refer to the class implementation; in this case the Native Client runtime system loads the module once and creates multiple instances that are managed by the module.

## Constructors

| Name | Description |
| ---- | ----------- |
| Instance () | Not implemented - Throws error if called |
| Instance(IntPtr handle) | Creates an object instance of Instance using the handle passed.  The handle is a pointer passed to the class when instantiated by the PepperPlugin Native Client implementation. |

## Methods

| Name          | Description |
| :------------- | ----------- |
| OnInitialize(InitializeEventArgs) | Raise event to intialize the instance with the provided arguments. This event will be raised immediately after the instance object is constructed.</br></br>**Remarks**: The OnInitialize method also enables derived classes to handle the event without attaching a delegate. This is the preferred technique for handling the event in a derived class. | 
| OnViewChanged(PepperSharp.View) | Raises the ViewChange event when the view information for the Instance has changed. See the <code>View</code> object for information.</br></br>Most implementations will want to check if the size and user visibility changed, and either resize themselves or start/stop generating updates.</br></br>**Remarks**: The OnViewChanged method also enables derived classes to handle the event without attaching a delegate. This is the preferred technique for handling the event in a derived class.
| OnFocusChanged(System.Boolean) | Raises the FocusChanged event when an instance has gained or lost focus.</br></br>Having focus means that keyboard events will be sent to the instance.  An instance's default condition is that it will not have focus.</br></br>The focus flag takes into account both browser tab and window focus as well as focus of the plugin element on the page. In order to be deemed to have focus, the browser window must be topmost, the tab must be selected in the window, and the instance must be the focused element on the page.</br><br>**Note**: Clicks on instances will give focus only if you handle the click event. Return true from HandleInputEvent in InputEvent (or use unfiltered events) to signal that the click event was handled. Otherwise, the browser will bubble the event and give focus to the element on the page that actually did end up consuming it.If you're not getting focus, check to make sure yo're either requesting them via RequestInputEvents() (which implicitly marks all input events as consumed) or via RequestFilteringInputEvents() and returning true from your event handler.</br></br>**Remarks**:              The OnFocusChanged method also enables derived classes to handle the event without attaching a delegate. This is the preferred technique for handling the event in a derived class. |
| OnInputEvents(PepperSharp.InputEvent) | Raises the HandleInput event from the browser.</br></br>The default implementation does nothing and returns false.</br<</br>In order to receive input events, you must register for them by calling RequestInputEvents() or RequestFilteringInputEvents(). By default, no events are delivered.</br></br>If the event was handled, it will not be forwarded to any default handlers.If it was not handled, it may be dispatched to a default handler.So it is important that an instance respond accurately with whether event propagation should continue.</br></br>Event propagation also controls focus.If you handle an event like a mouse event, typically the instance will be given focus. Returning false from a filtered event handler or not registering for an event type means that the click will be given to a lower part of the page and your instance  will not receive focus.This allows an instance to be partially transparent, where clicks on the transparent areas will behave like clicks to the underlying page.</br></br>In general, you should try to keep input event handling short. Especially for filtered input events, the browser or page may be blocked waiting for you to respond.</br></br>The caller of this function will maintain a reference to the input event resource during this call.  Unless you take a reference to the resource to hold it for later, you don't need to release it.</br></br>**Note**: If you're not receiving input events, make sure you register for the event classes you want by calling RequestInputEvents or RequestFilteringInputEvents. If you're still not receiving keyboard input events, make sure you're returning true (or using a non-filtered event handler) for mouse events. Otherwise, the instance will not receive focus and keyboard events will not be sent.</br></br>Refer to RequestInputEvents and RequestFilteringInputEvents for further information.</br></br>**Remarks**: The OnInputEvents method also enables derived classes to handle the event without attaching a delegate. This is the preferred technique for handling the event in a derived class. |

## Events

| Event         | Summary                                                                                                 |
| ------------- | ------------------------------------------------------------------------------------------------------- |
| ViewChanged   | Event when the view information for the Instance has changed.  |
| FocusChanged  | Event when an instance has gained or lost focus.  |
| HandleMessage | Event when the browser calls PostMessage() on the DOM element for the instance in JavaScript. |
| InputEvents   | Event when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript. |
| MouseDown  | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a MouseDown InputEvent.  |
| MouseUp | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a MouseUp InputEvent. |
| MouseEnter | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a MouseEnter InputEvent. |
| MouseLeave | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a MouseLeave InputEvent. |
| MouseMove  | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a MouseMove InputEvent. |
| ContextMenu | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a ContextMenu InputEvent. |
| Wheel | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a Wheel InputEvent. |
| KeyUp | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a KeyboardInputEvent for KeyUp. |
| KeyDown | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a KeyboardInputEvent for KeyDown. |
| KeyChar | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a KeyboardInputEvent for Char. |
| RawKeyDown | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a KeyboardInputEvent for RawKeydown. |
| TouchStart | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a TouchInputEvent for TouchStart. |
| TouchEnd | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a TouchInputEvent for TouchEnd. |
| TouchCancel | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a TouchInputEvent for TouchCancel. |
| TouchMove | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a TouchInputEvent for TouchMove. |
| IMECompositionEnd | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a IMEInputEvent for CompositionEnd. |
| IMECompositionStart | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a IMEInputEvent for CompositionStart. |
| IMECompositionUpdate | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a IMEInputEvent for CompositionUpdate. |
| IMEText | Event raised when the browser calls HandleInputEvent on the DOM element for the instance in JavaScript that represents a IMEInputEvent for Text. |

