//
// ScriptObject.cs
//

using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace WebSharpJs.Script
{

    public class ScriptObject : IDynamicMetaObjectProvider
    {
        /// <summary>
        /// scriptObject instance
        /// </summary>
        object websharpObject;

        IScriptObjectProxy scriptObjectProxy;

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
        public IScriptObjectProxy ScriptObjectProxy
        {
            get { return scriptObjectProxy; }
            set { scriptObjectProxy = value; }
        }

        #region Public interface

        public virtual async Task<bool> SetPropertyAsync(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return await TrySetPropertyAsync(name, value);
        }

        public virtual async Task<T> GetPropertyAsync<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return await TryGetPropertyAsync<T>(name);
        }

        public virtual async Task<T> InvokeAsync<T>(string name, params object[] args)
        {
            return await TryInvokeAsync<T>(name, args);
        }
        #endregion

        #region Supporting classes

        protected virtual async Task<T> TryInvokeAsync<T>(string name, params object[] parameters)
        {
            if (scriptObjectProxy != null)
            {
                var isScriptObject = false;
                Type type = typeof(T);
                if (type.GetIsSubclassOf(typeof(ScriptObject)))
                {
                    isScriptObject = true;
                }

                var parms = new
                {
                    function = name,
                    scriptObject = isScriptObject,
                    args = ScriptObjectUtilities.WrapScriptParms(parameters)
                };

                if (isScriptObject)
                {
                    var result = await scriptObjectProxy.TryInvokeAsync(parms);// JavaScriptProxy.websharp_invoke(parms);
                    return (T)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { result }, null); ;
                }
                else
                {
                    return (T)(await scriptObjectProxy.TryInvokeAsync(parms));
                }

            }

            return default(T);
        }

        protected virtual async Task<T> TryGetPropertyAsync<T>(string name)
        {
            if (scriptObjectProxy != null)
            {
                return await scriptObjectProxy.GetProperty<T>(name);
            }
            else
                return default(T);
        }

        protected virtual async Task<bool> TrySetPropertyAsync(string name, object value, bool createIfNotExists = true, bool hasOwnProperty = false)
        {
            try
            {
                if (scriptObjectProxy != null)
                {
                    return await scriptObjectProxy.SetProperty(name, value, createIfNotExists, hasOwnProperty);
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