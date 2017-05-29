using System.Threading.Tasks;

using WebSharpJs.Script;
using WebSharpJs.NodeJS;

namespace WebSharpJs.Electron
{

    public class IpcRendererEventListener : ScriptObjectCallback<IpcRendererEvent, object[]>
    {
        public IpcRendererEventListener(ScriptObjectCallbackDelegate callbackDelegate) : base(callbackDelegate)
        { }
    }

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
            return await Invoke<object>("send", eventName, args);
        }

        public async Task<object> SendSync(string eventName, params object[] args)
        {
            return await Invoke<object>("sendSync", eventName, args);
        }

        public async Task<object> SendToHost(string eventName, params object[] args)
        {
            return await Invoke<object>("sendToHost", eventName, args);
        }

        public async Task<EventEmitter> On(string eventName, IpcMainEventListener listener)
        {
            return await Invoke<EventEmitter>("on", eventName, listener);
        }

        public async Task<IpcMain> Once(string eventName, IpcMainEventListener listener)
        {
            return await Invoke<IpcMain>("once", eventName, listener);
        }

        public async Task<IpcRenderer> AddListener(string eventName, IpcRendererEventListener listener)
        {
            return await Invoke<IpcRenderer>("addListener", eventName, listener);
        }

        public async Task<IpcRenderer> RemoveListener(string eventName, IpcRendererEventListener listener)
        {
            return await Invoke<IpcRenderer>("removeListener", eventName, listener);
        }

        public async new Task<IpcRenderer> RemoveAllListeners(string eventName)
        {
            return await Invoke<IpcRenderer>("removeAllListeners", eventName);
        }

    }
}
