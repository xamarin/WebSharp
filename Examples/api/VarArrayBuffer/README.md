# Example VarArrayBuffer

Demonstrates prompting the user for a file, passing the file contents to PepperPlugin Instance as a VarArrayBuffer, then drawing a histogram representing the contents of the file to a 2D square.

## Pre-requisites

- [Node.js](https://nodejs.org/en/download/) (which comes with [npm](http://npmjs.com)) installed on your computer.
- [PepperPlugin](https://github.com/xamarin/WebSharp/tree/master/PepperPlugin) and its pre-requisites.
- The samples use a prebuilt version of electron-dotnet [Building on OSX](https://github.com/xamarin/WebSharp/tree/master/electron-dotnet#building-on-osx-electron)

## To Run

```bash
# Go into the repository
cd WebSharp\Examples\api\VarArrayBuffer
```

### Windows
```bash
# Compile the example project from Visual Studio 2015 Native Tools Command Prompt 
msbuild VarArrayBuffer.csproj
```

### Mac
```bash
# Compile the example project from Mac terminal 
xbuild VarArrayBuffer.csproj
```

### Install dependencies and run the app.
```bash
# Install dependencies and run the app
npm install && npm start
```
Learn more about Electron Dotnet and its API in the [Getting Started documentation](https://github.com/xamarin/WebSharp/tree/master/GettingStarted).

#### License [MIT](https://github.com/xamarin/WebSharp/blob/master/LICENSE)
