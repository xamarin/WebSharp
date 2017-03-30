using WebSharpJs.Script;

namespace WebSharpJs.DOM
{

    public class DOMObjectProxy : ScriptObjectProxy
    {
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
