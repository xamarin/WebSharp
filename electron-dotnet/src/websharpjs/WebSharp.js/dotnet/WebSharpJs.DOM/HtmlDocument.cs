using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{
    public sealed class HtmlDocument : WebSharpObject
    {
        static DOMObjectProxy scriptProxy;

        public static async Task<HtmlDocument> Instance()
        {
            var proxy = new HtmlDocument();
            await proxy.Initialize();
            return proxy;
        }

        private async Task Initialize()
        {
            if (scriptProxy == null)
            {
                scriptProxy = new DOMObjectProxy("document");
            }

            await scriptProxy.GetProxyObject();
            ScriptObjectProxy = scriptProxy;

        }


        public async Task<string> GetReadyState()
        {
            return await GetPropertyAsync<string>("readyState");
        }

        public async Task<bool> GetIsReady()
        {
            var state = await GetReadyState();
            if (state == "complete")
            {
                return true;
            }

            return false;
        }

        public async Task<string> GetDocumentUri()
        {
            return await GetPropertyAsync<string>("URI");
        }

        public async Task<Location> GetLocation()
        {
            var location = await GetPropertyAsync<object>("location");
            if (location != null)
            {
                var loc = new Location();
                ScriptObjectHelper.DictionaryToScriptableType((IDictionary<string, object>)location, loc);
                return loc;
            }
            return default(Location);
        }

        public async Task<HtmlElement> GetElementById(string id)
        {
            return await InvokeAsync<HtmlElement>("getElementById", id); ;
        }
    }

    [ScriptableType]
    public class Location
    {
        [ScriptableMember(ScriptAlias = "hash")]
        public string Hash { get; internal set; }
        [ScriptableMember(ScriptAlias = "host")]
        public string Host { get; internal set; }
        [ScriptableMember(ScriptAlias = "hostname")]
        public string HostName { get; internal set; }
        [ScriptableMember(ScriptAlias = "href")]
        public string HRef { get; internal set; }
        [ScriptableMember(ScriptAlias = "origin")]
        public string Origin { get; internal set; }
        [ScriptableMember(ScriptAlias = "pathname")]
        public string PathName { get; internal set; }
        [ScriptableMember(ScriptAlias = "port")]
        public string Port { get; internal set; }
        [ScriptableMember(ScriptAlias = "protocol")]
        public string Protocol { get; internal set; }
        [ScriptableMember(ScriptAlias = "search")]
        public string Search { get; internal set; }






    }
}
