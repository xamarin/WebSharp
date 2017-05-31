# Showing Windows Gracefully - Setting BackgroundColor

This tutorial will focus on the startup flicker of an Electron App and one approach to fixing it by setting the background color.

What causes the flicker is the initial loading period from when the app launches to when the initial content is loaded in.  Fast when using a local html file from the file system but there is still the initial flicker that takes away from how a native app might work.

[Create a new `WebSharp Electron Application`](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-electron-application.md#generate-a-websharp-electron-application) and open it in you favorite source editor.

Let's add a `<style></style>` element to the `index.html` file to demonstrate the flicker that was mentioned above.

Open the `index.html` source file and  

``` html
   <style media="screen">
      body {
        height: 100%;
        background-color: blueviolet
      }
    </style>
```

We will want to place this in `<head>` element after `<title>` as shown below:

``` html

  <head>
    <meta charset="UTF-8">
    <title>GracefulLoad</title>
    <style media="screen">
      body {
        height: 100%;
        background-color: blueviolet
      }
    </style>
  </head>

```

Now run the app.  

``` bash
# MacOSX Terminal or Windows command line
> npm start

```

As you can see we get the white background first and then the background color changes as the `index.html` is loaded in.  This is the flicker that we are referring to.

![flicker](images/flicker.gif)

One of the possible solutions for this is mentioned in the Electron documentation [Showing window gracefully](https://github.com/electron/electron/blob/master/docs/api/browser-window.md#showing-window-gracefully).

The solution we will be looking at is [setting backgroundcolor](https://github.com/electron/electron/blob/master/docs/api/browser-window.md#setting-backgroundcolor) alternative instead of the using the `ready-to-show` event solution.

The documentation recommends showing the window immediately, and setting the a `BackgroundColor` close to your app's background.

Go to the `src/Main/MainWindow.cs` source file and look for the `CreateWindow` method.

```cs

        async Task<int> CreateWindow (string __dirname)
        {
            // Create the browser window.
            mainWindow = await BrowserWindow.Create(new BrowserWindowOptions() { Width = 600, Height = 400 });

            // and load the index.html of the app.
            await mainWindow.LoadURL($"file://{__dirname}/index.html");

            // Open the DevTools
            //await mainWindow.GetWebContents().ContinueWith(
            //        (t) => { t.Result?.OpenDevTools(DevToolsMode.Bottom); }
            //);

            return await mainWindow.GetId();

        }

```

The `BrowserWindow` takes an instance of `BroswerWindowOptions` when creating the instance.  

```cs

            // Create the browser window.
            mainWindow = await BrowserWindow.Create(new BrowserWindowOptions() { Width = 600, Height = 400 });

```

What we will want to do is set the `BackgroundColor` property of the `BrowserWindowOption` instance that we will pass to the `Create` method of `BrowserWindow`. 

```cs

            // Create the browser window.
            mainWindow = await BrowserWindow.Create(new BrowserWindowOptions() { Width = 600, Height = 400, BackgroundColor = "#8A2BE2" });

```

`BackgroundColor` in an optional property that sets the Window's background color as `Hexadecimal` value, like `#66CD00` or `#FFF` or `#80FFFFFF` (alpha is supported). Default is `#FFF` (white).

More values can be found in the [Color Values](https://www.w3schools.com/colors/colors_hex.asp) section of the html documentation.

Now let's run the application again.

``` bash
# MacOSX Terminal or Windows command line
> npm start

```

![flicker](images/flicker-end.gif)

## Summary

In this tutorial we have attempted to fix the `white screen` flicker when starting the app.  If your application uses a combination of colors then setting the background color may not be enough.  The docs make a note of saying:

> Note that even for apps that use `ready-to-show` event, it is still recommended
to set `backgroundColor` to make app feel more native.

:bulb: By default when creating a `WebSharp Electron Application`, on-the-fly compiling is used and does have it's own delay that is added to the startup.  To compile the modules take a look at [Getting Started Building Websharp Electron Application Assemblies](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-building-assemblies.md).
