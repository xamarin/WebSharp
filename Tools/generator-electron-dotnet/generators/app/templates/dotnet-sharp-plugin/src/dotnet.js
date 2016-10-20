var dotnet = require('electron-dotnet');

var hello = dotnet.func('async (input) => { return ".NET welcomes " + input.ToString(); }');

//Make method externaly visible this will be referenced in the render.js file
exports.sayHello = arg => {
	hello('Electron', function (error, result) {
		if (error) throw error;
		console.log(result);
	});
}