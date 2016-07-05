Getting Started
===

Electron offers a Quick Start on their site: http://electron.atom.io/docs/tutorial/quick-start/ that explains how to get started.

Pre-requisites
---
- node.js can be found here - https://nodejs.org/
- 32 bit version of Electron is required right now - From a command prompt ```> npm install --arch=ia32 electron-prebuilt```.  This will install the 32 bit electron prebuilt which is required for now.  You can also install globally with the ```-g``` parameter.
- Make sure the PepperPlugin has been built.  https://github.com/xamarin/WebSharp/blob/master/PepperPlugin/readme.md


Install Electron quick start
---

Clone and run the code in this tutorial by using the atom/electron-quick-start repository.

Note: Running this requires Git and Node.js (which includes npm) on your system.

Change to directory \WebSharp\WebSharp\GettingStarted>

```
# Clone the repository
$ git clone https://github.com/electron/electron-quick-start
# Go into the repository
$ cd electron-quick-start
# Install dependencies and run the app
$ npm install --arch=ia32 && npm start
```


![elctron_quick_start](../GettingStarted/screenshots/electron-quick-start.PNG)

Register our plugin with Electron
---

We now need to tell electron that we will be using plugins so to activate them we need to edit the <b>_main.js_</b> file in the electron_quick_start directory.

Right after the line that reads ```let window``` we will need to append ```'register-pepper-plugins'``` to the command line switches to tell Electron which plugin will be activated and will be referenced by ```application/x-ppapi-PepperPlugin``` 

```javascript
// Keep a global reference of the window object, if you don't, the window will
// be closed automatically when the JavaScript object is garbage collected.
let mainWindow

// Tell electron that we will be using plugins and which one
var ppapiPath = __dirname + '\\..\\..\\PepperPlugin\\bin\\Win32\\Debug\\PepperPlugin.dll';
console.log('PPAPI path ' +  ppapiPath + ';application/x-ppapi-PepperPlugin');
app.commandLine.appendSwitch('register-pepper-plugins', ppapiPath + ';application/x-ppapi-PepperPlugin');
```

When we create our browser window we also need to specify that plugins will be used so in the same file find the code that reads ```mainWindow = new BrowserWindow({width: 800, height: 600})``` and lets add a reference to ```'webPreferences': { 'plugins': true }```

```
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

```
      <div id="pluginTarget" />
      <script>
        var pluginTarget = document.getElementById("pluginTarget");
        var moduleEl = document.createElement('embed');
        moduleEl.setAttribute('name', 'plugin');
        moduleEl.setAttribute('id', 'plugin');
        moduleEl.setAttribute('width', 300);
        moduleEl.setAttribute('height', 200);
        moduleEl.setAttribute('type', 'application/x-ppapi-PepperPlugin');
        // pepper specific attributes
        moduleEl.setAttribute('assembly', __dirname + "\\..\\bin\\Debug\\GettingStarted.dll")  // set assembly to load
        moduleEl.setAttribute('class', "GettingStarted.HelloWorld")               // set class of Plugin Instance definition
        pluginTarget.appendChild(moduleEl);

      </script>
```

The code you just added will append and ```<embed>``` tag dynamically to the html file that tells the PepperPlugin what assembly and class to load.

```
        // pepper specific attributes
        moduleEl.setAttribute('assembly', __dirname + "\\..\\bin\\Debug\\GettingStarted.dll")  // set assembly to load
        moduleEl.setAttribute('class', "GettingStarted.HelloWorld")               // set class of Plugin Instance definition
```

The workhorses of the embed element are the ```assembly``` and ```class``` attributes.

Now we can startup the application again and in the console you should see:

```
GettingStarted.HelloWorld: HelloWorld from C#
```

If there is an error about 'plugin can not be loaded' make sure that you have the 32 bit version of Electron installed.
