using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class Remote : EventEmitter
    {
        protected override string ScriptProxy => @"require('electron').remote;";
        protected override string Requires => @"";

        static Remote proxy;

        public static async Task<Remote> Instance()
        {
            if (proxy == null)
            {
                proxy = new Remote();
                await proxy.Initialize();
            }
            return proxy;

        }

        protected Remote() : base() { }

        protected Remote(object scriptObject) : base(scriptObject)
        { }

        public Remote(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Remote(ScriptObjectProxy sop)
        {
            return new Remote(sop);
        }

        public async Task<object> Require(string module)
        {
            return await Invoke<object>("require", module);
        }
    }
}
