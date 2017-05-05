//
// ScriptObject.cs
//


namespace WebSharpJs.Script
{

    internal struct ScriptParm
    {
        public int Category;
        public string Type;
        public int[] CallbackMapping;
        public object Value;
    }

    internal enum ScriptParmCategory
    {
        None = 0,
        ScriptObject = 1,
        ScriptableType = 2,
        ScriptValue = 3,
        ScriptCallback = 4,
        ScriptObjectCollection = 5,
        ScriptableTypeArray = 6,
    }
    
}