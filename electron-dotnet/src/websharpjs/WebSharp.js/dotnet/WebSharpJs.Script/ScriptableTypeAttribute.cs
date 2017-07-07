//
// ScriptableTypeAttribute.cs
//

using System;

namespace WebSharpJs.Script
{

    public enum ScriptableType
    {
        Default,
        Event
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class ScriptableTypeAttribute : Attribute
    {
        public ScriptableType ScriptableType { get; set; }
    }

}