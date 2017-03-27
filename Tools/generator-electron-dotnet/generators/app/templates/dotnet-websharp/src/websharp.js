var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/<%- wsClassName %>/bin/Debug/<%- wsClassName %>.dll");
var hello = dotnet.func("./src/<%- wsClassName %>/<%- wsClassName %>.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('<%- wsClassName %>', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}