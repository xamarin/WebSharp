# WebSharp

This project is intended to support running .NET code as an Electron Plugin, running code
using the Mono runtime, side-by-side the V8 engine and to provide both C# API bindings to
Electron's plugin API as well as the Web browser API.

This will be made up of a few components:

* The plugin that launches the Mono process and runs code on behalf of the user.
* The `Xamarin.PepperSharp.dll` API bindings that will provide access to the APIs
  surfaced to Chromium plugins to C# code.
* `Xamarin.W3C.dll` API bindings that will provide access to the browser APIs, 
  built on top of `Xamarin.PepperSharp.dll`

For the `Xamarin.W3C` APIs, we intend to use the TypeScript type definitions to generate
a strongly typed .NET API.

# Future Work

We hope that the `Xamarin.W3C` API would be used in the future when we compile .NET code
to [WebAssembly](https://webassembly.github.io/).
