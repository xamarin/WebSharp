using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Electron;
using WebSharpJs.Script;

//namespace Capture
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
            
            var desktopCapturer = await DesktopCapturer.Instance();

            // GetSources - Starts gathering information about all available desktop media sources, and calls 
            // ScriptObjectCallback<Error, DesktopCapturerSource[]> when finished.
            await desktopCapturer.GetSources(new DesktopCapturerOptions() { Types = DesktopCapturerType.Window | DesktopCapturerType.Screen },
                new ScriptObjectCallback<Error, DesktopCapturerSource[]>(
                     async (result) =>
                     {
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

             
                     }));
                
                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
//}
