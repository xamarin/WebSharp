//
// WebSharpHtmEvent.cs
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{
    internal class WebSharpHtmlEvent
    {
        WebSharpObject Source { get; set; }

        string EventName { get; set; }
        internal int UID { get; private set; }

        EventHandler<HtmlEventArgs> EventHandlers { get; set; }

        internal Func<object, Task<object>> EventCallbackFunction { get; private set; }
        bool defaultPrevented = false;
        bool cancelBubble = false;

        internal WebSharpHtmlEvent(WebSharpObject eventSource, string eventName)
        {
            Source = eventSource;
            EventName = eventName;
            UID = ScriptObjectUtilities.NextUID;

            EventCallbackFunction = async (evt) =>
            {
                Invoke(evt);
                return new { defaultPrevented, cancelBubble};
            };
        }

        internal object Invoke(object evt)
        {
            
            var eventArgs = new HtmlEventArgs();
            defaultPrevented = false;
            cancelBubble = false;
            eventArgs.PreventDefaultAction = () => { defaultPrevented = true; }; 
            eventArgs.StopPropagationAction = () => { cancelBubble = true; };

            if (evt != null)
            {
                try
                {
                    var dict = (IDictionary<string, object>)((object[])evt)[0];
                    ScriptObjectHelper.DictionaryToScriptableType(dict, eventArgs);
                }
                catch (Exception evtExc)
                {
                    Console.WriteLine($"Error parsing HtmlEventArgs: {evtExc.Message}");
                }
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
                    Console.WriteLine($"WebSharpHtmlEvent Invoke Exception {exc.Message}");
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
