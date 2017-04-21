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

        protected async Task<NodeObjectProxy> Initialize(params object[] parameters)
        {
            return await Initialize(ScriptProxy, Requires, parameters);
        }

        protected async Task<NodeObjectProxy> Initialize(string scriptObject, string requires, params object[] parameters)
        {
            var scriptProxy = ScriptObjectProxy as NodeObjectProxy;
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(scriptObject, requires);
            else
                scriptProxy = new NodeObjectProxy(scriptProxy);

            await scriptProxy.GetProxyObject(parameters);
            ScriptObjectProxy = scriptProxy;
            return scriptProxy;
        }
    }
}
