// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet

var dotnet = require('electron-dotnet');

var helloclr2v8 = dotnet.func(function () {/*
	async (input) => 
	{
		var result = new {
			anInteger = 1,
			aNumber = 3.1415,
			aString = "foobar",
			aBool = true,
			anObject = new { a = "b", c = 12 },
			anArray = new object[] { "a", 1, true },
			aBuffer = new byte[1024]
		};

		return result;
	}
*/});

function hello_clr2v8 () 
{
	helloclr2v8(null, function (error, result) {
		if (error) throw error;
		console.log('-----> In node.js:');
		console.log(result);
	});
}
