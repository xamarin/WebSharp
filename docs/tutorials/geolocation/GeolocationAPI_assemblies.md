# Creating Assemblies

In the previous parts we setup our application and now we are almost ready to start packaging our application.  One thing to consider though is that by default the managed code is compiled each time the application is run.  For a small application like this it may be fine but for larger ones these delays, while the source code is being compiled, may not be what you are looking for.

In this tutorial, we will be looking at compiling our source files that make up our application into libraries that can be loaded without the use of the `On-The-Fly` compiling.  If you read through [Getting Started Building Websharp Electron Application Assemblies](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-building-assemblies.md) you may find that is all you need.  If so then you can go ahead and skip the rest of this tutorial.

What we are going to focus on is how we can modify the application to run with both generated assemblies when packaged and use the `On-The-Fly` option while developing.

`Electron` does not offer any built in way to determine whether the app is being run in development or is packaged and running on an end user's machine.

There is a complete discussion of this found in the issue [Distinguishing "development" from "production"](https://github.com/electron/electron/issues/7714).

The solution that we show here is to use the [electron-is-dev](https://www.npmjs.com/package/electron-is-dev) npm package.  Use whatever works for your platform and environment.  The main point to take away from this tutorial is not the how to determine but the touchpoints in the application that need to be modified.

## Installing `electron-is-dev`

[electron-is-dev](https://www.npmjs.com/package/electron-is-dev) can be installed just as any other npm package.

Straight from the [Install](https://www.npmjs.com/package/electron-is-dev#install).

```bash
$ npm install --save electron-is-dev
```

> :bulb: Make sure you use the `--save` command so that when we package the application it will be included as a dependency.

## Preparing the sources

First, we will start by modifying the source files as outlined in [Getting Started Building Websharp Electron Application Assemblies](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-building-assemblies.md) which basically says that if we want to use the built assemblies then we need to make sure we have a `namespace` specified.

### Main process

We will start with the `Main` process.

#### Main: main.js

Open the `main.js` file and look for the `dotnet` definition.

```javascript

var dotnet = require('electron-dotnet');

//var main = dotnet.func(__dirname + "/src/Main/bin/Debug/MainWindow.dll");
var main = dotnet.func(__dirname + "/src/Main/MainWindow.cs");

```

From the `electron-is-dev` [Usage](https://www.npmjs.com/package/electron-is-dev#usage) section we need to wrap our code.  If the `require` is `true` then we are running in development and `false` then we are running a packaged version.

```javascript

var dotnet = require('electron-dotnet');

if (!require('electron-is-dev'))
  var main = dotnet.func(__dirname + "/src/Main/bin/Debug/MainWindow.dll");
else
  var main = dotnet.func(__dirname + "/src/Main/MainWindow.cs");

```

Now just one more little tweak.  We have now determined which process to run depending on development or production.

Above we mentioned that when loading from an assembly then we need to make sure we have a `namespace` specified.  That seems to be a real pain every time we want to change from one to the other then we have to keep modifying the sources.  One way to ease this is to use the `conditional compile symbols` parameter `symbols` when defining our source options.  When we added the extra source file into our `Location` by using the `itemgroup` parameter we also saw an option in the list for symbols.

The `.func` also takes a `<key,value>` options object:

| Key | Definition |
| --- | --- |
| source | The main source file |
| references | A string array of additional references that should be compiled. |
| itemgroup | A string array of additional sources that should be compiled. |
| symbols | A string array of conditional compile symbols that can be used in the sources. |

Define a `DEV` symbol to be passed so that we can wrap the `namespace` with it.  Of course, you can add as many as you would like.

```javascript

var dotnet = require('electron-dotnet');

if (!require('electron-is-dev'))
  var main = dotnet.func(__dirname + "/src/Main/bin/Debug/MainWindow.dll");
else
  var main = dotnet.func({source : __dirname + "/src/Main/MainWindow.cs", symbols: ["DEV"]});

```

#### Main: src/Main/MainWindows.cs

Now we need to get the associated source file ready.

Open the `src/Main/MainWindows.cs` source file and look for the commented out namespace line

```csharp

//namespace MainWindow
//{

```

and wrap it with the conditional compile symbol `DEV`.  Don't forget to remove the `//`'s from the beginning of the source lines.

```csharp

#if !DEV
namespace MainWindow
{
#endif 

```

Also, change the last commented out `//}` as well.

```csharp

#if !DEV
}
#endif

```

### Renderer process

The `Renderer` files will need to change as well.  We will just present the code instead of stepping through the changes as with the `Main` process files.

#### Renderer: /src/Location/geolocation.js

```javascript

if (!require('electron-is-dev'))
    var hello = dotnet.func(__dirname + "/Location/bin/Debug/Location.dll");
else
    var hello = dotnet.func({ source: __dirname + "/Location/Location.cs", 
                         references: [], 
                         itemgroup: [__dirname + "/Location/GeoLocationAPI.cs"], 
                         symbols: ["DEV"] });


```

#### Renderer: /src/Location/Location.cs

The `namespace` change as follows:

```csharp

#if !DEV
namespace Location
{
#endif

```

And do not forget the ending `//}`.

```csharp

#if !DEV
}
#endif

```

## Building the assemblies

The source file changes should now be out of the way and the project assemblies can be built.

When an application is generated there is also a common solution file generated called `Build.sln` in the source directory.

Instead of building each project separately, this solution file allows the developer to build all projects with a single common command.

Change to the project's `src` directory.

### Build.sln: Restore packages

Do a restore of the NuGet packages.

* Windows

```
\src> msbuild Build.sln /p:Configuration=Debug /p:Platform="Any CPU" /t:restore
```

* Mac

```bash
/src$ msbuild Build.sln /p:Configuration=Debug /p:Platform="Any CPU" /t:restore
```

### Build.sln: Build

* Windows

```
\src> msbuild Build.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

* Mac

```bash
/src$ msbuild Build.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

### Visual Studio IDE

The `Build.sln` file can also be opened in `Visual Studio` on either Windows or Mac.


## Testing

If you run the application again we will still get the `On-The-Fly` compiled version since we have not packaged it.  To test this it can be overridden by setting the `ELECTRON_IS_DEV` environment variable to `1`.

Somewhere in the `MainWindow.cs` source file the following lines can be added and when run can be viewed in the `console`.

```csharp

#if !DEV
    await console.Log("using .dll");
#else
    await console.Log("using OTF");
#endif

``` 

Now when run you should see a message printed that shows whether the application is using the assemblies or `On-The-Fly` compile option.

## Summary

This wraps up the application with one option that will help with managing which environment you are running with `development` or `production`.

By using the `conditional compile symbols` option when defining the managed code to JavaScript the sources become easier to handle the two environments.

The solution presented here is what was chosen for this simple project and may not meet all the requirements for your next application.  Make sure to look through the [Distinguishing "development" from "production"](https://github.com/electron/electron/issues/7714) issue.  They may implement something in the future.

A benefit of using assemblies is the execution speed.  A noticeable difference can be seen on startup.

In the [next tutorial](./GeolocationAPI_packager.md) we will be looking at using `electron-packager` to package our application into an executable that can be executed on the targeting platform.

For more information see the following:

1. Getting started documents

    * [See Getting Started on Windows](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md)
   
    * [See Getting Started on Mac](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md)

1. [Create a new `WebSharp Electron Application`](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-electron-application.md#generate-a-websharp-electron-application)

1. [DOM Overview](https://github.com/xamarin/WebSharp/blob/master/docs/tutorials/DOM/overview.md)

1. [Google API Keys](https://developers.google.com/console)

1. [Electron Environment Variables](https://github.com/electron/electron/blob/master/docs/api/environment-variables.md#environment-variables)

1. [Getting Started Building Websharp Electron Application Assemblies](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-building-assemblies.md)

1. [Distinguishing "development" from "production"](https://github.com/electron/electron/issues/7714).

1. [electron-is-dev](https://www.npmjs.com/package/electron-is-dev)
