(function()
{
    "use strict";

    let v8Util = process.atomBinding('v8_util');

    const websharpObjectCache = v8Util.createIDWeakMap();
    let websharpObjId = 0;

    // https://github.com/jprichardson/is-electron-renderer
    function IsRenderer () {
        // running in a web browser
        if (typeof process === 'undefined') return true

        // node-integration is disabled
        if (!process) return true

        // We're in node.js somehow
        if (!process.type) return false

        return process.type === 'renderer'
    }

    var RegisterScriptableObject = function (meta)
    {
        let id = v8Util.getHiddenValue(meta, 'websharpId');
        // We indiscriminately replace the object that is there.
        if (id) {
            websharpObjectCache.set(id, meta);
        }
        else
        {
            id = websharpObjId++;
            v8Util.setHiddenValue(meta, 'websharpId', id);
            websharpObjectCache.set(id, meta);
        }
        return id;

    }

    var GetScriptableObject = function (id) {
        //console.log('get scriptobject with ' + id);
        return websharpObjectCache.get(id);

    }

    var UnRegisterScriptableObject = function (key) {

        console.log('key ' + key );

    }

    var WrapEvent = function (event)
    {
        eventSrcID = (event.srcElement) ? event.srcElement : 'undefined';
        eventtype = event.type;
        status = eventSrcID + ' has received a ' + eventtype + ' event.';
        console.log(event);
        console.log(status);
        buttonname=new Array('Left','Right','','Middle');
            message='button : '+event.button+'\n';
            message+='altKey : '+event.altKey +'\n';
            message+='ctrlKey : '+event.ctrlKey +'\n';
            message+='shiftKey : '+event.shiftKey +'\n';
            console.log(message);
    }

    var UnwrapArgs = function (args) {

        const callbackArg = function(arg)
        {
            // We will treat Error differently
            if (arg instanceof Error)
            {
                let scriptableType = {};
                Object.getOwnPropertyNames(arg).forEach(function (k) {
                    scriptableType[k] = arg[k];
                });
                
                return scriptableType;
            }

            if (arg === Object(arg))
                return ObjectToScriptObject(arg);
            else
                return arg;
        }

        const parmToCallback = function (func) {
            return function () {
                
                // We need to preserver arity of the function callback parameters.
                let callbackData;
                // If we get called with arguments then we loop over them to create on object to be passed back
                // as our callback data.  Func<object, Task<object>>
                // We only receive one object in our managed code.
                if (arguments)
                {
                    if (arguments.length === 1)
                    {
                        let args = arguments;
                        if (Array.isArray(args[0]))
                        {
                            let argArray = []
                            for (var ai = 0; ai < args[0].length; ai++)
                            {
                                argArray.push(callbackArg(args[0][ai]));
                            }
                            callbackData = argArray;
                            try {
                                // ** Note **: We are using `this` for the scope of the fuction. This may need to be looked at
                                // later.
                                func.Value.apply(this, callbackData);
                            }
                            catch (ex) { ErrorHandler.Exception(ex); }
                        }
                        else
                        {
                            callbackData = callbackArg(args[0]);
                            try {
                                // ** Note **: We are using `this` for the scope of the fuction. This may need to be looked at
                                // later.
                                func.Value.apply(this, [callbackData]);
                            }
                            catch (ex) { ErrorHandler.Exception(ex); }
                        }
                    }
                    else
                    {
                        callbackData = [];
                        var args = Array.from(arguments);
                        for (var i = 0; i < args.length; i++) {
                            if (Array.isArray(args[i]))
                            {
                                let argArray = []
                                for (var ai = 0; ai < args[i].length; ai++)
                                {
                                    argArray.push(callbackArg(args[i][ai]));
                                }
                                callbackData.push(argArray);
                            }
                            else
                            {
                                callbackData.push(callbackArg(args[i]));
                            }
                        }
                        try {
                            // ** Note **: We are using `this` for the scope of the fuction. This may need to be looked at
                            // later.
                            func.Value.apply(this, [callbackData]);
                        }
                        catch (ex) { ErrorHandler.Exception(ex); }
                        }

                }
                else
                    try {
                        // ** Note **: We are using `this` for the scope of the fuction. This may need to be looked at
                        // later.
                        func.Value.apply(this);
                    }
                    catch (ex) { ErrorHandler.Exception(ex); }
                
            };

        }

        const parmToMeta = function (value) {
            if (value) {
                switch (value.Category) {
                    case 1:
                        return GetScriptableObject(value.Value);
                    case 2:
                        let stp = value.Value;
                        let scriptableType = {};
                        Object.getOwnPropertyNames(stp).forEach(function (k) {
                            scriptableType[k] = parmToMeta(stp[k]);
                        });
                        return scriptableType;
                    case 4:
                        return parmToCallback(value);
                    case 6:
                        let stpArray = value.Value;
                        let scriptableTypeArray = [];
                        for (var ai = 0; ai < stpArray.length; ai++)
                        {
                            scriptableTypeArray.push(parmToMeta(stpArray[ai]));
                        }
                        // Object.getOwnPropertyNames(stp).forEach(function (k) {
                        //     scriptableType[k] = parmToMeta(stp[k]);
                        // });
                        return scriptableTypeArray;
                    default:
                        return value.Value;
                }
            }
            return value;
        }

        let options = [];
        if (args) {
            var arrayLength = args.length;
            for (var i = 0; i < arrayLength; i++) {
                options.push(parmToMeta(args[i]));
            }
        }
        return options;
    }

    var WrapResult = function (result, parm) {

        const resultToObjectCollection = function (result) {
            let returnCollection = [];
            // https://www.w3.org/TR/dom/#htmlcollection
            // We need to have a special consideration for htmlcollection.
            // From docs HTMLCollection is an historical artifact we cannot rid the web of.
            // It is not an array either so will not be handled by the native code interface.
            // * Note * : This test here works for Chrome but may not be compatible for other
            // javascript runtimes.
            if (({}).toString.call(result) === '[object HTMLCollection]')
            {
                for (var i = 0; i < result.length; i++) {
                    returnCollection.push(ObjectToScriptObject(result[i]));
                }
            }
            else
            {
                for (var i=0; i < result.length; i++) {
                    returnCollection.push(ObjectToScriptObject(result[i]));
                }
            }

            return returnCollection;
        }

        let returnSO;
        if (result) {
            if (parm.category === 5) // ScriptObjectCollection
            {
                returnSO = resultToObjectCollection(result);
            }
            else {
                returnSO = ObjectToScriptObject(result);
            }            
        }
        return returnSO;
    }

    var ObjectToScriptObject = function (objToWrap)
    {

        let id = RegisterScriptableObject(objToWrap);
        let proxy = {};

        proxy.websharp_id = id;
        proxy.websharp_get_property = function (prop, cb) {
            //console.log('prop -> ' + prop.name + ' has property ' + objToWrap.hasOwnProperty(prop.name)  + ' [ ' + objToWrap[prop.name] + ' ]');
            let objProp = objToWrap[prop.name];

            if (prop.category > 0 && objProp != null )
            {
                let resultSO = WrapResult(objProp, prop);
                cb(null, resultSO);

            }
            else
                cb(null, objProp);
        }

        proxy.websharp_set_property = function (parms, cb) {
            let result = false;

            if (parms.createIfNotExists === true) {
                objToWrap[parms.property] = parms.value;
                result = true;
            }
            else {
                result = false;
                if (parms.hasOwnProperty === true) {
                    if (objToWrap.hasOwnProperty(parms.property)) {
                        objToWrap[parms.property] = parms.value;
                        result = true;
                    }
                }
                else {
                    objToWrap[parms.property] = parms.value;
                    result = true;
                }

            }
            cb(null, result);
        }

        proxy.websharp_get_attribute = function (prop, cb) {
            //console.log('attribute -> ' + prop + ' [ ' + objToWrap.getAttribute(prop) + ' ]');
            cb(null, objToWrap.getAttribute(prop));
        }

        proxy.websharp_set_attribute = function (prop, cb) {
            //console.log('set attribute -> ' + prop.name + ' [ ' + prop.value + ' ]');
            objToWrap.setAttribute(prop.name, prop.value);
            cb(null, true);
        }

        proxy.websharp_get_style_attribute = function (prop, cb) {
            //console.log('style attribute -> ' + prop + ' [ ' + objToWrap.style[prop] + ' ]');
            cb(null, objToWrap.style[prop]);
        }

        proxy.websharp_set_style_attribute = function (prop, cb) {
            //console.log('set style attribute -> ' + prop.name + ' [ ' + prop.value + ' ]');
            objToWrap.style[prop.name] = prop.value;
            cb(null, true);
        }

        proxy.websharp_invoke = function (parms, cb) {
            //console.log('invoking -> ' + parms.function + ' has function ' + (typeof objToWrap[parms.function] === 'function') + ' args [ ' + parms.args + ' ]');
            let invokeResult;

            if (typeof objToWrap[parms.function] === 'function') {

                let args = UnwrapArgs(parms.args);

                try{
                    invokeResult = objToWrap[parms.function].apply(objToWrap, args);
                }
                catch (ex)
                {
                    console.error(ex.message);
                    throw ex;
                }

                if (parms.category > 0 && invokeResult != null)
                {
                    let returnSO = WrapResult(invokeResult, parms);
                    cb(null, returnSO);
                }
                else
                    cb(null, invokeResult);
            }
            else
                cb('Invoke error: Function \'' + parms.function + '\' not found when. ', invokeResult);

        }

        proxy.websharp_addEventListener = function (eventCallback, cb) {
            //console.log('addEventListener -> ' + eventCallback.onEvent);
            objToWrap.addEventListener(eventCallback.onEvent, 
                function () {
                    //console.log('event triggered');
                    // We need to preserver arity of the function callback parameters.
                    let callbackData = [];
                    // we will only attach event information if it was asked for
                    if (eventCallback.handlerType) {
                        // If we get called with arguments then we loop over them to create on object to be passed back
                        // as our callback data.  Func<object, Task<object>>
                        // We only receive one object in our managed code.
                        if (arguments) {
                            var args = Array.from(arguments);
                            for (var i = 0; i < args.length; i++) {
                                //console.log(typeof args[i]);
                                var type = args[i].type;
                                if (args[i] instanceof Event) {
                                    //console.log('Event be defined ');
                                    var event = {};
                                    event['eventType'] = args[i].type;
                                    event['preventDefault'] = args[i].preventDefault;
                                    event['stopPropogation'] = args[i].stopPropogation;
                                    DOMEventProps.forEach(function (element) {
                                        event[element] = args[i][element];
                                    });
                                    callbackData.push(event);
                                    
                                }
                                else
                                    callbackData.push(args[i]);
                            }
                        }
                    }

                    try {
                        // ** Note **: We are using `this` for the scope of the fuction. This may need to be looked at
                        // later.
                        eventCallback.callback.apply(this, [callbackData]);
                    }
                    catch (ex) { ErrorHandler.Exception(ex); }
                
                }, false);
           
            cb(null, true);
        }

        proxy.websharp_proxied_object = function (data, cb) {
            cb(null, proxy.websharp_id);

        }

        return proxy;
    }


    var DOMEventProps = ["altKey",
        "bubbles",
        "cancelable",
        "changedTouches",
        "ctrlKey",
        "detail",
        "eventPhase",
        "metaKey",
        "pageX",
        "pageY",
        "shiftKey",
        "view",
	    "char",
        "charCode",
        "key",
        "keyCode",
        "button",
        "buttons",
        "clientX",
        "clientY",
        "offsetX",
        "offsetY",
        "pointerId",
        "pointerType",
        "screenX",
        "screenY",
        "targetTouches",
        "toElement",
        "touches"]

    module.exports = { RegisterScriptableObject, GetScriptableObject, UnRegisterScriptableObject, WrapEvent, UnwrapArgs, ObjectToScriptObject, IsRenderer };

})();