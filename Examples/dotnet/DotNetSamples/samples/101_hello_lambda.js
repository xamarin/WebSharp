// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet

var dotnet = require('electron-dotnet');

var hellolambda = dotnet.func('async (input) => { return ".NET welcomes " + input.ToString(); }');

function hello_lambda()
{
	hellolambda('Electron', function (error, result) {
		if (error) throw error;
		console.log(result);
	});
}

