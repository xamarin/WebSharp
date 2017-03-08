//
// HtmlObject.cs
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;


namespace WebSharpJs.Browser
{


    public partial class HtmlObject : WebSharpObject
    {

        class EventHandlerBag : GrabBag<WebSharpHtmlEvent>
        { }

        EventHandlerBag EventHandlers = new EventHandlerBag();

        public async Task<bool> AttachEventAsync(string eventName, EventHandler<HtmlEventArgs> handler)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException("eventName");

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            var scriptAlias = eventName;
            var ei = InstanceType.GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public);

            if (ei != null)
            {
                if (ei.IsDefined(typeof(ScriptableMemberAttribute), false))
                {
                    var att = ei.GetCustomAttribute<ScriptableMemberAttribute>(false);
                    scriptAlias = (att.ScriptAlias ?? scriptAlias);
                }
            }

            WebSharpHtmlEvent websharpEvent;
            var result = false;
            if (!EventHandlers.TryGetValue(scriptAlias, out websharpEvent))
            {

                websharpEvent = new WebSharpHtmlEvent(this, scriptAlias);
                if (JavaScriptProxy != null)
                {
                    var eventCallback = new
                    {
                        onEvent = scriptAlias,
                        callback = websharpEvent.EventCallbackFunction
                    };
                    result = await JavaScriptProxy.websharp_addEventListener(eventCallback);
                }
                EventHandlers[scriptAlias] = websharpEvent;
            }
            websharpEvent.AddEventHandler(handler);
            return result;
        }

        public async Task<bool> AttachEventAsync<HtmlEventArgs>(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException("eventName");

            WebSharpHtmlEvent websharpEvent;

            var scriptAlias = eventName;
            var ei = InstanceType.GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public);

            if (ei == null)
                return false;

            if (ei.IsDefined(typeof(ScriptableMemberAttribute), false))
            {
                var att = ei.GetCustomAttribute<ScriptableMemberAttribute>(false);
                scriptAlias = (att.ScriptAlias ?? scriptAlias);
            }

            var result = false;

            if (!EventHandlers.TryGetValue(eventName, out websharpEvent))
            {
                websharpEvent = new WebSharpHtmlEvent(this, eventName);
                if (JavaScriptProxy != null)
                {
                    var eventCallback = new
                    {
                        onEvent = scriptAlias,
                        callback = websharpEvent.EventCallbackFunction
                    };
                    result = await JavaScriptProxy.websharp_addEventListener(eventCallback);
                }
                EventHandlers[eventName] = websharpEvent;
            }
            return result;
        }


    }


}