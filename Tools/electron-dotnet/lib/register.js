module.exports = Register;

function Register(pluginPath) {

    
    electron = require('electron')
    const app = electron.app

    const isWindows = process.platform === 'win32';

    var ppapiPath = '';
    if (isWindows)
        ppapiPath = __dirname + '\\..\\bin\\Win32\\PepperPlugin.dll';
    if (pluginPath)
        ppapiPath = pluginPath + '\\PepperPlugin.dll';
    //console.log('PPAPI path ' + ppapiPath + ';application/electron-dotnet');
    app.commandLine.appendSwitch('register-pepper-plugins', ppapiPath + ';application/electron-dotnet');

}