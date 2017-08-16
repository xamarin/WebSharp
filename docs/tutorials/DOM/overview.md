# DOM Overview

When you create a [WebSharp Electron Application](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-electron-application.md) the `WebSharp.js.dll` assembly that is installed with `electron-dotnet` provides the bridge between `Node.js`, `Electron` and the managed code that is written.  This same assembly provides access to the HTML page's DOM elements.  A developer can take advantage of this assembly to access the DOM elements via the `WebSharpJs.Script` and `WebSharpJs.DOM` namespaces.  

Use the `GetHtmlDocument` method of the `HtmlPage` object to obtain a reference to the HTML page's Document Object Model (DOM).  Those that are familiar with `Silverlight`'s browser integration via `System.Windows.Browser` namespace should be able to get up and running fast as the API footprint is very close.

There are some differences though because of the different integration technoligies, `SilverLight` uses a plugin whereas `WebSharp` uses `Native Modules` as an interface, which affects the implementation details a little.  One of the main differences is `WebSharp`'s asynchronous only interface.

Besides the differences the developer will still work with managed objects in their code.  These objects are managed via the `WebSharp.js` assembly and can be passed back and forth between managed code and `Node.js/Electron`.

## Content

&nbsp;&nbsp;&nbsp;&nbsp;[Key Classes](#the-key-classes-of-the-websharpjsdom-namespace)  
&nbsp;&nbsp;&nbsp;&nbsp;[HTML Page](#html-page-gateway-to-dom)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[BrowserInformation](#browserinformation)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[HTML Window](#html-window)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Interaction with script code](#interaction-with-script-code)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[HTML Document](#html-document)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[HTML Element](#html-element)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[HTML Element Sample code](#tinkering-with-html-dom-elements)  

## The key classes of the WebSharpJs.DOM namespace

| Class | Description |
| --- | --- |
| HtmlPage | Represents the current HTML page.  It is the gateway to the browser's DOM via the `GetDocument` method (`HtmDocument`), which in turn allows the developer to access HTML elements (`HtmlElement`).  It also allows interacting with the browser window (`HtmlWindow` via `Electron`'s BrowserWindowProxy managed object) and obtain information about the environment the application is running within (`BrowserInformation`). | 
| BrowserInformation | Obtains name, version, and operating system of the web browser |
| HtmlDocument | Used to access the HTML DOM from managed code and represents the complete HTML Document that is loaded into the browser at the time.  Usually `HtmlElement` objects are queried for and obtained from this object. | 
| HtmlElement | Represents an HTML element in the page DOM.  There exists a full range of manipulation methods to interact with an `HtmlElement`  | 
| HtmlWindow | Represents an `Electron` window object in managed code via `Electron`'s BrowserWindowProxy.  Notice that this is not a normal `JavaScript` window as `Electron` intercepts the object and provides their own implementation.  For instance the `Prompt` method of the this object will fail as `Electron` does not provide an implementation at this time nor will they as per the documentation.   |
| HttpUtility | Provides static methods for encoding and decoding HTML and URL strings. |

## HTML Page: Gateway to DOM

The static `HtmlPage` class is where one begins to access the DOM.  From the page object you then have access to the following methods.

| Class | How To |
| --- | --- |
| HtmlDocument | var document = await HtmlPage.GetDocument(); |
| HtmlWindow | var window = await HtmlPage.GetWindow() |
| BrowserInformation | var info = await HtmlPage.GetBrowserInformation() |

### BrowserInformation

So for example we want to get the `BrowserInformation` we would have the following:

```cs
    var info = await HtmlPage.GetBrowserInformation();
   
    var infoText = $"Name: {info.Name}<br />Browser Version: {info.BrowserVersion}<br />Platform: {info.Platform}<br />Cookies Enabled: {info.CookiesEnabled}<br />User Agent: {info.UserAgent}";

    // other code following to update the DOM to display the infoText in the page.
```

* Mac

![browser info mac](./browserinfo/images/browserinformation.png)

* Windows

![browser info windows](./browserinfo/images/browserinformation-win.png)

See the [BrowserInformation source code](./browserinfo)


### HTML Window

The Html Window provides very limited access to the window.  Most of the time using the `BrowserWindow` objects of `Electron` will be more desirable.  `Electron` actually intercepts some of the functionality to provide their own implementation (See [BrowserWindowProxy](https://github.com/electron/electron/blob/master/docs/api/browser-window-proxy.md)) or throw an error as for `Prompt`. 

To obtain a reference to the `HtmlWindow` object use the following.

```cs
    var win = await HtmlPage.GetWindow();
```

Let's briefly look at the functionality of the `HtmlWindow` object.

| Method | Description |
| --- | --- |
| Alert | Displays a dialog box that contains an application-defined message. | 
| Confirm | Displays a confirmation dialog box that contains an optional message as well as OK and Cancel buttons.  This method will return a `true\false` value.  This method will also block the UI so be careful. | 
| Eval | Evaluates a string that contains arbitrary JavaScript code. | 
| Prompt | This method will actually throw an error `prompt() is and will not be supported.`  Use `Electron`'s `Dialog` objects. | 

#### Interaction with script code

One interesting function that one can do with the `HtmlWindow` object is interacting with code that is defined in the page.

Start by defining a `<script>` within the `<head>` section of of the page's html.

```html

  <head>
    <meta charset="UTF-8">
    <title>Hello</title>

    <script type="text/javascript">
        function SayHello(text) {
            var helloElement = document.getElementById("hello");
            helloElement.innerHTML = 'Hello: ' + text;
        }
    </script>

  </head>

```

The above code starts by searching for an element with a name of `hello` so define a `<p>` element somewhere in the `<body>` section of the page's html.

```html
    <p id="hello"></p>
```

The function will then update the `innerHTML` of that element with the text that is passed in.

Now from your managed code you can call this function by using the `Invoke` method of the `HtmlWindow` object.

```cs
            var win = await HtmlPage.GetWindow();
            await win.Invoke<object>("SayHello", " from HtmlWindow.");
```

If all is defined correctly you should see the `Hello: from HtmlWindow.` text displayed on your screen.

### HTML Document

The document object represents your web page.

If you want to access any element in an HTML page, you always start with accessing the document object.

| Method | Description |
| --- | --- |
| CreateElement | Creates a new HtmlElement object to represent a dynamically created HTML element, which you can then insert into the page.  | 
| GetBody | Gets an HtmlElement object that represents the `<body>` element in the HTML page. | 
| GetDocumentUri | Gets a Uniform Resource Identifier (URI) object. | 
| GetDocumentElement | Gets a reference to the browser's DOCUMENT element. | 
| GetElementById | Gets a single browser element.  | 
| GetElementsByTagName | Gets a collection of browser elements.  | 
| GetLocation | Returns a Location object, which contains information about the URL of the document.  | 
| GetIsReady | Gets a value that indicates whether the browser has completely loaded the HTML page. | 
| GetReadyState | Gets the string value that indicates the state of the the HTML page. | 
| QuerySelector | Returns the first Element within the document that matches the specified CSS selector, or group of CSS selectors.  | 
| QuerySelectorAll | Returns a collection Elements within the document that matches the specified CSS selector, or group of CSS selectors.  | 
| Submit | Submits the page, by posting a form and its data back to the server.  | 

The `HTMLDocument` object allows you to traverse the `HTMLElement`s of the document.

The example [domtree](./domtree) shows an example of traversing the page DOM using the `HTMLDocument` class.

* Mac

![dom traversal mac](./domtree/images/domtraversal.png)

* Windows

![dom traversal windows](./domtree/images/domtraversal-win.png)

The source code for creating the above can be found in the sources [domtree sources](./domtree/src/DOMInfo/DOMInfo.cs)

```cs


        var document = await HtmlPage.GetDocument();
        // Get a reference to the top-level <html> element.
        var element = await document.GetDocumentElement();
        // Process the starting element reference
        await ProcessElement(element, 0);

```

The actual traversal takes place in the `ProcessElement` method.

```cs

        private async Task ProcessElement(HtmlElement element, int indent)
        {
            // Ignore comments.
            if (await element.GetTagName() == "!") return;
            
            // Indent the element to help show different levels of nesting.
            elementTree += new String(' ', indent * 4);

            // Display the tag name.
            elementTree += "<" + await element.GetTagName();
            
            // Only show the id attribute if it's set.
            if (await element.GetId() != "") elementTree += " id=\"" + await element.GetId() + "\"";
            elementTree += ">\n";

            // Process all the elements nested inside the current element.
            foreach (var childElement in await element.GetChildren())
            {
                await ProcessElement(childElement, indent + 1);
            }
        }

``` 

### HTML Element

When traversing the DOM as described above each element that is traversed is a `HtmlElement` obtained by using the `GetChildren()` method of the element.  Other methods available from the `HtmlElement` can be found below.

| Method | Description |
| --- | --- |
| AppendChild | Appends a child element to the current HTML element's child collection.  Use `HtmlDocument.CreateElement` to create a new element. |
| AttachEventHandler | Adds a raised `JavaScript` event to managed code `EventHandler` |
| DetachEventHandler | Removes a managed code `EventHandler` attached to a raised `JavaScript` event. |
| Focus | Gives focus to the current element to receive events. |
| GetAttribute | Retrieves a named attribute on the current element.  |
| GetCssClass | Retrieves the CSS class name on the current element. |
| GetId | Retrieves the identifier of the current element. |
| GetParent | Retrieves a reference to the parent element of the current element. |
| GetProperty | Retrieves a named property on the current element.  Example:  `await element.GetProperty<string>("innerHtml")`.   |
| GetStyleAttribute | Retrieves a named CSS style attribute on the current element. |
| GetTagName | Retrieves the HTML tag name of the current element. |
| Invoke | Invokes a method on the current element.  Example: `await element.Invoke<object>("blur")` that will remove the focus from the current element. |
| QuerySelector | Returns the first Element within the document that matches the specified CSS selector, or group of CSS selectors.  | 
| QuerySelectorAll | Returns a collection Elements within the document that matches the specified CSS selector, or group of CSS selectors.  | 
| RemoveAttribute | Removes a named attribute on the current element.  |
| RemoveStyleAttribute | Removes a named CSS style attribute on the current element. |
| SetAttribute | Sets a named attribute value on the current element.  |
| SetCssClass | Sets the CSS class name on the current element. |
| SetProperty | Sets the value a named property on the current element.  Example:  `await element.SetProperty("innerHtml", "Hello")`.  |
| SetStyleAttribute | Sets a named CSS style attribute value on the current element.  |

#### Tinkering with HTML DOM Elements

For a better understanding of how to use the `HtmlElement` methods we will take a look a the following code that can be found in the [domelement sources](./domelement) and more specifically the [ElementTinkerer](./domelement/src/ElementTinkerer/ElementTinkerer.cs) class.

```cs

    // Get a reference to the HTML DOM document.
    var document = await HtmlPage.GetDocument();

    // Get a reference to the body element.
    var body = await document.GetElementById("docBody");
    // Create a <main> type element
    var mainElement = await document.CreateElement("main");
    // Append the newly created element to the page DOM
    await body.AppendChild(mainElement);
    
    // Get a collection of all the <main> type element in the page and using
    // Linq get the first of the collection.
    var content = (await document.GetElementsByTagName("main")).First();

    // Get a collection of all the <link> elements
    var links = await document.QuerySelectorAll("link[rel=\"import\"]");

    // Loop through all of the <link> elements found
    foreach (var link in links)
    {
        // Access the contents of the import document by examining the
        // import property of the corresponding <link> element
        var template = await link.GetProperty<HtmlElement>("import").ContinueWith(
                async (t) =>
                {
                    // For all imported contents obtain a reference to the <template>
                    return await t.Result?.QuerySelector(".task-template");
                }
            ).Result;
        // Create a clone of the template’s content using the importNode property and
        // passing in the content of the template
        var clone = await document.Invoke<HtmlElement>("importNode", 
                    await template.GetProperty<HtmlElement>("content"), true);

        // Append the newly created cloned template to the content element.
        await content.AppendChild(clone);
    }

```

This above demonstrates a rather contrived example but shows the use of many of the methods available to HtmlElement elements.  The `HtmlElement` class is not limited to only the relatively few methods described above can also access other features such as the HTML 5 Imports features.  Using the `GetProperty`, `SetProperty` and `Invoke` methods that are made available opens up a lot of flexibility.

For more information on HTML 5 Imports see the following links:

* [HTML 5 Imports specs](http://w3c.github.io/webcomponents/spec/imports/)

* Other reference links
    - [WebComponents.org introduction](https://www.webcomponents.org/community/articles/introduction-to-html-imports)
    - [html5rocks.com tutorial](https://www.html5rocks.com/en/tutorials/webcomponents/imports/)
    - [teamtreehouse.com introduction](http://blog.teamtreehouse.com/introduction-html-imports)

> :bulb: One aspect of targeting `Electron` and using the built in `Chromium` features is that we do not have to worry about browser compatibility.  Once `Chromium` supports it you can use it within the `Electron` application.

#### Listening to JavaScript Events

We can also listen too and handle the DOM events that are associated.  There exists a static class of predefined events, [WebSharpJs.DOM.HtmlEventNames](https://github.com/xamarin/WebSharp/blob/master/electron-dotnet/src/websharpjs/WebSharp.js/dotnet/WebSharpJs.DOM/HtmlEventNames.cs), that you may find useful instead of trying to remember the exact spelling of the event that can be listened for.

Some of the more common predefined events are listed below:

##### UI Events

| HtmlEventName | DOM | Fired When |
| --- | --- | --- |
| Change | change | The change event is fired for `<input>`,` <select>`, and `<textarea>` elements when a change to the element's value is committed by the user. |
| Select | select | Some text is being selected. |

##### Resource Events

| HtmlEventName | DOM | Fired When |
| --- | --- | --- |
| Abort | abort | The loading of a resource has been aborted. |
| Error | error | A resource failed to load. |
| Load | load | A resource and its dependent resources have finished loading. | 
| UnLoad | unload | The document or a dependent resource is being unloaded. |

##### Keyboard Events

| HtmlEventName | DOM | Fired When |
| --- | --- | --- |
| KeyDown | keydown | ANY key is pressed |
| KeyPress | keypress | ANY key except Shift, Fn, CapsLock is in pressed position. (Fired continously.) |
| KeyUp | keyup | ANY key is released |

##### Mouse Events

| HtmlEventName | DOM | Fired When |
| --- | --- | --- |
| Click | click | A pointing device button has been pressed and released on an element. |
| MouseOver | mouseover | A pointing device is moved onto the element that has the listener attached or onto one of its children. | 
| MouseOut | mouseout | A pointing device is moved off the element that has the listener attached or off one of its children. |
| MouseEnter | mouseenter | A pointing device is moved onto the element that has the listener attached. |
| ContextMenu | contextmenu | The right button of the mouse is clicked (before the context menu is displayed). |

##### Focus Events

| HtmlEventName | DOM | Fired When |
| --- | --- | --- |
| Focus | focus | An element has received focus (does not bubble). |
| Blur | blur | An element has lost focus (does not bubble). |

##### Drag and Drop

| HtmlEventName | DOM | Fired When |
| --- | --- | --- |
| DragEnter | dragenter | A dragged element or text selection enters a valid drop target. |
| DragEnd | dragend | A drag operation is being ended (by releasing a mouse button or hitting the escape key). |
| DragLeave | dragleave | A dragged element or text selection leaves a valid drop target. |
| DragOver | dragover | An element or text selection is being dragged over a valid drop target (every 350ms). |
| Drop | drop | An element is dropped on a valid drop target. |

> :bulb: Notice that the events are not prepended with `on`.  For more events take a look at the [Event reference](https://developer.mozilla.org/en-US/docs/Web/Events) documenation.

To listen to and react to an event you use the `AttachEvent` of an instance of `HtmlElement`, `HtmlDocument` and `HtmlWindow`.

A good code example can be found in the [HandClaps example](https://github.com/xamarin/WebSharp/tree/master/Examples/websharpjs/electron/handclaps)

In the [ClapsRenderer.cs](https://github.com/xamarin/WebSharp/blob/master/Examples/websharpjs/electron/handclaps/src/ClapsRenderer/ClapsRenderer.cs) source code we find examples of retrieving `HtmlElement`'s by using the `GetElementById` method as well as attaching event listeners to those elements.

```csharp

    var document = await HtmlPage.GetDocument();
    area = await document.GetElementById("input");
    output = await document.GetElementById("output");
    spaces = await document.GetElementById("spaced");
    emojiPicker = await document.GetElementById("emoji");
    length = await document.GetElementById("length");
    copy = await document.GetElementById("copy");

    await area.AttachEvent(HtmlEventNames.Input, updateOutput);
    await spaces.AttachEvent(HtmlEventNames.Change, updateOutput);
    await emojiPicker.AttachEvent(HtmlEventNames.Change, updateOutput);
    await copy.AttachEvent(HtmlEventNames.Click, copyClicked);

```

#### Dealing with element's attributes

An element's style can also be manipulated by using the following methods of the `HtmlElement`.

| Method | Description |
| --- | --- |
| GetAttribute | Retrieves a named attribute on the current element.  |
| GetStyleAttribute | Retrieves a named CSS style attribute on the current element. |
| RemoveAttribute | Removes a named attribute on the current element.  |
| RemoveStyleAttribute | Removes a named CSS style attribute on the current element. |
| SetAttribute | Sets a named attribute value on the current element.  |
| SetStyleAttribute | Sets a named CSS style attribute value on the current element.  |

To set the `style` attribute of an `HtmlElement` all at once you can use the following:

```csharp

    area = await document.GetElementById("input");
    await area.SetAttribute("style", "border: medium dashed green; background-color: yellow; width: 135px; height: 35px")

``` 

<input style="border: medium dashed green; background-color: yellow; width: 135px; height: 35px; color: blue">

Although it is possible to set the `style` attribute with `SetAttribute`, it is recommended that you use properties of the `Style` object so that already existing `CSS`` are not overwritten.:

```csharp

    await area.SetStyleAttribute("backgroundColor", "yellow");
    await area.SetStyleAttribute("borderStyle", "medium dashed green");
    await area.SetStyleAttribute("width", "135px");
    await area.SetStyleAttribute("height", "35px");

```

#### Working with Element's CSS classes

There are also methods that allow working with the Css classes of an element.


| Method | Description |
| --- | --- |
| GetCssClass | Retrieves the CSS class name on the current element. |
| SetCssClass | Sets the CSS class name on the current element. |

For example code of working with Css classes again look at the [HandClaps example](https://github.com/xamarin/WebSharp/tree/master/Examples/websharpjs/electron/handclaps)

In the [ClapsRenderer.cs](https://github.com/xamarin/WebSharp/blob/master/Examples/websharpjs/electron/handclaps/src/ClapsRenderer/ClapsRenderer.cs) source code uses the following code to work with the Css classes of the elements.  The code is modeled after `JQuery`'s routines.

```csharp

    static readonly char[] rnothtmlwhite = new char[] {' ','\r','\n','\t','\f' };
    bool IsHasClass(string elementClass, string className)
    {
        if (string.IsNullOrEmpty(elementClass) || string.IsNullOrEmpty(className))
            return false;
        return elementClass.Split(rnothtmlwhite, StringSplitOptions.RemoveEmptyEntries).Contains(className);
    }

    async Task<string> addClass(HtmlElement element, string klass, string elementClass = null)
    {
        if (string.IsNullOrEmpty(elementClass))
        {
            elementClass = await length.GetCssClass();
        }

        if (!IsHasClass(elementClass, klass))
        {
            elementClass += $" {klass} ";
            await element.SetCssClass(elementClass);
        }
        return elementClass;
    }
    async Task<bool> removeClass(HtmlElement element, string klass, string elementClass = null)
    {
        if (string.IsNullOrEmpty(elementClass))
        {
            elementClass = await length.GetCssClass();
        }

        if (IsHasClass(elementClass, klass))
        {
            var classList = elementClass.Split(rnothtmlwhite, StringSplitOptions.RemoveEmptyEntries).ToList();
            classList.Remove(klass);
            await element.SetCssClass(string.Join(" ",classList));
            return true;
        }
        return false;
    }

```

Example usage of the above methods.

```csharp

    if (result.Length > 140)
    {
        await addClass(length, "text-danger");
    }
    else 
    {
        await removeClass(length, "text-danger");
    }

```


