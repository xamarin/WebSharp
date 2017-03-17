using System;
using System.Threading.Tasks;

using WebSharpJs.Browser;

namespace <%- wsClassName %>
{
    public class Startup
    {

        static NodeConsole console;

        /// <summary>
        /// Default entry into managed code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<object> Invoke(object input)
        {
            if (console == null)
                console = await NodeConsole.Instance();

            try
            {
                console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
}
