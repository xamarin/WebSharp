(function()
{

    var load_runtime;
    var mount_runtime;
    var assembly_load;
    var find_class;
    var find_method;
    var invoke_method;
    var mono_string_get_utf8;
    var mono_string;
    var getClrFuncReflectionWrapFunc;
    var freeClrFuncHandle;

    var fs = require("fs");
    var path = require("path");
    if(!process.env.MONO_PATH) { 
        process.env.MONO_PATH = path.resolve(__dirname, './wasm/');
    }

    var mount_point = 'mono_path';

    var onWebSharpWASMInitialized = [];
    var onWebSharpWASMStarted = [];

    var Module = {

        print: function(x) { console.log ("WASM: " + x) },
        printErr: function(x) { console.log ("WASM-ERR: " + x) },
        ENVIRONMENT: 'NODE',
        locateFile: function (module)
        {
            var pathtomodule = path.resolve(__dirname, './wasm/',module);
            return pathtomodule;
        },
        onRuntimeInitialized: function ()
        {
            if (onWebSharpWASMInitialized) {
                if (typeof onWebSharpWASMInitialized == 'function') onWebSharpWASMInitialized = [onWebSharpWASMInitialized];
                while (onWebSharpWASMInitialized.length) {
                    onWebSharpWASMInitialized.shift()();
                }
            }           
        }
    };

    Module["preRun"] = [];
    Module["postRun"] = [];

    // override the preRun
    Module['preRun'].push(function() {

        console.log('preRun');

        // it is ok to call cwrap before the runtime is loaded. we don't need the code
        // and everything to be ready, since cwrap just prepares to call code, it 
        // doesn't actually call it
        load_runtime = Module.cwrap ('mono_wasm_load_runtime', null, ['string'])
        mount_runtime = Module.cwrap ('mono_wasm_mount_runtime', null, ['string', 'string'])
        assembly_load = Module.cwrap ('mono_wasm_assembly_load', 'number', ['string'])
        find_class = Module.cwrap ('mono_wasm_assembly_find_class', 'number', ['number', 'string', 'string'])
        find_method = Module.cwrap ('mono_wasm_assembly_find_method', 'number', ['number', 'string', 'number'])
        invoke_method = Module.cwrap ('mono_wasm_invoke_method', 'number', ['number', 'number', 'number'])
        getClrFuncReflectionWrapFunc = Module.cwrap ('mono_wasm_get_clr_func_reflection_wrap_func', 'number', ['string', 'string', 'string'] )
        freeClrFuncHandle = Module.cwrap ('mono_wasm_dispose_clr_func', null, ['number']);
        invokeClrWrappedFunc = Module.cwrap ('mono_wasm_invoke_clr_wrapped_func', 'number', ['number', 'number', 'number'] )
        mono_string_get_utf8 = Module.cwrap ('mono_wasm_string_get_utf8', 'number', ['number'])
        mono_string = Module.cwrap ('mono_wasm_string_from_js', 'number', ['string'])

        
    });

    // override the postRun
    Module['postRun'].push(function() {

        console.log('postRun');

        mount_runtime(path.resolve(process.env.MONO_PATH), mount_point);
        main_module = assembly_load ("websharpwasm")
        if (!main_module)
          throw 1;

        if (onWebSharpWASMStarted) {
            if (typeof onWebSharpWASMStarted == 'function') onWebSharpWASMStarted = [onWebSharpWASMStarted];
            while (onWebSharpWASMStarted.length) {
                onWebSharpWASMStarted.shift()();
            }
        }                  


    });

    exports.Module = Module;
    
    var WebSharpWASMModule = require('./wasm/websharpwasm.js');

    function invokeCLRFunction (klass, args) 
    {
        var stack = Module.stackSave ();

        try {        
            var args_mem = Module.stackAlloc (args.length);
            var eh_throw = Module.stackAlloc (4);
            for (var i = 0; i < args.length; ++i)
                Module.setValue (args_mem + i * 4, args [i], "i32");
            Module.setValue (eh_throw, 0, "i32");
        
            var res = invokeClrWrappedFunc (klass, args_mem, eh_throw);
        
            if (Module.getValue (eh_throw, "i32") != 0) {
                Module.stackRestore(stack);
                var msg = conv_string (res);
                throw new Error (msg); //the convention is that invoke_method ToString () any outgoing exception
            }
            return res;
        }    
        finally
        {
            Module.stackRestore(stack);
        }
        
    }
    
    var linkModule = function (options)
    {
        var fileNameWithExt  = path.basename(options.assemblyFile);
        var fileName = path.basename(options.assemblyFile, path.extname(options.assemblyFile));

        //Note: This should probably be rethought as it will actually write the class to 
        //our implementation directory so it can be loaded from our mono_path.
        try
        {
            // Make sure we use the "/" to create the path for unlinking.
            // If not the there are problems on Windows environments.
            // Not sure if this is a bug and needs reporting.
            var normalized = mount_point + "/" + fileNameWithExt;
            Module.FS_unlink(normalized);
        }
        catch (e) {
            console.log(e.message);
        }

        try {
            // Then we copy the module to our mount point
            var asm = fs.readFileSync(options.assemblyFile);
            Module.FS_createDataFile (mount_point, fileNameWithExt, asm, true, true, true);	
        }
        catch (e) {
            console.log(e.message);
        }
    }

    var parseCreateOptions = function(language, options) {
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
    
        if (options.itemgroup !== undefined) {
            if (!Array.isArray(options.itemgroup)) {
                throw new Error('The itemgroup property must be an array of strings.');
            }
    
            options.itemgroup.forEach(function (ref) {
                if (typeof ref !== 'string') {
                    throw new Error('The itemgroup property must be an array of strings.');
                }
            });
        }
    
        if (options.symbols !== undefined) {
            if (!Array.isArray(options.symbols)) {
                throw new Error('The symbols property must be an array of strings.');
            }
    
            options.symbols.forEach(function (ref) {
                if (typeof ref !== 'string') {
                    throw new Error('The symbols property must be an array of strings.');
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
    
        return options;
    };

    createClass = function(language, options)
    {

        try
        {
            return new ClrWrappedFunc(language, options);
        }
        catch (e)
        {
            throw new Error(
                "--- WebSharp: websharpwasm.js -- Loading assembly " + JSON.stringify(options) + "\n" + e.name + ': ' + e.message
            );
        }

    }    

    class ClrWrappedFunc
    {
        constructor(language, options)
        {
            var options = parseCreateOptions(language, options);

            var fileNameWithExt  = path.basename(options.assemblyFile);
            var fileName = path.basename(options.assemblyFile, path.extname(options.assemblyFile));
    
            linkModule(options);
    
            this._handle = getClrFuncReflectionWrapFunc(fileName, options.typeName, options.methodName);
    
            this._isDisposed = false;
        }

        get handle()
        {
            return this._handle;
        }

        get isDisposed()
        {
            return this._isDisposed;
        }

        invoke(args)
        {
            if (this._isDisposed)
            {
                throw "The object referenced by handle < " + this.handle + " > has been disposed.";
            }
            console.log("we are invoking handle < " + this._handle + " >");
            if (typeof args === 'undefined')
            {
                args = [0];
            }
            else
            {
                args = [mono_string(JSON.stringify(args))];
            }

            invokeCLRFunction(this._handle, args);
        }

        dispose()
        {
            console.log("Disposing object that is referenced by handle < " + this._handle + " >");
            freeClrFuncHandle(this._handle);
            this._isDisposed = true;
        }

    }

    var initializeClrFunc = function (options) 
    {
        var fileNameWithExt  = path.basename(options.assemblyFile);
        var fileName = path.basename(options.assemblyFile, path.extname(options.assemblyFile));

        linkModule(options);

        var func = getClrFuncReflectionWrapFunc(fileName, options.typeName, options.methodName);

        var jsClrWrapFunc = new ClrWrappedFunc(func);
        return jsClrWrapFunc;
    }


    module.exports = { WebSharpWASMModule, 
                        onWebSharpWASMInitialized, 
                        onWebSharpWASMStarted, 
                        initializeClrFunc,
                        createClass,
                        ClrWrappedFunc
                      };

})();

