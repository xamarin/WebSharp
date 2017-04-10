using System;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.NodeJS
{
    public class EventEmitter : NodeJsObject
    {
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

        static NodeObjectProxy scriptProxy;
        protected async Task Initialize()
        {

            if (scriptProxy == null)
            {
                scriptProxy = new NodeObjectProxy(@"new events.EventEmitter();",
                                                    @"var events = require('events');");
            }

            await scriptProxy.GetProxyObject();
            ScriptObjectProxy = scriptProxy;
        }
        
        public async Task<object> AddListener(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<object>("addListener", eventName, listener);
        }

        public async Task<bool> Emit(string eventName, params object[] args)
        {
            return await InvokeAsync<bool>("emit", eventName, args);
        }

        public async Task<object[]> EventNames()
        {
            return await InvokeAsync<object[]>("eventNames");
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

        public async Task<object> Once(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<object>("once", eventName, listener);
        }

        public async Task<object> PrependListener(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<object>("prependListener", eventName, listener);
        }

        public async Task<object> PrependOnceListener(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<object>("prependOnceListener", eventName, listener);
        }

        public async Task<object> RemoveAllListeners(string eventName)
        {
            return await InvokeAsync<object>("removeAllListeners", eventName);
        }

        public async Task<object> RemoveListener(string eventName, Func<object, Task<object>> listener)
        {
            return await InvokeAsync<object>("removeListener", eventName, listener);
        }

        public async Task<EventEmitter> SetMaxListeners(int n)
        {
            return await InvokeAsync<EventEmitter>("setMaxListeners", n);
        }

    }
}
