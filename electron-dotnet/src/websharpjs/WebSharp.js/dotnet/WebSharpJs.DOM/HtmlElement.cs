using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{
    public sealed class HtmlElement : HtmlObject
    {

        private HtmlElement() : base() { }
        private HtmlElement(object obj) : base(obj) { }

        public async Task<string> GetId()
        {
            return await GetProperty<string>("id");
        }

        public async Task<string> GetTagName()
        {
            return await GetProperty<string>("tagName");
        }

        public async Task<HtmlElement> GetParent()
        {
            return await GetProperty<HtmlElement>("parentElement");
        }

        public async Task<ScriptObjectCollection<HtmlElement>> GetChildren()
        {
            return await GetProperty<ScriptObjectCollection<HtmlElement>>("children");
        }

        [ScriptableMember(ScriptAlias = "click")]
        public event EventHandler OnClick;

        public async Task<object> Focus()
        {
            return await Invoke<object>("focus");
        }

        public async Task<string> GetCssClass()
        {
            return await GetProperty<string>("className");
        }

        public async Task<object> SetCssClass(string className)
        {
            return await SetProperty("className", className ?? string.Empty);
        }

        public async Task<object> GetStyleAttribute()
        {
            return await GetProperty<object>("style");
        }

        public async Task<object> GetNodeType()
        {
            return await GetProperty<object>("nodeType");
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

        public async Task AppendChild(HtmlElement element)
        {
            await Invoke<object>("appendChild", element);
        }

        public async Task RemoveChild(HtmlElement element)
        {
            await Invoke<object>("removeChild", element);
        }

    }
}
