var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/Request/bin/Debug/Request.dll");
var hello = dotnet.func("./src/Request/Request.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Request', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}