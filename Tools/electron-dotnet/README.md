# electron-dotnet

Provides helper methods to register and embed dotnet plugins in Electron.

Usage:
---

Register the dotnet plugin with Electron:

```javascript
require("electron-dotnet").Register();
```


To embed a Module Instance:

```javascript
      <script>
        var pluginTarget = document.getElementById("pluginTarget");
        var moduleEl = require('electron-dotnet').Embed({
            name: 'plugin',
            id: 'plugin',
            width: 300,
            height: 200,
            src: 'GettingStarted.HelloWorld',
            path: "..\\bin\\Debug\\"
        });
        pluginTarget.appendChild(moduleEl);
```

The Embed method is a helper to create an ```<embed>``` tag that hosts a Module Instance implementation.

- height : The displayed height of the resource, in CSS pixels.
- src : The dot net class the implements the Module Instance that is embedded.
- path : The path where the assemblies can be found.
- width : The displayed width of the resource, in CSS pixels.


See the WebSharp project's page [Getting Started](https://github.com/xamarin/WebSharp/tree/master/GettingStarted) for more information.
