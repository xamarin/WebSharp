// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet


var dotnet = require('electron-dotnet');

var hellosync = dotnet.func('async (input) => { return ".NET welcomes " + input.ToString(); }');

function hello_sync()
{
	// call the function synchronously
	var result = hellosync('Electron synchronously', true);
	console.log(result);

	// call the same function asynchronously
	hellosync('Electron asynchronously', function (error, result) {
		if (error) throw error;
		console.log(result);
	});
}
