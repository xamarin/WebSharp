using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.DOM;
using WebSharpJs.Script;

using GeoLocation;

//namespace Location
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
                var findMe = await document.GetElementById("findMe");
                var map = await document.GetElementById("map");
                var location = await document.GetElementById("location");
                var error = false;

                await findMe.AttachEvent("click",
                    new EventHandler(
                        async (sender, evt) => {

                            error = false;

                            await location.SetProperty("innerText", "Locating ...");
                            var geo = await GeoLocationAPI.Instance();
                            await geo.GetCurrentPosition(
                                new ScriptObjectCallback<GeoPosition>(
                                    async (cr) =>
                                    {
                                        var position = cr.CallbackState as GeoPosition;
                                        var coords = position.Coordinates;
                                        var posString = $"Your current position is:\nLatitude : {coords.Latitude}\nLongitude : {coords.Longitude}\nMore or less {coords.Accuracy} Meters.";
                                        await location.SetProperty("innerText", posString);
                                        var imageURL = $"https://maps.googleapis.com/maps/api/staticmap?center={coords.Latitude},{coords.Longitude}&zoom=13&size=300x300&sensor=false";
                                        await map.SetProperty("src", imageURL);
                                    }
                                ),
                                new ScriptObjectCallback<PositionError>(
                                    async (cr) =>
                                    {

                                        var err = cr.CallbackState as PositionError;
                                        var errString = $"ERROR({err.ErrorCode}): {err.Message}";
                                        if (error)
                                        {
                                            var errorText = await location.GetProperty<string>("innerText");
                                            errString = $"{errorText}\n{errString}"; 
                                        }
                                        error = true;
                                        await location.SetProperty("innerText", errString);
                                    }
                                ),
                                new PositionOptions() { EnableHighAccuracy = true, Timeout = 5000, MaximumAge = 0 }
                            );

                        }
                    )
                );

                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
//}
