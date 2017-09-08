var dotnet = require('electron-dotnet');

if (!require('electron-is-dev'))
	var hello = dotnet.func(require('path').join(__dirname, "/ConvertUI/bin/Debug/ConvertUI.dll").replace('app.asar', 'app.asar.unpacked'));
else
	var hello = dotnet.func({source : __dirname + "/ConvertUI/ConvertUI.cs", symbols: ["DEV"]});


//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('ConvertUI', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}