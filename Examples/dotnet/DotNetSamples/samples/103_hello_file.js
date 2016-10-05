// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet

var dotnet = require('electron-dotnet');

var hellofile = dotnet.func('./samples/103_hello_file.csx');

function hello_file()
{
	hellofile('Electron', function (error, result) {
		if (error) throw error;
		console.log(result);
	});
}