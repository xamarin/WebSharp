using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{
    public sealed class HtmlPage 
    {
        HtmlDocument htmlDocument;
        HtmlWindow htmlWindow;

        // Used to retrieve browser information
        static Func<object, Task<object>> browserInfoFunction = null;

        public async Task<HtmlDocument> GetDocument()
        {
            if (htmlDocument == null)
                return await HtmlDocument.Instance();
            else
                return htmlDocument;
        }

        public async Task<HtmlWindow> GetWindow()
        {
            if (htmlWindow == null)
                return await HtmlWindow.Instance();
            else
                return htmlWindow;
        }

        public async Task<BrowserInformation> GetBrowserInformation()
        {
            if (browserInfoFunction == null)
                browserInfoFunction = await WebSharp.CreateJavaScriptFunction(
                    @"return function (data, callback) {
                                    try { 
                                        let browserInfo = {}; 
                                        browserInfo['appName'] = navigator.appName;
                                        browserInfo['appVersion'] = navigator.appVersion;
                                        browserInfo['userAgent'] = navigator.userAgent;
                                        browserInfo['platform'] = navigator.platform;
                                        browserInfo['cookiesEnabled'] = navigator.cookiesEnabled;
                                        callback(null, browserInfo);
                                    } catch (e) { callback(e, null); }
                                    
                                }");
            var eval = await browserInfoFunction(null);
            if (eval != null)
            {
                var bi = new BrowserInformation();
                ScriptObjectHelper.DictionaryToScriptableType((IDictionary<string, object>)eval, bi);
                return bi;
            }
            return default(BrowserInformation);
        }
    }
}
