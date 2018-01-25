
var MonoSupportLib = {
	$MONO__postset: 'Module["pump_message"] = MONO.pump_message',
	$MONO: {
		pump_count: 0,
		pump_message: function () {
			while (MONO.pump_count > 0) {
				--MONO.pump_count;
				Module.ccall ("mono_background_exec");
			}
		}
	},

	schedule_background_exec: function () {
		++MONO.pump_count;
		if (ENVIRONMENT_IS_WEB) {
			window.setTimeout (MONO.pump_message, 0);
		}
	},
	// This routine will call back into the Module's compile routine that will
	// setup the correct wrapping of a function for execution running in Node.js
	// Also by calling it through javascript we will be able to debug certain call.
	nodeEval: function(dataPtr, is_exception) {
		var str = UTF8ToString (dataPtr);
		try {

			// Call the Module's compile routine which will return back a function
			var compiledFunc = Module.compile(str);
			// Execute the function
			var res = compiledFunc();
			if (typeof res === 'undefined' || res === null)
				return 0;
			res = res.toString ();
			setValue (is_exception, 0, "i32");
		} catch (e) {
			res = e.toString ();
			setValue (is_exception, 1, "i32");
			if (typeof res === 'undefined' || res === null)
				res = "unknown exception";
		}
		var buff = Module._malloc((res.length + 1) * 2);
		stringToUTF16 (res, buff, (res.length + 1) * 2);
		return buff;
	},
};

autoAddDeps(MonoSupportLib, '$MONO')
mergeInto(LibraryManager.library, MonoSupportLib)

