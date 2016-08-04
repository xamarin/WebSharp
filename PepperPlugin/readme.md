PepperPlugin
===

Pre-requisites
---

- [mono embedding](http://www.mono-project.com/docs/advanced/embedding/) is being used so an installation of [mono](http://www.mono-project.com/download/) needs to be installed. (Note: Mono 32 bit or Mono 64 bit)
- nacl_sdk

To compile the PepperPlugin solution the [nacl sdk](https://developer.chrome.com/native-client/sdk/download) is required.

You can find platform specific install instructions here: https://developer.chrome.com/native-client/sdk/download#installing-the-sdk.  
The install should be in the same directory level as the WebSharp checkout but not in it.

```
.
+-- nacl_sdk
            +-- naclsdk (and naclsdk.bat for Windows)
            +-- sdk_cache
            +-- sdk_tools
+-- WebSharp
```

To get the actual source files you will need to install the ```pepper-canary``` bundle.  Open a command prompt and change into the nacl_sdk directory that was just created and execute the following code.

```shell
> cd nacl_sdk
> naclsdk update pepper-canary
```

The previous command will install the source files that the PepperPlugin solution will use to build the necessary assemblies.

PepperPlugin Solution
---

You should now be able to compile the solution provided.  Open the ```PepperPlugin.sln``` file found in the ```WebSharp\PepperPlugin\src\``` directory and select Build for the configuration and platform.

You can also use the command line from a Visual Studio 2015 Native Command Prompt.

To build and target 64 bit version of the PepperPlugin which will only run with Electron 64 platforms.
```shell
> cd WebSharp\PepperPlugin\src
WebSharp\PepperPlugin\src> msbuild PepperPlugin.sln /t:build /p:Platform=x64 /p:Configuration=Release
```

To build and target 32 bit version of the PepperPlugin which will only run with Electron 32 platforms.
```shell
> cd WebSharp\PepperPlugin\src
WebSharp\PepperPlugin\src> msbuild PepperPlugin.sln /t:build /p:Platform=x86 /p:Configuration=Release
```

The solution is setup to build the PepperPlugin.dll, PepperSharp.dll and the examples that can be found in the Examples directory. 

PepperSharp - C# PPAPI binding assembly
---
To build only the PepperSharp.dll assembly

```shell
> cd WebSharp\PepperSharp
WebSharp\PepperSharp> msbuild PepperSharp.csproj /t:Rebuild /p:Configuration=Release /p:Platform=AnyCPU
```

Examples
---

Examples can be found here:  https://github.com/xamarin/WebSharp/tree/master/Examples/api

To run the examples you will first need to install electron 1.2.x.  See the [GettingStarted](../GettingStarted) document for installing electron.

Once electron is installed you can change into any of the directories and execute:

```WebSharp\Examples\api\WebSocket>electron .```
