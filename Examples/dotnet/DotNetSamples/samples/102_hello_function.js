// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet

var dotnet = require('electron-dotnet');

var hellofunction = dotnet.func(function () {/*
	async (input) => 
	{ 
		return ".NET welcomes " + input.ToString(); 
	}
*/});

function hello_function()
{
	hellofunction('Electron', function (error, result) {
		if (error) throw error;
		console.log(result);
	});
}