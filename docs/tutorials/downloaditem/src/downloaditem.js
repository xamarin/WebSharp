var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/SessionDownload/bin/Debug/SessionDownload.dll");
var hello = dotnet.func("./src/SessionDownload/SessionDownload.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('SessionDownload', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}