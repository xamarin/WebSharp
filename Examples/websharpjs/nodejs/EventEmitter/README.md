# eventemitter README

Example NodeJS EventEmitter

This example demonstrates creating and using a NodeJS EventEmitter.

## Features

Creating an `EventEmitter`.

- Add a couple of listeners to the emitter using `AddListener`.
- Log the number of listeners attached using `ListenerCount`.
- `Emit` the listeners which will log messages when the callback is executed.
- Remove the listeners using `RemoveAllListeners`.
- Log the number of listeners attached using `ListenerCount`.
- Attach a listener that will be emitted once using `Once`.
- Prepend a listener that will be executed only once using `PrependOnceListener`
- `Emit` those listeners. 

## Requirements

   * `electron-dotnet` needs to be built.  The easiest way is to use the provided `make` files available in the WebSharp base directory.  
   
      * [See Getting Started on Windows](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md)
   
      * [See Getting Started on Mac](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md)

> :bulb: Windows users need to make sure [Mono is available](https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md#setting-mono-path) in their %PATH%.

## Known Issues


## Release Notes


### 1.0.0

Initial release

