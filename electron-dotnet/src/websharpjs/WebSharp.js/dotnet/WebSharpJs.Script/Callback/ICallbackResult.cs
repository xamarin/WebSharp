using System;
using System.Threading.Tasks;

namespace WebSharpJs.Script
{
    public interface ICallbackResult
    {
        Object CallbackState { get; }
    }
}
