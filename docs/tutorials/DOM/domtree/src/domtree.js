var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/DOMInfo/bin/Debug/DOMInfo.dll");
var hello = dotnet.func("./src/DOMInfo/DOMInfo.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('DOMInfo', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}