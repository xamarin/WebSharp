//
// ScriptableMemberAttribute.cs
//

using System;

namespace WebSharpJs.Browser
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
    public sealed class ScriptableMemberAttribute : Attribute
    {
        public bool EnableCreateableTypes { get; set; }
        public string ScriptAlias { get; set; }
        public bool CreateIfNotExists { get; set; }
        public bool HasOwnProperty { get; set; }
    }
}