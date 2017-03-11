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

        [ScriptableMember(ScriptAlias = "click")]
        public event EventHandler OnClick;

        public async Task<object> Focus()
        {
            return await InvokeAsync<object>("focus");
        }
    }
}
