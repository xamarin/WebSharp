using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharpJs.Script
{
    public interface IScriptObjectProxy
    {
        
        int Handle { get; }
        Task<T> GetProperty<T>(string name);
        Task<bool> SetProperty(string name, object value, bool createIfNotExists = true, bool hasOwnProperty = false);
        Task<object> TryInvokeAsync(dynamic parameters);
        Task<bool> AddEventListener(object callback);
        dynamic JavascriptFunctionProxy { get; set; }
    }
}
