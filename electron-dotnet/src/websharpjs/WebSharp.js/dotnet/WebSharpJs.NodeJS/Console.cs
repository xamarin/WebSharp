using System.Threading.Tasks;

namespace WebSharpJs.NodeJS
{
    public class Console : NodeJsObject
    {
        static NodeObjectProxy scriptProxy;

        public static async Task<Console> Instance()
        {
            var proxy = new Console();
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
            return await Invoke<object>("log", message);
        }
            
        public async Task<object> Warn(object message)
        {
            return await Invoke<object>("warn", message);
        }

        public async Task<object> Error(object message)
        {
            return await Invoke<object>("error", message);
        }
    }
}
