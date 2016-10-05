// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet


// Compile Sample105.dll with
// - on Windows (.NET Framework):
//      csc.exe /target:library /debug Sample105.cs
// - on MacOS/Linux (Mono):
//      mcs -sdk:4.5 Sample105.cs -target:library

var dotnet = require('electron-dotnet');

var add7dll = dotnet.func('./samples/Sample105.dll');

function add7_dll ()
{
	add7dll(12, function (error, result) {
		if (error) throw error;
		console.log(result);
	});
}