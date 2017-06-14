var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/MinimizeToTray/bin/Debug/MinimizeToTray.dll");
var hello = dotnet.func("./src/MinimizeToTray/MinimizeToTray.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('MinimizeToTray', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}