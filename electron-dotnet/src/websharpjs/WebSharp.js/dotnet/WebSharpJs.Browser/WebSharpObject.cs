//
// WebSharpObject.cs
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;


namespace WebSharpJs.Browser
{


    public partial class WebSharpObject : ScriptObject
    {

        static CachedBag cachedObjects;

        /// <summary>
        /// Cached type of the instance
        /// </summary>
        Type InstanceType;

        PropertyBag CachedPropertyInfo
        {
            get
            {
                if (cachedPropertyInfo == null)
                {
                    var props = new PropertyBag();
                    var scriptAlias = string.Empty;
                    bool createIfNotExists;
                    bool hasOwnProperty;

                    foreach (var prop in InstanceType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    {
                        scriptAlias = prop.Name;
                        createIfNotExists = true;
                        hasOwnProperty = false;
                        if (prop.IsDefined(typeof(ScriptableMemberAttribute), false))
                        {
                            var att = prop.GetCustomAttribute<ScriptableMemberAttribute>(false);
                            scriptAlias = (att.ScriptAlias ?? scriptAlias);
                            createIfNotExists = att.CreateIfNotExists;
                            hasOwnProperty = att.HasOwnProperty;
                        }
                        props[prop.Name] = new Property(scriptAlias, createIfNotExists, hasOwnProperty);
                    }
                    return props;
                }

                return cachedPropertyInfo;
            }
        }

        /// <summary>
        /// Cached property info used for scriptAlias lookup for Properties
        /// </summary>
        PropertyBag cachedPropertyInfo;

        static WebSharpObject()
        {
            cachedObjects = new CachedBag();
        }

        private WebSharpObject(object obj) : base(obj)
        {
            InstanceType = GetType();
            if (cachedPropertyInfo == null)
                cachedPropertyInfo = CachedPropertyInfo;
        }

        public WebSharpObject()
        {
            InstanceType = GetType();
            if (cachedPropertyInfo == null)
                cachedPropertyInfo = CachedPropertyInfo;
        }

        static readonly string createScript = @"
                                return function (data, callback) {
                                    const dotnet = require('./electron-dotnet');
                                    const websharpjs = dotnet.WebSharpJs;

                                    
                                    const {remote} = require('electron')
                                    const {Menu, MenuItem} = remote

                                    let options = [];
                                    if (data) {
                                        var arrayLength = data.length;
                                        for (var i = 0; i < arrayLength; i++) {
                                                    
                                            let parm = data[i];
                                            switch (parm.Category)
                                            {
                                                case 1:
                                                    options.push(websharpjs.GetScriptableObject(parm.Value));
                                                    break;
                                                case 2:
                                                    options.push(parm.Value);
                                                    break;
                                                default:
                                                    options.push(parm.Value);
                                            }
                                        }
                                    }


                                    let wrapScriptObject = function (objToWrap)
                                    {
                                        let id = websharpjs.RegisterScriptableObject(objToWrap);  
                                        let proxy = {};

                                        proxy.websharp_id = id;
                                        proxy.websharp_get_property = function (prop, cb) {
                                            //console.log('prop -> ' + prop + ' has property ' + objToWrap.hasOwnProperty(prop)  + ' [ ' + objToWrap[prop] + ' ]');
                                            cb(null, objToWrap[prop]);
                                        }

                                        proxy.websharp_set_property = function (parms, cb) {

                                            let result = false;
                                            if (parms.createIfNotExists === true)
                                            {
                                                objToWrap[parms.property] = parms.value; 
                                                result = true;
                                            }
                                            else
                                            {
                                                result = false;
                                                if (parms.hasOwnProperty === true)
                                                {
                                                   if (objToWrap.hasOwnProperty(parms.property))
                                                   {
                                                      objToWrap[parms.property] = parms.value;
                                                      result = true;
                                                   }
                                                }
                                                else
                                                {
                                                    objToWrap[parms.property] = parms.value;
                                                    result = true;
                                                }
                                            
                                            }
                                            cb(null, result);
                                        }
                                        proxy.websharp_invoke = function (parms, cb) {
                                            console.log('invoking -> ' + parms.function + ' has function ' + (typeof objToWrap[parms.function] === 'function')  + ' args [ ' + parms.args + ' ]');
                                            let invokeResult;

                                            if (typeof objToWrap[parms.function] === 'function') {

                                                let args = [];
                                                if (parms.args) {
                                                    var arrayLength = parms.args.length;
                                                    for (var i = 0; i < arrayLength; i++) {
                                                    
                                                        let parm = parms.args[i];
                                                        //console.log(parm)
                                                        switch (parm.Category)
                                                        {
                                                            case 1:
                                                                args.push(websharpjs.GetScriptableObject(parm.Value));
                                                                break;
                                                            case 2:
                                                                args.push(parm.Value);
                                                                break;
                                                            default:
                                                                args.push(parm.Value);
                                                        }
                                                    }
                                                }


                                                invokeResult = objToWrap[parms.function].apply(objToWrap, args);
                                                cb(null, invokeResult);
                                            }
                                            else
                                                cb('Function ' + parm.function + ' does not exist. ', invokeResult);

                                        }

                                        proxy.websharp_addEventListener = function (eventCallback, cb) {
                                            console.log('addEventListener -> ' + eventCallback.onEvent);
                                            objToWrap.addEventListener(eventCallback.onEvent, function(evt) {
                                                eventCallback.callback(evt, null); 
                                            });
                                            cb(null, null);
                                        }
                                        
                                        proxy.websharp_proxied_object = function (data, cb) {
                                            cb(null, proxy.websharp_id);

                                        }

                                        return proxy;
                                    }
                                    let wsObj = $$$$javascriptObject$$$$

                                    let proxy = wrapScriptObject(wsObj);
                                      
                                    callback(null, proxy);
                                }
                            ";

        protected async Task CreateScriptObject(string javascriptObject, params object[] args)
        {
            object[] parms = args;

            if (args != null)
            {
                parms = ScriptObjectUtilities.WrapScriptParms(args);
            }

            Func<object, Task<object>> scriptProxy = await WebSharp.CreateJavaScriptFunction(createScript.Replace("$$$$javascriptObject$$$$", javascriptObject));
            JavaScriptProxy = await scriptProxy(parms);

        }
 
        class EventHandlerBag : GrabBag<WebSharpEvent>
        { }

        EventHandlerBag EventHandlers = new EventHandlerBag();

        public bool AttachEvent(string eventName, EventHandler handler)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException("eventName");

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            var scriptAlias = eventName;
            var ei = InstanceType.GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public);

            if (ei != null)
            {
                if (ei.IsDefined(typeof(ScriptableMemberAttribute), false))
                {
                    var att = ei.GetCustomAttribute<ScriptableMemberAttribute>(false);
                    scriptAlias = (att.ScriptAlias ?? scriptAlias);
                }
            }

            WebSharpEvent websharpEvent;
            if (!EventHandlers.TryGetValue(scriptAlias, out websharpEvent))
            {
                websharpEvent = new WebSharpEvent(this, scriptAlias);
                if (JavaScriptProxy != null)
                {
                    Task<object> to = Task.Run<object>(async () =>
                    {
                        var eventCallback = new
                        {
                            onEvent = scriptAlias,
                            callback = websharpEvent.EventCallbackFunction
                        };
                        return await JavaScriptProxy.websharp_addEventListener(eventCallback);
                    });
                    var result = to.Result;
                }
                EventHandlers[scriptAlias] = websharpEvent;
            }
            websharpEvent.AddEventHandler(handler);
            return true;
        }

        public bool AttachEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException("eventName");

            WebSharpEvent websharpEvent;

            var scriptAlias = eventName;
            var ei = InstanceType.GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public);

            if (ei == null)
                return false;

            if (ei.IsDefined(typeof(ScriptableMemberAttribute), false))
            {
                var att = ei.GetCustomAttribute<ScriptableMemberAttribute>(false);
                scriptAlias = (att.ScriptAlias ?? scriptAlias);
            }

            if (!EventHandlers.TryGetValue(eventName, out websharpEvent))
            {

                websharpEvent = new WebSharpEvent(this, eventName);
                if (JavaScriptProxy != null)
                {
                    Task<object> to = Task.Run<object>(async () =>
                    {
                        var eventCallback = new
                        {
                            onEvent = scriptAlias,
                            callback = websharpEvent.EventCallbackFunction
                        };
                        return await JavaScriptProxy.websharp_addEventListener(eventCallback);
                    });
                    var result = to.Result;
                }
                EventHandlers[eventName] = websharpEvent;
            }
            return true;
        }

        public override bool SetProperty(string name, object value)
        {
            Property property = null;
            if (cachedPropertyInfo.TryGetValue(name, out property))
                return base.TrySetProperty(property.ScriptAlias, value, property.CreateIfNotExists, property.HasOwnProperty);
            else
                return base.SetProperty(name, value);
        }

        public override object GetProperty(string name)
        {
            Property property = null;
            if (cachedPropertyInfo.TryGetValue(name, out property))
                return base.GetProperty(property.ScriptAlias);
            else
                return base.GetProperty(name);
        }

        public T GetProperty<T>(string name)
        {
            object o = GetProperty(name);
            return (T)o;
        }

        public override object Invoke(string name, params object[] args)
        {
            var scriptAlias = name;

            var mi = InstanceType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public);

            if (mi != null)
            {
                if (mi.IsDefined(typeof(ScriptableMemberAttribute), false))
                {
                    var att = mi.GetCustomAttribute<ScriptableMemberAttribute>(false);
                    scriptAlias = (att.ScriptAlias ?? scriptAlias);
                }
            }
            return base.Invoke(scriptAlias, args);
        }
    }

    internal class GrabBag<T> : Dictionary<string, T>
    { }

    internal class CachedBag : GrabBag<WeakReference>
    { }

    internal class PropertyBag : GrabBag<Property>
    { }


    internal class Property
    {
        public string ScriptAlias { get; set; }
        public bool CreateIfNotExists { get; set; }
        public bool HasOwnProperty { get; set; }

        public Property(string scriptAlias, bool createIfNotExists, bool hasOwnProperty)
        {
            ScriptAlias = scriptAlias;
            CreateIfNotExists = createIfNotExists;
            HasOwnProperty = hasOwnProperty;
        }
    }
}