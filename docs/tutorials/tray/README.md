# How To Minimize Your Application To The System's Notification Area.

The ability to minimize the application to the system's notification area, `System Tray` on Windows or `Status Menu Area` on a Mac, can be quite useful in keeping a clean work area.  One specific reason to provide this functionality would be when an application needs to run in the background but still be available if the user needs quick access to it.

When we minimize the application's main window we normally will remove it from the `Task Bar` on Windows or the `Dock` on Mac OSX.  The same goes for when the window is closed but needs to be handled differently on the two platforms.

`Electron` allow access to these areas by use of the [Tray object](https://github.com/electron/electron/blob/master/docs/api/tray.md). 

So let's look at some code.

You can start with a new project or follow along with the accompanying code.

[Create a new `WebSharp Electron Application`](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-electron-application.md#generate-a-websharp-electron-application) and open it in you favorite source editor.

## Creating the Tray

Open the `src/Main/MainWindow.cs` source file.  We will start with the `Tray` code by adding a new `CreateTray` method.

```cs

        async Task CreateTray (string __dirname)
        {
            
            if (IsWindows)
                tray = await Tray.Create($"{__dirname}/assets/icons/appicon.ico");
            else
                tray = await Tray.Create($"{__dirname}/assets/icons/appicon_16x16@2x.png");
            
            // Sets the hover text for this tray icon.
            await tray.SetToolTip("MinimizeToTray Sample Program");

            // Sets the title displayed aside of the tray icon in the status bar.
            //await tray.SetTitle("MinimizeToTray");

            // Create a context menu to be displayed on the Tray
            var menuItemOptions = new MenuItemOptions[]
            {
                new MenuItemOptions() { Label = "Show App",
                    Click = new ScriptObjectCallback<MenuItem, BrowserWindow, Event> (
                            async (ar) =>
                            {
                                //await console.Log("Show App");
                                if (mainWindow != null)
                                {
                                    if (await mainWindow.IsMinimized())
                                        await mainWindow.Restore();
                                    if (!await mainWindow.IsVisible())
                                        await mainWindow.Show();
                                    await mainWindow.Focus();
                                }
                            }
                        )
                },
                new MenuItemOptions() { Label = "Quit App",
                    Click = new ScriptObjectCallback<MenuItem, BrowserWindow, Event> (
                            async (ar) =>
                            {
                                //await console.Log("Quit App");
                                // Notify the close event that we will be quitting the app
                                await app.SetProperty("shouldQuit", true);
                                mainWindow = null;
                                await app.Quit();
                            }
                        )
                },

            };

            var contextMenu = await Menu.BuildFromTemplate(menuItemOptions);

            // Set our tray context menu
            await tray.SetContextMenu(contextMenu);

            // Allow the user to click on the tray icon to show the context menu.
            await tray.On("click", new ScriptObjectCallback (
                async (sr) =>
                {
                    if (mainWindow != null)
                    {
                        await tray.PopUpContextMenu();
                    }
                })
            );
        
        }

```

The first lines of code in the above method create the `Tray` but we need to know what platform we are targeting so that we can set the icon correctly.  It calls the helper property `IsWindows` to determine what platform we are targeting and pass in the correct file to load.

```cs

        static bool IsWindows
        {
            get {
                // Mac can also return Unix running under mono.
                return !(System.Environment.OSVersion.Platform == PlatformID.MacOSX || System.Environment.OSVersion.Platform == PlatformID.Unix);
            }
        }

```

> :bulb: On Mac using `Mono` the `Platform` will return `PlatformID.Unix`.

The code above should be easily understood through the comments provided but here are the steps.

- Create and setup a `Tray` instance.
- Create an array of `MenuOptions` that will be used to create the context menu.
- Create the actual menu with `await Menu.BuildFromTemplate` and attach it to the `Tray` with `SetContextMenu`.
- The last step here attaches a listener for the `click` event and when received pops up the context menu.  This could also have restored the application window as well.

To call the `CreateTray` method we will place it right before we create the main browser window in the main `Invoke` method.

```cs

                // We use app.IsReady instead of listening for the 'ready'event.
                // By the time we get here from the main.js module the 'ready' event has
                // already fired.
                if (await app.IsReady())
                {
                    await CreateTray(__dirname);
                    windowId = await CreateWindow(__dirname);
 
                }

``` 

That should do it.  If you now run the code your should see that the tray now appears in the `System Menu area` on Mac and in the `System Tray` on windows.

### Context Menu Items

In the above code there was a context menu added to the `Tray` that we created with two menu items.

- `Show App` menu item handles toggling of the window's visibility.

```js

                new MenuItemOptions() { Label = "Show App",
                    Click = new ScriptObjectCallback<MenuItem, BrowserWindow, Event> (
                            async (ar) =>
                            {
                                //await console.Log("Show App");
                                if (mainWindow != null)
                                {
                                    if (await mainWindow.IsMinimized())
                                        await mainWindow.Restore();
                                    if (!await mainWindow.IsVisible())
                                        await mainWindow.Show();
                                    await mainWindow.Focus();
                                }
                            }
                        )
                }

```

- `Quit App` quits the application and is not as straight forward.  What should be pointed out in that code is that a specific property will be added on the `app` called `shouldQuit`.  The property will interact with the `close` event processing discussed below and will make a little more sense later.  

```js

                new MenuItemOptions() { Label = "Quit App",
                    Click = new ScriptObjectCallback<MenuItem, BrowserWindow, Event> (
                            async (ar) =>
                            {
                                //await console.Log("Quit App");
                                // Notify the close event that we will be quitting the app
                                await app.SetProperty("shouldQuit", true);
                                mainWindow = null;
                                await app.Quit();
                            }
                        )
                },


```



## Window Minimize and Close

The trick is to catch the window's `close` and `minimize` events in the correct spots in `JavaScript` as well as our managed code.

### Handling the `minimize` event

Look for the `CreateWindow` method in the `src/Main/MainWindow.cs` source file.  When the `minimize` event is received we will want to hide the main window.

```cs

            // Listen for window minimize event so that we can hide
            // the main window.
            await mainWindow.On("minimize",
                new ScriptObjectCallback<Event>(async (evt) =>
                {
                    await mainWindow.Hide();
                }
            ));

```

On Windows when the application window is hidden the application icon is removed from the `Task Bar`.  With Mac OSX a little more work is needed to handle the `Dock`.

Two other events, `show` and `hide`, need to be listened for that will allow us to remove the application icon from the `Dock` on Mac OSX platforms. 

On `show` we we will need to show the `Dock` again.

```cs

            // Listen for window show event
            await mainWindow.On("show",
                new ScriptObjectCallback<Event>(async (evt) =>
                {
                    //console.Log("Show");
                    await tray.SetHighlightMode(TrayHighlightMode.Always);

                    // Mac specific
                    // During development this will not work well.
                    // The icon will revert back to the Electron icon.  Bug??
                    var dock = await app.Dock();
                    if (dock != null)
                    {
                        await dock.Show();
                        await dock.SetIcon($"{__dirname}/assets/icons/appicon.png");
                    }
                }
            ));

```

The `hide` event will allow the application to hide the `Dock`.

```cs
            // Listen for window show event
            await mainWindow.On("hide",
                new ScriptObjectCallback<Event>(async (evt) =>
                {
                    //console.Log("Hide");
                    await tray.SetHighlightMode(TrayHighlightMode.Never);

                    // Mac specific
                    var dock = await app.Dock();
                    if (dock != null)
                        await dock.Hide();
                }
            ));

```

> :bulb: When developing for Mac and the 'Dock' is hidden and then shown again you will see the `Electron` icon instead of the application icon.  Once packaged and branded this will not happen.  See the tutorial [Setting Application Icon](https://github.com/xamarin/WebSharp/blob/master/docs/tutorials/appicon/README.md) for more information on this.

### Mac specific `Dock` object

The `Dock` object is Mac specific and will return `null` when referenced from other platforms.

| Method | Description |
| --- | --- |
| `int` Bounce (DockBounceType) | Can be `DockBoundType.Critical` or `DockBoundType.Informational`. |
| CancelBounce(int id) | Cancel the bounce of `id`. |
| DownloadFinished(string filePath) | Bounces the Downloads stack if the filePath is inside the Downloads folder.|
| `bool` SetBadge(string text) | Sets the string to be displayed in the dock’s badging area. |
| `string` GetBadge() | The badge string of the dock. |
| Hide () | Hides the dock icon. |
| Show() | Shows the dock icon. |
| `bool` IsVisible() | Whether the dock icon is visible. |
| SetMenu(Menu menu) | Sets the application's [Dock Menu](https://developer.apple.com/macos/human-interface-guidelines/menus/dock-menus/).

## Handling the `close` event

Handling the `close` event of the window will need a little extra care.  The termination of the application should only happen when the menu option `Quit App` is selected from the `Tray` or when `Cmd-Q` is executed on Mac.  Any other attempts at quitting the application on either platform will result in the application window being hidden, removal of the app icon from the `Task Bar` or `Dock` which moves the application into the background accessible only from the `Tray`. 

To accomplish this the application needs to use the `PreventDefault` of the `close` event handler.

>  The `Event` interface's `preventDefault()` method tells the user agent that if the event goes unhandled, its default action should not be taken as it normally would be. The event continues to propagate as usual with only the exception that the event does nothing if no event listeners call `stopPropagation()`, which terminates propagation at once.  


> :exclamation: **Limitation of Manage Code Interface**: Due to the asynchronous interface provided by the managed code interface it is not be able to execute `Event.PreventDefault`.  By the time the function is executed it is too late to acually cancel the event itself.  This is unfortunately a limitation of the managed code async interface.  This will need to be handled in `JavaScript` code.

So let's open the `main.js` file and add an event listener for `close` in the `createWindow`.

```js

            // Emitted when the window is going to be closed. 
            // It's emitted before the beforeunload and unload event of the DOM. 
            mainWindow.on('close', function (e) {

              // Check if we should quit
              if (app.shouldQuit) {
                // Dereference the window object, usually you would store windows
                // in an array if your app supports multi windows, this is the time
                // when you should delete the corresponding element.
                mainWindow = null
              }
              else {
                // Calling event.preventDefault() will cancel the close.
                e.preventDefault();
                // Then we hide the main window.
                mainWindow.hide()
              }
            })

```

The close handler checks for a specific `shouldQuit` property on the `app` to be `true` and if not then the event's default functionality to close the window is canceled and is instead hidden from view with [Event.preventDefault()](https://developer.mozilla.org/en/docs/Web/API/Event/preventDefault).

This prevents the application from terminating only if specifically requested to do so when the `App Quit` menu option is clicked. 

If the application is run now it should act as expected on both Mac and Windows platforms but one more condition for the `Cmd-Q` situation on Mac needs to be handled.

When `Cmd-Q` is executed the the `before-quit` listener is fired on Mac platforms and will need to be handled in `JavaScript` as well.  

```js

// We need this for Mac functionality for Cmd-Q and Quit from app menu.

// 'before-quit' is emitted when Electron receives
// the signal to exit and wants to start closing windows 
// placed here instead of managed code so we catch the event in the correct order.
app.on('before-quit', function () { app.shouldQuit  = true; });

```

The above code only sets the `shouldQuit` property on `app` to `true` to notify the `close` handling above that it should quit the application instead of preventing the close.

Now when running the application it should work more or less the same on both platforms.


## Summary

The trick is to catch the window's `close` and `minimize` events in the correct spots.
