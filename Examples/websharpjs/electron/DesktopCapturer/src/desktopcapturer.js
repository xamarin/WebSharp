var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/Capture/bin/Debug/Capture.dll");
var hello = dotnet.func("./src/Capture/Capture.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Capture', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}