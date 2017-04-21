using System;
using System.Threading.Tasks;

namespace WebSharpJs.Script
{
    public interface IScriptObjectProxy
    {
        
        int Handle { get; }
        Task<T> GetProperty<T>(dynamic name);
        Task<bool> SetProperty(string name, object value, bool createIfNotExists = true, bool hasOwnProperty = false);
        Task<object> TryInvoke(dynamic parameters);
        Task<bool> AddEventListener(object callback);
        dynamic JavascriptFunctionProxy { get; set; }
        Func<object, Task<object>> JavascriptFuncion { get; set; }
    }
}
