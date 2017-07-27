using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class PowerSaveBlocker : EventEmitter
    {
        protected override string ScriptProxy => @"powerSaveBlocker;";
        protected override string Requires => @"const {powerSaveBlocker} = require('electron');";

        static PowerSaveBlocker proxy;

        public static async Task<PowerSaveBlocker> Instance()
        {
            if (proxy == null)
            {
                proxy = new PowerSaveBlocker();
                await proxy.Initialize();
            }
            return proxy;

        }

        protected PowerSaveBlocker() : base() { }

        protected PowerSaveBlocker(object scriptObject) : base(scriptObject)
        { }

        public PowerSaveBlocker(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator PowerSaveBlocker(ScriptObjectProxy sop)
        {
            return new PowerSaveBlocker(sop);
        }

        public async Task<int> Start(PowerSaveBlockerType type)
        {
            var psbtype = (type == PowerSaveBlockerType.PreventAppSuspension) ? "prevent-app-suspension" : "prevent-display-sleep";
            return await Invoke<int>("start", psbtype);
        }

        public async Task<bool> IsStarted(int id)
        {
            return await Invoke<bool>("isStarted", id);
        }

        public async Task Stop(int id)
        {
            await Invoke<object>("stop", id);
        }
    }

    //'prevent-app-suspension' | 'prevent-display-sleep'
    public enum PowerSaveBlockerType
    {
        PreventAppSuspension,
        PreventDisplaySleep
    }

}
