using System;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

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

                    var ipcMain = await IpcMain.Create();

                    ipcMain.On("print-to-pdf",
                        new IpcMainEventListener
                        (
                            async(result) =>
                            {
                                var state = result.CallbackState as object[];
                                var ipcMainEvent = (IpcMainEvent)state[0];
                                var parms = state[1] as object[];

                                var win = await BrowserWindow.FromWebContents(ipcMainEvent.Sender);
                                System.Console.WriteLine($"Asynchronous message from: {await win.GetTitle()}");
                                // foreach (var parm in parms)
                                //     System.Console.WriteLine($"\tparm: {parm}");



                                var contents = await win.GetWebContents();
                                await contents.PrintToPDF(new PrintToPDFOptions(),
                                    new ScriptObjectCallback<Error, byte[]>(
                                        async (cr) =>
                                        {
                                            await console.Log("callback in Print to Pdf");
                                            var PDFState = cr.CallbackState as object[];
                                            var error = PDFState[0] as Error;
                                            if (error != null)
                                                throw new Exception(error.Message);
                                            var buffer = PDFState[1] as byte[];
                                            string filename = Path.GetTempFileName() + ".pdf"; 
                                            File.WriteAllBytes(filename, buffer);
                                            var process = Process.Start(filename);
                                            await ipcMainEvent.Sender.Send("wrote-pdf", filename);

                                        }
                                    )


                                );
                            }
                        )
                    );
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
    }
//}
