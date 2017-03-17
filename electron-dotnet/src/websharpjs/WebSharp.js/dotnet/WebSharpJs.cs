using System;
using System.Threading.Tasks;

namespace WebSharpJs
{
    public class WebSharp
    {
        static bool initialized;
        static Func<object, Task<object>> compileFunc;

        public Task<object> InitializeInternal(object input)
        {
            if (!initialized)
            {
                compileFunc = (Func<object, Task<object>>)input;
                initialized = true;
            }
            return Task<object>.FromResult((object)null);
        }

        public static async Task<Func<object, Task<object>>> CreateJavaScriptFunction(string code)
        {

            if (compileFunc == null)
            {
                throw new InvalidOperationException("WebSharpJs.CreateJavaFunction cannot be used after WebSharpJs.Close had been called.");
            }
            Console.WriteLine("WebSharpJs: CreateJavaScriptFunction");
            return (Func<object, Task<object>>)await compileFunc(code);
        }

    }
}
