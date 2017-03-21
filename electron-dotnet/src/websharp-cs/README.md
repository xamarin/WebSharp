websharp-cs
=======

This is a C# compiler for websharp.js.  Uses Roslyn compiler to bring > 4.0 sdk functionality.

See [WebSharp overview](http://github.com/xamarin/WebSharp) and [electron-dotnet on GitHub](https://github.com/xamarin/WebSharp/tree/master/electron-dotnet) for more information. 

## Functionality

1. Uses Roslyn compiler.  This also means that it comes with extra .dll's that need to be available from Microsoft.

    - Microsoft.CodeAnalysis;
    - Microsoft.CodeAnalysis.CSharp;
    - Microsoft.CodeAnalysis.Emit;

2. Automatically adds a reference to WebSharpJs.dll when compiling and running.

## Building on Windows

The build is part of the build_websharp.bat process when building the websharpjs packages.

To build separately use msbuild from command line to compile the websharp-cs.sln:

``` cmd
msbuild websharp-cs.sln /p:Configuration=Release /p:Platform="Any CPU"
```

or using .Net Core CLI if you have it installed:

``` cmd
dotnet build websharp-cs.sln /p:Configuration=Release /p:Platform="Any CPU"
```

