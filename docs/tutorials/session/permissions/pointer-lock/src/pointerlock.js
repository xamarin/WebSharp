var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/Pointer/bin/Debug/Pointer.dll");
var hello = dotnet.func("./src/Pointer/Pointer.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Pointer', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}