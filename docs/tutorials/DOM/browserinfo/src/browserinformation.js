var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/Information/bin/Debug/Information.dll");
var hello = dotnet.func("./src/Information/Information.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Information', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}