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
        // if the meta object is null/undefined then just return
        // fixes errors on static methods like Menu.setApplicationMenu
        // that will not return back an object.
        if (!meta)
            return;

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

        var originalEvent = null;

        const callbackArg = function (arg, metaMap = { Category: 3, IsArray: 0 })
        {

            if (metaMap.Category === 6 && arg != null) // ScriptableTypeArray - an array of ScriptableTypes
            {
                let arrayElementMeta = metaMap;
                arrayElementMeta.Category = 2;
                if (Array.isArray(arg))
                {
                    let argArray = []
                    for (var ai = 0; ai < arg.length; ai++)
                    {
                        argArray.push(callbackArg(arg[ai], arrayElementMeta));
                    }
                    return argArray;
                }
                else
                {
                    return callbackArg(arg, metaMap);
                }
            }
            else if (metaMap.Category === 2 && arg !== null) // ScriptableType
            {
                let scriptableType = {};
                Object.getOwnPropertyNames(arg).forEach(function (k) {
                    // if the propery for the given key is an object
                    // we need to handle it special.
                    if (arg[k] === Object(arg[k]))
                    {
                        // if we have a scriptobject already then use it
                        if (v8Util.getHiddenValue(arg[k], 'websharpId'))
                            scriptableType[k] = ObjectToScriptObject(arg[k]);
                        else
                        {
                            // check our type mappings
                            if (metaMap.ScriptableMapping !== null && k in metaMap.ScriptableMapping)
                            {
                                // if it exists in the type mappings and it is specifically a ScriptObject category
                                // that we have not seen before then we generate a new ScriptObject Handle
                                let category = metaMap.ScriptableMapping[k];
                                if (category === 1) // ScriptObject
                                    scriptableType[k] = ObjectToScriptObject(arg[k]);
                                else
                                    scriptableType[k] = arg[k];
                                if (k == 'preventDefault') {
                                    originalEvent = arg;
                                }
                            }
                            else
                                scriptableType[k] = arg[k];
                        }
                    }
                    else
                        scriptableType[k] = arg[k];
                });
                
                return scriptableType;
            }

            if (arg === Object(arg) && (v8Util.getHiddenValue(arg, 'websharpId') || metaMap.Category === 1))
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
                    callbackData = [];
                    var args = Array.from(arguments);
                    for (var i = 0; i < args.length; i++) 
                    {
                        callbackData.push(callbackArg(args[i], func.MetaMapping[i]));
                    }
                    try {
                        // Receive result back from callback. 
                        func.Value(callbackData, 
                            function (error, eventResult) {
                                if (error) throw error;
                                if (eventResult) {
                                    //console.log(eventResult);
                                    if (eventResult['defaultPrevented']) {
                                        if (originalEvent && typeof originalEvent.preventDefault === 'function') {
                                            originalEvent.preventDefault();
                                        }
                                    }

                                }
                            }
                        );
                        
                    }
                    catch (ex) { ErrorHandler.Exception(ex); }
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
            else if (parm.category === 2) // ScriptableType
            {
                returnSO = {};
                Object.getOwnPropertyNames(result).forEach(function (k) {
                    returnSO[k] = result[k]; // We may need to pass more meta data in the future
                });
            }
            else if (parm.category === 6) // ScriptableTypeArray
            {
                returnSO = [];
                for (var ai = 0; ai < result.length; ai++) {
                    let arraySO = {};
                    let wrkObj = result[ai];
                    Object.getOwnPropertyNames(wrkObj).forEach(function (k) {
                        arraySO[k] = wrkObj[k]; // We may need to pass more meta data in the future
                    });

                    returnSO.push(arraySO);
                }
                return returnSO;
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

        return proxy;
    }

    var BridgeProxy = function (objToWrap)
    {

        let id = RegisterScriptableObject(objToWrap);
        let bridge = {};

        bridge.name = "Bridge";
        bridge.websharp_id = id;
        bridge.websharp_get_property = function (prop, cb) {
            let so = GetScriptableObject(prop.handle);
            if (so === 'undefined')
                 cb('Set Property error: Object with handle id \'' + prop.handle + '\' may have already been garbage collected.', null);

            //console.log('get prop -> ' + prop.name + ' has property ' + so.hasOwnProperty(prop.name)  + ' [ ' + so[prop.name] + ' ]');

            let objProp = so[prop.name];

            if (prop.category > 0 && objProp != null )
            {
                let resultSO = WrapResult(objProp, prop);
                cb(null, resultSO);

            }
            else
                cb(null, objProp);
        }

        bridge.websharp_set_property = function (parms, cb) {
            let so = GetScriptableObject(parms.handle);
            if (so === 'undefined')
                 cb('Set Property error: Object with handle id \'' + parms.handle + '\' may have already been garbage collected.', null);
            
            //console.log('set prop -> ' + parms.property + ' has property ' + so.hasOwnProperty(parms.property)  + ' [ ' + so[parms.property] + ' ]');

            let result = false;

            if (parms.createIfNotExists === true) {
                so[parms.property] = parms.value;
                result = true;
            }
            else {
                result = false;
                if (parms.hasOwnProperty === true) {
                    if (so.hasOwnProperty(parms.property)) {
                        so[parms.property] = parms.value;
                        result = true;
                    }
                }
                else {
                    so[parms.property] = parms.value;
                    result = true;
                }

            }
            cb(null, result);
        }

        bridge.websharp_get_attribute = function (prop, cb) {
            let so = GetScriptableObject(prop.handle);
            if (so === 'undefined')
                 cb('Get Attribute error: Object with handle id \'' + prop.handle + '\' may have already been garbage collected.', null);

            //console.log('attribute -> ' + prop + ' [ ' + objToWrap.getAttribute(prop) + ' ]');
            cb(null, so.getAttribute(prop));
        }

        bridge.websharp_set_attribute = function (prop, cb) {
            let so = GetScriptableObject(prop.handle);
            if (so === 'undefined')
                 cb('Set Attribute error: Object with handle id \'' + prop.handle + '\' may have already been garbage collected.', null);

            //console.log('set attribute -> ' + prop.name + ' [ ' + prop.value + ' ]');
            so.setAttribute(prop.name, prop.value);
            cb(null, true);
        }

        bridge.websharp_get_style_attribute = function (prop, cb) {
            let so = GetScriptableObject(prop.handle);
            if (so === 'undefined')
                 cb('Get Style error: Object with handle id \'' + prop.handle + '\' may have already been garbage collected.', null);

            //console.log('style attribute -> ' + prop + ' [ ' + objToWrap.style[prop] + ' ]');
            cb(null, so.style[prop.attribute]);
        }

        bridge.websharp_set_style_attribute = function (prop, cb) {
            let so = GetScriptableObject(prop.handle);
            if (so === 'undefined')
                 cb('Set Style error: Object with handle id \'' + prop.handle + '\' may have already been garbage collected.', null);

            //console.log('set style attribute -> ' + prop.name + ' [ ' + prop.value + ' ]');
            so.style[prop.name] = prop.value;
            cb(null, true);
        }

        bridge.websharp_invoke = function (parms, cb) {

            let so = GetScriptableObject(parms.handle);
            if (so === 'undefined')
                 cb('Invoke error: Object with handle id \'' + parms.handle + '\' may have already been garbage collected.', null);

            //console.log('invoking -> ' + parms.function + ' on handle ' + parms.handle + ' has function ' + (typeof so[parms.function] === 'function') + ' args [ ' + parms.args + ' ]');

            let invokeResult;

            if (typeof so[parms.function] === 'function') {

                let args = UnwrapArgs(parms.args);

                try{
                    invokeResult = so[parms.function].apply(so, args);
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

        bridge.websharp_addEventListener = function (eventCallback, cb) {

            let so = GetScriptableObject(eventCallback.handle);
            if (so === 'undefined')
                 cb('Invoke error: Object with handle id \'' + parms.handle + '\' may have already been garbage collected.', null);

            //console.log('addEventListener -> ' + eventCallback.onEvent + ' UID ' + eventCallback.uid + ' Handle: ' + eventCallback.handle);
            cb(null, EventHelper.add(so, eventCallback));
        }

        bridge.websharp_removeEventListener = function (eventCallback, cb) {

            let so = GetScriptableObject(eventCallback.handle);
            if (so === 'undefined')
                 cb('Invoke error: Object with handle id \'' + parms.handle + '\' may have already been garbage collected.', null);

            //console.log('removeEventListener -> ' + eventCallback.onEvent + ' UID: ' + eventCallback.uid + ' Handle: ' + eventCallback.handle);
            cb(null, EventHelper.remove(so, eventCallback));
        }

        return bridge;
    }

    var DOMEventProps = ["altKey",
        "bubbles",
        "cancelable",
        "changedTouches",
        "ctrlKey",
        "detail",
        "eventPhase",
        "metaKey",
        "shiftKey",
        "view",
	    "char",
        "charCode",
        "key",
        "keyCode",
        "pointerId",
        "pointerType",
        "screenX",
        "screenY",
        "targetTouches",
        "toElement",
        "touches"]

    var DOMMouseEventProps = ["pageX",
        "pageY",
        "button",
        "buttons",
        "clientX",
        "clientY",
        "offsetX",
        "offsetY",
        "layerX",
        "layerY",
        "movementX",
        "movementY"]

    module.exports = { RegisterScriptableObject, GetScriptableObject, UnRegisterScriptableObject, WrapEvent, UnwrapArgs, ObjectToScriptObject, IsRenderer, BridgeProxy };

    /*
    * Helper functions for managing events
    */
    var EventHelper = {

        add: function( elem, eventCallback ) {
            var wsevents = v8Util.getHiddenValue(elem, 'ws_events');
            if (!wsevents)
            {
                wsevents = {};
                v8Util.setHiddenValue(elem, 'ws_events',wsevents);
            }

            var handler = function () {
                //console.log('event triggered');
                // We need to preserver arity of the function callback parameters.
                let callbackData = [];

                let originalEvent;

                // we will only attach event information if it was asked for
                if (eventCallback.handlerType) {
                    // If we get called with arguments then we loop over them to create on object to be passed back
                    // as our callback data.  Func<object, Task<object>>
                    // We only receive one object in our managed code.
                    if (arguments) {
                        var args = Array.from(arguments);
                        for (var i = 0; i < args.length; i++) {
                            if (args[i] instanceof Event) {
                                if (!originalEvent)
                                    originalEvent = args[i];

                                var event = {};
                                var type = args[i].type;
                                event['eventType'] = type;
                                // load dom event info
                                DOMEventProps.forEach(function (element) {
                                    event[element] = args[i][element];
                                });

                                // load DOM mouse specific event info
                                if (args[i] instanceof MouseEvent)
                                {
                                    if (args[i].relatedTarget)
                                        event['relatedTarget'] = ObjectToScriptObject(args[i].relatedTarget);
                                    DOMMouseEventProps.forEach(function (element) {
                                        event[element] = args[i][element];
                                    });
                                }
                                
                                callbackData.push(event);
                                
                            }
                            else
                                callbackData.push(args[i]);
                        }
                    }
                }

                try {
                    eventCallback.callback(callbackData, 
                        function (error, eventResult) {
                                if (error) throw error;
                                if (eventResult)
                                {
                                    if (eventResult['defaultPrevented'])
                                    {
                                        if (typeof originalEvent.preventDefault === 'function')
                                        {
                                            originalEvent.preventDefault();
                                        }
                                    }
                                    if (eventResult['cancelBubble'])
                                    {
                                        if (typeof originalEvent.stopPropagation === 'function')
                                        {
                                            originalEvent.stopPropagation();
                                        }
                                    }
                                }
                            }
                    );
                }
                catch (ex) { ErrorHandler.Exception(ex); }
            
            }

            elem.addEventListener(eventCallback.onEvent, 
                handler, false);
            wsevents[eventCallback.uid] = handler;
            return true;
        },
        remove: function( elem, eventCallback ) {
            var wsevents = v8Util.getHiddenValue(elem, 'ws_events');
            if (!wsevents)
            {
                return false;
            }

            var handler = wsevents[eventCallback.uid];
            if (!handler)
                return false;
            elem.removeEventListener(eventCallback.onEvent, handler, false);
            delete wsevents[eventCallback.uid];
            return true;
        }
    }
})();

