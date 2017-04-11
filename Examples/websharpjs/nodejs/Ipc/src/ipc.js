var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/Ping/bin/Debug/Ping.dll");
var hello = dotnet.func("./src/Ping/Ping.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Ping', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}