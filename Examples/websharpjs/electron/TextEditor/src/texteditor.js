var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/DialogHelpers/bin/Debug/DialogHelpers.dll");
var hello = dotnet.func("./src/DialogHelpers/DialogHelpers.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('DialogHelpers', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}