try {
	if (process.versions.electron !== undefined)
		require('../lib/electron-dotnet.js');
}
catch (e) {
	console.log('***************************************');
	console.log(e);
	console.log('***************************************');
}

console.log('Success: platform check for electron-dotnet.js: node.js ' + process.arch + ' v' + process.versions.node );
