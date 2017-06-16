using System;
using System.Threading.Tasks;

using WebSharpJs.Electron;
using WebSharpJs.Script;

//namespace MainWindow
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

        static App app;
        static BrowserWindow mainWindow;
        static int windowId = 0;
        static Tray tray;

        /// <summary>
        /// Default entry into managed code.
        /// </summary>
        /// <param name="__dirname">The directory name of the current module. This the same as the path.dirname() of the __filename.</param>
        /// <returns></returns>
        public async Task<object> Invoke(string __dirname)
        {
            if (console == null)
                console = await WebSharpJs.NodeJS.Console.Instance();

            try
            {
                app = await App.Instance();

                // We use app.IsReady instead of listening for the 'ready'event.
                // By the time we get here from the main.js module the 'ready' event has
                // already fired.
                if (await app.IsReady())
                {
                    await CreateTray(__dirname);
                    windowId = await CreateWindow(__dirname);
 
                }

                await console.Log($"Loading: file://{__dirname}/index.html");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return windowId;


        }
        async Task<int> CreateWindow (string __dirname)
        {
            // Create the browser window.
            mainWindow = await BrowserWindow.Create(
                new BrowserWindowOptions() { 
                    Width = 600, 
                    Height = 400,
                    IconPath = $"{__dirname}/assets/icons/appicon.ico",
                }
            );

            // Listen for window minimize event so that we can hide
            // the main window.
            await mainWindow.On("minimize",
                new ScriptObjectCallback<Event>(async (evt) =>
                {
                    //console.Log("Minimize");
                    await mainWindow.Hide();
                }
            ));

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

            // and load the index.html of the app.
            await mainWindow.LoadURL($"file://{__dirname}/index.html");

            // Open the DevTools
            //await mainWindow.GetWebContents().ContinueWith(
            //        (t) => { t.Result?.OpenDevTools(DevToolsMode.Bottom); }
            //);

            return await mainWindow.GetId();

        }

        
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

        static bool IsWindows
        {
            get {
                // Mac can also return Unix running under mono.
                return !(System.Environment.OSVersion.Platform == PlatformID.MacOSX || System.Environment.OSVersion.Platform == PlatformID.Unix);
            }
        }
    }
//}
