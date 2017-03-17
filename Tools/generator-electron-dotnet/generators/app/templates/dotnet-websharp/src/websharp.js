var dotnet = require('electron-dotnet');

var hello = dotnet.func("./src/<%- wsClassName %>/bin/Debug/<%- wsClassName %>.dll");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Electron', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}