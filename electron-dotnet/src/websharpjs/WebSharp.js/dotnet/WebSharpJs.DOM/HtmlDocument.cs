using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{
    public sealed class HtmlDocument : WebSharpObject
    {
        static DOMObjectProxy scriptProxy;

        internal static async Task<HtmlDocument> Instance()
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
            return await GetProperty<string>("readyState");
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
            return await GetProperty<string>("URI");
        }

        public async Task<HtmlElement> GetDocumentElement()
        {
            return await GetProperty<HtmlElement>("documentElement");
        }

        public async Task<Location> GetLocation()
        {
            var location = await GetProperty<object>("location");
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
            return await Invoke<HtmlElement>("getElementById", id); ;
        }

        public async Task<ScriptObjectCollection<HtmlElement>> GetElementsByTagName(string tagName)
        {
            return await Invoke<ScriptObjectCollection<HtmlElement>>("getElementsByTagName", tagName); ;
        }

        public async Task<object> Submit()
        {
            // Posts user data from the FORM element in the browser's Document Object Model (DOM) to the server.

            //Remarks
            //If an HTML page has multiple FORM elements, this overload posts user data from the first FORM element to the server
            
            // First we need to get all the FORM elements that the page has.
            var forms = await GetProperty<ScriptObjectCollection<HtmlElement>>("forms");

            if (forms == null)
                return null;

            // Even if there is more than one on the page we will only
            // submit the first one that is found.
            await forms[0].Invoke<object>("submit");
            
            return null;
        }

        public async Task<object> Submit(string formId)
        {

            if (string.IsNullOrEmpty(formId))
                throw new ArgumentException($"Null or Empty is not valid for {nameof(formId)}");
            // Posts user data from the specified FORM element to the server.

            // Remarks
            // This overload posts user data from an HTML FORM element that is 
            // identified by the formId parameter.
            // For example, if your HTML document has an element defined as 
            // < form id = "xyz" />, you can submit its user data by calling 
            // HtmlDocument.Submit("xyz").

            // First we need to get all the FORM elements on the page
            var forms = await GetProperty<ScriptObjectCollection<HtmlElement>>("forms");

            if (forms == null)
                return null;

            // now lets iterate over all of the forms until we find the one we need.
            foreach(var form in forms)
            {
                var id = await form.GetId();

                if (id == formId)
                {
                    await form.Invoke<object>("submit");
                    break;
                }

            }

            return null;
        }

        public async Task<HtmlElement> CreateElement(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
                throw new ArgumentException($"Null or Empty is not valid for {nameof(tagName)}");

            return await Invoke<HtmlElement>("createElement", tagName);
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
