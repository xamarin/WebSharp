//
// ScriptObject.cs
//

using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace WebSharpJs.Browser
{

    public class ScriptObject : IDynamicMetaObjectProvider
    {
        /// <summary>
        /// scriptObject instance
        /// </summary>
        object websharpObject;

        dynamic javascriptFunctionProxy;

        public ScriptObject()
        {
            Initialize(this);
        }

        public ScriptObject(object scriptObject)
        {
            Initialize(scriptObject);
        }

        protected virtual void Initialize(object scriptObject)
        {
            websharpObject = scriptObject;
        }

        public object ManageObject
        {
            get { return websharpObject; }
        }

        /// <summary>
        /// ScriptObject proxy implementation
        /// </summary>
        public dynamic JavaScriptProxy
        {
            get { return javascriptFunctionProxy; }
            set { javascriptFunctionProxy = value; }
        }

        #region Public interface

        public virtual bool SetProperty(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return TrySetProperty(name, value);
        }

        public virtual async Task<bool> SetPropertyAsync(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return await TrySetPropertyAsync(name, value);
        }

        public virtual object GetProperty(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            object result;
            TryGetProperty(name, out result);
            return result;
        }

        public virtual async Task<T> GetPropertyAsync<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return await TryGetPropertyAsync<T>(name);
        }

        public virtual object Invoke(string name, params object[] args)
        {
            object result = TryInvoke(name, args);
            return result;
        }

        public virtual async Task<T> InvokeAsync<T>(string name, params object[] args)
        {
            return await TryInvokeAsync<T>(name, args);
        }
        #endregion

        #region Supporting classes

        protected virtual object TryInvoke(string name, params object[] parameters)
        {
            if (javascriptFunctionProxy != null)
            {
                
                Task<object> to = Task.Run<object>(async () =>
                {
                    var parms = new
                    {
                        function = name,
                        args = ScriptObjectUtilities.WrapScriptParms(parameters)
                    };
                    return await JavaScriptProxy.websharp_invoke(parms);
                });

                return to.Result;
            }

            return null;
        }

        protected virtual async Task<T> TryInvokeAsync<T>(string name, params object[] parameters)
        {
            if (javascriptFunctionProxy != null)
            {

                var parms = new
                {
                    function = name,
                    args = ScriptObjectUtilities.WrapScriptParms(parameters)
                };
                return await JavaScriptProxy.websharp_invoke(parms);

            }

            return default(T);
        }

        protected virtual bool TryGetProperty(string name, out object result)
        {

            result = null;
            if (javascriptFunctionProxy != null)
            {
                Task<object> to = Task.Run<object>(async () =>
                {
                    return await JavaScriptProxy.websharp_get_property(name);
                });

                result = to.Result;
                return true;
            }

            return false;
        }

        protected virtual async Task<T> TryGetPropertyAsync<T>(string name)
        {

            if (javascriptFunctionProxy != null)
            {
                return await JavaScriptProxy.websharp_get_property(name) ?? default(T);
            }
            else
                return default(T);
        }

        protected virtual bool TrySetProperty(string name, object value, bool createIfNotExists = true, bool hasOwnProperty = false)
        {
            try
            {
                if (javascriptFunctionProxy != null)
                {
                    Task<bool> to = Task.Run<bool>(async () =>
                    {
                        var parms = new
                        {
                            property = name,
                            value = value,
                            createIfNotExists = createIfNotExists,
                            hasOwnProperty = hasOwnProperty
                        };
                        return await JavaScriptProxy.websharp_set_property(parms);
                    });
                    return to.Result;
                }
            }
            catch { }

            return false;
        }

        protected virtual async Task<bool> TrySetPropertyAsync(string name, object value, bool createIfNotExists = true, bool hasOwnProperty = false)
        {
            try
            {
                if (javascriptFunctionProxy != null)
                {
                    var parms = new
                    {
                        property = name,
                        value = value,
                        createIfNotExists = createIfNotExists,
                        hasOwnProperty = hasOwnProperty
                    };
                    return await JavaScriptProxy.websharp_set_property(parms);
               }
            }
            catch { }

            return false;
        }
        #endregion


        #region IDynamicMetaObjectProvider interface implementation

        /// <summary>
        /// Creates a default DynamicMetaObject under which the dynamic binding is valid 
        /// Fullfills the IDynamicMetaObjectProvider interface
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
        {
            return new DynamicMetaObject(parameter, BindingRestrictions.Empty, this);
        }
        #endregion
    }
}