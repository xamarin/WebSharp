var dotnet = require('electron-dotnet');

//var hello = dotnet.func("./src/PdfRenderer/bin/Debug/PdfRenderer.dll");
var hello = dotnet.func("./src/PdfRenderer/PdfRenderer.cs");

//Make method externaly visible this will be referenced in the renderer.js file
exports.sayHello = arg => {
	hello('PdfRenderer', function (error, result) {
		if (error) throw error;
		if (result) console.log(result);
	});
}