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

        public async Task<EventEmitter> AddListener(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<EventEmitter>("addListener", eventName, listener);
        }

        public async Task<bool> Emit(string eventName, params object[] args)
        {
            return await InvokeAsync<bool>("emit", eventName, args);
        }

        public async Task<string[]> EventNames()
        {
            return await InvokeAsync<string[]>("eventNames");
        }

        public async Task<int> GetMaxListeners()
        {
            return await InvokeAsync<int>("getMaxListeners");
        }

        public async Task<int> ListenerCount(string eventName)
        {
            return await InvokeAsync<int>("listenerCount", eventName);
        }

        public async Task<object[]> Listeners(string eventName)
        {
            return await InvokeAsync<object[]>("listeners", eventName);
        }

        public async Task<EventEmitter> On(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<EventEmitter>("on", eventName, listener);
        }

        public async Task<EventEmitter> Once(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<EventEmitter>("once", eventName, listener);
        }

        public async Task<EventEmitter> PrependListener(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<EventEmitter>("prependListener", eventName, listener);
        }

        public async Task<EventEmitter> PrependOnceListener(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<EventEmitter>("prependOnceListener", eventName, listener);
        }

        public async Task<EventEmitter> RemoveAllListeners(string eventName)
        {
            return await InvokeAsync<EventEmitter>("removeAllListeners", eventName);
        }

        public async Task<EventEmitter> RemoveListener(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<EventEmitter>("removeListener", eventName, listener);
        }

        public async Task<EventEmitter> SetMaxListeners(int n)
        {
            return await InvokeAsync<EventEmitter>("setMaxListeners", n);
        }

    }
}
