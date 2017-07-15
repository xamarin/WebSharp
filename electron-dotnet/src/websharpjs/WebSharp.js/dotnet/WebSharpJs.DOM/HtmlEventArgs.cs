//
// HtmlEventArgs.cs
//


using System;
using System.Threading.Tasks;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{

    [ScriptableType]
    public class HtmlEventArgs : EventArgs
    {
        [ScriptableMember(ScriptAlias = "altKey")]
        public bool AltKey
        {
            get;
            internal set;
        }

        [ScriptableMember(ScriptAlias = "charCode")]
        public int CharacterCode
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "clientX")]
        public int ClientX
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "clientY")]
        public int ClientY
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "ctrlKey")]
        public bool CtrlKey
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "eventType")]
        public string EventType
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "keyCode")]
        public int KeyCode
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "button")]
        public MouseButtons MouseButton
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "offsetX")]
        public int OffsetX
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "offsetY")]
        public int OffsetY
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "screenX")]
        public int ScreenX
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "screenY")]
        public int ScreenY
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "shiftKey")]
        public bool ShiftKey
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "movementX")]
        public int MovementX
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "movementY")]
        public int MovementY
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "relatedTarget")]
        public HtmlElement RelatedTarget
        {
            get;
            internal set;

        }

        [ScriptableMember(ScriptAlias = "dataTransfer")]
        public DataTransfer DataTransfer
        {
            get;
            internal set;

        }

        // Set by WebSharpHtmlEvent.cs
        internal Action PreventDefaultAction { get; set; }

        public void PreventDefault()
        {
            PreventDefaultAction();
        }

        // Set by WebSharpHtmlEvent.cs
        internal Action StopPropagationAction { get; set; }

        public void StopPropagation()
        {
            StopPropagationAction();
        }

    }

}