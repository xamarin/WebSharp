using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{
    public static class HtmlPage 
    {
        static HtmlDocument htmlDocument;
        static HtmlWindow htmlWindow;

        // Used to retrieve browser information
        static Func<object, Task<object>> browserInfoFunction = null;

        public static async Task<HtmlDocument> GetDocument()
        {
            if (htmlDocument == null)
                htmlDocument = await HtmlDocument.Instance();
            return htmlDocument;
        }

        public static async Task<HtmlWindow> GetWindow()
        {
            if (htmlWindow == null)
                htmlWindow = await HtmlWindow.Instance();
            return htmlWindow;
        }

        public static async Task<BrowserInformation> GetBrowserInformation()
        {
            if (browserInfoFunction == null)
                browserInfoFunction = await WebSharp.CreateJavaScriptFunction(
                    @"return function (data, callback) {
                                    try { 
                                        let browserInfo = {}; 
                                        browserInfo['appName'] = navigator.appName;
                                        //browserInfo['appVersion'] = navigator.appVersion;
                                        browserInfo['appVersion'] = process.versions.chrome;
                                        browserInfo['userAgent'] = navigator.userAgent;
                                        browserInfo['platform'] = navigator.platform;
                                        browserInfo['cookieEnabled'] = navigator.cookieEnabled;
                                        browserInfo['appCodeName'] = navigator.appCodeName;
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
