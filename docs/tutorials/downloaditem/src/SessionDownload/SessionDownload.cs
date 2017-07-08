using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Electron;
using WebSharpJs.Script;
using WebSharpJs.DOM;

//namespace SessionDownload
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
                // Get a reference to the DOM Document
                var document = await HtmlPage.GetDocument();

                // Get a reference to the "link" element
                var link = await document.GetElementById("link");
                // Attach an event listener to the "click" event
                await link.AttachEvent("click", onMouseClick);

                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }

        private async void onMouseClick(object sender, HtmlEventArgs args)
        {
            // Prevent the default click handler `will-download` from firing.
            args.PreventDefault();
            //args.StopPropagation();

            // Using the sender object we will obtain the `href`
            // information from the `<a>` anchor element.
            var target = sender as HtmlElement;
            var href = await target.GetProperty<string>("href");
            await console.Log($"clicked {await target?.GetId()} for {href}");
            
            // Notifiy the Main process that it should handle the download
            var ipcRenderer = await IpcRenderer.Create();
            ipcRenderer.Send("download-file", href);

        }
    }
//}
