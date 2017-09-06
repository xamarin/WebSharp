using System;
using System.Threading.Tasks;

using System.Diagnostics;

using WebSharpJs.Electron;
using WebSharpJs.Script;

#if !DEV
namespace MainWindow
{
#endif    
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

        static App app;
        static BrowserWindow mainWindow;
        static int windowId = 0;

        static readonly string FFMPegWindows = "ffmpeg.exe";
        static readonly string FFMPegMac = "ffmpeg";

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

                var ipcMain = await IpcMain.Create();

                await ipcMain.AddListener("submitform",
                    new IpcMainEventListener(
                        async (result) =>
                        {
                            var state = result.CallbackState as object[];
                            var ipcMainEvent = (IpcMainEvent)state[0];
                            var parms = state[1] as object[];

                            var engineCompleteHandler = new EventHandler<MediaToolkit.ConversionCompleteEventArgs>(
                                async (s, e) =>
                                {
                                    await ipcMainEvent.Sender.Send("completed");
                                    await mainWindow.SetProgressBar(-1, 
                                        new ProgressBarOptions() { Mode = ProgressBarMode.None});
                                }
                            );

                            var engineProgressHandler = new EventHandler<MediaToolkit.ConvertProgressEventArgs>(
                                async (s, e) =>
                                {
                                    var progress = (float)((float)e.ProcessedDuration.Ticks / e.TotalDuration.Ticks);
                                    await mainWindow.SetProgressBar(progress, new ProgressBarOptions() { Mode = ProgressBarMode.Normal });
                                }
                            );

                            var outputFile = System.IO.Path.Combine(parms[1].ToString(), parms[2].ToString());

                            try
                            {

                                await Task.Run(() =>
                                    Convert.FFMPEGWrapper.Convert(parms[0].ToString(),
                                    outputFile,
                                    engineProgressHandler,
                                    engineCompleteHandler)
                                );
                            }
                            catch (Exception convertExc)
                            {
                                var errDialog = await Dialog.Instance();
                                await errDialog.ShowErrorBox("FFMPeg engine failure", convertExc.Message);
                                await ipcMainEvent.Sender.Send("completedWithErrors");
                            }

                        }
                    )

                );

                await ipcMain.AddListener("getMetaData",
                    new IpcMainEventListener(
                        async (result) =>
                        {
                            var state = result.CallbackState as object[];
                            var ipcMainEvent = (IpcMainEvent)state[0];
                            var parms = state[1] as object[];

                            foreach (var parm in parms)
                                System.Console.WriteLine($"\tparm: {parm}");
                            try 
                            {
                                var meta = await Convert.FFMPEGWrapper.GetMetaData(parms[0].ToString());

                                await ipcMainEvent.Sender.Send("gotMetaData", meta.AudioData, meta.VideoData, meta.Duration.ToString());
                            }
                            catch (Exception convertExc)
                            {
                                var errDialog = await Dialog.Instance();
                                await errDialog.ShowErrorBox("FFMPeg engine failure", convertExc.Message);
                            }                            

                        }
                    )

                );

                await console.Log($"Loading: file://{__dirname}/index.html");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return windowId;


        }

        async Task<int> CreateWindow(string __dirname)
        {
            // Create the browser window.
            mainWindow = await BrowserWindow.Create(
                new BrowserWindowOptions()
                {
                    Width = 800,
                    Height = 600,
                    IconPath = $"{__dirname}/assets/icons/appicon.ico"
                }

            );

            // and load the index.html of the app.
            await mainWindow.LoadURL($"file://{__dirname}/index.html");

            // Open the DevTools
            //await mainWindow.GetWebContents().ContinueWith(
            //        (t) => { t.Result?.OpenDevTools(DevToolsMode.Bottom); }
            //);

            // Did finish load
            await mainWindow.GetWebContents().ContinueWith(
                    (t) =>
                    {
                        var webcontents = t.Result;
                        webcontents.On("did-finish-load",
                            new ScriptObjectCallback(
                                async (ar) =>
                                {
                                    try
                                    {
                                        var ready = await InitializeEngine();
                                        if (!ready)
                                        {
                                            var errDialog = await Dialog.Instance();
                                            await errDialog.ShowErrorBox("FFMPeg engine failure", "FFMPeg could not be initialized");
                                        }
                                    }
                                    catch (Exception initExc)
                                    {
                                        var errDialog = await Dialog.Instance();
                                        await errDialog.ShowErrorBox("FFMPeg engine failure", initExc.Message);
                                    }

                                }
                            )
                        );
                    }
            );

            return await mainWindow.GetId();

        }

        async Task<bool> InitializeEngine()
        {
            // Work around a bug with App Name and thus UserData not 
            // being set in development mode
#if DEV
                await app.SetName("Convert");
                var ffMpegPath = await app.GetPath(AppPathName.UserData);
                ffMpegPath = (ffMpegPath.EndsWith("Electron")) ? ffMpegPath.Replace(@"Electron", "Convert") : ffMpegPath;
#else
            var ffMpegPath = await app.GetPath(AppPathName.UserData);
#endif

            // Now append the ffmpeg to complete the path
            ffMpegPath = System.IO.Path.Combine(ffMpegPath, IsWindows ? FFMPegWindows : FFMPegMac);

            //await console.Log($"app name: {await app.GetName()}  temppath: {ffMpegPath}");

            return await Convert.FFMPEGWrapper.InitializeEngine(ffMpegPath);
        
        }

        static bool IsWindows
        {
            get {
                // Mac can also return Unix running under mono.
                return !(System.Environment.OSVersion.Platform == PlatformID.MacOSX || System.Environment.OSVersion.Platform == PlatformID.Unix);
            }
        }        
    }
    
#if !DEV
}
#endif
