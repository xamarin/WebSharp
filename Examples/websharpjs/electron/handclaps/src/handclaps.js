var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/ClapsRenderer/bin/Debug/ClapsRenderer.dll");
var hello = dotnet.func("./src/ClapsRenderer/ClapsRenderer.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('ClapsRenderer', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}