var dotnet = require('electron-dotnet');

if (!require('electron-is-dev'))
	var hello = dotnet.func(__dirname + "/Location/bin/Debug/Location.dll");
else
	var hello = dotnet.func({ source: __dirname + "/Location/Location.cs", 
 						references: [], 
 						itemgroup: [__dirname + "/Location/GeoLocationAPI.cs"], 
 						symbols: ["DEV"] });

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Location', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}