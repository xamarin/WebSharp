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
            if (!typeof(MulticastDelegate).GetTypeInfo().IsAssignableFrom(type?.BaseType))
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

        static bool IsScriptObject(object meta)
        {
            return (meta as ScriptObject) != null;
        }

        static bool IsScriptableType(object meta)
        {
            return meta.GetType().IsAttributeDefined<ScriptableTypeAttribute>(false);

        }

        static bool IsCallback(object meta)
        {
            return IsCallbackFunction(meta?.GetType());
        }

        static bool IsArrayOfScriptableType (object meta)
        {
            return meta != null && meta.GetType().IsArray && meta.GetType().GetElementType().IsAttributeDefined<ScriptableTypeAttribute>(false);
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

        static ScriptParm ScriptObjectToMeta(ScriptObject meta)
        {
            ScriptParm scriptParm;
            if (meta.ScriptObjectProxy != null)
                scriptParm = new ScriptParm { Category = (int)ScriptParmCategory.ScriptObject, Type = "ScriptObject", Value = meta.ScriptObjectProxy.Handle };
            else
                scriptParm = new ScriptParm { Category = (int)ScriptParmCategory.ScriptObject, Type = meta.GetType().ToString(), Value = meta };

            return scriptParm;
        }

        static ScriptParm CallBackToMeta(object meta)
        {
            
            if (meta != null)
            {
                var scriptCallback = Cast(meta, meta.GetType());
                return new ScriptParm { Category = (int)ScriptParmCategory.ScriptCallback, Type = "ScriptCallback", Value = scriptCallback };
            }
            else
            {
                return new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = meta.GetType().ToString(), Value = meta };
            }
        }

        static ScriptParm EnumToMeta(object meta, ConvertEnum enumConversionType)
        {

            switch (enumConversionType)
            {
                case ConvertEnum.ToLower:
                    return new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = meta.GetType().ToString(), Value = meta.ToString().ToLower() };
                case ConvertEnum.ToUpper:
                    return new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = meta.GetType().ToString(), Value = meta.ToString().ToUpper() };
                case ConvertEnum.Numeric:
                    return new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = meta.GetType().ToString(), Value = (int)Enum.Parse(meta.GetType(), meta.ToString()) };
                default:
                    return new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = meta.GetType().ToString(), Value = meta };
            }
        }

        static ScriptParm ScriptableTypeArrayToMeta(object meta)
        {
            var propArray = (Array)meta;

            if (propArray == null || propArray.Length == 0)
                return new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = meta.GetType().ToString(), Value = propArray };
            else
            {
                // We will loop through this ourselves instead of bringing LINQ and Casting into this.
                var dynDic = new object[propArray.Length];
                for (int x = 0; x < propArray.Length; x++)
                {
                    dynDic[x] = ParmToMetaData(propArray.GetValue(x));  //ScriptObjectHelper.ScriptableTypeToDictionary(propArray.GetValue(x));
                }
                return new ScriptParm { Category = (int)ScriptParmCategory.ScriptableTypeArray, Type = meta.GetType().ToString(), Value = dynDic };
            }
        }

        static ScriptParm ScriptableTypeToMeta(object meta)
        {
            var parmType = meta.GetType();
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

                    var propValue = propertyInfo.GetValue(meta);
                    fieldMappings.Add(scriptAlias, ParmToMetaData(propValue, enumConversionType));
                }
            }
            return new ScriptParm { Category = (int)ScriptParmCategory.ScriptableType, Type = "ScriptableType", Value = fieldMappings };

        }


        static ScriptParm ParmToMetaData(object parm, ConvertEnum enumConversionType = ConvertEnum.Default)
        {
            ScriptParm scriptParm;

            // Let's handle null parameters
            // example of this is BrowserWindow.SetMenu(null) so the menu does not show
            if (parm == null)
                scriptParm = new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = "null", Value = parm };
            else if (IsScriptObject(parm))
                scriptParm = ScriptObjectToMeta((ScriptObject)parm);
            else if (IsScriptableType(parm))
                scriptParm = ScriptableTypeToMeta(parm);
            else if (IsCallback(parm))
                scriptParm = CallBackToMeta(parm);
            else if (IsArrayOfScriptableType(parm))
                scriptParm = ScriptableTypeArrayToMeta(parm);
            else if (parm.GetType().IsEnum)
                scriptParm = EnumToMeta(parm, enumConversionType);
            else
                scriptParm = new ScriptParm { Category = (int)ScriptParmCategory.ScriptValue, Type = parm.GetType().ToString(), Value = parm };
            return scriptParm;
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
                    parms[p++] = ParmToMetaData(parm);
                }
            }
            return parms;
        }
    }
}