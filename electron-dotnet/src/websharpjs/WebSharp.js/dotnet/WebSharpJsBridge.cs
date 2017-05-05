using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharpJs
{
    internal class WebSharpJsBridge
    {

        public dynamic JavaScriptBridge { get; set; }

        internal WebSharpJsBridge()
        {
            //Console.WriteLine("new Bridge created");
        }

        public async Task<object> TryInvoke(dynamic parms)
        {
            return await JavaScriptBridge.websharp_invoke(parms);
        }

        public async Task<T> GetProperty<T>(dynamic name)
        {
            return await JavaScriptBridge.websharp_get_property(name);
        }

        public async Task<bool> SetProperty(dynamic parms)
        {
            return await JavaScriptBridge.websharp_set_property(parms);
        }

        public async Task<bool> AddEventListener(object eventCallback)
        {
            return await JavaScriptBridge.websharp_addEventListener(eventCallback);
        }
    }
}
