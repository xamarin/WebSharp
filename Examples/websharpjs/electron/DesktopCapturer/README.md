# desktopcaptuer README

Example shows how to capture video from a desktop windows using Electron DesktopCapturer. 

The DesktopCapturer Accesses information about media sources that can be used to capture audio and video from the desktop using the navigator.webkitGetUserMedia API.

## Features

The `IpcMain` module is an instance of the `EventEmitter` class. When used in the main process, it handles asynchronous and synchronous messages sent from a renderer process (web page). Messages sent from a renderer will be emitted to this module.

Another feature that is demonstrated here is the creating and calling a `JavaScript` function from the CLR code.  See `Define a custom JavaScript function`

Inside `Capture.cs` you will find:

- Creating a `DesktopCapturer` object instance.

``` c-sharp
    var desktopCapturer = await DesktopCapturer.Instance();
```

- Calling the `GetSources` method of the `DesktopCapturer`.  The options that are passed specify the types of sources as `DesktopCapturer.Type.Window` and `DesktopCapturerType.Screen`.  

``` c-sharp

    new DesktopCapturerOptions() { Types = DesktopCapturerType.Window | DesktopCapturerType.Screen }

```

``` c-sharp

            // GetSources - Starts gathering information about all available desktop media sources, and calls 
            // ScriptObjectCallback<Error, DesktopCapturerSource[]> when finished.
            await desktopCapturer.GetSources(new DesktopCapturerOptions() { Types = DesktopCapturerType.Window | DesktopCapturerType.Screen },
                new ScriptObjectCallback<Error, DesktopCapturerSource[]>(
                     async (result) =>
                     {
                         // Code implementation is below 

                     }));
```

- Loop over all the data sources until we find the one that we want.  In this case we will look for a source that has the name `Entire screen`.

``` c-sharp

                         var resultState = (object[])result.CallbackState;
                         var error = resultState[0] as Error;
                         var sources = resultState[1] as DesktopCapturerSource[];
                         if (error != null)
                         {
                             throw new Exception(error.Message);
                         }

                         if (sources == null)
                             return;

                         // Loop through all available sources.
                         foreach(var source in sources)
                         {
                             // Log the sources and their thumbnail sizes.
                             await console.Log($"source id: {source.Id} name: {source.Name} size: {await source.Thumbnail.GetSize()}");

                             // Grab the "Entire screen" id
                             if (source.Name == "Entire screen")
                             {
                                 // Pass the source id.
                                 await mediaStuff(source.Id);
                             }
                         }


```

- Define a custom JavaScript function,` mediaStuff`, that works with the [navigator.mediaDevices.getUserMedia API](https://developer.mozilla.org/en/docs/Web/API/MediaDevices/getUserMedia).  There is no existing CLR binding for this API at this time.  This works on the `<video></video>` tag in the `index.html` file.

``` c-sharp

            var mediaStuff = await WebSharpJs.WebSharp.CreateJavaScriptFunction(
                @"
                    return function (data, callback) {
                        console.log('mediadevice id --------:' + data);
                        navigator.mediaDevices.getUserMedia({
                                audio: false,
                                video: {
                                  mandatory: {
                                    chromeMediaSource: 'desktop',
                                    chromeMediaSourceId: data,
                                    minWidth: 640,
                                    maxWidth: 640,
                                    minHeight: 360,
                                    maxHeight: 360
                                  }
                                }
                              }).then(handleStream).catch(handleError);

                        function handleStream (stream) {
                            console.log('handle stream');
                            document.querySelector('video').src = URL.createObjectURL(stream)
                        }

                        function handleError (e) {
                          console.log(e)
                        }

                        callback(null, null);

                    }");

``` 

The workhorse in the above code is the static function `WebSharp.CreateJavaScriptFunction` that the `WebSharp.js` managed assembly makes available to developers.

The `CreateJavaScriptFunction` accepts a string containing code in `Nodejs`, compiles it and returns a `JavaScript` function callable from the C# implementation. The `JavaScript` function must have the following signature:

   * It must accept one parameter and a callback.  `return function (data, callback)`
   * The callback must be called with an error and one return value.  `callback(null, null);`

The following is the pattern when scripting `Nodejs` functions:

``` javascript
        return function (data, callback) {
        
            //...... Your implementation code here .......

            callback(null, null);
        }

```

Inside `index.html` you will find.

- The definition of a `<video></video>` HTML Element which the video will be attached to.

``` html

<!DOCTYPE html>
<html>
  <head>
    <meta charset="UTF-8">
    <title>DesktopCapturer</title>
  </head>
  <body>
    <h1>DesktopCapturer</h1>
    <!-- All of the Node.js APIs are available in this renderer process. -->
    We are using node <script>document.write(process.versions.node)</script>,
    Chromium <script>document.write(process.versions.chrome)</script>,
    and Electron <script>document.write(process.versions.electron)</script>.
  </body>
  <video id="screenshot" src=""></video>
  <script>
    // You can also require other files to run in this process
    require('./renderer.js')
  </script>
</html>

```




![screen shot windows](images/capture-windows.png)


![screen shot mac](images/capture-mac.png)

More information can be found in the [desktopCapturer documentation](https://github.com/electron/electron/blob/master/docs/api/desktop-capturer.md)


## Requirements

   * `electron-dotnet` needs to be built.  The easiest way is to use the provided `make` files available in the WebSharp base directory.  
   
      * [See Getting Started on Windows](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md)
   
      * [See Getting Started on Mac](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md)

> :bulb: Windows users need to make sure [Mono is available](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md#setting-mono-path) in their %PATH%.

## Known Issues



## Release Notes



### 1.0.0

Initial release