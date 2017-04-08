var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/Emitter/bin/Debug/Emitter.dll");
var hello = dotnet.func("./src/Emitter/Emitter.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Emitter', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}