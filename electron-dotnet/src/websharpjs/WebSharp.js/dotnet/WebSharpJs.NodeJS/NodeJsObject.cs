using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.NodeJS
{
    public class NodeJsObject : WebSharpObject
    {
        protected NodeJsObject() : base() { }

        protected NodeJsObject(dynamic obj) : base((object)obj)
        {
            var dict = obj as IDictionary<string, object>;

            // The key `websharp_id` represents a wrapped proxy object
            if (dict != null && dict.ContainsKey("websharp_id"))
            {
                ScriptObjectProxy = new NodeObjectProxy(obj);
            }
        }

        protected NodeJsObject(ScriptObjectProxy scriptObject)
        {
            ScriptObjectProxy = new NodeObjectProxy(scriptObject);
        }

        public static explicit operator NodeJsObject(ScriptObjectProxy sop)
        {
            return new NodeJsObject(sop);
        }

        protected virtual string ScriptProxy => string.Empty;
        protected virtual string Requires => string.Empty;

        static NodeObjectProxy scriptProxy;
        protected async Task Initialize()
        {
            await Initialize(ScriptProxy, Requires);
        }

        protected async Task Initialize(string scriptObject, string requires)
        {

            if (scriptProxy == null)
            {
                scriptProxy = new NodeObjectProxy(scriptObject, requires);
            }

            await scriptProxy.GetProxyObject();
            ScriptObjectProxy = scriptProxy;
        }
    }
}
