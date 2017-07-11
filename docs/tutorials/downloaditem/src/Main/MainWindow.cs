using System;
using System.Threading.Tasks;

using WebSharpJs.Electron;
using WebSharpJs.Script;
using WebSharpJs.DOM;


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

                    // var web = await mainWindow.GetWebContents();
                    // var session = await web.GetSession();

                    // There are functionality differences between mac and windows.  You will need to try
                    // this to see if it works for you in all scenarios.  For instance this works on a Mac,
                    // at least it does on the ones that were tested, but windows always displays the save dialog.
                    // await session.On("will-download",
                    //     new ScriptObjectCallback<Event, DownloadItem, WebContents>(
                    //         async (callbackResult) => 
                    //         {
                    //             var cr = new WillDownloadResult(callbackResult);         
                    //             await cr.DownloadItem.SetSavePath($"downloads/");
                    //             var fileURL = await cr.DownloadItem.GetURL();
                    //             await console.Log($"Downloading: {fileURL}");
                    //             await HandleDownload(cr, fileURL);
                    //         }
                    //     )
                    // );

                    // attach a listener to download the file.
                    var ipcMain = await IpcMain.Create();
                    ipcMain.On("download-file",
                        new IpcMainEventListener
                        (
                            async(result) =>
                            {
                                var state = result.CallbackState as object[];
                                var ipcMainEvent = (IpcMainEvent)state[0];
                                var parms = state[1] as object[];

                                // foreach (var parm in parms)
                                //     System.Console.WriteLine($"\tparm: {parm}");

                                // Call the DownloadFile helper method
                                await DownloadFile(parms[0].ToString());
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

        // Attaches a will-download listener to the session and initiates the download
        async Task DownloadFile(string fileURL)
        {
            // Get a reference to a Session object in this case
            // from the mainWindow.
            var webContents = await mainWindow.GetWebContents();
            var session = await webContents.GetSession();
            
            // Call the DownloadURL() method from the WebContents
            await webContents.DownloadURL(fileURL);

            await session.Once("will-download",
                new ScriptObjectCallback<Event, DownloadItem, WebContents>(
                    async (callbackResult) => 
                    {
                        await HandleDownload(new WillDownloadResult(callbackResult), fileURL);
                    }
                )
            );

        }

        // Handles the file download from a will-download listener attached to a session.
        async Task HandleDownload(WillDownloadResult cr, string fileURL)
        {
            var filename = System.IO.Path.GetFileName(fileURL);

            // Set our save path.  If it does not exist it will silently be created.
            await cr.DownloadItem.SetSavePath($"downloads/{filename}");

            // Get the total size of the file to be downloaded so we can calculate
            // the percentage progress.
            var size = await cr.DownloadItem.GetTotalBytes();
            
            // Listen for the updated event
            await cr.DownloadItem.On("updated",
                new ScriptObjectCallback<Event, string>(
                    async (updatedResult) => 
                    {
                        var update = updatedResult.CallbackState as object[];
                        var updateState = update[1].ToString();

                        // calculate the percentage
                        var percentage = (await cr.DownloadItem.GetReceivedBytes()) / (float)size;

                        // Set the progress bar.
                        await mainWindow.SetProgressBar(percentage, new ProgressBarOptions() {Mode = ProgressBarMode.Normal});

                        if (updateState == "interrupted")
                            await mainWindow.SetProgressBar(percentage, new ProgressBarOptions() {Mode = ProgressBarMode.Error});
                        else if (updateState == "progressing")
                        {
                            if (await cr.DownloadItem.IsPaused())
                                await mainWindow.SetProgressBar(percentage, new ProgressBarOptions() {Mode = ProgressBarMode.Paused});
                            else
                                await mainWindow.SetProgressBar(percentage, new ProgressBarOptions() {Mode = ProgressBarMode.Normal});                                                                
                        }
                    }
                )
            );
            
            // Listen for when the file is finished downloaded.
            await cr.DownloadItem.Once("done",
                new ScriptObjectCallback<Event, string>(
                    async (doneResult) => 
                    {
                        var done = doneResult.CallbackState as object[];
                        var doneState = done[1].ToString(); 

                        // check if the download completed and set the progress bar option 
                        // accordingly.
                        if (doneState == "completed")
                        {
                            if (IsWindows)
                            {
                                // Flash the frame on windows.
                                await mainWindow.SetProgressBar(1.0f, new ProgressBarOptions() {Mode = ProgressBarMode.None});
                                await mainWindow.FlashFrame(true);
                            }
                            else
                            {
                                // Bounce the doc on Mac.
                                await mainWindow.SetProgressBar(-1.0f);
                                var dock = await app.Dock();
                                await dock.Bounce(DockBounceType.Informational);
                                await dock.DownloadFinished(fileURL);
                            }
                        }
                        else 
                            await mainWindow.SetProgressBar(1.0f, new ProgressBarOptions() {Mode = ProgressBarMode.Error});
                    }
                )
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

