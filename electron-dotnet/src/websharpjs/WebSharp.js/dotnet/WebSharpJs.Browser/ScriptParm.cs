//
// ScriptObject.cs
//


namespace WebSharpJs.Browser
{

    internal struct ScriptParm
    {
        public int Category;
        public string Type;
        public object Value;
    }

    internal enum ScriptParmCategory
    {
        ScriptObject = 1,
        ScriptableType = 2,
        ScriptValue = 3,
        ScriptCallback = 4
    }
    
}