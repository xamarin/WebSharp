using System.Threading.Tasks;

namespace WebSharpJs.DOM
{
    // Note: With Electron a BrowserWindowProxy is returned for the window object
    // so even though it is mapped here to an HtmlWindow all the code is proxied to
    // to the BrowserWindow object.
    public sealed class HtmlWindow : HtmlObject
    {
        static DOMObjectProxy scriptProxy;

        internal static async Task<HtmlWindow> Instance()
        {
            var proxy = new HtmlWindow();
            await proxy.Initialize();
            return proxy;
        }

        internal async Task Initialize()
        {
            if (scriptProxy == null)
            {
                scriptProxy = new DOMObjectProxy("window");
            }

            await scriptProxy.GetProxyObject();
            ScriptObjectProxy = scriptProxy;

        }

        public async Task Alert(string alertText)
        {
            await Invoke<object>("alert", alertText);
        }

        public async Task<bool> Confirm(string confirmText)
        {
            return await Invoke<bool>("confirm", confirmText);
        }

        public async Task<string> Prompt(string promptText)
        {
            // With Electron a BrowserWindowProxy is returned for the window object
            // which intercepts the prompt() function and throws an error.
            return await Invoke<string>("prompt", promptText);
        }

        public async Task<object> Eval(string code)
        {
            // With Electron a BrowserWindowProxy is returned for the window object
            // so we can use eval here.
            return await Invoke<object>("eval", code);
        }


    }
}
