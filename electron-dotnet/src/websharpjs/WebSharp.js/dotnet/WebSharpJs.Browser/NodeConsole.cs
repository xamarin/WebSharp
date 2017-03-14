using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharpJs.Browser
{
    public class NodeConsole : WebSharpObject
    {
        static NodeObjectProxy scriptProxy;

        public static async Task<NodeConsole> Instance()
        {
            var proxy = new NodeConsole();
            await proxy.Initialize();
            return proxy;
        }

        private async Task Initialize()
        {
            if (scriptProxy == null)
            {
                scriptProxy = new NodeObjectProxy("console", "");
            }

            await scriptProxy.GetProxyObject();
            ScriptObjectProxy = scriptProxy;

        }

        public async Task<object> Log(object message)
        {
            return await InvokeAsync<object>("log", message);
        }
            
        public async Task<object> Warn(object message)
        {
            return await InvokeAsync<object>("warn", message);
        }

        public async Task<object> Error(object message)
        {
            return await InvokeAsync<object>("error", message);
        }
    }
}
