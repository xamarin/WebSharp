using WebSharpJs.Script;

namespace WebSharpJs.NodeJS
{

    public class NodeObjectProxy : ScriptObjectProxy
    {

        static readonly string createScript = @"
                                        return function (data, callback) {
                                            const dotnet = require('./electron-dotnet');
                                            const websharpjs = dotnet.WebSharpJs;

                                            $$$$nodeRequires$$$$

                                            let options = websharpjs.UnwrapArgs(data);

                                            let wsObj = $$$$javascriptObject$$$$;

                                            let proxy = websharpjs.ObjectToScriptObject(wsObj);

                                            callback(null, proxy);
 
                                        }
                                    ";

        public NodeObjectProxy(ScriptObjectProxy sop)
        {
            JavascriptFunctionProxy = sop.JavascriptFunctionProxy;
        }

        public NodeObjectProxy(dynamic proxy)
        {
            JavascriptFunctionProxy = proxy;
        }

        public string Requires { get; private set; }

        public NodeObjectProxy(string scriptObject, string requires)
        {
            ScriptObject = scriptObject;
            Requires = requires;
        }

        protected override string ScriptFunction => createScript.Replace("$$$$javascriptObject$$$$", ScriptObject).Replace("$$$$nodeRequires$$$$", Requires);

    }
}
