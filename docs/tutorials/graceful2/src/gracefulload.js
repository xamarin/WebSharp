var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/GracefulLoad/bin/Debug/GracefulLoad.dll");
var hello = dotnet.func("./src/GracefulLoad/GracefulLoad.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('GracefulLoad', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}