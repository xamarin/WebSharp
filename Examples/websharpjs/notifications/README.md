# notifications README

A sample that demonstrates Electron's Notification API using Electron DotNet's WebSharpJs interface. 

More information about the Notification API can be found [here](https://notifications.spec.whatwg.org/#api).

## Features

Interaction with Electrons's Notification API from a C# program.  

* Usage of `WebSharpJs` interface to create bindings between C# and Electron API.
    * The `WebSharpJs` assembly provides the interaction with `Electron`.  This managed assembly exposes the static function `CreateJavaScriptFunction` that accepts a string containing code in `Nodejs` or 'Electron', compiles it and returns a `JavaScript` function callable from the C# implementation.  The `JavaScript` function must have the following signature:

        * It must accept one parameter and a callback.  `return function (data, callback)`
        * The callback must be called with an error and one return value.  `callback(null, null);`

        The following is the pattern when scripting `Nodejs` and `Electron` functions:

``` javascript
            return function (data, callback) {
            
                //...... Your implementation code here .......

                callback(null, null);
            }

```

* Simplified approach of mapping a JavaScript object making it available to C# using WebSharpJs
    * Create a NotificationOptions object.
    
    * Create a Notification object
        * Provide bindings to javascript object that are callable from the C# program.
        * Provide bindings to `onclick`, `onerror` and `onshow` events.
        * Demonstrates how to call properties and functions on a mapped JavaScript object.

* Create Custom EventHandler class.
    * This class allows the developer to attach C# delegates to the `onclick`, `onerror` and `onshow` events of the Notification object.
    * Provides two delegates to demonstrate different ways of marshalling event information between the JavaScript object and the C# program.
        * Handler delegate which provides the main interaction of the EventHandler.
        * Formatter delegate which is a custom implementation that will format the information to be marshalled.
            * Receives the Event and the Notification proxy as a marshalled dynamic object that can be referenced in the C# program.
            * Event objects can not be serialized so in the code we take a short cut so we can get the Event object marshalled from JavaScript to our C# Delegates.
            * See implementation code.

* Demonstrates one approach for interacting with the DOM objects from the C# progarm, in this specific scenario it is attaching a click delegate to the button existing in the DOM which calls back into C# program.

![notifications](images/notifications.gif)

> Note: Implementation presented here may not be complete and only serves as an example for how one could approach the mapping.

## Requirements

You need [node.js](https://nodejs.org/en/) installed and available in your $PATH.
   * Plugins will not be used in this example.

   * `electron-dotnet` needs to be built.  The easiest way is to use the provided `make` files available in the WebSharp base directory.  
   
      * [Windows Prerequisites](https://github.com/xamarin/WebSharp/tree/master/electron-dotnet#building-on-windows)
   
     ``` bash
     # Windows Visual Studio 2015 Command Line Prompt 
     nmake /f Makefile.win buildRelease
     ```

      * [Mac Prerequisites](https://github.com/xamarin/WebSharp/tree/master/electron-dotnet#building-on-osx)
     ``` bash
     # Mac OSX terminal with XCode tools available for build.
     make setup  # only needs to be run the first time
     make build
     ```

## The Structure of the application

```
.
|--- .eslintrc.json
|--- .gitignore
|--- .vscode                           // VS Code integration
     |--- launch.json                  // Launch Configurations
     |--- settings.json
|--- .vscodeignore
|--- electron-dotnet-quickstart.md
|--- index.html                       // Html to be displayed in the app window
|--- jsconfig.json
|--- main.js                          // Defines the electron main process
|--- node_modules
     |--- All the node files used to run the electron application
|--- package.json                     // Various project metadata
|--- README.md                        // You are reading this now
|--- renderer.js                      // Required in index.html and executed in the renderer process for that window 
|--- src                              // sources
|--- src                              // sources
     |--- notifications.cs              // C# implementation 
     |--- project.json                // Defines compilation information 
     |--- notifications.js              // javascript code implementation
```

## Known Issues

Notifications interface may not be complete.  May need some fixing up before used in an actual application.

## Release Notes

### 0.0.1

Initial release

---

**Enjoy!**
