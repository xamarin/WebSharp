
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.DOM.Events
{
    [ScriptableType(ScriptableType = ScriptableType.Event)]
    public class Event
    {
        [ScriptableMember(ScriptAlias = "type")]
        public string Type { get; private set; }
        [ScriptableMember(ScriptAlias = "bubbles")]
        public bool Bubbles { get; internal set; }
        [ScriptableMember(ScriptAlias = "cancelable")]
        public bool Cancelable { get; internal set; }


        public Event()
        {

        }

        public Event(string type, bool bubbles = false, bool cancelable = false)
            : this()
        {
            Type = type;
            Bubbles = bubbles;
            Cancelable = cancelable;
        }
    }
}
