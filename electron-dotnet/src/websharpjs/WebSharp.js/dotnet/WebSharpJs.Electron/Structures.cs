using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    [ScriptableType]
    public struct FileFilter
    {
        [ScriptableMember(ScriptAlias = "name")]
        public string Name { get; set; }
        [ScriptableMember(ScriptAlias = "extensions")]
        public string[] Extensions { get; set; }
    }

    [ScriptableType]
    public struct Size
    {
        public static readonly Size Empty = new Size();

        [ScriptableMember(ScriptAlias = "width")]
        public float Width { get; set; }
        [ScriptableMember(ScriptAlias = "height")]
        public float Height { get; set; }

        public Size(dynamic obj)
        {
            var dict = obj as System.Collections.Generic.IDictionary<string, object>;
            Width = System.Convert.ToSingle(dict["width"]);
            Height = System.Convert.ToSingle(dict["height"]);
        }
    }

    [ScriptableType]
    public struct Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle(0,0);

        [ScriptableMember(ScriptAlias = "x")]
        public float X { get; set; }
        [ScriptableMember(ScriptAlias = "y")]
        public float Y { get; set; }
        [ScriptableMember(ScriptAlias = "width")]
        public float Width { get; set; }
        [ScriptableMember(ScriptAlias = "height")]
        public float Height { get; set; }

        public Rectangle(float x, float y, float width = 0, float height = 0)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }


        public Rectangle(dynamic obj)
        {
            var dict = obj as System.Collections.Generic.IDictionary<string, object>;
            X = System.Convert.ToSingle(dict["x"]);
            Y = System.Convert.ToSingle(dict["y"]);
            Width = System.Convert.ToSingle(dict["width"]);
            Height = System.Convert.ToSingle(dict["height"]);
        }
    }

}
