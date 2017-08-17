var dotnet = require('electron-dotnet');

//var hello = dotnet.func(__dirname + "/Dragger/bin/Debug/Dragger.dll");
var hello = dotnet.func(__dirname + "/Dragger/Dragger.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Dragger', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}