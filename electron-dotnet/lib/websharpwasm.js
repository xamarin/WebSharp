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
    var path = require("path");
    if(!process.env.MONO_PATH) { 
        process.env.MONO_PATH = path.resolve(__dirname, './wasm/');
    }

    var Module = {

        print: function(x) { console.log ("WASM: " + x) },
        printErr: function(x) { console.log ("WASM-ERR: " + x) },
        ENVIRONMENT: 'NODE',
        locateFile: function (module)
        {
            var pathtomodule = path.resolve(__dirname, './wasm/',module);
            return pathtomodule;
        },
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
        mono_string_get_utf8 = Module.cwrap ('mono_wasm_string_get_utf8', 'number', ['number'])
        mono_string = Module.cwrap ('mono_wasm_string_from_js', 'number', ['string'])
        
    });

    // override the postRun
    Module['postRun'].push(function() {

        console.log('postRun');

        mount_runtime(path.resolve(process.env.MONO_PATH), 'mono_path');
        main_module = assembly_load ("websharpwasm")
        if (!main_module)
          throw 1;
    });

    exports.Module = Module;
    
    var WebSharpWASMModule = require('./wasm/websharpwasm.js');

    module.exports = { WebSharpWASMModule };

})();

