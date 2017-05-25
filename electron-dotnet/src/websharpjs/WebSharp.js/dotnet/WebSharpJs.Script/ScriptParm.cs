//
// ScriptObject.cs
//


using System.Collections.Generic;

namespace WebSharpJs.Script
{

    internal struct ScriptParm
    {
        public int Category;
        public string Type;
        public MetaData[] MetaMapping;
        public object Value;
    }

    internal struct MetaData
    {
        public int Category;
        public int IsArray;
        public IDictionary<string, int> ScriptableMapping;
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