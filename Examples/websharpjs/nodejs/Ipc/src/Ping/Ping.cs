using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;

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

                await console.Log(await ipcRenderer.SendSync("synchronous-message", "synchronous ping")); // prints "pong"

                ipcRenderer.On("asynchronous-reply", (async (args) =>
                {
                    var argsArray = args as object[];
                    await console.Log(argsArray[1]); // prints "pong"
                    return null;
                }));
                await ipcRenderer.Send("asynchronous-message", "asynchronous ping");

                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
//}
