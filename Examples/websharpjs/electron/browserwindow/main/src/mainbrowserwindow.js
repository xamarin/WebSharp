var dotnet = require('electron-dotnet');

//var createMainWindow = dotnet.func("./src/MainWindow/bin/Debug/MainWindow.dll");
var createMainWindow = dotnet.func("./src/MainWindow/MainWindow.cs");

//Make method externaly visible this will be referenced in the main.js file
exports.createMainWindow = arg => {
	createMainWindow(arg, function (error, result) {
		if (error) throw error;
		return result;
	});
}