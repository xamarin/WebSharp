using System;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.NodeJS
{
    public class EventEmitter : NodeJsObject
    {

        protected override string ScriptProxy => @"new events.EventEmitter();";
        protected override string Requires => @"var events = require('events');";

        public static async Task<EventEmitter> Create()
        {
            var proxy = new EventEmitter();
            await proxy.Initialize();
            return proxy;
        }

        protected EventEmitter() : base() { }

        protected EventEmitter(object scriptObject) : base(scriptObject)
        { }

        public EventEmitter(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator EventEmitter(ScriptObjectProxy sop)
        {
            return new EventEmitter(sop);
        }

        public async Task<EventEmitter> AddListener(string eventName, IScriptObjectCallback listener)
        {
            return await Invoke<EventEmitter>("addListener", eventName, listener);
        }

        public async Task<bool> Emit(string eventName, params object[] args)
        {
            return await Invoke<bool>("emit", eventName, args);
        }

        public async Task<string[]> EventNames()
        {
            return await Invoke<string[]>("eventNames");
        }

        public async Task<int> GetMaxListeners()
        {
            return await Invoke<int>("getMaxListeners");
        }

        public async Task<int> ListenerCount(string eventName)
        {
            return await Invoke<int>("listenerCount", eventName);
        }

        public async Task<object[]> Listeners(string eventName)
        {
            return await Invoke<object[]>("listeners", eventName);
        }

        public async Task<EventEmitter> On(string eventName, IScriptObjectCallback listener)
        {
            return await Invoke<EventEmitter>("on", eventName, listener);
        }

        public async Task<EventEmitter> Once(string eventName, IScriptObjectCallback listener)
        {
            return await Invoke<EventEmitter>("once", eventName, listener);
        }

        public async Task<EventEmitter> PrependListener(string eventName, IScriptObjectCallback listener)
        {
            return await Invoke<EventEmitter>("prependListener", eventName, listener);
        }

        public async Task<EventEmitter> PrependOnceListener(string eventName, IScriptObjectCallback listener)
        {
            return await Invoke<EventEmitter>("prependOnceListener", eventName, listener);
        }

        public async Task<EventEmitter> RemoveAllListeners(string eventName)
        {
            return await Invoke<EventEmitter>("removeAllListeners", eventName);
        }

        public async Task<EventEmitter> RemoveListener(string eventName, IScriptObjectCallback listener)
        {
            return await Invoke<EventEmitter>("removeListener", eventName, listener);
        }

        public async Task<EventEmitter> SetMaxListeners(int n)
        {
            return await Invoke<EventEmitter>("setMaxListeners", n);
        }

    }
}
