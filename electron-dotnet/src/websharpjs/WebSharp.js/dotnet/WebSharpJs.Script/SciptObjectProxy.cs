using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharpJs.Script
{
    public class ScriptObjectProxy : IScriptObjectProxy
    {

        Func<object, Task<object>> javascriptFunction;
        private dynamic javascriptFunctionProxy;

        static readonly string createScript = @"
                                return function (data, callback) {
                                    const dotnet = require('./electron-dotnet');
                                    const websharpjs = dotnet.WebSharpJs;

                                    let options = websharpjs.UnwrapArgs(data);

                                    let wsObj = $$$$javascriptObject$$$$;

                                    let proxy = websharpjs.ObjectToScriptObject(wsObj);

                                    callback(null, proxy);
                                }
                            ";

        public ScriptObjectProxy(dynamic proxy)
        {
            javascriptFunctionProxy = proxy;
        }

        protected ScriptObjectProxy()
        { }

        protected virtual string ScriptFunction => createScript.Replace("$$$$javascriptObject$$$$", ScriptObject);
        public virtual string ScriptObject { get; protected set; }

        public int Handle => javascriptFunctionProxy.websharp_id;
		public dynamic JavascriptFunctionProxy { get { return javascriptFunctionProxy; } set { javascriptFunctionProxy = value; } }

        public Func<object, Task<object>> JavascriptFuncion { get => javascriptFunction; set => javascriptFunction = value; }

        public virtual async Task GetProxyObject(params object[] args)
        {
            object[] parms = args;

            if (args != null && args.Length > 0)
            {
                parms = ScriptObjectUtilities.WrapScriptParms(args);
            }
            
            if (JavascriptFuncion == null)
            {
                JavascriptFuncion = await WebSharp.CreateJavaScriptFunction(ScriptFunction);
            }

            javascriptFunctionProxy = await JavascriptFuncion(parms);
        }

        public async Task<T> GetProperty<T>(dynamic name)
        {

            if (javascriptFunctionProxy != null)
            {
                return await javascriptFunctionProxy.websharp_get_property(name) ?? default(T);
            }
            else
                return default(T);
        }

        public async Task<bool> SetProperty(string name, object value, bool createIfNotExists = true, bool hasOwnProperty = false)
        {
            try
            {
                if (javascriptFunctionProxy != null)
                {
                    var parms = new
                    {
                        property = name,
                        value = value,
                        createIfNotExists = createIfNotExists,
                        hasOwnProperty = hasOwnProperty
                    };
                    return await javascriptFunctionProxy.websharp_set_property(parms);
                }
            }
            catch { }

            return false;
        }

        public async Task<object> TryInvoke(dynamic parms)
        {
            if (javascriptFunctionProxy != null)
            {
                return await javascriptFunctionProxy.websharp_invoke(parms);
            }
            return null;

        }

        public async Task<bool> AddEventListener(object eventCallback)
        {
            if (javascriptFunctionProxy != null)
            {
                return await javascriptFunctionProxy.websharp_addEventListener(eventCallback);
            }

            return false;
        }
    }
}
