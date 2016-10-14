# PepperPlugin

WebSharp's implementation of the Native Client API's allows the developer to run DotNet code out-of-process as an embedded plugin within a webpage.

What follows are the instructions on how to compile WebSharp's plugin interface PepperPlugin. 

## Pre-requisites

- [mono embedding](http://www.mono-project.com/docs/advanced/embedding/) is being used so an installation of [mono](http://www.mono-project.com/download/) needs to be installed. (Note: Mono 32 bit or Mono 64 bit)
- [Native Client SDK](https://developer.chrome.com/native-client) Native Client is a sandbox for running compiled C and C++ code in the browser efficiently and securely, independent of the user’s operating system.
- Windows requires at least a Visual Studio 2015 Community Edition.

## Compiling PepperPlugin

To compile the PepperPlugin solution the [Native Client SDK Download](https://developer.chrome.com/native-client/sdk/download) will need to be downloaded and installed.  Basically it is the following:

- Download the [Native Client SDK](https://developer.chrome.com/native-client/sdk/download)
- Unzipping the downloaded .zip file into the correct directory structure.  The PepperPlugin project uses the project sources to compile and link against.
- Installing the correct sdk bundle
- Building the PepperPlugin 
 
### _Downloading and Installing the Native Client SDK_

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

### _Installing the correct sdk bundle_

To get the actual source files you will need to install the ```pepper-canary``` bundle.  Open a command prompt and change into the nacl_sdk directory that was just created and execute the following code.

```bash
> cd nacl_sdk
> naclsdk update pepper_canary
```

The previous command will install the source files that the PepperPlugin solution will use to build the necessary assemblies.

#### _Setup via make (Mac) and nmake (Windows)_

There are also provided make files for setting up the Native Client SDK that you can try out but the above is the detailed way if this does not work for some reason.

Windows Makefile.win will need to be run via a Visual Studio 2015 Native Tools Command Prompt and requires at least PowerShell version 5.

```bash
# Make sure you are in the main WebSharp directory in a Visual Studio 2015 Native Tools Command Prompt
cd WebSharp
WebSharp> nmake -f Makefile.win setup
```

Mac Makefile can be executed from a Mac terminal

```bash
# Make sure you are in the main WebSharp directory in a Mac terminal
cd WebSharp
WebSharp$ make setup
```

### _Building PepperPlugin_

#### *Windows*

Make sure you have the [Native Client SDK](https://developer.chrome.com/native-client) installed as explained above.

You should now be able to compile the solution provided.  Open the ```PepperPlugin.sln``` file found in the ```WebSharp\PepperPlugin\src\``` directory and select Build for the configuration and platform.

The solution can also be built from the command line using a Visual Studio 2015 Native Command Prompt.

To build and target 64 bit version of the PepperPlugin which will only run with Electron 64 platforms.
```shell
> cd WebSharp\PepperPlugin\src
WebSharp\PepperPlugin\src> msbuild PepperPlugin.sln /t:build /p:Platform=x64 /p:Configuration=Release
```

This can also be run via the Makefile.win.

```bash
# Make sure you are in the main WebSharp directory in a Visual Studio 2015 Native Tools Command Prompt
cd WebSharp
WebSharp> nmake -f Makefile.win buildx64
```


To build and target 32 bit version of the PepperPlugin which will only run with Electron 32 platforms.
```shell
> cd WebSharp\PepperPlugin\src
WebSharp\PepperPlugin\src> msbuild PepperPlugin.sln /t:build /p:Platform=x86 /p:Configuration=Release
```

This can also be run via the Makefile.win.

```bash
# Make sure you are in the main WebSharp directory in a Visual Studio 2015 Native Tools Command Prompt
cd WebSharp
WebSharp> nmake -f Makefile.win buildx86
```

The PepperPlugin.sln solution is setup to build the PepperPlugin.dll, PepperSharp.dll and the examples that can be found in the Examples directory. 

#### *Mac*

You will need to have the [Native Client SDK](https://developer.chrome.com/native-client) as mentioned above.

A makefile is provided in the ```Websharp/PepperPlugin/src``` directory.

```shell
$ cd WebSharp/PepperPlugin/src
$ export NACL_SDK_ROOT=../../../nacl_sdk/pepper_canary/
$ make
```

This can also be run via the Makefile

```bash
# Make sure you are in the main WebSharp directory in a Mac terminal
cd WebSharp
# This will also export the NACL_SDK_ROOT correctly so no need to set it.
WebSharp$ make build
```

The makefile will try to use pkg-config for Compiling and Linking first

If you have another ```mono``` installation that you would like to use instead of the default that is installed you can also set ```MONO_ROOT```.

```shell
WebSharp/PepperPlugin/src$ export MONO_ROOT=path.to.mono
```

The makefile tries to use ```pkg-config``` for Compiling and Linking first if MONO_ROOT is not set.  You can verify if the pkg-config is setup correctly by doing the following:

```shell
WebSharp/PepperPlugin/src$ pkg-config --libs mono-2
```

If the mono-2 package is not found then the message below may be shown:

```
Package mono-2 was not found in the pkg-config search path.
Perhaps you should add the directory containing `mono-2.pc'
to the PKG_CONFIG_PATH environment variable
No package 'mono-2' found
```

You may need to specify the search path explicitly. This may be installation dependent, but in most cases will look like:

```shell
WebSharp/PepperPlugin/src$ export PKG_CONFIG_PATH=/Library/Frameworks/Mono.framework/Versions/Current/lib/pkgconfig
```

### Mono and Windows Path

Mono has to be available in the windows %PATH% for everything to run correctly.

For _development purposes_ you can can add a ```Pre-Build Event``` to the PepperPlugin project that will copy the ```mono-2.0.dll``` to the output directory instead of having to set the %PATH% all the time. 

  * x86
    ```shell
    xcopy /y /d  "C:\Program Files %28x86%29\Mono\bin\mono-2.0.dll" $(OutDir)
    ```

  * x64
    ```shell
    xcopy /y /d  "C:\Program Files\Mono\bin\mono-2.0.dll" $(OutDir)   
    ```

For more information on setting the windows path see [Setting Mono path on Windows](https://github.com/xamarin/WebSharp/tree/master/electron-dotnet#setting-mono-path).

### PepperSharp - C# PPAPI binding assembly

To build only the PepperSharp.dll assembly

```shell
> cd WebSharp\PepperSharp
WebSharp\PepperSharp> msbuild PepperSharp.csproj /t:Rebuild /p:Configuration=Release|Debug /p:Platform=AnyCPU
```

Examples
---

Examples can be found here:  https://github.com/xamarin/WebSharp/tree/master/Examples/api

To run the examples you will first need to install electron 1.2.x.  See the [GettingStarted](../GettingStarted) document for installing electron.

Once electron is installed you can change into any of the directories and execute:

```WebSharp\Examples\api\WebSocket>electron .```
