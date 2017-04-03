var fs = require('fs')
    , path = require('path')
    , builtWebSharp = path.resolve(__dirname, '../build/Release/' + 'websharp_monoclr.node')
    , websharp;

var targetMap = [
    [ /^1\.4/, '1.4.0' ],
    [ /^1\.5/, '1.5.0' ],
    [ /^1\.6/, '1.6.0' ],    
];

function determineTarget() {
    for (var i in targetMap) {
        if (process.versions.electron.match(targetMap[i][0])) {
            return targetMap[i][1];
        }
    }

    throw new Error('The websharp module has not been pre-compiled for Electron version ' + process.versions.electron +
        '. You must build a custom version of websharp.node. Please refer to your platform at https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md for MacOSX or https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md for Windows. ' +
        'for building instructions.');
}


var versionMap = [
    [ /^6\./, '6.5.0' ],
    [ /^7\.[0-3]/, '7.0.0' ],
    [ /^7\.4/, '7.4.0' ],
];

function determineVersion() {
    for (var i in versionMap) {
        if (process.versions.node.match(versionMap[i][0])) {
            return versionMap[i][1];
        }
    }

    throw new Error('The websharp module has not been pre-compiled for node.js version ' + process.version +
        '. You must build a custom version of websharp.node. Please refer to your platform at https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md for MacOSX or https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md for Windows. ' +
        'for building instructions.');
}

function whereis () {
    var pathSep = process.platform === 'win32' ? ';' : ':';

    var directories = process.env.PATH.split(pathSep);

    for (var i = 0; i < directories.length; i++) {
    	for (var j = 0; j < arguments.length; j++) {
    		var filename = arguments[j];
	        var filePath = path.join(directories[i], filename);

	        if (fs.existsSync(filePath)) {
	            return filePath;
	        }
	    }
    }

    return null;
}

var websharpNative;
if (process.platform === 'win32') {
    if (!whereis("mono.exe"))
    {
        throw new Error('The websharp module for using mono embedding has been specified but mono can not be found' +
        '. You must build a custom version of websharp.node for using mono embedding or make sure that mono is in your %PATH%.' +
        ' Please refer to https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md for building instructions.');
    }
    
    process.env.WEBSHARP_NATIVE = path.resolve(__dirname, './native/' + process.platform + '/' + process.arch + '/' + determineTarget() + '/' + determineVersion() + '/' + 'websharp_monoclr');
}

if (process.env.WEBSHARP_NATIVE) {
    websharpNative = process.env.WEBSHARP_NATIVE;
}
else if (fs.existsSync(builtWebSharp)) {
    websharpNative = builtWebSharp;
}
else if (process.platform === 'win32') {
    websharpNative = path.resolve(__dirname, './native/' + process.platform + '/' + process.arch + '/' + determineTarget() + '/' + determineVersion() + '/' + 'websharp_monoclr');
}
else {
    throw new Error('The websharp native module is not available at ' + builtWebSharp 
        + '. You can use WEBSHARP_NATIVE environment variable to provide alternate location of websharp.node. '
        + 'If you need to build websharp.node, follow build instructions for your platform at https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-mac.md for MacOSX or https://github.com/xamarin/WebSharp/blob/master/docs/getting-started/getting-started-dev-windows.md for Windows.');
}
if (process.env.WEBSHARP_DEBUG) {
    console.log('Load websharp native library from: ' + websharpNative);
}

process.env.WEBSHARP_NATIVE = websharpNative;
websharp = require(websharpNative);
exports.Register = require("./register.js")
exports.Embed = require("./embed.js")
exports.WebSharpJs = require("./websharpjs.js")
exports.func = function(language, options) {
    if (!options) {
        options = language;
        language = 'cs';
    }

    if (typeof options === 'string') {
        if (options.match(/\.dll$/i)) {
            options = { assemblyFile: options };
        }
        else {
            options = { source: options };
        }
    }
    else if (typeof options === 'function') {
        var originalPrepareStackTrace = Error.prepareStackTrace;
        var stack;
        try {
            Error.prepareStackTrace = function(error, stack) {
                return stack;
            };
            stack = new Error().stack;
        }
        finally
        {
            Error.prepareStackTrace = originalPrepareStackTrace;
        }
        
        options = { source: options, jsFileName: stack[1].getFileName(), jsLineNumber: stack[1].getLineNumber() };
    }
    else if (typeof options !== 'object') {
        throw new Error('Specify the source code as string or provide an options object.');
    }

    if (typeof language !== 'string') {
        throw new Error('The first argument must be a string identifying the language compiler to use.');
    }
    else if (!options.assemblyFile) {
        
        // use the internal websharp-cs module for C# modules.
        if (language !== 'cs' || !fs.existsSync(path.resolve(__dirname, '../lib/bin/websharp-cs/websharp-cs.dll')))
        {
            var compilerName = 'edge-' + language.toLowerCase();
            var compiler;
            try {
                compiler = require(compilerName);
            }
            catch (e) {
                throw new Error("Unsupported language '" + language + "'. To compile script in language '" + language +
                    "' an npm module '" + compilerName + "' must be installed.");
            }

            try {
                options.compiler = compiler.getCompiler();
            }
            catch (e) {
                throw new Error("The '" + compilerName + "' module required to compile the '" + language + "' language " +
                    "does not contain getCompiler() function.");
            }
        
            if (typeof options.compiler !== 'string') {
                throw new Error("The '" + compilerName + "' module required to compile the '" + language + "' language " +
                    "did not specify correct compiler package name or assembly.");
            }

            // Set the Class name to load from the compiler assembly
            options.compilerClass = 'EdgeCompiler'
        }
        else {
            // We set the compiler to our internal websharp-cs.dll
            options.compiler = path.resolve(__dirname, '../lib/bin/websharp-cs/websharp-cs.dll');
            // Set the Class name to load from the compiler assembly
            options.compilerClass = 'WebSharpCompiler';
        }

    }

    if (!options.assemblyFile && !options.source) {
        throw new Error('Provide DLL or source file name or .NET script literal as a string parmeter, or specify an options object '+
            'with assemblyFile or source string property.');
    }
    else if (options.assemblyFile && options.source) {
        throw new Error('Provide either an asseblyFile or source property, but not both.');
    }

    if (typeof options.source === 'function') {
        var match = options.source.toString().match(/[^]*\/\*([^]*)\*\/\s*\}$/);
        if (match) {
            options.source = match[1];
        }
        else {
            throw new Error('If .NET source is provided as JavaScript function, function body must be a /* ... */ comment.');
        }
    }

    if (options.references !== undefined) {
        if (!Array.isArray(options.references)) {
            throw new Error('The references property must be an array of strings.');
        }

        options.references.forEach(function (ref) {
            if (typeof ref !== 'string') {
                throw new Error('The references property must be an array of strings.');
            }
        });
    }

    if (options.assemblyFile) {
        if (!options.typeName) {
            var matched = options.assemblyFile.match(/([^\\\/]+)\.dll$/i);
            if (!matched) {
                throw new Error('Unable to determine the namespace name based on assembly file name. ' +
                    'Specify typeName parameter as a namespace qualified CLR type name of the application class.');
            }

            options.typeName = matched[1] + '.Startup';
        }
    }
    else if (!options.typeName) {
        options.typeName = "Startup";
    }

    if (!options.methodName) {
        options.methodName = 'Invoke';
    }

    return websharp.initializeClrFunc(options);
};

var initialize = exports.func({
    assemblyFile: __dirname + '/bin/WebSharpJs.dll',
    typeName: 'WebSharpJs.WebSharp',
    methodName: 'InitializeInternal'
});

var compileFunc = function (data, callback) {
    var wrapper = '(function () { ' + data + ' })';
    var funcFactory = eval(wrapper);
    var func = funcFactory();
    if (typeof func !== 'function') {
        throw new Error('Node.js code must return an instance of a JavaScript function. '
            + 'Please use `return` statement to return a function.');
    }

    callback(null, func);
};

initialize(compileFunc, function (error, data) {
    if (error) throw error;
});
