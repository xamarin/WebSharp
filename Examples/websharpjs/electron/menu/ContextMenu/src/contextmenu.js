var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/ContextMenuRenderer/bin/Debug/ContextMenuRenderer.dll");
var createMain = dotnet.func("./src/ContextMenuRenderer/ContextMenuRenderer.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.createMainWindow = arg => {
	createMain(arg, function (error, result) {
		if (error) throw error;
		return result;
	});
}