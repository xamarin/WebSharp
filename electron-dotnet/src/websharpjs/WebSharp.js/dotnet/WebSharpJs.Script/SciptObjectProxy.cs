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

        public Func<object, Task<object>> JavascriptFunction { get => javascriptFunction; set => javascriptFunction = value; }

        public virtual async Task GetProxyObject(params object[] args)
        {
            object[] parms = args;

            if (args != null && args.Length > 0)
            {
                parms = ScriptObjectUtilities.WrapScriptParms(args);
            }
            
            if (JavascriptFunction == null)
            {
                JavascriptFunction = await WebSharp.CreateJavaScriptFunction(ScriptFunction);
            }

            javascriptFunctionProxy = await JavascriptFunction(parms);
        }

    }
}
