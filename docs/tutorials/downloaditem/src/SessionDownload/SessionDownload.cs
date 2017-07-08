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
                var document = await HtmlPage.GetDocument();

                var link = await document.GetElementById("link");
                await link.AttachEvent("click", onMouseClick);

                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }

        private async void onMouseClick(object sender, HtmlEventArgs args)
        {
            
            args.PreventDefault();
            //args.StopPropagation();
            var target = sender as HtmlElement;
            var href = await target.GetProperty<string>("href");
            await console.Log($"clicked {await target?.GetId()} for {href}");
            
            var ipcRenderer = await IpcRenderer.Create();
            ipcRenderer.Send("download-file", href);

        }
    }
//}
