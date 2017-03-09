# Getting Started on Mac

The goal of this doc is to get you setup with a flexible workflow using `WebSharp` on the Mac.  

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


## Requirements

- [XCode](https://itunes.apple.com/us/app/xcode/id497799835?mt=12). Apple’s XCode development software is used to build Mac and iOS apps, but it also includes the tools you need to compile software for use on your Mac. XCode is free and you can find it in the [Apple App Store](https://itunes.apple.com/us/app/xcode/id497799835?mt=12).
- [nodejs](http://nodejs.org/).  Head over to [http://nodejs.org/](http://nodejs.org/) and click the install button to download the latest package.
- [Native Abstractions for Node.js](https://github.com/nodejs/nan)
- [mono embedding](http://www.mono-project.com/docs/advanced/embedding/) is being used so an installation of [mono](http://www.mono-project.com/download/) will need to be installed and available in your path.  If [Visual Studio for Mac](https://developer.xamarin.com/releases/vs-mac/) is already installed then that is the `Mono SDK` that will be used.
- [Native Client SDK](https://developer.chrome.com/native-client).  Native Client is a sandbox for running compiled C and C++ code in the browser efficiently and securely, independent of the user’s operating system.  No need to do anything here as it will be installed during the [Environment Setup](#environment-setup) step below. 
- [.NET Core](https://www.microsoft.com/net/core) _*[optional]*_.  Version >= 1.0.1 - You can use electron-dotnet.js on OSX with either `Mono` or `.NET Core `installed, or both.  During the build if `.Net Core` is detected then the CoreCLR module will be built as well.  by default ```electron-dotnet``` will use `Mono`. You opt in to using `.NET Core` with the `EDGE_USE_CORECLR` environment variable: 

```bash
        EDGE_USE_CORECLR=1
```   

## WebSharp Source Code

We first need to compile WebSharp.

* Clone the WebSharp repository
    * For SSH

    ``` bash
    # Mac Terminal
    $ mkdir projects
    $ cd projects
    projects$
    projects$ git clone git@github.com:xamarin/WebSharp.git
    ```

    * For HTTPS 

    ``` bash
    # Mac Terminal
    $ mkdir projects
    $ cd projects
    projects$
    projects$ git clone https://github.com/xamarin/WebSharp.git
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

To make things easier to get started we have provided a `makefile` that provides two targets.

* build
* setup

From the terminal window change into the WebSharp directory that was just cloned.

``` bash
projects$ cd WebSharp
projects/WebSharp$
``` 
The rest of the commands entered below will take place in the WebSharp directory.

Type `make` in your terminal

``` bash
WebSharp$ make
```

Which will present you with the following information.

``` bash

Usage:
make build - builds the bindings
make setup - sets up the environment for you
```

### Environment setup

Setting up your environment is the first thing that needs to be done.  This step will download the [Native Client SDK](https://developer.chrome.com/native-client) dependencies for building the PepperPlugin native interface and a version of NuGet that will be used in the `build` target.

From the terminal type the following:

```
WebSharp$ make setup
```

You will be presented with output similar to the following:

```
curl -O 'http://storage.googleapis.com/nativeclient-mirror/nacl/nacl_sdk/nacl_sdk.zip'
  % Total    % Received % Xferd  Average Speed   Time    Time     Time  Current
                                 Dload  Upload   Total   Spent    Left  Speed
100 39269  100 39269    0     0  81990      0 --:--:-- --:--:-- --:--:-- 82152
unzip nacl_sdk.zip -d ../
Archive:  nacl_sdk.zip
  inflating: ../nacl_sdk/naclsdk     
  inflating: ../nacl_sdk/naclsdk.bat  
  inflating: ../nacl_sdk/sdk_cache/naclsdk_manifest2.json  
  inflating: ../nacl_sdk/sdk_tools/cacerts.txt  
  inflating: ../nacl_sdk/sdk_tools/sdk_update.py  
  inflating: ../nacl_sdk/sdk_tools/download.py  
  inflating: ../nacl_sdk/sdk_tools/__init__.py  
  inflating: ../nacl_sdk/sdk_tools/config.py  
  inflating: ../nacl_sdk/sdk_tools/sdk_update_main.py  
  inflating: ../nacl_sdk/sdk_tools/sdk_update_common.py  
  inflating: ../nacl_sdk/sdk_tools/command/uninstall.py  
  inflating: ../nacl_sdk/sdk_tools/command/sources.py  
  inflating: ../nacl_sdk/sdk_tools/command/info.py  
  inflating: ../nacl_sdk/sdk_tools/command/__init__.py  
  inflating: ../nacl_sdk/sdk_tools/command/update.py  
  inflating: ../nacl_sdk/sdk_tools/command/list.py  
  inflating: ../nacl_sdk/sdk_tools/command/command_common.py  
  inflating: ../nacl_sdk/sdk_tools/third_party/__init__.py  
  inflating: ../nacl_sdk/sdk_tools/third_party/fancy_urllib/__init__.py  
  inflating: ../nacl_sdk/sdk_tools/third_party/fancy_urllib/README.chromium  
  inflating: ../nacl_sdk/sdk_tools/manifest_util.py  
  inflating: ../nacl_sdk/sdk_tools/LICENSE  
  inflating: ../nacl_sdk/sdk_tools/cygtar.py  
../nacl_sdk/naclsdk update pepper_canary # Downloads the real SDK. This takes a while
Downloading bundle pepper_canary
|================================================|  323174174
...................  124944/323174 kB

```

Once the the [Native Client SDK](https://developer.chrome.com/native-client) is downloaded it will then setup the build requirements for `electron-dotnet`.

``` bash
Updating bundle pepper_canary to version 58, revision 447822
(mkdir -p electron-dotnet/tools/build)
(mkdir -p electron-dotnet/tools/build/nuget)
(cd electron-dotnet/tools/ && { mcs download.cs ; mono download.exe 'http://nuget.org/nuget.exe' ./build/nuget.exe;  mono ./build/nuget.exe update -self; cd -; })
Downloading http://nuget.org/nuget.exe to ./build/nuget.exe...
Done.
Checking for updates from https://www.nuget.org/api/v2/.
Currently running NuGet.exe 2.8.6.
Updating NuGet.exe to 3.5.0.
Update successful.
```

A more detailed explanation of the setup process can be found in the [PepperPlugin README](https://github.com/xamarin/WebSharp/tree/master/PepperPlugin).

### Build

The `build` step builds all the components of the `WebSharp` project from one command, which includes the following.

* PepperPlugin - WebSharp's native interface to the [Native Client SDK](https://developer.chrome.com/native-client) 
* PepperSharp - WebSharp's managed interface to the [Native Client SDK](https://developer.chrome.com/native-client)
* electron-dotnet - WebSharp's interface allowing scripting of `Node.js` and `Electron` api's from managed code.
* WebSharp.js - WebSharp's managed code interface to scripting `Node.js` and `Electron` api's from managed code.
* generator-electron-dotnet - Yeoman generator for electron-dotnet.  The simplest way to add a new Electron DotNet application.

From the terminal type the following:

``` bash
WebSharp$ make build
```

Then sit back and wait.  When it has finished everything should be built and available to start developing.

> :bulb: During the `build` process you may get a warning similar to:

```
pepper_canary already exists, but has an update available.
Run update with the --force option to overwrite the existing directory.
Warning: This will overwrite any modifications you have made within this directory.
```

The command to run from the `WebSharp` repo directory is:

``` bash
..\nacl_sdk\naclsdk.bat update pepper_canary --force
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

``` bash
WebSharp$ cd electron-dotnet        # go into the package directory
WebSharp\electron-dotnet$ npm link  # creates global link
```

That is it!  Everything is setup for you with the single command.

Now what ever project you are working on you can use the symlink instead of keeping track of where the local copy of `electron-dotnet` resides.

``` bash
~$ cd some-other-project                     # go into some other package directory.
some-other-project$ npm link electron-dotnet # link-install the `electron-dotnet` package
```

Of course if the 'electron-dotnet' is no longer needed in your project it can be deferenced simply by using `unlink`

``` bash
~$ cd some-other-project                       # go into some other package directory.
some-other-project$ npm unlink electron-dotnet # dereference the `electron-dotnet` package
```


#### Gotchas using symlink method 

`WebSharp` uses native modules internally for both the `PepperPlugin` and `electron-dotnet` implementations.  If you are testing the different versions of `Electron` and symlinking instead of `npm install`ing you may run into the following `NODE_MODULE_VERSION` mismatch error.

>The module '.../WebSharp/electron-dotnet/build/Release/edge_nativeclr.node'
was compiled against a different Node.js version using
NODE_MODULE_VERSION XX. This version of Node.js requires
NODE_MODULE_VERSION YY. Please try re-compiling or re-installing
the module (for instance, using `npm rebuild` or`npm install`). 

Take the suggested `rebuild` route as outlined in the error message but instead USE [electron-rebuild](https://github.com/electron/electron-rebuild) and NOT `npm rebuild`

``` bash
some-other-project$ npm install --save-dev electron-rebuild    # install electron-rebuild
some-other-project$ ./node_modules/.bin/electron-rebuild -w electron-dotnet -f        # rebuilds `electron-dotnet` native Node.js modules
```

`electron-rebuild` rebuilds native Node.js modules against the version of Node.js that your Electron project is using.

When it is finished you should see the message: `✔ Rebuild Complete`


### Install `electron-dotnet`

Instead of using the symlink suggestion above you may prefer to install `electron-dotnet` directly from the local `WebSharp` build.

What this means is that the install will contain the exact state of `electron-dotnet` of the install.  If you want to test any modification you will need to do the `npm uninstall install` dance.

You will also need to remember the install path of the `WebSharp` build whereas using the symlink method it is not necassary.

To use `npm install` do the following from a terminal inside of the project directory you are working on.

``` bash
~$ cd some-other-project     # go into some other package directory.
some-other-project$ npm install --save-dev path-to-WebSharp/electron-dotnet/      # install 'electron-dotnet'
```

During the install into the project the native modules of `WebSharp` will automatically be rebuilt.

#### Gotchas using npm install

You will still get the `NODE_MODULE_VERSION` mismatch error as described above when using symlink.

>The module '.../WebSharp/electron-dotnet/build/Release/edge_nativeclr.node'
was compiled against a different Node.js version using
NODE_MODULE_VERSION XX. This version of Node.js requires
NODE_MODULE_VERSION YY. Please try re-compiling or re-installing
the module (for instance, using `npm rebuild` or`npm install`). 

Internally `WebSharp` is still using native modules for both the `PepperPlugin` and `electron-dotnet` implementations whether symlinked or installed.  

You have two choices here.  

* Use the `electron-rebuild` method.

    ``` bash
    some-other-project$ npm install --save-dev electron-rebuild    # install electron-rebuild
    some-other-project$ ./node_modules/.bin/electron-rebuild -w electron-dotnet -f        # rebuilds `electron-dotnet` native Node.js modules
    ```

    `electron-rebuild` rebuilds native Node.js modules against the version of Node.js that your Electron project is using.

    When it is finished you should see the message: `✔ Rebuild Complete`

* Uninstall `electron-dotnet` and then reinstall it.  This will rebuild the modules against the Electron version you have installed.

    ``` bash
    some-other-project$ npm uninstall electron-dotner    # uninstall electron-rebuild
    some-other-project$ npm install --save-dev path-to-WebSharp/electron-dotnet/      # install 'electron-dotnet'
    ```

    During the install you will see the module rebuilding the `electron-dotnet` native modules.

    ``` bash

    > node tools/install.js

  CXX(target) Release/obj.target/edge_nativeclr/src/mono/clractioncontext.o
  CXX(target) Release/obj.target/edge_nativeclr/src/mono/clrfunc.o
  CXX(target) Release/obj.target/edge_nativeclr/src/mono/clrfuncinvokecontext.o
  CXX(target) Release/obj.target/edge_nativeclr/src/mono/monoembedding.o
  CXX(target) Release/obj.target/edge_nativeclr/src/mono/task.o
  CXX(target) Release/obj.target/edge_nativeclr/src/mono/dictionary.o
  CXX(target) Release/obj.target/edge_nativeclr/src/mono/nodejsfunc.o
  CXX(target) Release/obj.target/edge_nativeclr/src/mono/nodejsfuncinvokecontext.o

  etc ....

    ```


