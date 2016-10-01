module.exports = Embed;

function Embed(attributes) {

    const path = require('path');

    var appPath = process.cwd();

    // Create our embed tag to load the pepper module instance
    var moduleEl = document.createElement('embed');
    for (var att in attributes) {
        if (att.toLowerCase() === 'path')
            appPath = attributes[att];
        else
            moduleEl.setAttribute(att, attributes[att]);
    }

    if (!path.isAbsolute(appPath))
        appPath = path.join(process.cwd(), appPath);

    moduleEl.setAttribute('type', 'application/electron-dotnet');
    moduleEl.setAttribute('path', appPath);  // set assembly to load
    return moduleEl;
}