var dotnet = require('electron-dotnet');

var notifications = dotnet.func({
        assemblyFile: __dirname + '/bin/Debug/net451/' 
                + ((process.platform === 'darwin') ? 'osx.10.12-x64' : 'win10-x64') 
                + '/publish/notifications.dll',
        typeName: 'Notifications.Notifications',
        methodName: 'AddNotifications' // This must be Func<object,Task<object>>
    });

//Make method externaly visible this will be referenced in the renderer.js file
exports.addNotifications = arg => {
    let parms = {};
    parms.dirname = __dirname;  // We want to pass the __dirname as a parameter to be referenced for loading image files.

	notifications(parms, function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}