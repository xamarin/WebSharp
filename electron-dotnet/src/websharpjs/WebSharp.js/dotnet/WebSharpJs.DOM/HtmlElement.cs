using System;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{
    public sealed class HtmlElement : HtmlObject
    {

        private HtmlElement() : base() { }
        private HtmlElement(object obj) : base(obj) { }

        public HtmlElement(ScriptObjectProxy scriptObject) : base(scriptObject) { }

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

            var parm = new
            {   
                handle = Handle,
                attribute = name,
            };

            return await WebSharp.Bridge.JavaScriptBridge.websharp_get_attribute(parm);
        }

        public async Task<bool> RemoveAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var parm = new
            {   
                handle = Handle,
                attribute = name
            };

            return await WebSharp.Bridge.JavaScriptBridge.websharp_set_attribute(parm);
        }

        public async Task<bool> SetAttribute(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var parm = new
            {   
                handle = Handle,
                attribute = name,
                value = value
            };

            return await WebSharp.Bridge.JavaScriptBridge.websharp_set_attribute(parm);
        }

        public async Task<string> GetStyleAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var parm = new
            {   
                handle = Handle,
                attribute = name,
            };

            return await WebSharp.Bridge.JavaScriptBridge.websharp_get_style_attribute(parm);
        }

        public async Task<bool> SetStyleAttribute(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var parm = new
            {
                handle = Handle,
                attribute = name,
                value = value
            };

            return await WebSharp.Bridge.JavaScriptBridge.websharp_set_style_attribute(parm);
        }

        public async Task<bool> RemoveStyleAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var parm = new
            {
                handle = Handle,
                attribute = name
            };

            return await WebSharp.Bridge.JavaScriptBridge.websharp_set_style_attribute(parm);
        }

        public async Task AppendChild(HtmlElement element)
        {
            await Invoke<object>("appendChild", element);
        }

        public async Task RemoveChild(HtmlElement element)
        {
            await Invoke<object>("removeChild", element);
        }

        public async Task<HtmlElement> QuerySelector(string tagName)
        {
            return await Invoke<HtmlElement>("querySelector", tagName); ;
        }

        public async Task<ScriptObjectCollection<HtmlElement>> QuerySelectorAll(string tagName)
        {
            return await Invoke<ScriptObjectCollection<HtmlElement>>("querySelectorAll", tagName); ;
        }

    }
}
