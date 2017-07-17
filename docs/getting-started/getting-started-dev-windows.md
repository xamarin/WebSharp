# Getting Started on Windows

The goal of this doc is to get you setup with a flexible workflow using `WebSharp` on Windows.  

Developers may be coming from different backgrounds and may not be familiar with the different technologies, tools or commands that are involved using `WebSharp`.  Maybe this is your first time using `Electron` and `Node.js` so we will try to make it as painless as possible.

We will not be discussing installing any of the `Requirements` here only getting a `WebSharp` development environment ready to go and referencing `WebSharp` in your projects.  There are plenty of guides available for installing the `Requirements`.

## Content

&nbsp;&nbsp;&nbsp;&nbsp;[Requirements](#requirements)  
&nbsp;&nbsp;&nbsp;&nbsp;[WebSharp Source](#websharp-source-code)  
&nbsp;&nbsp;&nbsp;&nbsp;[Building WebSharp](#building-websharp)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Environment Setup](#environment-setup)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Build](#build)  
&nbsp;&nbsp;&nbsp;&nbsp;[Referencing `electron-dotnet` in your projects](#referencing-electron-dotnet)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Symlink `electron-dotnet`](#symlink-electron-dotnet)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Gotchas using symlink method](#gotchas-using-symlink-method)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Install `electron-dotnet`](#install-electron-dotnet)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Gotchas using install method](#gotchas-using-symlink-method)  
&nbsp;&nbsp;&nbsp;&nbsp;[Potential Issues](#potential-issues)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Setting mono path](#setting-mono-path)  
&nbsp;&nbsp;&nbsp;&nbsp;[Building against Mono 4.8](#building-against-mono-48)  

## Requirements

- [Visual Studio 2015 or greater](https://www.visualstudio.com/downloads/) community editions work just fine.
- [node-gyp windows install](https://github.com/nodejs/node-gyp#installation) Node.js native addon build tool.
- [nodejs](http://nodejs.org/).  Head over to [http://nodejs.org/](http://nodejs.org/) and click the install button to download the latest package.
- [Native Abstractions for Node.js](https://github.com/nodejs/nan)
- [mono embedding](http://www.mono-project.com/docs/advanced/embedding/) is being used so an installation of [mono](http://www.mono-project.com/download/) will need to be installed and available in your path.  For now you will need both a 32 bit and a 64 bit version of mono installed for building.
- [Native Client SDK](https://developer.chrome.com/native-client).  Native Client is a sandbox for running compiled C and C++ code in the browser efficiently and securely, independent of the user’s operating system.  No need to do anything here as it will be installed during the [Environment Setup](#environment-setup) step below. 

## WebSharp Source Code

We first need to compile WebSharp.

* Clone the WebSharp repository
    * For SSH

    ``` command
    # DOS command line
    # use the md or mkdir
    > mkdir projects
    > cd projects
    projects>
    projects> git clone git@github.com:xamarin/WebSharp.git
    ```

    * For HTTPS 

    ``` command
    # DOS command line
    # use the md or mkdir
    > mkdir projects
    > cd projects
    projects>
    projects> git clone https://github.com/xamarin/WebSharp.git
    ```

You should now have the following directory structure

```
.
|--- projects                           
     |--- WebSharp 
          |--- Examples           // Where you can find Example code.
          |--- GettingStarted     // A GettingStarted project for PepperPlugins
          |--- PepperPlugin       // Sources for WebSharp's PepperPlugin Native interface
          |--- PepperSharp        // Sources for WebSharp's Managed PepperPlugin interface
          |--- Tools              // Various helper tools and where the Yeoman generator source can be found
          |--- docs               // Various documentation
          |--- electron-dotnet    // WebSharp's Node.js module
          |--- Makefile           // Make file used for building on Mac
          |--- Makefile.win       // nmake file used for building on windows
          |--- README.md
          |--- setup.ps1          // support file used by Windows setup process
```

This will be your entry point for development.

## Building WebSharp

The windows build will require the following commands to be executed in a Visual Studio Command Prompt. 

To make things easier to get started we have provided a `Makefile.win` to be used by `nmake` that provides various targets.

* build
* setup

From the terminal window change into the WebSharp directory that was just cloned.

``` bash
projects> cd WebSharp
projects/WebSharp>
``` 
The rest of the commands entered below will take place in the WebSharp directory.

Type `nmake -f Makefile.win` in your command line

``` 
WebSharp> nmake -f Makefile.win
```

Which will present you with the following information.

``` command
Usage:
nmake buildRelease - builds the bindings for x86 (Win32) and x64 architectures - Release
nmake buildDebug - builds the bindings for x86 (Win32) and x64 architecture - Debug
nmake buildx64 - builds the bindings for x64 architecture - Release
nmake buildx86 - builds the bindings for x86 (Win32) architecture - Release
nmake buildx64Debug - builds the bindings for x64 architecture - Debug
nmake buildx86Debug - builds the bindings for x86 (Win32) architecture - Debug
nmake setup - sets up the environment for you
```

### Environment setup

Setting up your environment is the first thing that needs to be done.  This step will download the [Native Client SDK](https://developer.chrome.com/native-client) dependencies for building the PepperPlugin native interface and a version of NuGet that will be used in the `build` target.

From the terminal type the following:

```
WebSharp>nmake -f Makefile.win setup
```

You will be presented with output similar to the following:

```
Microsoft (R) Program Maintenance Utility Version 14.10.25017.0
Copyright (C) Microsoft Corporation.  All rights reserved.

        powershell -executionpolicy bypass -File .\setup.ps1
        ..\nacl_sdk\naclsdk.bat update pepper_canary
Downloading bundle pepper_canary
|================================================|  319336756
..  16711/319336 kB

```

Once the the [Native Client SDK](https://developer.chrome.com/native-client) is downloaded it will then begin the update of the `pepper_canary` bundle.

```

Microsoft (R) Program Maintenance Utility Version 14.10.25017.0
Copyright (C) Microsoft Corporation.  All rights reserved.

        powershell -executionpolicy bypass -File .\setup.ps1
        ..\nacl_sdk\naclsdk.bat update pepper_canary
Downloading bundle pepper_canary
|================================================|  319336756
..................................................  319336/319336 kB
Updating bundle pepper_canary to version 58, revision 447822
|------------------------------------------------|
.................................................

WebSharp>

```

A more detailed explanation of the setup process can be found in the [PepperPlugin README](https://github.com/xamarin/WebSharp/tree/master/PepperPlugin).

### Build

The `build` step builds all the components of the `WebSharp` project from one command, which includes the following.

* PepperPlugin - WebSharp's native interface to the [Native Client SDK](https://developer.chrome.com/native-client) 
* PepperSharp - WebSharp's managed interface to the [Native Client SDK](https://developer.chrome.com/native-client)
* WebSharp.js - WebSharp's managed code interface to scripting `Node.js` and `Electron` api's from managed code.
* generator-electron-dotnet - Yeoman generator for electron-dotnet.  The simplest way to add a new Electron DotNet application.

From the terminal type the following:

``` command
WebSharp>nmake -f Makefile.win buildRelease
```

Then sit back and wait.  When it has finished everything should be built and available to start developing.

> :bulb: During the `build` process you may get a warning similar to:

```
pepper_canary already exists, but has an update available.
Run update with the --force option to overwrite the existing directory.
Warning: This will overwrite any modifications you have made within this directory.
```

The command to run from the `WebSharp` repo directory is:

``` command
..\nacl_sdk\naclsdk.bat update pepper_canary --force
```

#### Build electron-dotnet

electron-dotnet is WebSharp's interface allowing scripting of Node.js and Electron api's from managed.  Each version requires a separate build. 

To build one of the versions of `Node.js` officially released by [Node.js](http://nodejs.org/dist), targeting a version of `Electron`, do the following:

```
# Visual Studio Command Prompt
WebSharp> cd electron-dotnet\tools
WebSharp\electron-dotnet\tools> build.bat
```

```
# Visual Studio Command Prompt
Usage: build.bat debug|release target "{version} {version}" ...
e.g. build.bat release 1.4.0 6.5.0
e.g. build.bat release 1.5.0 7.0.0
e.g. build.bat release 1.6.0 7.4.0
```

> :bulb: the `Node.js` version number you provide must be a version number corresponding to one of the subdirectories of http://nodejs.org/dist. The command will build both `x32/ia32` and `x64` architectures (assuming you use a `x64` machine). The command will also copy the `websharp\_\*.node` executables to appropriate locations under `lib\native` directory where they are looked up from at runtime. The `npm install` step copies the C standard library shared DLL to the location of the `websharp\_\*.node` files for the component to be ready to go.

#### Windows Build Example

`Electron` target `1.5.0` is built against `Nodejs` version `7.0.0` but the `1.5.1` target needs to be built against the `7.4.0` version of `Nodejs`.

To support these types of scenarios the directory structure of the native builds, for the different target/versions, have the following structure.  The native output directory will include the `Electron` target for both `x32/ia32` and `x64` platforms.

```
.
|--- lib
     |--- native
          |--- win32
               |--- ia32
                    |--- target electron
                         |--- version nodejs
               |--- x64
                    |--- target electron
                         |--- version nodejs
```                         

To use `build.bat` in the scenario above one would issue the command as follows:

```
WebSharp\electron-dotnet\tools>build.bat release 1.5.0 7.0.0 7.4.0
```

Starting with 1.4.0 - 1.6.0 of Electron here are the following commands:

```
WebSharp\electron-dotnet\tools>build.bat release 1.4.0 6.5
WebSharp\electron-dotnet\tools>build.bat release 1.5.0 7.0.0 7.4.0
WebSharp\electron-dotnet\tools>build.bat release 1.6.0 7.4.0
```

The `electron-dotnet.js` source also needs to be updated so that the correct native `Nodejs` node module version can be looked up and loaded.

The `targetMap` needs to be modified for the supported `Electron` targets.

``` js
var targetMap = [
    [ /^1\.4/, '1.4.0' ],
    [ /^1\.5/, '1.5.0' ],
    [ /^1\.6/, '1.6.0' ],    
];
``` 

As well as the `versionMap` for the `Nodejs` versions.

``` js
var versionMap = [
    [ /^6\./, '6.5.0' ],
    [ /^7\.[0-3]/, '7.0.0' ],
    [ /^7\.4/, '7.4.0' ],
];
```

## Referencing `electron-dotnet`

During the development process there are two ways to reference `electron-dotnet` in your `Electron` applications.

* [Symlinking](https://docs.npmjs.com/cli/link) `electron-dotnet` using `npm link`
* Installing `electron-dotnet` directly in your project using `npm install`.

### Symlink `electron-dotnet`

Right now `WebSharp` is going through development and is still in transition.  Maybe you want to help with the effort or make some private changes to the repo for personal use.  Either way the `npm link` command is a fast way to allow you to make changes while working on an application or `WebSharp` directly.

`npm-link` allows you to symlink a package folder helping you to work on modules locally and use the code across your project(s) without publishing it to a npm repository.  So if changes are made to the repo and the symlink to `electron-dotnet` was used they will automatically be available to the project without having to do the `npm uninstall install` dance all the time.  Your project is asssured to use the most recent modifications.

To set this up we will need to be in the `electron-dotnet` subdirectory of `WebSharp` and create this symlink.

From the terminal type the following:

``` command
WebSharp> cd electron-dotnet        # go into the package directory
WebSharp\electron-dotnet> npm link  # creates global link
```

That is it!  Everything is setup for you with the single command.

Now what ever project you are working on you can use the symlink instead of keeping track of where the local copy of `electron-dotnet` resides.

``` command
> cd some-other-project                     # go into some other package directory.
some-other-project> npm link electron-dotnet # link-install the `electron-dotnet` package
```

Of course if the 'electron-dotnet' is no longer needed in your project it can be deferenced simply by using `unlink`

``` command
> cd some-other-project                       # go into some other package directory.
some-other-project$ npm unlink electron-dotnet # dereference the `electron-dotnet` package
```


#### Gotchas using symlink method 

`WebSharp` uses native modules internally for both the `PepperPlugin` and `electron-dotnet` implementations.  If you are testing the different versions of `Electron` and symlinking instead of `npm install`ing you may run into the following error.

>"Uncaught Error: Cannot find module 'WebSharp\el
ectron-dotnet\lib\native\win32\x64\1.6.0\7.4.0\websharp_monoclr'"

Make sure you build a version of `electron-dotnet` that corresponds to the version outlined in the error.

### Install `electron-dotnet`

Instead of using the symlink suggestion above you may prefer to install `electron-dotnet` directly from the local `WebSharp` build.

What this means is that the install will contain the exact state of `electron-dotnet` of the install.  If you want to test any modification you will need to do the `npm uninstall install` dance.

You will also need to remember the install path of the `WebSharp` build whereas using the symlink method it is not necassary.

To use `npm install` do the following from a terminal inside of the project directory you are working on.

``` bash
> cd some-other-project     # go into some other package directory.
some-other-project> npm install --save-dev path-to-WebSharp/electron-dotnet/      # install 'electron-dotnet'
```

> :bulb: When installed into the project the only native modules of `WebSharp` that will be available are those that have been built in the build step above.

#### Gotchas using npm install

You will still get the `Cannot find module ...` mismatch error as described above when using symlink.

>"Uncaught Error: Cannot find module 'el
ectron-dotnet\lib\native\win32\x64\1.6.0\7.4.0\websharp_monoclr'"

Internally `WebSharp` is still using native modules for both the `PepperPlugin` and `electron-dotnet` implementations whether symlinked or installed.  

You have only one choice here.  

* Uninstall `electron-dotnet`, build a version for the Electron and Node.js version and then reinstall it.  


## Potential Issues

When using windows the user may get the following error:

>Uncaught Error: The websharp module for using mono embedding has been specified but mono can not be found. You must build a custom version of websharp.node for using mono embedding or make sure that mono is in your %PATH%. Please refer to https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md for building instructions.

To get around this the user may need to add `Mono` to their %PATH%

### Setting mono path

`Mono` must be in the developers %PATH% for mono support to be built and also in the users %PATH% during the application execution.

The following options can be used.

  * Option 1:

    * Use Mono's ```setmonopath.bat``` batch command before starting the electron application:

      * x64
        ```
        > "c:\Program Files\Mono\bin\setmonopath.bat"
        ```

      * x86
        ```
        > "c:\Program Files (x86)\Mono\bin\setmonopath.bat"
        ```

  * Option 2:
    * Custom path environment variable set to the correct mono before starting the electron application.
    ```
    SET PATH=%PATH%;c:\path\to\mono
    ```

  * Option 3:
    * Set the path in the ```main.js``` before calling any ```electron-dotnet``` functions.
    
      ```js
        if (process.platform === 'win32')
        {
            if (process.arch === 'x64')
                process.env.PATH = "c:\\Program Files\\Mono\\bin;" + process.env.PATH;
            else
                process.env.PATH = "c:\\\Program Files (x86)\\\Mono\\bin;" + process.env.PATH;
        }
      ```
  * Option 4: 
    * Set the path on Windows 10 and Windows 8
      * In Search, search for and then select: System (Control Panel)
      * Click the Advanced system settings link.
      * Click Environment Variables. ...
      * In the Edit System Variable (or New System Variable) window, specify the value of the PATH environment variable.
        * x64
          ```
          "c:\Program Files\Mono\bin;"
          ```
        * x86
          ```  
          "c:\Program Files (x86)\Mono\bin;"
          ```
     
     > :exclamation: Option 4 is not very flexible

### Cannot find module 'nan'

If you run into the following error make sure you have installed the 'nan' as per the pre-requisites above:

> Error: Cannot find module 'nan'
    at Function.Module._resolveFilename (module.js:455:15)
    at Function.Module._load (module.js:403:25)
    at Module.require (module.js:483:17)
    at require (internal/module.js:20:19)
    at [eval]:1:1
    at ContextifyScript.Script.runInThisContext (vm.js:25:33)
    at Object.exports.runInThisContext (vm.js:77:17)
    at Object.<anonymous> ([eval]-wrapper:6:22)
    at Module._compile (module.js:556:32)
    at bootstrap_node.js:357:29

The command to run from the `WebSharp` repo directory is:

``` command

WebSharp> cd electron-dotnet        # go into the package directory
WebSharp\electron-dotnet> npm install nan  # makes NAN available.

```

This will install NAN locally to the project.

To install globally:

``` command

> npm install nan -g  # makes NAN available globally

```


## Building against Mono 4.8

There seems to have been a small regression building with Mono 4.8 and `glib.h`.

You may see something like:

>C:\Program Files\Mono\include\mono-2.0\mono/metadata/exception.h(4): fatal error C1083: Cannot open include file: 'glib
.h': No such file or directory [c:\WebSharp\electron-dotnet\build\websharp_monoclr.vcxproj]

To get around this build error you can create a new file called `glib.h` with the following content.

``` C++
#define gpointer void *
#define g_assert(x) assert(x)
```

Once created you can copy this to the `Mono` install `include` folders.

* x64
  - C:\Program Files\Mono\include\mono-2.0

* x86
  - C:\Program Files (x86)\Mono\include\mono-2.0

> :bulb: This will be fixed in the next release see [PR 4240](https://github.com/mono/mono/pull/4240/)

