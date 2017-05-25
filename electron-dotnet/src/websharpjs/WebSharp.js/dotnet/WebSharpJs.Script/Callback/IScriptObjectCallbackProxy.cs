using System;
using System.Threading.Tasks;

namespace WebSharpJs.Script
{
    internal interface IScriptObjectCallbackProxy
    {
        Func<object, Task<object>> CallbackProxy { get; }
        MetaData[] TypeMappings { get; }

    }

}
