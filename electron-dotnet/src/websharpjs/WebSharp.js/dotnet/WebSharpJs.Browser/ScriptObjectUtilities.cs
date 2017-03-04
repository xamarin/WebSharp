//
// ScriptObjectUtilities.cs
//

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace WebSharpJs.Browser
{

    public static class ScriptObjectUtilities
    {

        static readonly Type CallbackResultType = typeof(Task<object>);

        #region .NET Core extension helper methods
        public static bool IsAttributeDefined<TAttribute>(this MemberInfo memberInfo)
        {
            return memberInfo.IsAttributeDefined(typeof(TAttribute));
        }

        public static bool IsAttributeDefined(this MemberInfo memberInfo, Type attributeType)
        {
            return memberInfo.GetCustomAttribute(attributeType) != null;
        }

        public static bool IsAttributeDefined<TAttribute>(this Type type, bool inherited = true)
        {
            return type.IsAttributeDefined(typeof(TAttribute), inherited);
        }

        public static bool IsAttributeDefined(this Type type, Type attributeType, bool inherited = true)
        {
            return type.GetTypeInfo().GetCustomAttribute(attributeType) != null;
        }

        public static Type BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        public static Type GetGenericTypeDefinition(this Type type)
        {
            return type.GetTypeInfo().GetGenericTypeDefinition();
        }

        public static Type[] GetGenericArguments( this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments();
        }
        #endregion

        public static bool IsDelegate(Type type)
        {
            return typeof(MulticastDelegate).IsAssignableFrom(type.BaseType());
        }

        /// <summary>
        /// Checks if the Func is of the pattern Func<,Task<object>> which represents the callback bridge pattern
        /// used between JavaScript <> Managed code.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCallbackFunction(Type type)
        {
            if (!typeof(MulticastDelegate).IsAssignableFrom(type.BaseType()))
                return false;
            return (type.GetTypeInfo().IsGenericType
                && typeof(Func<,>).IsAssignableFrom(type.GetGenericTypeDefinition())
                && typeof(Task<object>).IsAssignableFrom(type.GetGenericArguments()[1]));
        }

        /// <summary>
        /// Casts our Func<,Task<object>> to Func<object,Task<object>> for use in the callback bridge pattern
        /// used between JavaScript <> Managed code. 
        /// </summary>
        /// <param name="callbackFunction"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Delegate Cast(object callbackFunction, Type type)
        {
            // Based loosely on http://stackoverflow.com/questions/16590685/using-expression-to-cast-funcobject-object-to-funct-tret?answertab=active#tab-top
            var param = Expression.Parameter(typeof(object));
            var convertedParam = new Expression[] { Expression.Convert(param, type.GetGenericArguments()[0]) };
            var func = (MulticastDelegate)callbackFunction;

            // This is gnarly... If a func contains a closure, then even though its static, its first
            // param is used to carry the closure, so its as if it is not a static method, so we need
            // to check for that param and call the func with it if it has one...
            Expression call;
            call = Expression.Convert(
                func.Target == null
                ? Expression.Call(func.GetMethodInfo(), convertedParam)
                : Expression.Call(Expression.Constant(func.Target), func.GetMethodInfo(), convertedParam), CallbackResultType);

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(object), CallbackResultType);
            return Expression.Lambda(delegateType, call, param).Compile();
        }

        public static object[] WrapScriptParms(object [] args)
        {
            object[] parms = null;
            if (args != null)
            {
                parms = new object[args.Length];
                int p = 0;

                foreach (var parm in args)
                {
                    var so = parm as ScriptObject;
                    if (so != null)
                    {
                        if (so.JavaScriptProxy != null)
                            parms[p] = new ScriptParm { Category = (int)ScriptParmCategory.ScriptObject, Type = "ScriptObject", Value = so.JavaScriptProxy.websharp_id };
                        else
                            parms[p] = new ScriptParm { Category = (int)ScriptParmCategory.ScriptObject, Type = so.GetType().ToString(), Value = so };
                    }
                    else
                    {
                        var parmType = parm.GetType();
                        if (parmType.IsAttributeDefined<ScriptableTypeAttribute>(false))
                        {
                            var fieldMappings = new Dictionary<string, ScriptParm>();
                            var scriptAlias = string.Empty;
                            var properties = parmType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            for (int i = 0; i < properties.Length; i++)
                            {
                                var propertyInfo = properties[i];
                                scriptAlias = propertyInfo.Name;
                                if (propertyInfo.IsDefined(typeof(ScriptableMemberAttribute), false))
                                {
                                    var att = propertyInfo.GetCustomAttribute<ScriptableMemberAttribute>(false);
                                    scriptAlias = (att.ScriptAlias ?? scriptAlias);
                                }
                                
                                if (IsCallbackFunction(propertyInfo.PropertyType))
                                {
                                    
                                    var cbv = propertyInfo.GetValue(parm);
                                    if (cbv != null)
                                    {
                                        var scriptCallback = Cast(cbv, propertyInfo.PropertyType);
                                        fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptCallback, Type = "ScriptCallback", Value = scriptCallback }); 
                                    }
                                    else
                                    {
                                        fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = propertyInfo.PropertyType.ToString(), Value = propertyInfo.GetValue(parm) });
                                    }
                                }
                                else
                                    fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = propertyInfo.PropertyType.ToString(), Value = propertyInfo.GetValue(parm) });
                            }
                            parms[p] = new ScriptParm { Category = (int)ScriptParmCategory.ScriptableType, Type = "ScriptableType", Value = fieldMappings };
                        }
                        else
                        {
                            parms[p] = new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = parm.GetType().ToString(), Value = parm };
                        }
                    }

                    //Console.WriteLine(parms[p]);
                    p++;
                }


            }
            return parms;
        }
    }
}