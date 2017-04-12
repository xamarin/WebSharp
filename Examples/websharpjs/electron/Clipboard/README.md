# clipboard README

Example of Electron&#39;s Clipboard 

Interacts with Electron&#39; Clipboard through C# to perform copy and paste operations on the system clipboard..

## Features

Perform copy and paste operations on the system clipboard.

Inside `Clipper.cs` you will find:

Referencing the `Clipboard` instance.
- Sending and reading back information to/from the system clipbard.
- Querying information such as AvailableFormats.
- Clearing the clipboard.
- Bookmark information is sent and received using `Bookmark` class.
- Writing Data to the clipboard using ClipboardData class.

![screen shot windows](images/Clipper-windows.png)

![screen shot mac](images/Clipper-mac.png)

More information can be found in the [clipboard documentation](https://github.com/electron/electron/blob/master/docs/api/clipboard.md)

## Requirements

   * `electron-dotnet` needs to be built.  The easiest way is to use the provided `make` files available in the WebSharp base directory.  
   
      * [See Getting Started on Windows](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md)
   
      * [See Getting Started on Mac](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md)

> :bulb: Windows users need to make sure [Mono is available](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md#setting-mono-path) in their %PATH%.

## Known Issues



## Release Notes



### 1.0.0

Initial release
