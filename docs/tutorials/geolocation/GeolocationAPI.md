# Scripting `navigator.geolocation`

In [Part 1](./README.ME) we created our project, setup the very simple UI and set up DOM interaction in the `Location.cs` renderer part of the application.

In `Part 2` we will be looking at:

* High level view of the Bridge interface that is used by `WebSharp`.
* Adding a new source module to the project.
* Incorporating the new source module in our application.
* Creating the basic singleton code to reference the `navigator,geolocation` object.

## Bridge (high level)

First let's generally talk about what is meant with the word `scripting`.

`WebSharp` interacts with the `Electron`,  `Node.js` and ultimately the JavaScript environment via a defined asynchronous bridge interface written as a native module.

The bridge interface is accessed through the `WebSharpJs.Script.ScriptObject` class which represents a JavaScript object and allows very general access to certain functionalities such as:

| Method | Description |
| --- | --- |
| GetProperty<T>(string name) | Retrieves the `name` property value on a JavaScript object |
| SetProperty(string name, object value) | Sets the property `name` on a JavaScript object |
| Invoke<T>(string name, params object[] args) | Invoke the method `name` on a JavaScript object |

The next class is the `WebSharpJs.DOM.HtmlObject` which extends the `WebSharpJs.Script.ScriptObject` adding access to `Events` on a JavaScript object. 

| Method | Description |
| --- | --- |
| AttachEvent(string eventName, System.EventHandler<HtmlEventArgs> handler) | Add an HtmlEventArgs event listener |
| DetachEvent(string eventName, System.EventHandler<HtmlEventArgs> handler) | Remove an HtmlEventArgs event listener |
| AttachEvent(string eventName, System.EventHandler handler) | Add a `System.EventHandler` event listener |
| DetachEvent(string eventName, System.EventHandler handler) | Remove a `System.EventHandler` event listener |

 The `WebSharpJs.DOM.HtmlObject` class is used to allow [DOM access](https://github.com/xamarin/WebSharp/tree/master/docs/tutorials/DOM) and is the class that we will be interested in using to allow access to the `navigator.geolocation` JavaScript object.

## Adding a new source module `GeoLocationAPI.cs`

To keep our project source code clean we will want to add a new source member instead of adding our implementation directly to the `Location.cs` source file.

To do this for compiled assemblies we would just add the source module and then add a reference to it in the `Location.csproj` file.

```xml

  <ItemGroup>
    <Reference Include="System" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Location.cs" />
    <Compile Include="GeoLocationAPI.cs" />
  </ItemGroup>

``` 

This can be edited by hand or from your editor.  Now when you build the assemblies it will automatically be included.

When developing you may prefer to use the On-The-Fly compile option.  This is just as simple but would be accomplished through the JavaScript definition.

Open the `geolocation.js` file and let's take a look.

By default the application creates a function definition into our C# code as follows:

```javascript

var hello = dotnet.func(__dirname + "/Location/Location.cs");

``` 

What we would like to do is include this new source module when compiling the main `Location.cs` source.  The concept is the same as adding a new source module to a `.csproj` file.

```javascript

var hello = dotnet.func({ source: __dirname + "/Location/Location.cs", 
						references: [], 
						itemgroup: [__dirname + "/Location/GeoLocationAPI.cs"], 
						symbols: [] });

```

The `.func` also takes a `<key,value>` options object:

| Key | Definition |
| --- | --- |
| source | The main source file |
| references | A string array of additional references that should be compiled. |
| itemgroup | A string array of additional sources that should be compiled. |
| symbols | A string array of conditional compile symbols that can be used in the sources. |

The one we are interested in is the `itemgroup`.

```javascript

  itemgroup: [__dirname + "/Location/GeoLocationAPI.cs"]

```

> :bulb: You will want to include the `__dirname` as well so that an application that has been packaged can find the source module as well if not using pre-compiled assemblies.

So if you have not already done so go ahead and add a source file named `GeolocationAPI.cs` to the `Location` directory.

```

|--- src                              // sources
     |--- Location
          |--- Location.cs               // Application Implementation
          |--- GeoLocationAPI.cs         // navigator.geolocation implementation

```

## Extending `HtmlObject`

We will get into the implementation details of the `GeoLocation` in just a bit.  First lets get our module setup to access `navigator.geolocation`.

```cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebSharpJs.Script;
using WebSharpJs.DOM;

namespace GeoLocation
{
    // https://dev.w3.org/geo/api/spec-source.html#navi-geo
    public class GeoLocationAPI : HtmlObject
    {
        
        // Save off our instance object so that we do not create the
        // code on every access of Instance().  
        static GeoLocationAPI instance;

        /// <summary>
        /// Obtain a reference to the navigation.geolocation JavaScript object
        /// </summary>
        /// <returns></returns>
        public static async Task<GeoLocationAPI> Instance()
        {
            if (instance == null)
            {
                // Create a new GeoLocationAPI proxy object
                instance = new GeoLocationAPI();
                
                // Create a proxy object of the navigator.geolocation 
                // JavaScript object
                var scriptProxy = new DOMObjectProxy("navigator.geolocation");

                // Initialize the proxy object.  This will compile the code
                // to access the native bridge.
                await scriptProxy.GetProxyObject();
                
                // Tell our GeoLocationAPI instance to use the bridge proxy
                instance.ScriptObjectProxy = scriptProxy;

            }
            return instance;
        }
    }
}
```

First the `using` statements that will be used.

```cs

using WebSharpJs.Script;  // Access to ScriptObject
using WebSharpJs.DOM;     // Access to HtmlObject

```

The following implementation code is mostly boilerplate for a singleton class.  There is no need to create an instance of a `navigator.geolocation` object when only a reference to that object will suffice.  Thus we are using the singleton pattern.

As stated above we will be extending the `HtmlObject` class as it provides everything we need to access Html JavaScript objects.

```cs

   public class GeoLocationAPI : HtmlObject
    {
        
    }

```

Looking a little more into the implementation we have a static `Instance()` method that follows the singleton pattern creation of an object.

```cs

    // Save off our instance object so that we do not create the
    // code on every access of Instance().  
    static GeoLocationAPI instance;

    /// <summary>
    /// Obtain a reference to the navigation.geolocation JavaScript object
    /// </summary>
    /// <returns></returns>
    public static async Task<GeoLocationAPI> Instance()
    {
        if (instance == null)
        {
            // Create a new GeoLocationAPI proxy object
            instance = new GeoLocationAPI();
            
            // Create a proxy object of the navigator.geolocation 
            // JavaScript object
            var scriptProxy = new DOMObjectProxy("navigator.geolocation");

            // Initialize the proxy object.  This will compile the code
            // to access the native bridge.
            await scriptProxy.GetProxyObject();
            
            // Tell our GeoLocationAPI instance to use the bridge proxy
            instance.ScriptObjectProxy = scriptProxy;

        }
        return instance;
    }

```

The work of obtaining the actual reference to `navigator.geolocation` is all done with the following two lines of code.

```cs

    // Create a proxy object of the navigator.geolocation 
    // JavaScript object
    var scriptProxy = new DOMObjectProxy("navigator.geolocation");

    // Initialize the proxy object.  This will compile the code
    // to access the native bridge.
    await scriptProxy.GetProxyObject();


```

The `DOMObjectProxy` object creates and binds the native bridge code with the `navigator.geolocation` JavaScript object.

Now if you go to the `Location.cs` source file we can obtain an instance of the `GeoLocation` object that we just created.

Add the using statement of our new class module.

```cs

using Geolocation;

```

Right after setting our location text we can add a reference to our new class.

```cs

    await location.SetProperty("innerText", "Locating ...");
    var geo = await GeoLocationAPI.Instance();

```

## Summary

In the part of the tutorials we looked very briefly at the how we can access the bridge interface via the `ScriptObject` class.

We then went on to define and include another source file.  When using the On-The-Fly compiling option we have to make a few changes how the C# code is defined to JavaScript.  By using the `itemgroup` option key we can pass in a list of extra source modules that will be included when building the main module function.

We finished up by looking at defining a singleton instance implementation that allows us to access the `navigator.geolocation` JavaScript object from managed code.

In the next part we will look at the actual implementation of the `GeoLocationAPI` module and integrating it within our managed code.

> :bulb: The `navigation.geolocation` can only be referenced in the `Renderer` process.  If you try referencing from the Main process you will get an error about the `navigation` object not being defined.

For more information see the following:

1. Getting started documents

    * [See Getting Started on Windows](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md)
   
    * [See Getting Started on Mac](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md)

1. [Create a new `WebSharp Electron Application`](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-electron-application.md#generate-a-websharp-electron-application)

1. [DOM Overview](https://github.com/xamarin/WebSharp/blob/master/docs/tutorials/DOM/overview.md)

1. [Google API Keys](https://developers.google.com/console)















