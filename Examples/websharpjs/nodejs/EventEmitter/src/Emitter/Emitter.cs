using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;

//namespace Emitter
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
                Func<object, Task<object>> listener1 = (async (commands) =>
                {
                    console.Log($"Listener 1 executed");
                    return null;
                });


                var eventEmitter = await EventEmitter.Create();
                eventEmitter.AddListener("connection", listener1);
                eventEmitter.AddListener("connection", (async (commands) =>
                {

                    console.Log($"Listener 2 executed.");
                    return null;
                }));

                console.Log($"# of 'connection' listeners {await eventEmitter.ListenerCount("connection")}");

                console.Log($"emitted {await eventEmitter.Emit("connection")}");

                console.Log("Removing all listeners");

                eventEmitter.RemoveAllListeners("connection");

                console.Log($"# of 'connection' listeners {await eventEmitter.ListenerCount("connection")}");

                eventEmitter.Once("foo", (async (x) =>
                {

                    console.Log("a");
                    return null;
                }));

                eventEmitter.PrependOnceListener("foo", (async (x) =>
                {
                    
                    console.Log("b");
                    return null;
                }));

                eventEmitter.Emit("foo"); // Should be logged first b then a
                eventEmitter.Emit("foo"); // Listeners will not be called again.

            }
            catch (Exception exc) { console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
//}
