using System.Threading.Tasks;

using WebSharpJs.Script;
using WebSharpJs.NodeJS;

namespace WebSharpJs.Electron
{

    public class IpcMainEventListener : ScriptObjectCallback<IpcMainEvent, object[]> 
    {
        public IpcMainEventListener(ScriptObjectCallbackDelegate callbackDelegate) : base(callbackDelegate)
        { }
    }

    public class IpcMain : EventEmitter
    {

        protected override string ScriptProxy => @"ipcMain; ";
        protected override string Requires => @"const {ipcMain} = require('electron');";

        public new static async Task<IpcMain> Create()
        {
            var proxy = new IpcMain();
            await proxy.Initialize();
            return proxy;
        }

        protected IpcMain() : base() { }

        protected IpcMain(object scriptObject) : base(scriptObject)
        { }

        public IpcMain(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator IpcMain(ScriptObjectProxy sop)
        {
            return new IpcMain(sop);
        }

        public async Task<EventEmitter> On(string eventName, IpcMainEventListener listener)
        {
            return await Invoke<EventEmitter>("on", eventName, listener);
        }

        public async Task<IpcMain> Once(string eventName, IpcMainEventListener listener)
        {
            return await Invoke<IpcMain>("once", eventName, listener);
        }

        public async Task<IpcMain> AddListener(string eventName, IpcMainEventListener listener)
        {
            return await Invoke<IpcMain>("addListener", eventName, listener);
        }

        public async Task<IpcMain> RemoveListener(string eventName, IpcMainEventListener listener)
        {
            return await Invoke<IpcMain>("removeListener", eventName, listener);
        }

        public async new Task<IpcMain> RemoveAllListeners(string eventName)
        {
            return await Invoke<IpcMain>("removeAllListeners", eventName);
        }

    }

}