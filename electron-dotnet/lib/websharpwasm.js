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
    

    var initializeClrFunc = function (options) 
    {
        var fileNameWithExt  = path.basename(options.assemblyFile);
        var fileName = path.basename(options.assemblyFile, path.extname(options.assemblyFile));

        // Note: This should probably be rethought as it will actually write the class to 
        // our implementation directory so it can be loaded from our mono_path.
        Module.FS_unlink(path.join(mount_point, fileNameWithExt));
        var asm = fs.readFileSync(options.assemblyFile);
        Module.FS_createDataFile (mount_point,fileNameWithExt, asm, true, true, true);	

        var func = getClrFuncReflectionWrapFunc(fileName, options.typeName, options.methodName);

        var jsClrWrapFunc = function (args) 
        {
            console.log('function pointer - ' + func);
            if (typeof args === 'undefined')
            {
                args = [0];
            }
            else
            {
                args = [mono_string(JSON.stringify(args))];
            }

            invokeCLRFunction(func, args);
        }

        return jsClrWrapFunc;
    }


    module.exports = { WebSharpWASMModule, onWebSharpWASMInitialized, onWebSharpWASMStarted, initializeClrFunc };

})();

