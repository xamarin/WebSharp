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

        static readonly Type ScriptObjectType = typeof(ScriptObject);

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
            set { scriptObjectProxy = value;  handle = scriptObjectProxy.Handle; }
        }

        #region Public interface

        internal int handle;

        public int Handle => handle;

        public virtual async Task<bool> SetProperty(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Argument: {nameof(name)} Can not be null or empty");

            return await TrySetProperty(name, value);
        }

        public virtual async Task<T> GetProperty<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Argument: {nameof(name)} Can not be null or empty");

            return await TryGetProperty<T>(name);
        }

        public virtual async Task<T> Invoke<T>(string name, params object[] args)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Argument: {nameof(name)} Can not be null or empty");

            return await TryInvoke<T>(name, args);
        }
        #endregion

        #region Supporting methods

        T GetReturnValue<T> (ScriptParmCategory parmCategory, object result)
        {
            Type type = typeof(T);

            if (result == null)
                return default(T);

            else if (parmCategory == ScriptParmCategory.ScriptObjectCollection)
            {
                var objArray = result as object[];
                if (objArray == null)
                    return default(T);

                try
                {
                    var array = Array.CreateInstance(type.GetGenericArguments()[0], objArray.Length);
                    for (int a = 0; a < array.Length; a++)
                    {
                        array.SetValue(Activator.CreateInstance(type.GetGenericArguments()[0], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { objArray[a] }, null), a);
                    }
                    return (T)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { array }, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return default(T);
                }

            }
            else if (parmCategory == ScriptParmCategory.ScriptableType)
            {
                return ScriptObjectHelper.AnonymousObjectToScriptableType<T>(result);
            }
            else if (parmCategory == ScriptParmCategory.ScriptableTypeArray)
            {
                try
                {
                    var objArray = result as object[];
                    var arrayType = type.GetElementType();
                    var array = Array.CreateInstance(arrayType, objArray.Length);
                    for (int a = 0; a < array.Length; a++)
                    {
                        array.SetValue(ScriptObjectHelper.AnonymousObjectToScriptableType(arrayType, objArray[a]), a);
                    }
                    return (T)(object)array; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return (T)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { result }, null);
            }
            else
                return (T)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { result }, null);
        }

        ScriptParmCategory GetParmCategory<T>()
        {
            Type type = typeof(T);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ScriptObjectCollection<>))
            {
                return ScriptParmCategory.ScriptObjectCollection;
            }
            else if (type.IsSubclassOf(ScriptObjectType))
            {
                return ScriptParmCategory.ScriptObject;
            }
            else if (type.IsAttributeDefined<ScriptableTypeAttribute>(false))
            {
                return ScriptParmCategory.ScriptableType;
            }
            else if (type.IsArray && type.GetElementType().IsAttributeDefined<ScriptableTypeAttribute>(false))
            {
                return ScriptParmCategory.ScriptableTypeArray;
            }

            return ScriptParmCategory.None;
        }


        protected virtual async Task<T> TryInvoke<T>(string name, params object[] parameters)
        {
            if (scriptObjectProxy != null)
            {
                var parmCategory = GetParmCategory<T>();

                var parms = new
                {
                    handle = Handle,
                    function = name,
                    category = (int)parmCategory,
                    args = ScriptObjectUtilities.WrapScriptParms(parameters)
                };

                var result = await WebSharp.Bridge.TryInvoke(parms);
                return (parmCategory == ScriptParmCategory.None) ? (T)result : GetReturnValue<T>(parmCategory, result);

            }

            return default(T);
        }

        protected virtual async Task<T> TryGetProperty<T>(string name)
        {
            var parmCategory = GetParmCategory<T>();

            var parms = new
            {
                handle = Handle,
                name = name,
                category = (int)parmCategory,
            };

            var result = await WebSharp.Bridge.GetProperty<object>(parms);
            return (parmCategory == ScriptParmCategory.None) ? (T)result : GetReturnValue<T>(parmCategory, result);
        }

        protected virtual async Task<bool> TrySetProperty(string name, object value, bool createIfNotExists = true, bool hasOwnProperty = false)
        {
            try
            {
                var parms = new
                {
                    handle = Handle,
                    property = name,
                    value = value,
                    createIfNotExists = createIfNotExists,
                    hasOwnProperty = hasOwnProperty
                };
                var result = await WebSharp.Bridge.SetProperty(parms);
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