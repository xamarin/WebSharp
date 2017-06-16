# How To Minimize Your Application To The System's Notification Area.

The ability to minimize the application to the system's notification area, `System Tray` on Windows or `Status Menu Area` on a Mac, can be quite useful in keeping a clean work area.  One specific reason to provide this functionality would be when an application needs to run in the background but still be available if the user needs quick access to it.

When we minimize the application's main window we normally will remove it from the `Task Bar` on Windows or the `Dock` on Mac OSX.

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

### Handling the `close` event



## Summary

The trick is to catch the window's `close` and `minimize` events in the correct spots.
