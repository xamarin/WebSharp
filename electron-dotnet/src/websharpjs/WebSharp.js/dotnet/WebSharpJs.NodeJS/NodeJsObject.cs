using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.NodeJs
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

    }
}
