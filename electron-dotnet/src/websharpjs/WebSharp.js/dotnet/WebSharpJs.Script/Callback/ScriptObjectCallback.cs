using System;
using System.Threading.Tasks;

namespace WebSharpJs.Script
{
    public delegate Task ScriptObjectCallbackDelegate(ICallbackResult ar);

    public class ScriptObjectCallback : IScriptObjectCallback, IScriptObjectCallbackProxy
    {

        static readonly MetaData[] NoMappings = new MetaData[0];
        internal Delegate Callback;
        Func<object, Task<object>> callbackProxy;

        Func<object, Task<object>> IScriptObjectCallbackProxy.CallbackProxy => callbackProxy;
        MetaData[] IScriptObjectCallbackProxy.TypeMappings => NoMappings;

        public ScriptObjectCallback(ScriptObjectCallbackDelegate callbackDelegate)
        {
            Callback = callbackDelegate;
            callbackProxy = (async (evt) =>
            {
                Invoke(evt);
                return null;
            });
        }

        void Invoke(object evtParm)
        {
            try
            {
                ScriptObjectCallbackResult socr = new ScriptObjectCallbackResult(null);
                Callback.DynamicInvoke(socr);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }

    public class ScriptObjectCallback<T> : IScriptObjectCallback, IScriptObjectCallbackProxy
    {

        internal Delegate Callback;
        MetaData[] mappings;
        Type[] argumentTypes;
        Func<object, Task<object>> callbackProxy;

        bool defaultPrevented = false;
        bool cancelBubble = false;

        Action PreventDefaultAction;

        Func<object, Task<object>> IScriptObjectCallbackProxy.CallbackProxy => callbackProxy;

        MetaData[] IScriptObjectCallbackProxy.TypeMappings => mappings;

        public ScriptObjectCallback(ScriptObjectCallbackDelegate callbackDelegate)
        {
            Callback = callbackDelegate;
            argumentTypes = this.GetType().GetGenericArguments();
            // if their are no generic arguments on the class then this could be
            // an object that extends one of the ScripObjectCallback<,> classes
            // so we will also try that.
            if (argumentTypes == null || argumentTypes.Length == 0)
                argumentTypes = this.GetType().BaseType.GetGenericArguments();

            mappings = ScriptObjectUtilities.GenerateMetaData(argumentTypes, true);
            PreventDefaultAction = new Action(() => { defaultPrevented = true; });

            callbackProxy = (async (evt) =>
            {
                Invoke(evt);
                return new { defaultPrevented, cancelBubble }; ;
            });
        }

        void Invoke(object evtParm)
        {
            object[] parms;

            defaultPrevented = false;
            cancelBubble = false;
            
            if (evtParm.GetType().IsArray)
            {
                try
                {
                    var evt = (object[])evtParm;
                    parms = new object[evt.Length];
                    for (int mi = 0; mi < evt.Length; mi++)
                    {
                        if (mi < argumentTypes.Length)
                        {
                            parms[mi] = ScriptObjectUtilities.MapToType((ScriptParmCategory)(mappings[mi].Category), mappings[mi].IsArray,
                                evt[mi], argumentTypes[mi], PreventDefaultAction);

                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception from MapToType in ScriptObjectCallback:Invoke: " + ex.Message);
                    parms = new object[] { null };
                }
            }
            else
            {
                try
                {
                    parms = new object[] {
                        ScriptObjectUtilities.MapToType((ScriptParmCategory)(mappings[0].Category), mappings[0].IsArray,
                        evtParm, argumentTypes[0])
                    };

                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("exception from MapToType in ScriptObjectCallback:Invoke: " + ex.Message);
                    parms = new object[] { null };
                }

            }

            try
            {
                ScriptObjectCallbackResult socr;
                if (parms.Length == 1)
                    socr = new ScriptObjectCallbackResult(parms[0]);
                else
                    socr = new ScriptObjectCallbackResult(parms);

                Callback.DynamicInvoke(socr);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            
        }
    }

    public class ScriptObjectCallback<T1, T2> : ScriptObjectCallback<T1>
    {
        public ScriptObjectCallback(ScriptObjectCallbackDelegate callbackDelegate) : base(callbackDelegate)
        { }

    }

    public class ScriptObjectCallback<T1, T2, T3> : ScriptObjectCallback<T1>
    {
        public ScriptObjectCallback(ScriptObjectCallbackDelegate callbackDelegate) : base(callbackDelegate)
        { }

    }

    public class ScriptObjectCallback<T1, T2, T3, T4> : ScriptObjectCallback<T1>
    {
        public ScriptObjectCallback(ScriptObjectCallbackDelegate callbackDelegate) : base(callbackDelegate)
        { }

    }

    public class ScriptObjectCallback<T1, T2, T3, T4, T5> : ScriptObjectCallback<T1>
    {
        public ScriptObjectCallback(ScriptObjectCallbackDelegate callbackDelegate) : base(callbackDelegate)
        { }

    }

}
