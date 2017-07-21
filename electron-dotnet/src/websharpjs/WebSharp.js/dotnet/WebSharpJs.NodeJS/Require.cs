using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.NodeJS
{
    public class Require : NodeJsObject
    {
        static NodeObjectProxy scriptProxy;

        public static async Task<Require> Instance()
        {
            var proxy = new Require();
            await proxy.Initialize();
            return proxy;
        }

        private async Task Initialize()
        {
            if (scriptProxy == null)
            {
                scriptProxy = new NodeObjectProxy("require", "");
            }

            await scriptProxy.GetProxyObject();
            ScriptObjectProxy = scriptProxy;

        }

        public async Task<object> Resolve(string id)
        {
            return await Invoke<object>("resolve", id);
        }

        public async Task<System.Collections.Generic.IDictionary<string, object>> GetCache()
        {
            return await GetProperty<System.Collections.Generic.IDictionary<string, object>>("cache");
        }

        public async Task<System.Collections.Generic.IDictionary<string, object>> GetExtensions()
        {
            return await GetProperty<System.Collections.Generic.IDictionary<string, object>>("extensions");
        }
    }
}
