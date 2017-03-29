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
* `System.Windows.Browser` this API surfaces the DOM tree to C# as a strongly
  typed set of objects.  An MIT licensed implementation of this is part of 
  [Moonlight](https://github.com/mono/moon/tree/master/class/System.Windows.Browser) that could be refactored.

For the `Xamarin.W3C` APIs, we intend to use the TypeScript type definitions to generate
a strongly typed .NET API.

# Current Work

This is a work in progress, and we are only getting started.   This means that the activation
workflow for .NET code is not finalized, nor is the API that is exposed to .NET code running
on Electron.

With that being said, check the [GettingStarted](./docs/getting-started) documents for the basics.

# Future Work

We hope that the `Xamarin.W3C` API would be used in the future when we compile .NET code
to [WebAssembly](https://webassembly.github.io/).
