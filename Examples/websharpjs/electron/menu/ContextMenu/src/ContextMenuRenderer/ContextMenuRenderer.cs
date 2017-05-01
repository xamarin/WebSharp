using System;
using System.Threading.Tasks;

using WebSharpJs.Electron;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

//namespace ContextMenuRenderer
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

        static BrowserWindow mainWindow;
        static Menu menu;

        /// <summary>
        /// Default entry into managed code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<object> Invoke(string __dirname)
        {
            if (console == null)
                console = await WebSharpJs.NodeJS.Console.Instance();

            try
            {
                // Create the browser window.
                mainWindow = await BrowserWindow.Create(new BrowserWindowOptions() {Width = 600, Height = 400});

                // and load the index.html of the app.
                await mainWindow.LoadURL($"file://{__dirname}/index.html");

                await mainWindow.GetWebContents().ContinueWith(
                        (t) => { 
                            var webContents = t.Result;

                            // Subscribe to the "context-menu" event on the WebContents
                            webContents?.On("context-menu", async (cmevt) => 
                            {
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

                                // popup our menu here
                                menu.Popup();
                                return null;
                            } );

                            // Open the DevTools
                            webContents?.OpenDevTools(DevToolsMode.Bottom);    
                        }
                );

                // Emitted when the window is closed.
                await mainWindow.On("closed", async (evt) => 
                {
                    await console.Log("Received closed event");
                    System.Console.WriteLine("Received closed event");
                    mainWindow = null;
                    return null;
                });

                await console.Log($"Loading: file://{__dirname}/index.html");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return await mainWindow.GetId();
        }

        async Task<object> MenuItemClicked (object[] mi)
        {
            try
            {
                var menuItemSelected = (MenuItem)(ScriptObjectHelper.AnonymousObjectToScriptObjectProxy(mi[0]));
                console.Log($"MenuItem: {await menuItemSelected.GetLabel()} was selected and the item is checked {await menuItemSelected.GetChecked()}");
            }
            catch (Exception exc)
            {
                System.Console.WriteLine(exc);
            }

            return null;
        }
    }
//}
