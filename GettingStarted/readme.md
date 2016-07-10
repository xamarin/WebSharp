
Notice
======

This workflow is just a temporary workflow while we get the effort bootstrapped, it will likely take a very
different shape in the future.   While currently this references binaries, we will be adding a source-code
mode with Roslyn, where we automatically compile the code on demand.

This is just a workflow that is being used during development.   We are open to suggestions on how to improve
this.

Getting Started
===============

Electron offers a Quick Start on their site: http://electron.atom.io/docs/tutorial/quick-start/ that explains how to get started.

Pre-requisites
---
- node.js can be found here - https://nodejs.org/
- 32 bit version of Electron is required right now - From a command prompt ```> npm install --arch=ia32 electron-prebuilt```.  This will install the 32 bit electron prebuilt which is required for now.  You can also install globally with the ```-g``` parameter.
- Visual Studio 2015 minimum Community edition
- Make sure the PepperPlugin has been built.  https://github.com/xamarin/WebSharp/blob/master/PepperPlugin/readme.md


Install Electron quick start
---

Clone and run the code in this tutorial by using the [atom/electron-quick-start](https://github.com/electron/electron-quick-start) repository.

Note: Running this requires [Git](https://git-scm.com/) and [Node.js](https://nodejs.org/en/download/) (which includes [npm](https://www.npmjs.com/)) on your system.

Change to directory ```WebSharp\GettingStarted>```

```shell
# Clone the repository
$ git clone https://github.com/electron/electron-quick-start
# Go into the repository
$ cd electron-quick-start
# Install dependencies and run the app
$ npm install --arch=ia32 && npm start
```


![elctron_quick_start](../GettingStarted/screenshots/electron-quick-start.PNG)


Building GettingStarted example
---

Open the project solution GettingStarted.sln in Visual Studio 2015 and build it.  This can also be done from the Visual Studio 2015 Command prompt as well by executing ```msbuild GettingStarted.sln```




Register our plugin with Electron
---

We now need to tell electron that we will be using plugins so to activate them we need to edit the <b>_main.js_</b> file in the electron_quick_start directory.

Right after the line that reads ```let window``` we will need to append ```'register-pepper-plugins'``` to the command line switches to tell Electron which plugin will be activated and will be referenced by ```application/electron-dotnet``` 

```javascript
// Keep a global reference of the window object, if you don't, the window will
// be closed automatically when the JavaScript object is garbage collected.
let mainWindow

// Register the dotnet plugin with Electron:
require('../../Tools/electron-dotnet').Register();

```

When we create our browser window we also need to specify that plugins will be used so in the same file find the code that reads ```mainWindow = new BrowserWindow({width: 800, height: 600})``` and lets add a reference to ```'webPreferences': { 'plugins': true }```

```javascript
  // Create the browser window.
    mainWindow = new BrowserWindow(
        {
            width: 800,
            height: 600,
            'webPreferences': {
                'plugins': true
            }
        })
```

We now have Electron ready to start loading plugins.

Embedding classes
---

All the .dll assemblies we want to load will be embedded using ```<embed></embed>```.  So lets load our .Net assembly.

Open the file named ```index.html``` and scroll down to see the code for the body.  We will be embedding our plugin instance here.

Add the following code inside the ```<body>``` tag of the html.

```html
      <div id="pluginTarget" />
      <script>
        var pluginTarget = document.getElementById("pluginTarget");
        var moduleEl = require('../../Tools/electron-dotnet').Embed({
            name: 'plugin',
            id: 'plugin',
            width: 300,
            height: 200,
            src: 'GettingStarted.HelloWorld',
            path: "..\\bin\\Debug\\"
        });
        pluginTarget.appendChild(moduleEl);

      </script>
```

The electron-dotnet's Embed method is a helper to create an ```<embed>``` tag that hosts a Module Instance implementation.

- height : The displayed height of the resource, in CSS pixels.
- src : The dot net class the implements the Module Instance that is embedded.
- path : The path where the assemblies can be found.
- width : The displayed width of the resource, in CSS pixels.

Now we can startup the application again with ```npm start``` and in the console you should see:

```
GettingStarted.HelloWorld: HelloWorld from C#
```
![GettingStarted](../GettingStarted/screenshots/GettingStarted.PNG)

If there is an error about 'plugin can not be loaded' make sure that you have the 32 bit version of Electron installed.
