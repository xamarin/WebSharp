using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Electron;
using WebSharpJs.DOM;

//namespace Clipper
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
               var clipboard = await Clipboard.Instance();

                await console.Log("Available Formats");
                var formats = await clipboard.AvailableFormats();
                for (int f = 0; f < formats.Length; f++)
                    await console.Log(formats[f]);

                // if we have an image format on the clipboard the we need to readit and set the image's
                // source element.
                if (formats.Length == 1 && formats[0] == "image/png")
                {
                    // Read the clipboard Image
                    var image = await clipboard.ReadImage();

                    // If we have the image
                    if (image != null)
                    {
                        // Get access to the DOM
                        var document = await HtmlPage.GetDocument();

                        // Get a reference to the img element
                        var imageElement = await document.GetElementById("image");
                        if (imageElement != null)
                        {
                            // set the src property to the data string of the image/
                            await imageElement.SetProperty("src", await image.ToDataURL());
                        }
                    }
                }

                await clipboard.WriteText("Example String copied to clipboard");
                await console.Log($"Clipboard text read: {await clipboard.ReadText()}");

                await console.Log("Clearing clipboard");
                await clipboard.Clear();

                await console.Log("Available Formats");
                formats = await clipboard.AvailableFormats(ClipboardType.Selection);
                for (int f = 0; f < formats.Length; f++)
                    await console.Log(formats[f]);

                await console.Log("Writing data to the clipboard");
                await clipboard.Write(new ClipboardData() { Text = "test", HTML = "<b>test</b>" });

                await console.Log("Available Formats");
                formats = await clipboard.AvailableFormats(ClipboardType.Selection);
                for (int f = 0; f < formats.Length; f++)
                    await console.Log(formats[f]);

                await console.Log($"Reading Clipboard Text data: {await clipboard.ReadText()}");
                await console.Log($"Reading Clipboard HTML data: {await clipboard.ReadHTML()}");
                await console.Log($"Reading Clipboard RTF data: {await clipboard.ReadRTF()}");

                await console.Log($"Has {await clipboard.Has("<p>selection</p>")}");

                await console.Log("Writing bookmark to the clipboard");
                await clipboard.WriteBookmark("Electron Homepage", @"https://electron.atom.io");
               
                await console.Log(await clipboard.ReadBookmark());

                await console.Log("Available Formats");
                formats = await clipboard.AvailableFormats(ClipboardType.Selection);
                for (int f = 0; f < formats.Length; f++)
                    await console.Log(formats[f]);

                await console.Log($"Hello:  {input}");                

            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
//}
