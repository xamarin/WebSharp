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

                // We use app.IsReady instead of listening for the 'ready'event.
                // By the time we get here from the main.js module the 'ready' event has
                // already fired.
                if (await app.IsReady())
                {
                    windowId = await CreateWindow(__dirname);
                }


                await console.Log($"Loading: file://{__dirname}/index.html");

            // Note: Sending a synchronous message from the render process will block 
            // the whole renderer process, unless you know what you are doing you should never use it.
            //
            // We will define the synchronous function via a javascript function
            // There is a limitation on synchronous messages via the CLR because of the 
            // event.returnValue will not be set via the CLR.
            // If synchronous messaging is desired then a CLR implementation is what
            // you want.
            var synchronousFunction = await WebSharpJs.WebSharp.CreateJavaScriptFunction(
                    @"
                        return function (data, callback) {
                            const {ipcMain} = require('electron')
                            ipcMain.on('synchronous-message', (event, arg) => {
                                console.log(arg)  // prints 'ping'
                                event.returnValue = 'synchronous pong'
                            });
                            callback(null, null);

                        }");

                await synchronousFunction(null); // attach our synchronous message listener

                IpcMain ipcMain = await IpcMain.Create();
                ipcMain.On("asynchronous-message",
                    new IpcMainEventListener
                    (
                        async(result) =>
                        {
                            var state = result.CallbackState as object[];
                            var ipcMainEvent = (IpcMainEvent)state[0];
                            var parms = state[1] as object[];

                            System.Console.WriteLine($"Asynchronous message from: {await ipcMainEvent.Sender.GetTitle()}");
                            foreach (var parm in parms)
                                System.Console.WriteLine($"\tparm: {parm}");
                        
                            await ipcMainEvent.Sender.Send("asynchronous-reply", "asynchronous-pong1", "asynchronous-pong2");
                        }
                    )
                );

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
    }
//}
