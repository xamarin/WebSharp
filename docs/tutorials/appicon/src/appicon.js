var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/Iconic/bin/Debug/Iconic.dll");
var hello = dotnet.func("./src/Iconic/Iconic.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Iconic', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}