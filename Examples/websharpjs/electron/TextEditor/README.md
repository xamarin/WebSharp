# texteditor README

Example Electron Dialogs API

Display native system dialogs for opening and saving files, alerting, etc.

To show these dialogs we are going to implement a very basic text editor.

## Features

- Accessing the DOM using WebSharpJs.DOM name space.

    - Obtaining a reference to the underlying `HtmlDocument` using the static `HtmlPage` class.

    ``` cs
           document = await page.GetDocument();
    ```
    - Using the `HtmlDocument` to obtain references to `HtmlElements` by their `id`
    ``` cs
            // Get the HtmlElement with the id of "openFile"
            var openFile = await document.GetElementById("openFile");
    ```
    - Adding event handlers to handle `click` events of the `HtmlElement`.

    ``` cs
            // Attach a "click" event handler to the openFile HtmlElement
                await openFile.AttachEvent("click", new EventHandler(async (sender, evt) =>
                {
                    // event code here
                }
                ));

    ```
    - Retrieving and Setting properties on an HtmlElement.

    ``` cs
        // and get it's "value" property to be written out
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] result = encoding.GetBytes(await editor.GetProperty<string>("value"));
    ```

    ``` cs
        // and set it's "value" property to the text that was read from the file
        await editor.SetProperty("value", System.Text.Encoding.ASCII.GetString(result));

    ```

- Electron Dialog features.

    - Opening a dialog to select a file using `ShowOpenDialog` with a list of custom `FileFilters`.
    ``` cs
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
                    openFileCallback
                );
    ```
    - Use of call back functions.
    ``` cs
                    await dialog.ShowOpenDialog(
                    openOptions,
                    openFileCallback
                );
    ```

    ``` cs
        async Task<object> openFileCallback(object files)
        {
            // code here ...
        }
 
    ```
    - Opening a dialog to save a file using `ShowSaveDialog` with a list of custom `FileFilters`.    

    ``` cs
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
                    saveFileCallback
                );
    ```
    - Show a message box with a custom properties.

    ``` cs
                var messageBoxOptions = new MessageBoxOptions()
                {
                    MessageBoxType = MessageBoxType.Info,
                    Title = "Text Editor",
                    Message = "The file has been saved!!",
                    Buttons = new string[] { "OK" }
                };

                await dialog.ShowMessageBox(messageBoxOptions);    
    ```
    - Error messages when something goes wrong.

    ``` cs
        try
        {
            // Save code goes here.
        }
        catch (Exception exc)
        {
            await dialog.ShowErrorBox("There was an error saving the file.", exc.Message);
        }    
    ```

More information can be found in the [dialogs documentation](https://github.com/electron/electron/blob/master/docs/api/dialog.md)

## Requirements

   * `electron-dotnet` needs to be built.  The easiest way is to use the provided `make` files available in the WebSharp base directory.  
   
      * [See Getting Started on Windows](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md)
   
      * [See Getting Started on Mac](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md)

> :bulb: Windows users need to make sure [Mono is available](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md#setting-mono-path) in their %PATH%.

## Known Issues



## Release Notes



### 1.0.0

Initial release