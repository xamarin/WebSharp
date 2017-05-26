using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;

using WebSharpJs.DOM;
using WebSharpJs.Electron;
using WebSharpJs.Script;

//namespace DialogHelpers
//{
public class Startup
{

    static WebSharpJs.NodeJS.Console console;
    static Dialog dialog;
    static HtmlDocument document;

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
            // Obtain a reference to a Dialog instance.
            dialog = await Dialog.Instance();

            var page = new HtmlPage();
            document = await page.GetDocument();

            // Get the HtmlElement with the id of "openFile"
            var openFile = await document.GetElementById("openFile");

            // Attach a "click" event handler to the openFile HtmlElement
            await openFile.AttachEvent("click", 
                new EventHandler(async (sender, evt) =>
                {


                    // Create an OpenDialogOptions reference with custom FileFilters
                    var openOptions = new OpenDialogOptions()
                    {
                        Filters = new FileFilter[]
                        {
                            new FileFilter() { Name = "text", Extensions = new string[] {"txt"}},
                            new FileFilter() { Name = "All Files", Extensions = new string[] {"*"} }
                        }
                    };

                    // Now show the open dialog passing in the options created previously
                    // and a call back function when a file is selected.
                    // If a call back function is not specified then the ShowOpenDialog function
                    // will return an array of the selected file(s) or an empty array if no
                    // files are selected.
                    await dialog.ShowOpenDialog(
                        openOptions,
                        new ScriptObjectCallback<string[]> (openFileCallback)
                    );
            
                })
            );

            // Get the HtmlElement with the id of "saveFile"
            var saveFile = await document.GetElementById("saveFile");

            // Attach a "click" event handler to the saveFile HtmlElement
            await saveFile.AttachEvent("click", new EventHandler(async (sender, evt) => 
            {
                // Create an OpenDialogOptions reference with custom FileFilters
                var saveOptions = new SaveDialogOptions()
                {
                    Filters = new FileFilter[]
                    {
                        new FileFilter() { Name = "text", Extensions = new string[] {"txt"}},
                        new FileFilter() { Name = "All Files", Extensions = new string[] {"*"} }
                    }
                };

                // Now show the save dialog passing in the options created previously
                // and a call back function to be called when the save button is clicked.
                // If a call back function is not specified then the ShowSaveDialog function
                // will return a string or an empty string if no file was specified.
                await dialog.ShowSaveDialog(
                    saveOptions,
                    new ScriptObjectCallback<string> (saveFileCallback)
                );
            }
            ));
            
        }
        catch (Exception exc) { await dialog.ShowErrorBox("There was an error: ", exc.Message); }

        return null;


    }

    /// <summary>
    /// Callback function when files are selected from the ShowOpenDialog function.
    /// </summary>
    /// <param name="files">File(s) selected</param>
    /// <returns>null</returns>
    async Task openFileCallback(ICallbackResult ar)
    {
        
        // See if we have any file(s) selected
        var fileNames = ar.CallbackState as object[];

        if (fileNames == null || fileNames.Length == 0)
            return;

        // Grab the first file name specified
        var fileName = (string)fileNames[0];

        // Open and read the file asynchronously.
        byte[] result;
        using (FileStream SourceStream = File.Open(fileName, FileMode.Open))
        {
            result = new byte[SourceStream.Length];
            await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
        }

        // When the file has been read get a reference to the "editor" HtmlElement
        var editor = await document.GetElementById("editor");

        // and set it's "value" property to the text that was read from the file
        await editor.SetProperty("value", System.Text.Encoding.ASCII.GetString(result));

    }

    async Task saveFileCallback(ICallbackResult ar)
    {
        var fileName = ar.CallbackState;
        if (fileName == null || string.IsNullOrEmpty((string)fileName))
            return;

        // Get a reference to the "editor" HtmlElement
        var editor = await document.GetElementById("editor");

        // and get it's "value" property to be written out
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] result = encoding.GetBytes(await editor.GetProperty<string>("value"));

        try
        {
            using (FileStream SourceStream = File.Open((string)fileName, FileMode.OpenOrCreate))
            {
                SourceStream.Seek(0, SeekOrigin.End);
                await SourceStream.WriteAsync(result, 0, result.Length);
                var messageBoxOptions = new MessageBoxOptions()
                {
                    MessageBoxType = MessageBoxType.Info,
                    Title = "Text Editor",
                    Message = "The file has been saved!!",
                    Buttons = new string[] { "OK" }
                };

                await dialog.ShowMessageBox(messageBoxOptions);
            };
        }
        catch (Exception exc)
        {
            await dialog.ShowErrorBox("There was an error saving the file.", exc.Message);
        }

    }
}
//}
