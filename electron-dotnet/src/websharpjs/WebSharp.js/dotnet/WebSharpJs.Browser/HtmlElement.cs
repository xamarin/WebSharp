using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharpJs.Browser
{
    public sealed class HtmlElement : HtmlObject
    {

        private HtmlElement() : base() { }
        private HtmlElement(object obj) : base(obj) { }

        public async Task<string> GetId()
        {
            return await GetPropertyAsync<string>("id");
        }

        public async Task<string> GetTagName()
        {
            return await GetPropertyAsync<string>("tagName");
        }

        public async Task<HtmlElement> GetParent()
        {
            return await GetPropertyAsync<HtmlElement>("parentElement");
        }

        [ScriptableMember(ScriptAlias = "click")]
        public event EventHandler OnClick;

        public async Task<object> Focus()
        {
            return await InvokeAsync<object>("focus");
        }



        public async Task<object> GetStyleAttribute()
        {
            return await GetPropertyAsync<object>("style");
        }

        public async Task<object> GetNodeType()
        {
            return await GetPropertyAsync<object>("style");
        }

        public async Task<string> GetAttribute (string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var jsfp = ScriptObjectProxy.JavascriptFunctionProxy;
            if (jsfp != null)
            {
                return await jsfp.websharp_get_attribute(name);
            }
            else
                return string.Empty;
        }

        public async Task<bool> SetAttribute(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var jsfp = ScriptObjectProxy.JavascriptFunctionProxy;
            if (jsfp != null)
            {
                var parm = new
                {
                    name = name,
                    value = value
                };
                return await jsfp.websharp_set_attribute(parm) ;
            }
            return false;
        }

        public async Task<string> GetStyleAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var jsfp = ScriptObjectProxy.JavascriptFunctionProxy;
            if (jsfp != null)
            {
                return await jsfp.websharp_get_style_attribute(name);
            }
            else
                return string.Empty;
        }

        public async Task<bool> SetStyleAttribute(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var jsfp = ScriptObjectProxy.JavascriptFunctionProxy;
            if (jsfp != null)
            {
                var parm = new
                {
                    name = name,
                    value = value
                };
                return await jsfp.websharp_set_style_attribute(parm);
            }
            return false;
        }
    }
}
