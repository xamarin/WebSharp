using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Electron;
using WebSharpJs.Script;
using WebSharpJs.DOM;

//namespace PdfRenderer
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
                var printPDFBtn = await document.GetElementById("print-pdf");

                var ipcRenderer = await IpcRenderer.Create();

                await printPDFBtn?.AttachEvent("click", 
                    new EventHandler(async (sender, evt) =>
                    {
                        await console.Log("clicked");
                        ipcRenderer.Send("print-to-pdf");
                    })
                );

                ipcRenderer.On("wrote-pdf", 
                    new IpcRendererEventListener(async (result) =>
                {
                    var state = result.CallbackState as object[];
                    var parms = state[1] as object[];
                    foreach(var parm in parms)
                        await console.Log(parm); 
                    var pathLabel = await document.GetElementById("file-path");
                    await pathLabel.SetProperty("innerHTML", $"Wrote PDF to: {parms[0]}");
                }));
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
//}
