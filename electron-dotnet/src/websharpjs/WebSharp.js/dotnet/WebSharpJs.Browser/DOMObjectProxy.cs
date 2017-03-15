using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebSharpJs.Browser
{
    
    public class DOMObjectProxy : ScriptObjectProxy
    {
        //Func<object, Task<object>> scriptProxy;

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

        public DOMObjectProxy(dynamic proxy) 
        {
            JavascriptFunctionProxy = proxy;
        }

        public DOMObjectProxy(string scriptObject)
        {
            ScriptObject = scriptObject;
        }

    }
}
