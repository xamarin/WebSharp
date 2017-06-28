using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.DOM;

//namespace Information
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

        /// <summary>
        /// Default entry into managed code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<object> Invoke(object input)
        {
            if (console == null)
                console = await WebSharpJs.NodeJS.Console.Instance();

            try
            {
                var document = await HtmlPage.GetDocument();
                var info = await HtmlPage.GetBrowserInformation();
   
                var infoText = $"Name: {info.Name}<br />Browser Version: {info.BrowserVersion}<br />Platform: {info.Platform}<br />Cookies Enabled: {info.CookiesEnabled}<br />User Agent: {info.UserAgent}";

                var paragraph = await document.GetElementById("info");
                await paragraph.SetProperty("innerHTML", infoText);

                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
//}
