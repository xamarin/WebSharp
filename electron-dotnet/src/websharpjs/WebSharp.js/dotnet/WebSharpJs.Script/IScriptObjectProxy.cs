using System;
using System.Threading.Tasks;

namespace WebSharpJs.Script
{
    public interface IScriptObjectProxy
    {
        
        int Handle { get; }
        dynamic JavascriptFunctionProxy { get; set; }
        Func<object, Task<object>> JavascriptFunction { get; set; }
    }
}
