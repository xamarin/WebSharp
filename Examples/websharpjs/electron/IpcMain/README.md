# ipcmain README

Example of IPC Communications between Renderer (uses IpcRenderer) and Main (uses IpcMain) Processes.

Communicate asynchronously from a renderer process to the main process.

## Features

The `IpcMain` module is an instance of the `EventEmitter` class. When used in the main process, it handles asynchronous and synchronous messages sent from a renderer process (web page). Messages sent from a renderer will be emitted to this module.

Inside `MainWindow.cs` you will find:
- Creating an IpcMain object instance.
- Waiting `On` a synchronous message from the Renderer process.  This is implemented via a JavaScript function embedded in the `MainWindow.cs`.

``` c-sharp
            // Note: Sending a synchronous message from the render process will block 
            // the whole renderer process, unless you know what you are doing you should never use it.
            //
            // We will define the synchronous function via a javascript function
            // There is a limitation on synchronous messages via the CLR because of the 
            // event.returnValue will not be set via the CLR.
            // If synchronous messaging is desired then a CLR implementation is what
            // you want.
            var synchronousFunction = await WebSharpJs.WebSharp.CreateJavaScriptFunction(
                    @"
                        return function (data, callback) {
                            const {ipcMain} = require('electron')
                            ipcMain.on('synchronous-message', (event, arg) => {
                                console.log(arg)  // prints 'ping'
                                event.returnValue = 'synchronous pong'
                            });
                            callback(null, null);

                        }");

                await synchronousFunction(null); // attach our synchronous message listener

```

- Sending a synchronous response back to the Renderer process.  See above `event.returnValue`.
- Waiting `On` an asynchronous message from Renderer process and sending back a response via the IpcMainEvent.Sender property.

``` c-sharp
               ipcMain.On("asynchronous-message",
                    new IpcMainEventListener
                    (
                        async(result) =>
                        {
                            // process result here

                            await ipcMainEvent.Sender.Send("asynchronous-reply", "asynchronous-pong1", "asynchronous-pong2");
                        }
                    )
                );
```


The `IpcRenderer` module is an instance of the `EventEmitter` class. It provides a few methods so you can send synchronous and asynchronous messages from the render process (web page) to the main process. You can also receive replies from the main process.

Inside `Ping.cs` you will find:
- Creating an IpcRenderer object instance.
- Sending a synchronous message to the main process and logging the response.
- Waiting `On` a message.
- Receiving a message and logging the respons.


![screen shot windows](images/Ipc-windows.png)


![screen shot mac](images/Ipc-mac.png)

More information can be found in the [ipcRenderer documentation](https://github.com/electron/electron/blob/master/docs/api/ipc-renderer.md)


## Requirements

   * `electron-dotnet` needs to be built.  The easiest way is to use the provided `make` files available in the WebSharp base directory.  
   
      * [See Getting Started on Windows](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md)
   
      * [See Getting Started on Mac](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md)

> :bulb: Windows users need to make sure [Mono is available](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md#setting-mono-path) in their %PATH%.


## Limitations

> :exclamation: Limitation on synchronous events.  They will block the Renderer process when implemented on the Main process in CLR code.  The synchronous messaging support is only meant to support CLR <-> JavaScript.  If developer really needs synchronous messaging CLR <-> CLR then the developer will need to implement this using whatever means that is available via the CLR.

View warning note in the Electron documentation for IpcRenderer:  https://electron.atom.io/docs/api/ipc-renderer/#ipcrenderersendsyncchannel-arg1-arg2-

More information on synchronous and asynchronous communications:
https://github.com/electron/electron/issues/5750



## Known Issues



## Release Notes



### 1.0.0

Initial release