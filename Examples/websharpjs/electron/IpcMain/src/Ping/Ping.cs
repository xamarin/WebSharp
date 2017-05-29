using System;
using System.Threading.Tasks;

using WebSharpJs.Electron;
using WebSharpJs.Script;

//namespace Ping
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

        /// <summary>
        /// Default entry into managed code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<object> Invoke(object input)
        {
            if (console == null)
                console = await WebSharpJs.NodeJS.Console.Instance();

        try
        {
            IpcRenderer ipcRenderer = await IpcRenderer.Create();

            // Note: Sending a synchronous message from the render process will block 
            // the whole renderer process, unless you know what you are doing you should never use it.
            //
            // View notes in MainWindow.cs
            await console.Log(await ipcRenderer.SendSync("synchronous-message", "synchronous ping")); // prints "pong"

            ipcRenderer.On("asynchronous-reply", 
                new IpcRendererEventListener(async (result) =>
            {
                var state = result.CallbackState as object[];
                var parms = state[1] as object[];
                foreach(var parm in parms)
                    await console.Log(parm); // prints "pong"
            }));

            await ipcRenderer.Send("asynchronous-message", "asynchronous ping");

            await console.Log($"Hello:  {input}");
        }
        catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

        return null;


        }
    }
//}
