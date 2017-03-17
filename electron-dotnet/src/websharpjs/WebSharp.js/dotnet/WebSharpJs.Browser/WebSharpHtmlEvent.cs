//
// WebSharpHtmEvent.cs
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace WebSharpJs.Browser
{
    internal class WebSharpHtmlEvent
    {
        WebSharpObject Source { get; set; }

        string EventName { get; set; }

        EventHandler<HtmlEventArgs> EventHandlers { get; set; }

        internal Func<object, Task<object>> EventCallbackFunction { get; private set; }

        internal WebSharpHtmlEvent(WebSharpObject eventSource, string eventName)
        {
            Source = eventSource;
            EventName = eventName;

            EventCallbackFunction = (async (evt) =>
            {
                Invoke(evt);
                return null;
            });
        }

        internal object Invoke(object evt)
        {

            var eventArgs = new HtmlEventArgs();
            if (evt != null)
            {
                var dict = (IDictionary<string, object>)((object[])evt)[0];
                ScriptObjectHelper.DictionaryToScriptableType(dict, eventArgs);
            }

            EventHandlers?.Invoke(this.Source, eventArgs);

            if (EventHandlers == null)
            {
                try
                {
                    var eventDelegate = (MulticastDelegate)Source.GetType().GetTypeInfo().GetField(EventName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(Source);
                    eventDelegate.DynamicInvoke(new object[] { Source, EventArgs.Empty });
                }
                catch (Exception exc)
                {
                    // We get an exception if there is nothing to invoke
                    Console.WriteLine($"exception {exc.Message}");
                }
            }
            return null;
        }

        internal void AddEventHandler(EventHandler<HtmlEventArgs> handler)
        {

            EventHandlers += handler;
        }

        internal void RemoveEventHandler(EventHandler<HtmlEventArgs> handler)
        {
            if (EventHandlers != null)
            {
                EventHandlers -= handler;
            }
        }

    }
}
