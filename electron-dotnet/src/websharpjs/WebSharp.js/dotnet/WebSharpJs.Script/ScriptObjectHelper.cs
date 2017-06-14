//
// ScriptObjectHelpers.cs
//

using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebSharpJs.Script
{

    public static class ScriptObjectHelper
    {

        public static IDictionary<string, object> ScriptableTypeToDictionary(object value)
        {
            var parmType = value.GetType();
            var propertyMappings = new Dictionary<string, object>();

            if (parmType.IsAttributeDefined<ScriptableTypeAttribute>(false))
            {
                
                var scriptAlias = string.Empty;
                var properties = parmType.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                for (int i = 0; i < properties.Length; i++)
                {
                    var propertyInfo = properties[i];
                    var enumConversionType = ConvertEnum.Default;

                    if (propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.GetMethod != null)
                    {
                        scriptAlias = propertyInfo.Name;
                        if (propertyInfo.IsDefined(typeof(ScriptableMemberAttribute), false))
                        {
                            var att = propertyInfo.GetCustomAttribute<ScriptableMemberAttribute>(false);
                            scriptAlias = (att.ScriptAlias ?? scriptAlias);
                            enumConversionType = att.EnumValue;
                        }

                        if (propertyInfo.PropertyType.IsEnum)
                        {
                            switch (enumConversionType)
                            {
                                case ConvertEnum.ToLower:
                                    propertyMappings.Add(scriptAlias, propertyInfo.GetValue(value).ToString().ToLower());
                                    break;
                                case ConvertEnum.ToUpper:
                                    propertyMappings.Add(scriptAlias, propertyInfo.GetValue(value).ToString().ToUpper());
                                    break;
                                case ConvertEnum.Numeric:
                                    propertyMappings.Add(scriptAlias, (int)Enum.Parse(propertyInfo.PropertyType, propertyInfo.GetValue(value).ToString()));
                                    break;
                                default:
                                    propertyMappings.Add(scriptAlias, propertyInfo.GetValue(value));
                                    break;
                            }

                        }
                        else
                        {
                            propertyMappings.Add(scriptAlias, propertyInfo.GetValue(value));
                        }
                    }
                }
            }

            return propertyMappings;
        }

        public enum ScriptMemberMappingDirection
        {
            ScriptAliasToMember,
            MemberToScriptAlias
        }

        public static IDictionary<string, string> GetScriptMemberMappings (Type type, ScriptMemberMappingDirection direction = ScriptMemberMappingDirection.MemberToScriptAlias )
        {
            var map = new Dictionary<string, string>();
            var scriptAlias = string.Empty;

            // add properties
            foreach (PropertyInfo pi in type.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                if (pi.IsSpecialName)
                    continue;

                scriptAlias = pi.Name;
                if (pi.IsDefined(typeof(ScriptableMemberAttribute), false))
                {
                    var att = pi.GetCustomAttribute<ScriptableMemberAttribute>(false);
                    scriptAlias = (att.ScriptAlias ?? scriptAlias);
                }

                if (direction == ScriptMemberMappingDirection.MemberToScriptAlias)
                    map[pi.Name] = scriptAlias;
                else
                    map[scriptAlias] = pi.Name;
            }

            // add events
            foreach (EventInfo ei in type.GetTypeInfo().GetEvents())
            {
                if (ei.IsSpecialName)
                    continue;

                scriptAlias = ei.Name;
                if (ei.IsDefined(typeof(ScriptableMemberAttribute), false))
                {
                    var att = ei.GetCustomAttribute<ScriptableMemberAttribute>(false);
                    scriptAlias = (att.ScriptAlias ?? scriptAlias);
                }

                if (direction == ScriptMemberMappingDirection.MemberToScriptAlias)
                    map[ei.Name] = scriptAlias;
                else
                    map[scriptAlias] = ei.Name;
            }

            // add methods
            foreach (MethodInfo mi in type.GetTypeInfo().GetMethods())
            {
                if (mi.IsSpecialName)
                    continue;

                scriptAlias = mi.Name;
                if (mi.IsDefined(typeof(ScriptableMemberAttribute), false))
                {
                    var att = mi.GetCustomAttribute<ScriptableMemberAttribute>(false);
                    scriptAlias = (att.ScriptAlias ?? scriptAlias);
                }

                if (direction == ScriptMemberMappingDirection.MemberToScriptAlias)
                    map[mi.Name] = scriptAlias;
                else
                    map[scriptAlias] = mi.Name;
            }
            return map;
        }

        public static ScriptObjectProxy AnonymousObjectToScriptObjectProxy(dynamic source)
        {
            IDictionary<string, object> dict = source;
            
            // The key `websharp_id` represents a wrapped proxy object
            if (dict.ContainsKey("websharp_id"))
            {
                return new ScriptObjectProxy(source);
            }
            return null;
        }

        public static object AnonymousObjectToScriptObjectType(Type type, dynamic source)
        {
            var proxy = AnonymousObjectToScriptObjectProxy(source);
            if (proxy != null)
                return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { proxy }, null);
            else
                return null;

        }

        public static bool DictionaryToScriptableType(IDictionary<string, object> parm, object obj)
        {

            var parmType = obj.GetType();
            var success = false;

            if (parmType.IsAttributeDefined<ScriptableTypeAttribute>(false))
            {
                var mappings = GetScriptMemberMappings(parmType, ScriptMemberMappingDirection.ScriptAliasToMember);

                foreach(var key in parm.Keys)
                {
                    if (mappings.ContainsKey(key))
                    {
                        var pi = parmType.GetProperty(mappings[key]);
                        if (pi.SetMethod != null)
                        {
                            if (pi.PropertyType.IsEnum)
                            {
                                // Try to map enum string values to their enum types.
                                var lowerKey = parm[key].ToString().ToLower();
                                foreach (var ev in Enum.GetNames(pi.PropertyType))
                                {
                                    if (ev.ToLower() == lowerKey)
                                        pi.SetValue(obj, Enum.Parse(pi.PropertyType, ev));
                                }
                            }
                            else
                            {
                                var scriptObject = parm[key] as IDictionary<string, object>;
                                if (scriptObject != null)
                                {
                                    if (scriptObject.ContainsKey("websharp_id"))
                                    {
                                        pi.SetValue(obj, AnonymousObjectToScriptObjectType(pi.PropertyType, parm[key]));
                                    }
                                    else
                                    {
                                        pi.SetValue(obj, AnonymousObjectToScriptableType(pi.PropertyType, parm[key]));
                                    }
                                }
                                else
                                {
                                    pi.SetValue(obj, parm[key]);
                                }
                            }
                            success = true;
                        }
                    }
                }

            }
            return success;
        }

        public static T AnonymousObjectToScriptableType<T>(object obj) 
        {
            var typeOfT = typeof(T);
            var st = (T)Activator.CreateInstance(typeOfT);
            var dic = obj as IDictionary<string, object>;
            if (dic == null)
            {
                return st;
            }

            DictionaryToScriptableType(dic, st);
            return st;

        }

        public static object AnonymousObjectToScriptableType(Type type, object obj)
        {
            var st = Activator.CreateInstance(type);
            var dic = obj as IDictionary<string, object>;
            if (dic == null)
            {
                return st;
            }

            DictionaryToScriptableType(dic, st);
            return st;

        }
    }
}