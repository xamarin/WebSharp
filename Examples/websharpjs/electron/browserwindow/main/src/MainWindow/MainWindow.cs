using System;
using System.Threading.Tasks;

using WebSharpJs.Electron;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

//namespace MainWindow
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

        static BrowserWindow mainWindow;

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

                // Open the DevTools
                await mainWindow.GetWebContents().ContinueWith(
                        (t) => { t.Result?.OpenDevTools(DevToolsMode.Bottom); }
                );

                // Emitted when the window is closed.
                await mainWindow.On("closed", new ScriptObjectCallback<Event>(async (evt) => 
                {
                    await console.Log("Received closed event");
                    System.Console.WriteLine("Received closed event");
                    mainWindow = null;
                }));

                await console.Log($"Loading: file://{__dirname}/index.html");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return await mainWindow.GetId();


        }
    }
//}
