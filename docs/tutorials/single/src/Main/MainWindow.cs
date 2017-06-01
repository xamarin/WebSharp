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

                var isQuit = await MakeSingleInstance();
                if (isQuit)
                    return await app.Quit();

                // We use app.IsReady instead of listening for the 'ready'event.
                // By the time we get here from the main.js module the 'ready' event has
                // already fired.
                if (await app.IsReady())
                {
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
            mainWindow = await BrowserWindow.Create(new BrowserWindowOptions() { Width = 600, Height = 400 });

            // and load the index.html of the app.
            await mainWindow.LoadURL($"file://{__dirname}/index.html");

            // Open the DevTools
            //await mainWindow.GetWebContents().ContinueWith(
            //        (t) => { t.Result?.OpenDevTools(DevToolsMode.Bottom); }
            //);

            return await mainWindow.GetId();

        }

        // Make this app a single instance app.
        //
        // The main window will be restored and focused instead of a second window
        // opened when a person attempts to launch a second instance.
        //
        // Returns true if the current version of the app should quit instead of
        // launching.
        async Task<bool> MakeSingleInstance()
        {
            return await app.MakeSingleInstance(new ScriptObjectCallback<string[], string>(
                    async (cr) =>
                    {
                        var state = cr.CallbackState as object[];
                        if (state != null)
                        {
                            // let's get the arguments that we were called with
                            var args = state[0] as object[];

                            await console.Log($"we have {args?.Length} args ");
                            foreach (string arg in args)
                                await console.Log($"   arg: {arg}");
                            if (state[1] != null)
                                await console.Log($"working directory: {state[1]}");
                        }

                        if (mainWindow != null)
                        {
                            if (await mainWindow.IsMinimized())
                                await mainWindow.Restore();
                            await mainWindow.Focus();
                        }
                        
                    }


                ));
        }       
    }
//}
