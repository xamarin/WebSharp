using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class PowerMonitor : EventEmitter
    {
        protected override string ScriptProxy => @"powerMonitor;";
        protected override string Requires => @"const {powerMonitor} = require('electron');";

        static PowerMonitor proxy;

        public static async Task<PowerMonitor> Instance()
        {
            if (proxy == null)
            {
                proxy = new PowerMonitor();
                await proxy.Initialize();
            }
            return proxy;

        }

        protected PowerMonitor() : base() { }

        protected PowerMonitor(object scriptObject) : base(scriptObject)
        { }

        public PowerMonitor(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator PowerMonitor(ScriptObjectProxy sop)
        {
            return new PowerMonitor(sop);
        }

        public async Task<EventEmitter> OnAC(ScriptObjectCallback listener)
        {
            return await On("on-ac", listener);
        }

        public async Task<EventEmitter> OnceAC(ScriptObjectCallback listener)
        {
            return await Once("on-ac", listener);
        }

        public async Task<EventEmitter> AddACListener(ScriptObjectCallback listener)
        {
            return await AddListener("on-ac", listener);
        }

        public async Task<EventEmitter> RemoveACListener(ScriptObjectCallback listener)
        {
            return await RemoveListener("on-ac", listener);
        }

        public async Task<EventEmitter> OnBattery(ScriptObjectCallback listener)
        {
            return await On("on-battery", listener);
        }

        public async Task<EventEmitter> OnceBattery(ScriptObjectCallback listener)
        {
            return await Once("on-battery", listener);
        }

        public async Task<EventEmitter> AddBatteryListener(ScriptObjectCallback listener)
        {
            return await AddListener("on-battery", listener);
        }

        public async Task<EventEmitter> RemoveBatteryListener(ScriptObjectCallback listener)
        {
            return await RemoveListener("on-battery", listener);
        }

        public async Task<EventEmitter> OnResume(ScriptObjectCallback listener)
        {
            return await On("on-resume", listener);
        }

        public async Task<EventEmitter> OnceResume(ScriptObjectCallback listener)
        {
            return await Once("on-resume", listener);
        }

        public async Task<EventEmitter> AddResumeListener(ScriptObjectCallback listener)
        {
            return await AddListener("on-resume", listener);
        }

        public async Task<EventEmitter> RemoveResumeListener(ScriptObjectCallback listener)
        {
            return await RemoveListener("on-resume", listener);
        }

        public async Task<EventEmitter> OnSuspend(ScriptObjectCallback listener)
        {
            return await On("on-suspend", listener);
        }

        public async Task<EventEmitter> OnceSuspend(ScriptObjectCallback listener)
        {
            return await Once("on-suspend", listener);
        }

        public async Task<EventEmitter> AddSuspendListener(ScriptObjectCallback listener)
        {
            return await AddListener("on-suspend", listener);
        }

        public async Task<EventEmitter> RemoveSuspendListener(ScriptObjectCallback listener)
        {
            return await RemoveListener("on-suspend", listener);
        }
    }
}
