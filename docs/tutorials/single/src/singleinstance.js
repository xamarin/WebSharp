var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/Single/bin/Debug/Single.dll");
var hello = dotnet.func("./src/Single/Single.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Single', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}