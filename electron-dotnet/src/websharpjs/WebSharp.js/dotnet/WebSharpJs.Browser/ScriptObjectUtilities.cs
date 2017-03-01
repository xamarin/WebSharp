//
// ScriptObjectUtilities.cs
//

using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebSharpJs.Browser
{

    public static class ScriptObjectUtilities
    {

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

        #endregion

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
                            var fieldMappings = new Dictionary<string, object>();
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
                                fieldMappings.Add(scriptAlias, propertyInfo.GetValue(parm));
                            }
                            parms[p] = new ScriptParm { Category = (int)ScriptParmCategory.ScriptType, Type = "ScriptType", Value = fieldMappings };
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