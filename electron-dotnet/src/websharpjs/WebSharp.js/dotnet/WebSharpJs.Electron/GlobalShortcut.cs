using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class GlobalShortcut : EventEmitter
    {
        protected override string ScriptProxy => @"globalShortcut;";
        protected override string Requires => @"const {globalShortcut} = require('electron');";

        static GlobalShortcut proxy;
        
        public static new async Task<GlobalShortcut> Create()
        {
            throw new NotSupportedException("Create() is not valid.  Use Instance() to obtain a reference to GlobalShortcut.");
        }

        public static async Task<GlobalShortcut> Instance()
        {
            if (proxy == null)
            {
                proxy = new GlobalShortcut();
                await proxy.Initialize();
            }
            return proxy;

        }

        protected GlobalShortcut() : base() { }

        protected GlobalShortcut(object scriptObject) : base(scriptObject)
        { }

        public GlobalShortcut(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator GlobalShortcut(ScriptObjectProxy sop)
        {
            return new GlobalShortcut(sop);
        }

        public async Task<bool> Register(Accelerator accelerator, ScriptObjectCallback callback)
        {
            return await Invoke<bool>("register", accelerator.ToString(), callback);
        }

        public async Task<bool> IsRegistered(Accelerator accelerator)
        {
            return await Invoke<bool>("isRegistered", accelerator.ToString());
        }

        public async Task UnRegister(Accelerator accelerator)
        {
            await Invoke<object>("unregister", accelerator.ToString());
        }

        public async Task UnRegisterAll()
        {
            await Invoke<object>("unregisterAll");
        }
    }
}
