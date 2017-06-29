var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/ElementTinkerer/bin/Debug/ElementTinkerer.dll");
var hello = dotnet.func("./src/ElementTinkerer/ElementTinkerer.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('ElementTinkerer', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}