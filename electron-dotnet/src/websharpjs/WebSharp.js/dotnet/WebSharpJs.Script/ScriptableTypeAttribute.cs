//
// ScriptableTypeAttribute.cs
//

using System;

namespace WebSharpJs.Script
{

    public enum ScriptableType
    {
        Default,
        Event,
        DomNoInterfaceObjectAttribute  // it indicates that an interface object will not exist for the interface in the ECMAScript binding
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class ScriptableTypeAttribute : Attribute
    {
        public ScriptableType ScriptableType { get; set; }
    }

}