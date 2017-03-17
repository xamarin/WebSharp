//
// WebSharpEvent.cs
//

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WebSharpJs.Browser
{
    internal class WebSharpEvent
    {
        WebSharpObject Source { get; set; }

        string EventName { get; set; }

        EventHandler EventHandlers { get; set; }

        internal Func<object, Task<object>> EventCallbackFunction { get; private set; }

        internal WebSharpEvent(WebSharpObject eventSource, string eventName)
        {
            Source = eventSource;
            EventName = eventName;

            EventCallbackFunction = (async (evt) =>
            {
                
                Invoke(new object[] { EventArgs.Empty });
                return null;
            });
        }

        internal object Invoke(object[] args)
        {
            EventHandlers?.Invoke(this.Source, EventArgs.Empty);

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

        internal void AddEventHandler(EventHandler handler)
        {
            EventHandlers += handler;
        }

        internal void RemoveEventHandler(EventHandler handler)
        {
            if (EventHandlers != null)
            {
                EventHandlers -= handler;
            }
        }

    }

}
