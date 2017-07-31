var dotnet = require('electron-dotnet');

//var hello = dotnet.func(__dirname + "/Location/bin/Debug/Location.dll");
var hello = dotnet.func({ source: __dirname + "/Location/Location.cs", 
						references: [], 
						itemgroup: ["./src/Location/GeoLocationAPI.cs"], 
						symbols: [] });

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('Location', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}