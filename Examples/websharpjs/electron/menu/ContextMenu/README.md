# contextmenu README

Example of context menu using Menu and MenuItem

A context, or right-click, menu can be created with the <code>Menu</code> and <code>MenuItem</code> modules as well. You can right-click anywhere in this app to see an example context menu.

The example uses `Menu` and multiple instances of `MenuItem`s created through the use of the strongly typed `MenuItemOptions` class passed as a paramenter to the `MenuItems`'s `Create` instance method.

The menu itself demonstrates the use of sub menus using predefined `Roles` from the `View` menu item as well as assigning a call back to the `Click` menu items.

## Features

Inside `ContextMenuRenderer.cs` you will find:

- Creating and opening a `BrowserWindow`:

``` c-sharp
                // Create the browser window.
                mainWindow = await BrowserWindow.Create(new BrowserWindowOptions() {Width = 600, Height = 400});
```

> :bulb: The `BrowserWindowOptions` is strongly typed class of available options that is passed to the `BrowserWindow` on creation.  In this case it only specifies the `Width` and `Height` of the window.  For a list of the available bound options see the [browserwindow api documentation](https://github.com/electron/electron/blob/master/docs/api/browser-window.md#new-browserwindowoptions) 

- Loading the content into the `BrowserWindow` by using `LoadURL`:

``` c-sharp
                // and load the index.html of the app.
                await mainWindow.LoadURL($"file://{__dirname}/index.html");
```

The `__dirname` parameter is passed from `main.js` when calling this function.

- Referencing the `WebContents` of the window.

``` c-sharp
                await mainWindow.GetWebContents().ContinueWith(
                        (t) => { 
                            var webContents = t.Result;
                            // rest of your code here
                        }

```

- The `WebContents` is an `EventEmitter` so we can attach to specific events.  In this case we will listen to the `context-menu` event.

``` c-sharp

                            // Subscribe to the "context-menu" event on the WebContents
                            webContents?.On("context-menu", async (cmevt) => 
                            {
                                // menu creation code here

                                // popup our menu here
                                menu.Popup()
                            }

```


- Opening the `DevTools`

``` c-sharp
               // Open the DevTools
                await mainWindow.GetWebContents().ContinueWith(
                        (t) => { t.Result?.OpenDevTools(DevToolsMode.Bottom); }
                );
```

By passing the `DevToolsMode` you can control the dock state of how the development tools are opened.  Defaults to last used dock state. In `Undocked` mode it's possible to dock back. In `Detach` mode it's not.
  Available DevToolsMode enumeration values are:

``` c-sharp
    public enum DevToolsMode
    {
        Right,
        Bottom,
        Undocked,
        Detach
    }
```

- Create our menu that is called from the `context-menu` event listener above.

``` c-sharp

                                // if we have not created the menu then do it now
                                if (menu == null)
                                {
                                    menu = await Menu.Create();
                                    await menu.Append(await MenuItem.Create(new MenuItemOptions() { Label = "Hello", Click = MenuItemClicked }));
                                    await menu.Append(await MenuItem.Create(new MenuItemOptions() { Type = MenuItemType.Separator }));
                                    await menu.Append(await MenuItem.Create(
                                                        new MenuItemOptions() {Label = "View",
                                                                SubMenuOptions = new MenuItemOptions[]
                                                                {
                                                                    new MenuItemOptions() {Role = MenuItemRole.Reload},
                                                                    new MenuItemOptions() {Role = MenuItemRole.ToggleDevTools},
                                                                    new MenuItemOptions() {Type = MenuItemType.Separator},
                                                                    new MenuItemOptions() {Role = MenuItemRole.ResetZoom},
                                                                    new MenuItemOptions() {Role = MenuItemRole.ZoomIn},
                                                                    new MenuItemOptions() {Role = MenuItemRole.ZoomOut},
                                                                    new MenuItemOptions() {Type = MenuItemType.Separator},
                                                                    new MenuItemOptions() {Role = MenuItemRole.ToggleFullScreen},
                                                                }
                                                        }
                                    ));
                                    await menu.Append(await MenuItem.Create(new MenuItemOptions() { Type = MenuItemType.Separator }));
                                    await menu.Append(await MenuItem.Create(new MenuItemOptions() { Label = "WebSharp", Type = MenuItemType.Checkbox, Checked = true, Click = MenuItemClicked }));
                                }

``` 

> :bulb: Notice the way to specify submenu items from the `View` menu item.  Use `SubMenuOptions` to specify an array of `MenuItemOptions[]` or `SubMenuMenu` to specify an already created `Menu` instance.

Setting up and calling the create method is done through the `main.js` javascript file that creates the application.

- We reference our `ContextMenuRenderer` module.

``` js
    var mainbrowserwindow = require("./src/contextmenu.js");
```
The `contextmenu.js` exports `createMainWindow` function that we will use to call the `MainWindow` module. 

- In the `createwindow` function we call the exported `createMainWindow` function. 

``` js
function createWindow () {

  var windowId = mainbrowserwindow.createMainWindow(__dirname);

  if (windowId) console.log('Main Window Id: ' + result)

}

```

![screen shot windows](images/contextmenu-windows.png)

![screen shot mac](images/contextmenu-mac.png)

More information can be found in the [Menu documentation](https://electron.atom.io/docs/api/menu/#class-menu)

## Requirements

   * `electron-dotnet` needs to be built.  The easiest way is to use the provided `make` files available in the WebSharp base directory.  
   
      * [See Getting Started on Windows](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md)
   
      * [See Getting Started on Mac](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md)

> :bulb: Windows users need to make sure [Mono is available](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md#setting-mono-path) in their %PATH%.

## Known Issues



## Release Notes

### 1.0.0

Initial release