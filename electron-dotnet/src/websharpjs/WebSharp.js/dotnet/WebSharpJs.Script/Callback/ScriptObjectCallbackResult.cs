using System;
using System.Threading.Tasks;

namespace WebSharpJs.Script
{
    public class ScriptObjectCallbackResult : ICallbackResult
    {
        object callbackState;
        public Object CallbackState => callbackState;

        public ScriptObjectCallbackResult(Object state)
        {
            callbackState = state;
        }
    }
}
