# Example InputEvent

Demonstrates API for input events

## Pre-requisites

- [Node.js](https://nodejs.org/en/download/) (which comes with [npm](http://npmjs.com)) installed on your computer.
- [PepperPlugin](https://github.com/xamarin/WebSharp/tree/master/PepperPlugin) and its pre-requisites.
- The samples use a prebuilt version of electron-dotnet [Building on OSX](https://github.com/xamarin/WebSharp/tree/master/electron-dotnet#building-on-osx-electron)

## To Run

```bash
# Go into the repository
cd WebSharp\Examples\api\InputEvent
```

### Windows
```bash
# Compile the example project from Visual Studio 2015 Native Tools Command Prompt 
msbuild InputEvent.csproj
```

### Mac
```bash
# Compile the example project from Mac terminal 
xbuild InputEvent.csproj
```

### Install dependencies and run the app.
```bash
# Install dependencies and run the app
npm install && npm start
```
Learn more about Electron Dotnet and its API in the [Getting Started documentation](https://github.com/xamarin/WebSharp/tree/master/GettingStarted).

#### License [MIT](https://github.com/xamarin/WebSharp/blob/master/LICENSE)
