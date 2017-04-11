using System.Threading.Tasks;

using WebSharpJs.Script;
using WebSharpJs.NodeJS;

namespace WebSharpJs.Electron
{
    public class IpcRenderer : EventEmitter
    {

        protected override string ScriptProxy => @"ipcRenderer; ";
        protected override string Requires => @"const {ipcRenderer} = require('electron');";

        public new static async Task<IpcRenderer> Create()
        {
            var proxy = new IpcRenderer();
            await proxy.Initialize();
            return proxy;
        }

        protected IpcRenderer() : base() { }

        protected IpcRenderer(object scriptObject) : base(scriptObject)
        { }

        public IpcRenderer(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator IpcRenderer(ScriptObjectProxy sop)
        {
            return new IpcRenderer(sop);
        }

        public async Task<object> Send(string eventName, params object[] args)
        {
            return await InvokeAsync<object>("send", eventName, args);
        }

        public async Task<object> SendSync(string eventName, params object[] args)
        {
            return await InvokeAsync<object>("sendSync", eventName, args);
        }

    }
}
