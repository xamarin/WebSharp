# Geolocation API Specification

In [Part 1](./README.ME) we created our project, setup the very simple UI and set up DOM interaction in the `Location.cs` renderer part of the application.

In [Part 2](./GeolocationAPI.md) we looked at:

* High-level view of the Bridge interface that is used by `WebSharp`.
* Adding a new source module to the project.
* Incorporating the new source module in our application.
* Creating the basic singleton code to reference the `navigator.geolocation` object.

In `Part 3` we will be looking at using the documentation from the [Geolocation API Specification](https://dev.w3.org/geo/api/spec-source.html#navi-geo) to focus on the implementation details of the `GeoLocationAPI` module. Specifically modeling the data structures found in the API documentation as well as the callback definitions that should be implemented.


## Marshalling data

The `WebSharp` native bridge interface uses objects that have properties that can be enumerated.  It can marshal any JSON-serializable value between `.NET` and `Node.js` (although JSON serialization is not used in the process). 

> :bulb: The bridge also supports marshalling between `Node.js` `Buffer` instance and a CLR `byte[]` array to help you efficiently pass binary data.

For example:

```csharp

    public class Person
    {
        public string firstname { get; private set; }
        public string lastname { get; private set; }
        public double age { get; private set; }
        
    }

```    

The above class will be accessible to JavaScript as:

```javascript

{ 
    firstname: "Foo",
    lastname: "Able",
    age: 34.6,
}

```

To help with the mapping of data structures between managed and JavaScript objects we can use two attributes that are defined in the `WebSharpJs.Scripting` namespace.

| Attribute | Description |
| --- | --- |
| [ScriptableType] | A class or structure attribute used by the bridge interface to identify that the object will have [ScriptableMember] attributes.  |
| [ScriptableMember(ScriptAlias = "xxxxxx")] | a member attribute used by the bridge to map the strongly typed managed code names to the JavaScript equivalent. |

The data passed back and forth by the bridge are basically `IDictionary<string, object>` implementations. By using the attributes introduced above we can map these values to their equivalents on the JavaScript side.

For example:

```csharp

    [ScriptableType]
    public class Person
    {
        [ScriptableMember(ScriptAlias = "firstname")]
        public string FirstName { get; private set; }
        [ScriptableMember(ScriptAlias = "lastname")]
        public string LastName { get; private set; }
        [ScriptableMember(ScriptAlias = "age")]
        public double Age { get; private set; }
        
    }

```

The example above shows the same class `Person` as mentioned above but with the attributes applied.

So in our managed code, we can do the following:

```csharp

   var person = new Person();
   person.FirstName = "Foo";
   person.LastName = "Able";
   person.Age = 34.6;

```

The bridge then knows by the usage of the attributes how to map the data to the JavaScript interface and vice-a-versa working round trip.

Now that we have touched on the subject of how data is passed back and forth let's look at the API that we will be defining.

## API Description

In the [previous section](./GeolocationAPI.md) we created a singleton code base modeling the `navigator.geolocation` that allows access to the JavaScript implementation.  What we did not look at is the actual information passed back and forth between the JavaScript object and our managed code.

The next sections come from [Geolocation API Specification](https://dev.w3.org/geo/api/spec-source.html#navi-geo) and will borrow heavily from text obtained from there.

### Geolocation interface

The `Geolocation` object is used by scripts to programmatically determine the location information associated with the hosting device. The location information is acquired by applying a user-agent specific algorithm, creating a `Position` object, and populating that object with appropriate data accordingly.

From the documentation link:

```idl

 [NoInterfaceObject]
 interface Geolocation { 
   void getCurrentPosition(PositionCallback successCallback,
                           optional PositionErrorCallback errorCallback,
                           optional PositionOptions options);

   long watchPosition(PositionCallback successCallback,
                      optional PositionErrorCallback errorCallback,
                      optional PositionOptions options);

   void clearWatch(long watchId);
 };

 callback PositionCallback = void (Position position);

 callback PositionErrorCallback = void (PositionError positionError);

```

We are really only interested in the `getCurrentPosition()` method in our interface and the other two methods `watchPosition()` and `clearWatch()` will be left as an extra exercise for those that need them.

```idl

  void getCurrentPosition(PositionCallback successCallback,
                           optional PositionErrorCallback errorCallback,
                           optional PositionOptions options);

```

The `getCurrentPosition()` method takes two callbacks, one required and on optional, and an optional `PositionOptions` data structure.

We will use the `ScriptObjectCallback` class to define the two call backs `successCallback` and `errorCallback`.

```csharp

ScriptObjectCallback<Position> successCallback;

```

and the optional `errorCallback` parameter:

```csharp

ScriptObjectCallback<PositionError> errorCallback = null;

```

which leaves us with the optional `PositionOptions`.  

The full definition can be found below.

```csharp

        public async Task GetCurrentPosition(ScriptObjectCallback<Position> successCallback,
              ScriptObjectCallback<PositionError> errorCallback = null,
              PositionOptions options = null      )
        {
            await Invoke<object>("getCurrentPosition", successCallback, errorCallback, options);
        }


``` 

> :bulb: Note that we call `Invoke` with the optional parameters as well which default to `null`.  This depends on the JavaScript interface you are interfacing with.  Other interfaces may not allow being called with `null` as an optional parameter meaning you may have to wrap your managed code accordingly.

Now let's move on to the data structures used to marshal the data back and forth.

### PositionOptions interface

The `getCurrentPosition()` method accepts an optional  `PositionOptions` objects as their third argument.

In ECMAScript, PositionOptions objects are represented using regular native objects with optional properties named enableHighAccuracy, timeout and maximumAge.

```idl

dictionary PositionOptions {
    boolean enableHighAccuracy = false;
    [Clamp] unsigned long timeout = 0xFFFFFFFF;
    [Clamp] unsigned long maximumAge = 0;
  };

```

For our managed code interface we can model the options `dictionary` object as follows:

```cs

    [ScriptableType]
    public class PositionOptions {

        [ScriptableMember(ScriptAlias = "enableHighAccuracy")]
        public bool EnableHighAccuracy { get; set; }
        [ScriptableMember(ScriptAlias = "timeout")]
        public ulong Timeout { get; set; } = 0xFFFFFFFF;
        [ScriptableMember(ScriptAlias = "maximumAge")]
        public ulong MaximumAge { get; set; } = 0;
    }

```

As mentioned above the underlying data structure that the bridge used for marshaling is essentially an `IDictionary<string, object>`.

### Position interface

The `Position` interface is the container for the geolocation information returned by this API. 


```idl

  [NoInterfaceObject]
  interface Position {
    readonly attribute Coordinates coords;
    readonly attribute DOMTimeStamp timestamp;
  };

```

The above IDL definition can be defined as follows:

```csharp

    [ScriptableType]
    public class Position
    {
        [ScriptableMember(ScriptAlias = "coords")]
        public Coordinates Coordinates { get; private set; }
        [ScriptableMember(ScriptAlias = "timestamp")]
        public double Timestamp { get; private set; }
    }

```


### Coordinates interface

The geographic coordinate reference.  More information about each field can be found in the documentation link.

```idl

  [NoInterfaceObject]
  interface Coordinates {
    readonly attribute double latitude;
    readonly attribute double longitude;
    readonly attribute double? altitude;
    readonly attribute double accuracy;
    readonly attribute double? altitudeAccuracy;
    readonly attribute double? heading;
    readonly attribute double? speed;
  };

```

The above IDL definition can be defined as follows:

```csharp

    [ScriptableType]
    public class Coordinates
    {
        [ScriptableMember(ScriptAlias = "latitude")]
        public double Latitude { get; private set; }

        [ScriptableMember(ScriptAlias = "longitude")]
        public double Longitude { get; private set; }
        
        [ScriptableMember(ScriptAlias = "altitude")]
        public double Altitude { get; private set; }
        
        [ScriptableMember(ScriptAlias = "accuracy")]
        public double Accuracy { get; private set; }
        
        [ScriptableMember(ScriptAlias = "altitudeAccuracy")]
        public double AltitudeAccuracy { get; private set; }
        
        [ScriptableMember(ScriptAlias = "heading")]
        public double Heading { get; private set; }
        
        [ScriptableMember(ScriptAlias = "speed")]
        public double Speed { get; private set; }
    }

```

### PositionError interface

```idl

  [NoInterfaceObject]
  interface PositionError {
    const unsigned short PERMISSION_DENIED = 1;
    const unsigned short POSITION_UNAVAILABLE = 2;
    const unsigned short TIMEOUT = 3;
    readonly attribute unsigned short code;
    readonly attribute DOMString message;
  };
  
```

The above IDL definition can be defined as follows:

```csharp

    [ScriptableType]
    public class PositionError {

        [ScriptableMember(ScriptAlias = "code")]
        public int Code { get; private set; }
        
        public PositionErrorCode ErrorCode 
        { 
            get 
            {
                return (PositionErrorCode)Code;
            }
        }

        [ScriptableMember(ScriptAlias = "message")]
        public string Message { get; private set; }
    }

    public enum PositionErrorCode
    {
        PermissionDenied = 1,
        PositionUnAvailable = 2,
        TimeOut = 3
    }

```

In the above code we also mapped an `enum PositionErrorCode` to the values:

```idl

    const unsigned short PERMISSION_DENIED = 1;
    const unsigned short POSITION_UNAVAILABLE = 2;
    const unsigned short TIMEOUT = 3;

```

As well as a property accessor `ErrorCode`.

```cs

        public PositionErrorCode ErrorCode 
        { 
            get 
            {
                return (PositionErrorCode)Code;
            }
        }

```

To view the full code implementation take a look at the [GeoLocationAPI.cs](./src/Location/GeoLocationAPI.cs) source.

## Using GeoLocationAPI

Everything should be in place to incorporate it in our source code.

Open `Location.cs` source file.  Find from our previous tutorial where we obtained an instance of our `GeoLocation` class and let's start incorporating the code to call out to `GetCurrentPosition` we just finished implementing.

```csharp

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


```

First, let's provide a call back to receive our position information when the `GetCurrentPosition` method completes successfully.

```csharp

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

```

The `ScriptObjectCallback` and `ScriptObjectCallback<T>` instances reference a method to be called when an asynchronous operation completes.

The result of asynchronous callback operation `  ` takes an `ICallbackResult` parameter, which is subsequently used to obtain the results of the asynchronous operation.

In the code above we attached a lambda method as the callback method but we could have just as easily used a defined method as the method to be called as follows:

```csharp

    async Task ProcessPositionInformation(ICallbackResult result)
    {
        // method implementation here
        
    }

```

And used it as follows:

```csharp

    var successCallback = new ScriptObjectCallback<Position>(ProcessPositionInformation);

```

We do the same for errors if the `GetCurrentPosition` method does not complete successfully.

```csharp

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

```

Now when the application is run we should have some errors displayed to the screen.

![map](./images/error.png)

## Providing the Google API Key

At the beginning of the tutorials, a list of requirements was presented with one of them being a [Google API Key](./geolocation#api-keys).  If you have not done that now would be a good time so we can get this working.

To provide the key we will need to set the [GOOGLE_API_KEY](https://github.com/electron/electron/blob/master/docs/api/environment-variables.md#google_api_key) environment variable before the application is run.

In the `main.js` file you can set it there with:

```javascript

process.env.GOOGLE_API_KEY = 'YOUR_KEY_HERE';

```

The we chose to do this is by setting it as a parameter the [package.json](./package.json#L14) file and referencing that variable in our [main.js](./main.js#L5-L7) file.

```javascript

// Read and set our Google API Key from our package file.
var package = require('./package.json');
process.env.GOOGLE_API_KEY = package.GOOGLE_API_KEY;

```

Now if you run the application and the API key provided is correct we should be able to see a small map of our current position.

![map](./images/map.png)

## Summary

:sweat_drops: Phew, that was long but continuing from the [previous tutorial](./GeolocationAPI.md) we should have successfully incorporated the code in our application.

We also touched on how the bridge interface marshals data to and from managed code to JavaScript and vice-a-versa.

Then we defined our interface based on the IDL definitions from the [Geolocation API Specification](https://dev.w3.org/geo/api/spec-source.html#navi-geo) documentation.

When incorporating the code we briefly went over the `ScriptObjectCallback`, defining the callback method and obtaining results from the `CallbackState`.  

Worth mentioning is that in the examples here there was only one parameter that was returned in the `CallbackState` but if there are multiple parameters returned to the callback method then they can be accessed as an `object[]` array.


In the [next part](./GeolocationAPI_assemblies.md) we will look at creating assemblies from our managed code as well as one way that the `development` and `production` execution environment can be managed.

> **Warning:**  There are some objects that have their values implemented directly on the object itself like the geolocation objects Position and Coordinates.  That means they can not be persisted with the likes of the javascript JSON.stringify().  The bridge interface works on the same principle of being able to enumerate those properties.  If no properties exist what we attempt to do is flatten that object by iterating through the object keys that creates a proxy object with properties so that we can pass it back through the bridge interface.  This may change in the future and require a specific interface to tell the bridge that the object needs to be flattened.




> :bulb: The `navigator.geolocation` can only be referenced in the `Renderer` process.  If you try referencing from the Main process you will get an error about the `navigator` object not being defined.

For more information see the following:

1. Getting started documents

    * [See Getting Started on Windows](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md)
   
    * [See Getting Started on Mac](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md)

1. [Create a new `WebSharp Electron Application`](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-websharp-electron-application.md#generate-a-websharp-electron-application)

1. [DOM Overview](https://github.com/xamarin/WebSharp/blob/master/docs/tutorials/DOM/overview.md)

1. [Google API Keys](https://developers.google.com/console)

1. [Electron Environment Variables](https://github.com/electron/electron/blob/master/docs/api/environment-variables.md#environment-variables)














