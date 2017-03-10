using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharpJs.Browser
{
    public class HtmlElement : HtmlObject
    {
        static Func<object, Task<object>> scriptProxy;

        public static async Task<HtmlElement> Instance()
        {
            var proxy = new HtmlElement();
            await proxy.Initialize();
            return proxy;
        }

        private async Task Initialize()
        {

            if (scriptProxy == null)
                scriptProxy = await CreateScriptObject("document;");
            else
                await CreateScriptObject(scriptProxy);
        }
    }
}
