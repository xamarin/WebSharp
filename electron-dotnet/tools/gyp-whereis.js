// Utility module used in binding.gyp to search for the existance of a file 
// in the path.
var fs = require('fs');
var path = require('path');

module.exports = function() {
    var pathSep = process.platform === 'win32' ? ';' : ':';
    var directories = process.env.PATH.split(pathSep);

    for (var i = 0; i < directories.length; i++) {
    	for (var j = 0; j < arguments.length; j++) {
    		var filename = arguments[j];
	        var filePath = path.join(directories[i], filename);

	        if (fs.existsSync(filePath)) {
				console.log(filePath);
				return filePath;
	        }
	    }
    }
	console.log(null);
    return null;
}