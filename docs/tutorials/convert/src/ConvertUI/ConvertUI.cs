using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

using WebSharpJs.NodeJS;
using WebSharpJs.DOM;
using WebSharpJs.DOM.Events;
using WebSharpJs.Script;
using WebSharpJs.Electron;
using System.Collections.Generic;

#if !DEV
namespace ConvertUI
{
#endif    
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

        HtmlElement btnSubmit;
        HtmlElement btnSource;
        HtmlElement btnDestination;
        HtmlElement source;
        HtmlElement destination;
        HtmlElement name;

        HtmlDocument document;

        IpcRenderer ipcRenderer;

        static readonly WebSharpJs.DOM.Events.Event changeEvent = new WebSharpJs.DOM.Events.Event(HtmlEventNames.Change);

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

                ipcRenderer = await IpcRenderer.Create();

                document = await HtmlPage.GetDocument();

                var form = await document.QuerySelector("form");

                // Input fields
                source = await form.QuerySelector("input[name=\"source\"]");
                destination = await form.QuerySelector("input[name=\"destination\"]");
                name = await form.QuerySelector("input[name=\"name\"]");

                // Buttons
                btnSource = await document.GetElementById("chooseSource");
                btnDestination = await document.GetElementById("chooseDestination");
                
                btnSubmit = await form.QuerySelector("button[type=\"submit\"]");

                // Handle input fields
                await source.AttachEvent(HtmlEventNames.Input, updateUI);
                await source.AttachEvent(HtmlEventNames.Change, updateUI);

                await destination.AttachEvent(HtmlEventNames.Input, updateUI);
                await destination.AttachEvent(HtmlEventNames.Change, updateUI);

                await name.AttachEvent(HtmlEventNames.Input, updateUI);

                // Handle buttons
                await btnSource.AttachEvent(HtmlEventNames.Click, handleSource);
                await btnDestination.AttachEvent(HtmlEventNames.Click, handleDestination);

                // Handle form Submit
                await form.AttachEvent(HtmlEventNames.Submit, 
                    async (s,e) => {
                        e.PreventDefault();

                        await ToggleUI(true);

                        // Send off the convert request
                        await ipcRenderer.Send("submitform",
                            await source.GetProperty<string>("value"),
                            await destination.GetProperty<string>("value"),
                            await name.GetProperty<string>("value")
                        );
                    }
                );

                await ipcRenderer.AddListener("gotMetaData",
                    new IpcRendererEventListener(
                        async (result) =>
                        {
                            var state = result.CallbackState as object[];
                            var ipcMainEvent = (IpcRendererEvent)state[0];
                            var parms = state[1] as object[];

                            var audio = ScriptObjectHelper.AnonymousObjectToScriptableType<Audio>(parms[0]);
                            var video = ScriptObjectHelper.AnonymousObjectToScriptableType<Video>(parms[1]);
                            var duration = TimeSpan.Parse(parms[2].ToString());

                            await updateInfo(audio, video, duration);
                        }

                    )

                );

                await ipcRenderer.AddListener("completed",
                    new IpcRendererEventListener(
                        async (result) =>
                        {
                            await ToggleUI(false);
                        }

                    )

                );

                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;
        }

        async void handleSource (object sender, EventArgs args)
        {
            var dialog = await Dialog.Instance();

            // Create an OpenDialogOptions reference with custom FileFilters
            var openOptions = new OpenDialogOptions()
            {
                Filters = new FileFilter[]
                {
                    //new FileFilter() { Name = "All Files", Extensions = new string[] {"*"} },
                    new FileFilter() { Name = "Movies", Extensions = new string[] {"mkv", "avi"}},
                    
                }
                
            };

            // Now show the open dialog passing in the options created previously
            // and a call back function when a file is selected.
            // If a call back function is not specified then the ShowOpenDialog function
            // will return an array of the selected file(s) or an empty array if no
            // files are selected.
            var sourceFile = await dialog.ShowOpenDialog(await BrowserWindow.GetFocusedWindow(),
                openOptions
            );

            if (sourceFile != null && sourceFile.Length > 0)
            {
                await source.SetProperty("value", sourceFile[0]);
                await name.SetProperty("value", Path.GetFileNameWithoutExtension(sourceFile[0]) + ".m4v");
                await source.DispatchEvent(changeEvent);

                await ipcRenderer.Send("getMetaData", sourceFile[0]);
            }

        }

        async void handleDestination (object sender, EventArgs args)
        {
            var dialog = await Dialog.Instance();

            // Create an OpenDialogOptions reference with custom Properties
            var openOptions = new OpenDialogOptions()
            {
                PropertyFlags = OpenDialogProperties.OpenDirectory | OpenDialogProperties.CreateDirectory                
            };

            // Now show the open dialog passing in the options created previously
            // and a call back function when a file is selected.
            // If a call back function is not specified then the ShowOpenDialog function
            // will return an array of the selected file(s) or an empty array if no
            // files are selected.
            var destDirectory = await dialog.ShowOpenDialog(await BrowserWindow.GetFocusedWindow(),
                openOptions
            );
            if (destDirectory != null && destDirectory.Length > 0)
            {
                await destination.SetProperty("value", destDirectory[0]);
                await destination.DispatchEvent(changeEvent);
            }

        }
        
        async Task ToggleUI(bool protect)
        {
            if (protect)
            {
                await btnSource.SetAttribute("disabled", "");
                await btnDestination.SetAttribute("disabled", "");
                await destination.SetAttribute("readonly", "");
                await name.SetAttribute("readonly", "");
                // update our submit button to loading and disabled
                await btnSubmit.SetProperty("innerHTML", "<i class='fa fa-circle-o-notch fa-spin'></i> Converting ...");
                await btnSubmit.SetAttribute("disabled", "");

            }
            else
            {
                await btnSource.RemoveAttribute("disabled");
                await btnDestination.RemoveAttribute("disabled");
                await destination.RemoveAttribute("readonly");
                await name.RemoveAttribute("readonly");

                // update our submit button to loading and disabled
                await btnSubmit.SetProperty("innerText", "Convert");
                await btnSubmit.RemoveAttribute("disabled");

            }
        }

        async void updateUI (object sender, EventArgs args)
        {

            var srcValue = (await source.GetProperty<string>("value")).Trim();
            var destValue = (await destination.GetProperty<string>("value")).Trim();
            var nameValue = (await name.GetProperty<string>("value")).Trim();

            if (!string.IsNullOrEmpty(srcValue) 
                && !string.IsNullOrEmpty(destValue)
                && !string.IsNullOrEmpty(nameValue))
            {
                await btnSubmit.RemoveAttribute("disabled");
                await removeClass(btnSubmit, "btn-outline-primary");
                await addClass(btnSubmit, "btn-primary");
            }
            else
            {
                await btnSubmit.SetAttribute("disabled", "");
                await removeClass(btnSubmit, "btn-primary");
                await addClass(btnSubmit, "btn-outline-primary");
            }
                
        }

        async Task updateInfo(Audio audio, Video video, TimeSpan duration)
        {
            var durInfo = await document.GetElementById("duration");
            await durInfo.SetProperty("innerText", duration.ToString());
            var videoFormat = await document.GetElementById("videoFormat");
            var vidString = $"<b>Format:</b> {video.Format}<br/><b>Fps:</b> {video.Fps}";
            await videoFormat.SetProperty("innerHTML", vidString);
            var audioFormat = await document.GetElementById("audioFormat");
            var audString = $"<b>Format:</b>{audio.Format}<br/><b>BitRate:</b> {audio.BitRateKbs}";
            await audioFormat.SetProperty("innerHTML", audString);
        }


        # region CSS Class minipulation
        static readonly char[] rnothtmlwhite = new char[] {' ','\r','\n','\t','\f' };
        bool IsHasClass(string elementClass, string className)
        {
            if (string.IsNullOrEmpty(elementClass) || string.IsNullOrEmpty(className))
                return false;
            return elementClass.Split(rnothtmlwhite, StringSplitOptions.RemoveEmptyEntries).Contains(className);
        }

        async Task<string> addClass(HtmlElement element, string klass, string elementClass = null)
        {
            if (string.IsNullOrEmpty(elementClass))
            {
                elementClass = await element.GetCssClass();
            }

            if (!IsHasClass(elementClass, klass))
            {
                elementClass += $" {klass} ";
                await element.SetCssClass(elementClass);
            }
            return elementClass;
        }
        async Task<bool> removeClass(HtmlElement element, string klass, string elementClass = null)
        {
            if (string.IsNullOrEmpty(elementClass))
            {
                elementClass = await element.GetCssClass();
            }

            if (IsHasClass(elementClass, klass))
            {
                var classList = elementClass.Split(rnothtmlwhite, StringSplitOptions.RemoveEmptyEntries).ToList();
                classList.Remove(klass);
                await element.SetCssClass(string.Join(" ",classList));
                return true;
            }
            return false;
        }
        #endregion

        
    }

    [ScriptableType]
    public class Video
    {
        public string Format { get; internal set; }
        public string ColorModel { get; internal set; }
        public string FrameSize { get; internal set; }
        public int? BitRateKbs { get; internal set; }
        public double Fps { get; internal set; }
    }

    [ScriptableType]
    public class Audio
    {

        public string Format { get; internal set; }
        public string SampleRate { get; internal set; }
        public string ChannelOutput { get; internal set; }
        public int BitRateKbs { get; internal set; }
    }
#if !DEV    
}
#endif
