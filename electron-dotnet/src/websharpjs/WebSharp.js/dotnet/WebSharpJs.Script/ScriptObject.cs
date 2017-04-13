﻿//
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
            set { scriptObjectProxy = value; }
        }

        #region Public interface

        public virtual async Task<bool> SetProperty(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return await TrySetProperty(name, value);
        }

        public virtual async Task<T> GetProperty<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return await TryGetProperty<T>(name);
        }

        public virtual async Task<T> Invoke<T>(string name, params object[] args)
        {
            return await TryInvoke<T>(name, args);
        }
        #endregion

        #region Supporting classes

        protected virtual async Task<T> TryInvoke<T>(string name, params object[] parameters)
        {
            if (scriptObjectProxy != null)
            {
                var parmCategory = ScriptParmCategory.None;

                Type type = typeof(T);

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ScriptObjectCollection<>))
                {
                    parmCategory = ScriptParmCategory.ScriptObjectCollection;
                }
                else if (type.IsSubclassOf(ScriptObjectType))
                {
                    parmCategory = ScriptParmCategory.ScriptObject;
                }

                var parms = new
                {
                    function = name,
                    category = (int)parmCategory,
                    args = ScriptObjectUtilities.WrapScriptParms(parameters)
                };

                if (parmCategory != ScriptParmCategory.None)
                {
                    var result = await scriptObjectProxy.TryInvokeAsync(parms);
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
                    else
                        return (T)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { result }, null);
                }
                else
                {
                    return (T)(await scriptObjectProxy.TryInvokeAsync(parms));
                }

            }

            return default(T);
        }

        protected virtual async Task<T> TryGetProperty<T>(string name)
        {
            if (scriptObjectProxy != null)
            {

                var parmCategory = ScriptParmCategory.None;

                Type type = typeof(T);

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ScriptObjectCollection<>))
                {
                    parmCategory = ScriptParmCategory.ScriptObjectCollection;
                }
                else if (type.IsSubclassOf(ScriptObjectType))
                {
                    parmCategory = ScriptParmCategory.ScriptObject;
                }

                var parms = new
                {
                    name = name,
                    category = (int)parmCategory,
                };

                if (parmCategory != ScriptParmCategory.None)
                {
                    var result = await scriptObjectProxy.GetProperty<object>(parms);
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
                    else
                        return (T)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { result }, null);
                }
                else
                    return (T)(await scriptObjectProxy.GetProperty<T>(parms));
            }
            else
                return default(T);
        }

        protected virtual async Task<bool> TrySetProperty(string name, object value, bool createIfNotExists = true, bool hasOwnProperty = false)
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