//
// ScriptableTypeAttribute.cs
//

using System;

namespace WebSharpJs.Script
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class ScriptableTypeAttribute : Attribute
    { }

}