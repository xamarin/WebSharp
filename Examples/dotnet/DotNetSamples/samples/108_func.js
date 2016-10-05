// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet


var dotnet = require('electron-dotnet');

var addAndMultiplyBy2 = dotnet.func(function () {/*
	using System.Collections.Generic;

	async (dynamic data) => 	
	{
		int sum = (int)data.a + (int)data.b;
		var multiplyBy2 = (Func<object,Task<object>>)data.multiplyBy2;
		return await multiplyBy2(sum);
	}
*/});

var payload = {
	a: 2,
	b: 3,
	multiplyBy2: function(input, callback) {
		callback(null, input * 2);
	}
};

function addAndMultiply_By2()
{
	addAndMultiplyBy2(payload, function (error, result) {
		if (error) throw error;
		console.log(result);
	});
}
