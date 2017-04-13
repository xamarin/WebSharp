//
// HtmlObject.cs
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;


using WebSharpJs.Script;

namespace WebSharpJs.DOM
{


    public class HtmlObject : WebSharpObject
    {

        class EventHandlerBag : GrabBag<WebSharpHtmlEvent>
        { }

        EventHandlerBag EventHandlers = new EventHandlerBag();

        public async Task<bool> AttachEvent(string eventName, EventHandler<HtmlEventArgs> handler)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException("eventName");

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            var scriptAlias = eventName;
            var ei = InstanceType.GetTypeInfo().GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public);

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
                if (ScriptObjectProxy != null)
                {
                    var eventCallback = new
                    {
                        onEvent = scriptAlias,
                        callback = websharpEvent.EventCallbackFunction
                    };
                    result = await ScriptObjectProxy.AddEventListener(eventCallback);
                }
                EventHandlers[scriptAlias] = websharpEvent;
            }
            websharpEvent.AddEventHandler(handler);
            return result;
        }

        public async Task<bool> AttachEvent<HtmlEventArgs>(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException("eventName");

            WebSharpHtmlEvent websharpEvent;

            var scriptAlias = eventName;
            var ei = InstanceType.GetTypeInfo().GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public);

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
                if (ScriptObjectProxy != null)
                {
                    var eventCallback = new
                    {
                        onEvent = scriptAlias,
                        callback = websharpEvent.EventCallbackFunction
                    };
                    result = await ScriptObjectProxy.AddEventListener(eventCallback);
                }
                EventHandlers[eventName] = websharpEvent;
            }
            return result;
        }

        protected HtmlObject() : base() { }
        protected HtmlObject(object obj) : base(obj)
        {
            var dict = obj as IDictionary<string, object>;

            // The key `websharp_id` represents a wrapped proxy object
            if (dict != null && dict.ContainsKey("websharp_id"))
            {
                ScriptObjectProxy = new DOMObjectProxy(obj);
            }
        }
    }


}