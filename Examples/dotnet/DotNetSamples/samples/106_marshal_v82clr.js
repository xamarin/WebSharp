// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet


var dotnet = require('electron-dotnet');

var hellov82clr = dotnet.func(function () {/*
	using System.Collections.Generic;

	async (data) =>
	{
		Console.WriteLine("-----> In .NET:");
		foreach (var kv in (IDictionary<string,object>)data)
		{
			Console.WriteLine(kv.Key + ": " + kv.Value.GetType());
		}

		return null;
	}
*/});

var payload = {
	anInteger: 1,
	aNumber: 3.1415,
	aString: 'foobar',
	aBool: true,
	anObject: {},
	anArray: [ 'a', 1, true ],
	aBuffer: new Buffer(1024)
}

function hello_v82clr ()
{
	console.log('-----> In node.js:');
	console.log(payload);

	hellov82clr(payload, function (error, result) {
		if (error) throw error;
	});
}
