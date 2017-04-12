var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/Clipper/bin/Debug/Clipper.dll");
var hello = dotnet.func("./src/Clipper/Clipper.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Clipper', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}