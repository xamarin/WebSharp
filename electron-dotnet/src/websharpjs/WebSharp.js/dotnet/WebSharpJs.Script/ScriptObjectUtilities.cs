//
// ScriptObjectUtilities.cs
//

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace WebSharpJs.Script
{

    public static class ScriptObjectUtilities
    {

        static readonly Type CallbackResultType = typeof(Task<object>);

        #region Reflection extension helper methods
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
            return type.GetCustomAttribute(attributeType) != null;
        }

        #endregion

        /// <summary>
        /// Checks if the Func is of the pattern Func<,Task<object>> which represents the callback bridge pattern
        /// used between JavaScript <> Managed code.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCallbackFunction(Type type)
        {
            if (!typeof(MulticastDelegate).GetTypeInfo().IsAssignableFrom(type.BaseType))
                return false;
            return (type.GetTypeInfo().IsGenericType &&
                 (
                 typeof(Func<,>).GetTypeInfo().IsAssignableFrom(type.GetGenericTypeDefinition())
                 && typeof(Task<object>).GetTypeInfo().IsAssignableFrom(type.GetGenericArguments()[1]))
                 || (type.GetTypeInfo().IsGenericType
                 && typeof(Func<,,>).GetTypeInfo().IsAssignableFrom(type.GetGenericTypeDefinition())
                 && typeof(Task<object>).GetTypeInfo().IsAssignableFrom(type.GetGenericArguments()[2]))
                 || (type.GetTypeInfo().IsGenericType
                 && typeof(Func<,,,>).GetTypeInfo().IsAssignableFrom(type.GetGenericTypeDefinition())
                 && typeof(Task<object>).GetTypeInfo().IsAssignableFrom(type.GetGenericArguments()[3]))
                 || (type.GetTypeInfo().IsGenericType
                 && typeof(Func<,,,,>).GetTypeInfo().IsAssignableFrom(type.GetGenericTypeDefinition())
                 && typeof(Task<object>).GetTypeInfo().IsAssignableFrom(type.GetGenericArguments()[4])));
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
                        if (so.ScriptObjectProxy != null)
                            parms[p] = new ScriptParm { Category = (int)ScriptParmCategory.ScriptObject, Type = "ScriptObject", Value = so.ScriptObjectProxy.Handle };
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
                            var enumConversionType = ConvertEnum.Default;

                            var properties = parmType.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            for (int i = 0; i < properties.Length; i++)
                            {

                                var propertyInfo = properties[i];
                                if (propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.GetMethod != null)
                                {
                                    scriptAlias = propertyInfo.Name;
                                    if (propertyInfo.IsDefined(typeof(ScriptableMemberAttribute), false))
                                    {
                                        var att = propertyInfo.GetCustomAttribute<ScriptableMemberAttribute>(false);
                                        scriptAlias = (att.ScriptAlias ?? scriptAlias);
                                        enumConversionType = att.EnumValue;
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
                                    {
                                        // This handles simple arrays of ScriptableTypes.
                                        // We may need to re visit this in the future if they become more complicated.
                                        if (propertyInfo.PropertyType.IsArray && propertyInfo.PropertyType.GetElementType().IsAttributeDefined<ScriptableTypeAttribute>(false))
                                        {
                                            var propArray = (Array)propertyInfo.GetValue(parm);

                                            if (propArray == null || propArray.Length == 0)
                                                fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = propertyInfo.PropertyType.ToString(), Value = propArray });
                                            else
                                            {
                                                // We will loop through this ourselves instead of bringing LINQ and Casting into this.
                                                var dynDic = new object[propArray.Length];
                                                for (int x = 0; x < propArray.Length; x++)
                                                {
                                                    dynDic[x] = ScriptObjectHelper.ScriptableTypeToDictionary(propArray.GetValue(x));
                                                }
                                                fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = propertyInfo.PropertyType.ToString(), Value = dynDic });
                                            }
                                        }
                                        else
                                        {
                                            if (propertyInfo.PropertyType.IsEnum)
                                            {
                                                switch (enumConversionType)
                                                {
                                                    case ConvertEnum.ToLower:
                                                        fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = propertyInfo.PropertyType.ToString(), Value = propertyInfo.GetValue(parm).ToString().ToLower() });
                                                        break;
                                                    case ConvertEnum.ToUpper:
                                                        fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = propertyInfo.PropertyType.ToString(), Value = propertyInfo.GetValue(parm).ToString().ToUpper() });
                                                        break;
                                                    case ConvertEnum.Numeric:
                                                        fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = propertyInfo.PropertyType.ToString(), Value = (int)Enum.Parse(propertyInfo.PropertyType, propertyInfo.GetValue(parm).ToString()) });
                                                        break;
                                                    default:
                                                        fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = propertyInfo.PropertyType.ToString(), Value = propertyInfo.GetValue(parm) });
                                                        break;
                                                }

                                            }
                                            else
                                            {
                                                var propValue = propertyInfo.GetValue(parm);
                                                so = propValue as ScriptObject;
                                                if (so != null)
                                                {
                                                    if (so.ScriptObjectProxy != null)
                                                    {
                                                        fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptObject, Type = "ScriptObject", Value = so.ScriptObjectProxy.Handle });
                                                    }
                                                    else
                                                    {
                                                        fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptObject, Type = so.GetType().ToString(), Value = propValue });
                                                    }

                                                }
                                                else
                                                    fieldMappings.Add(scriptAlias, new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = propertyInfo.PropertyType.ToString(), Value = propValue });
                                            }
                                        }
                                    }
                                }
                            }
                            parms[p] = new ScriptParm { Category = (int)ScriptParmCategory.ScriptableType, Type = "ScriptableType", Value = fieldMappings };
                        }
                        else
                        {
                            if (IsCallbackFunction(parm.GetType()))
                                parms[p] = new ScriptParm { Category = (int)ScriptParmCategory.ScriptCallback, Type = "ScriptCallback", Value = parm };
                            else
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