# Getting Started Building Websharp Electron Application Assemblies

In this section we will compile the `World.cs` module into an assembly to be loaded.  The `Hello` application interacts with `Node.js` via the `WebSharp.js` NuGet package that allows scripting of `Node.js` functions, notably access to the console via the `NodeConsole` instance.

One thing to point out is that these are helper files for Mac and Windows to include the `WebSharp.js` packages.  The `WebSharp.js` package allows your assembly to script and interact with `Node.js`.

We will cover the use of the extra files in the `World` directory to include the `WebSharp.js` package.  You can remove them to create your own flow or use them as a base. 

If you already have a pre-compiled assembly that will not need `Node.js` scripting then it can already be referenced in your `Node.js` code.  In that case you may want to skip the next part and go directly to [Including assemblies]().

## Content

&nbsp;&nbsp;&nbsp;&nbsp;[Requirements](#requirements)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Windows](#requirements-windows)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[MacOSX](#requirements-macosx)  
&nbsp;&nbsp;&nbsp;&nbsp;[Preparing code](#preparing-code-worldcs)  
&nbsp;&nbsp;&nbsp;&nbsp;[Over view of the Support Files](#over-view-of-the-support-files)  
&nbsp;&nbsp;&nbsp;&nbsp;[Building on Windows](#building-on-windows)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[msbuild](#windows-msbuild)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Visual Studio IDE](#windows-vs)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[.Net Core](#windows-net-core)  
&nbsp;&nbsp;&nbsp;&nbsp;[Building on MacOSX](#building-on-macosx)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[xbuild](#macosx-xbuild)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Visual Studio IDE](#macosx-vs)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[msbuild](#macosx-msbuild)  

## Requirements

### Requirements Windows

- [Visual Studio 2017](https://www.visualstudio.com/downloads/) community editions work just fine.  This will allow building using `msbuild` command line as well as via the IDE.
- [.Net Core 1.1](https://www.microsoft.com/net/download/core) allows building from the command line.  This is optional but does provide a consistant build interface between platforms.
- [Package Sources](https://github.com/xamarin/WebSharp/blob/master/docs/vsc/vsc-package-sources.md) because the application references the `WebSharp.js` package.  > :bulb:This can also be set in the `nuget.config` file described below.

### Requirements MacOSX

- [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/) allows building using `xbuild` from the terminal as well as via the IDE.
- [.Net Core](https://www.microsoft.com/net/download/core) allows building from the command line.  This is optional but does provide a consistant build interface between platforms.
- [NuGet CLI >= 3.5](https://docs.microsoft.com/en-us/nuget/guides/install-nuget#nuget-cli) for restoring NuGet packages.

## Over view of the support files

In the previous section we mentioned that there was a directory generated with additional files to help create an assembly.

```
|--- src                              // sources
     |--- World
          |--- nuget.config           // Configuring NuGet behavior
          |--- packages.config        // Used to track installed packages
          |--- World_macosx.csproj    // MacOSX project  
          |--- World.cs               // Application Implementation
          |--- World.csproj           // Windows project 
```

Let's take a brief look at them.

### Configuring NuGet behavior: nuget.config

The `nuget.config` file is a project specific file.  It allows control over settings as they apply to this project.  NuGet uses multiple files to configure how NuGet packages can be consumed.  We will not go into all those possibilities here but a good overview is here: [Configuring NuGet behavior](https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior)

> :bulb:Since we do not have `WebSharp.js` available in the NuGet main repository yet we will be using a local repo from our build.

To setup a local `WebSharp` repository take a look at the following [Setting up Package Sources](https://github.com/xamarin/WebSharp/blob/master/docs/vsc/vsc-package-sources.md)

If for some reason the local `WebSharp` package source is not working for your operating system you can use this file to set the package repository path for `WebSharp.js` on the project level.  

Edit the `nuget.config` file and look for `<packageSources>`

``` xml
  <packageSources>
    <add key="NuGet official package source" value="https://nuget.org/api/v2/" />
    <!--add key="LocalWebSharp" value="Path-To-WebSharp\WebSharp\electron-dotnet\tools\build\nuget" /-->
  </packageSources>
```

Uncomment the key entry reading `LocalWebSharp`and 
replace the `value` with your specific path to the WebSharp repository that you cloned.

``` xml
    <add key="LocalWebSharp" value="c:\projects\WebSharp\electron-dotnet\tools\build\nuget" />

```

We are assuming that you cloned the `WebSharp` repository to `c:\projects\`.

### Track installed packages: packages.config

Package Restore uses the information in packages.config to reinstall all dependencies.

More information can be found at [Consume Packages](https://docs.microsoft.com/en-us/nuget/consume-packages/overview-and-workflow)

### Windows project: World.csproj

Defines a windows specific project that creates a .net library assembly.

### MacOSX project: World_macosx.csproj

Defines a Mac OSX specific project that creates a .net library assembly.

## Preparing Code: World.cs

The first thing we will need to do is modify the `World.cs` source to make sure it conforms to how asseblies are referenced.

As was mentioned when compiling the source code on the fly the class must be named `Startup` and it must have an `Invoke` method that matches the `Func<object,Task<object>>` delegate signature. 

When compiling to an assembly a `namespace` is required.  Edit the file and make sure the `namespace` is uncommented and saved before moving on to the building the assembly sections following.

``` csharp

using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJs;

namespace World
{
    public class Startup
    {

        static NodeConsole console;

        /// <summary>
        /// Default entry into managed code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<object> Invoke(object input)
        {
            if (console == null)
                console = await NodeConsole.Instance();

            try
            {
                console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
}

```

Once the source is saved we can then move on to building the assembly.



## Building on Windows

This section will show multiple ways to use the extra files to create an assembly on windows.

### Windows: MSBuild

You will need to have the `Visual Studio Command Prompt` or `Developer Command Prompt` open and positioned to the application `src\World` directory.  If you are new to this see [Searching for the Command Prompt on your machine](https://msdn.microsoft.com/en-us/library/ms229859(v=vs.110).aspx#Anchor_0)

Once opened you will need to change into `Hello`'s src directory. 

``` 
> cd Path-to-application\src\World
```

Do a restore of the NuGet packages.

```
\src\World> msbuild World.csproj /t:restore
```

The following is a sample of the output after the restore.

```
Microsoft (R) Build Engine version 15.1.548.43366
Copyright (C) Microsoft Corporation. All rights reserved.

Build started 4/4/2017 10:37:14 AM.
Project "c:\projects\HelloWorldApplication\src\World\World.csproj" on node 1 (restore target(s)).
Restore:
  Restoring packages for c:\projects\HelloWorldApplication\src\World\World.csproj...
  Committing restore...
  Generating MSBuild file c:\projects\HelloWorldApplication\src\World\obj\World.csproj.nuget.g.props
  .
  Generating MSBuild file c:\projects\HelloWorldApplication\src\World\obj\World.csproj.nuget.g.targe
  ts.
  Writing lock file to disk. Path: c:\projects\HelloWorldApplication\src\World\obj\project.assets.js
  on
  Restore completed in 281.21 ms for c:\projects\HelloWorldApplication\src\World\World.csproj.

  NuGet Config files used:
      c:\projects\HelloWorldApplication\src\World\NuGet.Config
      C:\Users\kenne\AppData\Roaming\NuGet\NuGet.Config
      C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.Offline.config

  Feeds used:
      https://nuget.org/api/v2/
      https://api.nuget.org/v3/index.json
      c:\projects\WebSharp\electron-dotnet\tools\build\nuget
      C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\
Done Building Project "c:\projects\HelloWorldApplication\src\World\World.csproj" (restore target(s))
.


Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:01.02
```

Then we build with the `msbuild` command.

```
\src\World>msbuild World.csproj /p:Configuration=Debug /p:Platform="Any CPU"
```

The assembly should now be found in in the `bin\Debug\` folder.

### Windows: Visual Studio IDE

You will need to have `Visual Studio 2015 or greater` installed.  Community Editions work fine.

This is probably the easiest way to build the assembly.  From Visual Studio click on *File > Open > Project/Solution*, once the dialog shows up navigate to the project source, select the `World.csproj` file and click `Open`.  This will add a new solution as well as the project file.

Compile the solution to create the assembly.  This will also restore the `WebSharp.js` package automatically before compiling.

### Windows: .Net Core

To compile with `.Net Core` you will need to have it installed.

This is almost the same the `msbuild` scenario described above except you can do this from a normal command line.

Open a windows command line and change into the application source directory.

``` 
> cd Path-to-application\src\World
```

Do a restore of the NuGet packages.

```
\src\World>dotnet restore World.csproj
```

The following is a sample of the output after the restore.

```
\src\World>dotnet restore World.csproj
  Restoring packages for c:\projects\HelloWorldApplication\src\World\World.csproj...
  Generating MSBuild file c:\projects\HelloWorldApplication\src\World\obj\World.csproj.nuget.g.props.
  Generating MSBuild file c:\projects\HelloWorldApplication\src\World\obj\World.csproj.nuget.g.targets.
  Writing lock file to disk. Path: c:\projects\HelloWorldApplication\src\World\obj\project.assets.json
  Restore completed in 132.07 ms for c:\projects\HelloWorldApplication\src\World\World.csproj.

  NuGet Config files used:
      c:\projects\HelloWorldApplication\src\World\NuGet.Config
      C:\Users\kenne\AppData\Roaming\NuGet\NuGet.Config
      C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.Offline.config

  Feeds used:
      https://nuget.org/api/v2/
      https://api.nuget.org/v3/index.json
      C:\projects\WebSharp\electron-dotnet\tools\build\nuget
      C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\
```

Then we build with the `dotnet build` command.

```
\src\World>dotnet build World.csproj /p:Configuration=Debug
```

The assembly should now be found in in the `bin\Debug\` folder. 

## Building on MacOSX

This section will show multiple ways to use the extra files to create an assembly on windows.

### MacOSX: XBuild

You will need to have a terminal open and positioned to the application `src\World` directory. 

Once opened you will need to change into `Hello`'s src directory. 

``` bash
$ cd Path-to-application/src/World
```

Do a restore of the NuGet packages.

``` bash
/src/World> nuget restore World_macosx.csproj
```

The following is a sample of the output after the restore.

```
HelloWorldApplication/src/World/packages'

NuGet Config files used:
    /projects/WebSharp/websharpjs/HelloWorldApplication/src/World/nuget.config
    /Users/Jimmy/.config/NuGet/NuGet.Config

Feeds used:
    /Users/Jimmy/.nuget/packages/
    https://nuget.org/api/v2/
    https://www.nuget.org/api/v2/
    /projects/WebSharp/electron-dotnet/tools/build/nuget

Installed:
    1 package(s) to packages.config projects
```

> :bulb: If the `WebSharp.js` package is not restored take a look at [Configuring NuGet behavior](#configuring-nuget-behavior-nugetconfig).

Then we build with the `xbuild` command.

```
/src/World$ xbuild World_macosx.csproj 
```

The assembly should now be found in in the `bin\Debug\` folder.

### MacOSX: Visual Studio IDE

You will need to have `Visual Studio for Mac` installed.

This is probably the easiest way to build the assembly.  From Visual Studio click on *File > Open*, once the dialog shows up navigate to the project source, select the `World_macosx.csproj` file and click `Open`.  This will add a new solution as well as the project file.

Compile the solution to create the assembly.  This will also restore the `WebSharp.js` package automatically before compiling.

### MacOSX: MSBuild

You will need to have a terminal open and positioned to the application `src\World` directory. 

Once opened you will need to change into `Hello`'s src directory. 

``` bash
$ cd Path-to-application/src/World
```

Do a restore of the NuGet packages.

``` bash
/src/World> nuget restore World_macosx.csproj
```

The following is a sample of the output after the restore.

```
HelloWorldApplication/src/World/packages'

NuGet Config files used:
    /projects/WebSharp/websharpjs/HelloWorldApplication/src/World/nuget.config
    /Users/Jimmy/.config/NuGet/NuGet.Config

Feeds used:
    /Users/Jimmy/.nuget/packages/
    https://nuget.org/api/v2/
    https://www.nuget.org/api/v2/
    /projects/WebSharp/electron-dotnet/tools/build/nuget

Installed:
    1 package(s) to packages.config projects
```

> :bulb: If the `WebSharp.js` package is not restored take a look at [Configuring NuGet behavior](#configuring-nuget-behavior-nugetconfig).

Then we build with the `msbuild` command.

```
/src/World$ msbuild World_macosx.csproj 
```

The assembly should now be found in in the `bin\Debug\` folder.