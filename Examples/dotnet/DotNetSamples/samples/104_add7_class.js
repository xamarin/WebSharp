// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet

var dotnet = require('electron-dotnet');

var add7class = dotnet.func(function () {/*
	using System.Threading.Tasks;

	public class Startup
	{
		public async Task<object> Invoke(object input)
		{
			return this.Add7((int)input);
		}

		int Add7(int v) 
		{
			return Helper.Add7(v);
		}
	}

	static class Helper
	{
		public static int Add7(int v)
		{
			return v + 7;
		}
	}
*/});

function add7_class()
{
	add7class(12, function (error, result) {
		if (error) throw error;
		console.log(result);
	});
}