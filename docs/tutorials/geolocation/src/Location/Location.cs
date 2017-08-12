using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.DOM;
using WebSharpJs.Script;

using GeoLocation;

#if !DEV
namespace Location
{
#endif
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

                await findMe.AttachEvent(HtmlEventNames.Click,
                    new EventHandler(
                        async (sender, evt) => {

                            error = false;

                            await location.SetProperty("innerText", "Locating ...");
                            var geo = await GeoLocationAPI.Instance();
                            await geo.GetCurrentPosition(
                                new ScriptObjectCallback<Position>(
                                    async (cr) =>
                                    {
                                        // Obtain our position from the CallbackState
                                        var position = cr.CallbackState as Position;
                                        // Reference the Coordinates class
                                        var coords = position.Coordinates;
                                        // Create our location information string
                                        var posString = $"Your current position is:\nLatitude : {coords.Latitude}\nLongitude : {coords.Longitude}\nMore or less {coords.Accuracy} Meters.";
                                        // Set the location status text
                                        await location.SetProperty("innerText", posString);
                                        // Retrieve the image of the location using the longitude and latitude
                                        // properties of the Coordinates class. 
                                        var imageURL = $"https://maps.googleapis.com/maps/api/staticmap?center={coords.Latitude},{coords.Longitude}&zoom=13&size=300x300&sensor=false";
                                        // Set the src property of the <image> tag
                                        await map.SetProperty("src", imageURL);
                                    }
                                ),
                                new ScriptObjectCallback<PositionError>(
                                    async (cr) =>
                                    {
                                        // Obtain the error from CallbackState
                                        var err = cr.CallbackState as PositionError;
                                        // Format a string with the error
                                        var errString = $"ERROR({err.ErrorCode}): {err.Message}";
                                        // If we already have an error then we will want to append
                                        // the error to the already existing error(s) that exist.
                                        if (error)
                                        {
                                            var errorText = await location.GetProperty<string>("innerText");
                                            errString = $"{errorText}\n{errString}"; 
                                        }
                                        error = true;
                                        // Set the status area text for feedback to the user
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
#if !DEV
}
#endif
