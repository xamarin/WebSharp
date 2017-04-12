using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Electron;

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

                await clipboard.WriteText("Example String copied to clipboard");
                await console.Log($"Clipboard text read: {await clipboard.ReadText()}");

                await console.Log("Clearing clipboard");
                await clipboard.Clear();

                await console.Log("Available Formats");
                var formats = await clipboard.AvailableFormats(ClipboardType.Selection);
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
