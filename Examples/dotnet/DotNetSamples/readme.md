# Example DotNetSamples

The DotNetSamples sample demonstrates calling .net code in process from an electron application.  An overview can be found in the [electron-dotnet module overview](https://github.com/xamarin/WebSharp/tree/master/electron-dotnet)

Foundations:

- 101_hello_lambda.js - prescriptive interface pattern
- 102_hello_function.js - multiline function comment
- 103_hello_file.js - separate file
- 104_add7_class.js - entire class instead of lambda
- 105_add7_dll.js - pre-compiled DLL
- 106_marshal_v82cls.js - data from V8 to CLR
- 107_marshal_clr2v8.js - data from CLR to V8
- 108_func.js - marshal func from v8 to CLR and call node from .NET
- 109_sync.js - async and sync calling conventions
- 110_clr_instance.js - calling clr instance
- 111_clr_listener.js - Yeilding control to the clr and regaining the control back.


## Pre-requisites

- [Node.js](https://nodejs.org/en/download/) (which comes with [npm](http://npmjs.com)) installed on your computer.
- [Native Abstractions for Node.js](https://github.com/nodejs/nan) if there are errors during install pertaining to nan you may need to install this as well ```npm install nan``` 
- [PepperPlugin](https://github.com/xamarin/WebSharp/tree/master/PepperPlugin) and its pre-requisites.

## Install

```bash
# Go into the repository
cd WebSharp\Examples\dotnet\DotNetSamples
# Install the electron requirements
npm install
# Install the electron-dotnet module
npm install ../../../electron-dotnet
```

*Note: You will also need to build one of the .dll's used in the samples.*

### Mac
```bash
# Compile the sample dll
mcs -sdk:4.5 ./samples/Sample105.cs -target:library
```

## To Run

From the DotNetSamples directory.

```bash
# Start the electron sample
npm start
```

