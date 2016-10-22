# Welcome to your first Electron DotNet Application

## What's in the folder
* This folder contains all of the files necessary for your application
* `package.json` - this is the manifest file in which you declare your application.  The format of package.json is exactly the same as that of Node’s modules, and the script specified by the main field is the startup script of your app, which will run the `main` process.  `Note:` If the main field is not present in package.json, Electron will attempt to load an index.js.
* `main.js` - this is the main file where you will provide the implementation of your application.  The main.js should create windows and handle system events.
* `index.html` - this is the web page you want to show.
* `<%- name %>.cs` - this src file defines a C# PepperPlugin implementation that will be printed to the console by default.

## Get up and running straight away
* press `F5` to open a new window with your application loaded.
  * On Windows look for `Launch Windows` target
  * On Mac select the `Launch Mac` target
* run your command from the command terminal `npm start`.
  * (`Ctrl+Shift+P` or `Cmd+Shift+P` on Mac) and typing `View:Toggle Integrated Terminal`
  * Use the (`Ctrl+'` or `Cmd+'` on Mac) keyboard shortcut with the backtick character.
  * Use the `View | Toggle Integrated Terminal` menu command.

## Debugging
* Requires `Node.js` and the `Chrome Debugger`.
  * Install the [Chrome Debugger](https://marketplace.visualstudio.com/items?itemName=msjsdiag.debugger-for-chrome) with `ext install debugger-for-chrome`

## Explore the electron-dotnet docs
* you can view the electron-dotnet information by clicking [here](https://github.com/xamarin/WebSharp/tree/master/electron-dotnet)

