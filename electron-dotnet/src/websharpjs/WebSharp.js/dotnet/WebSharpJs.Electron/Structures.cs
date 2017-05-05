using System;
using System.Threading.Tasks;
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

        public override string ToString()
        {
            return $"FileFilter {{ Name: {Name} Extenstions: {Extensions} }}";
        }
    }

    [ScriptableType]
    public struct Size
    {
        public static readonly Size Empty = new Size();

        [ScriptableMember(ScriptAlias = "width")]
        public int Width { get; set; }
        [ScriptableMember(ScriptAlias = "height")]
        public int Height { get; set; }

        public Size(dynamic obj)
        {
            var dict = obj as System.Collections.Generic.IDictionary<string, object>;
            Width = System.Convert.ToInt32(dict["width"]);
            Height = System.Convert.ToInt32(dict["height"]);
        }

        public override string ToString()
        {
            return $"Size {{ Width: {Width} Height: {Height} }}";
        }
    }

    [ScriptableType]
    public struct SizeF
    {
        public static readonly SizeF Empty = new SizeF();

        [ScriptableMember(ScriptAlias = "width")]
        public float Width { get; set; }
        [ScriptableMember(ScriptAlias = "height")]
        public float Height { get; set; }

        public SizeF(dynamic obj)
        {
            var dict = obj as System.Collections.Generic.IDictionary<string, object>;
            Width = System.Convert.ToSingle(dict["width"]);
            Height = System.Convert.ToSingle(dict["height"]);
        }

        public override string ToString()
        {
            return $"Size {{ Width: {Width} Height: {Height} }}";
        }
    }

    [ScriptableType]
    public struct Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle(0,0);

        [ScriptableMember(ScriptAlias = "x")]
        public int X { get; set; }
        [ScriptableMember(ScriptAlias = "y")]
        public int Y { get; set; }
        [ScriptableMember(ScriptAlias = "width")]
        public int Width { get; set; }
        [ScriptableMember(ScriptAlias = "height")]
        public int Height { get; set; }

        public Rectangle(int x, int y, int width = 0, int height = 0)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }


        public Rectangle(dynamic obj)
        {
            var dict = obj as System.Collections.Generic.IDictionary<string, object>;
            X = System.Convert.ToInt32(dict["x"]);
            Y = System.Convert.ToInt32(dict["y"]);
            Width = System.Convert.ToInt32(dict["width"]);
            Height = System.Convert.ToInt32(dict["height"]);
        }

        public override string ToString()
        {
            return $"Rect {{ X: {X} Y: {Y} Width: {Width} Height: {Height} }}";
        }
    }

    [ScriptableType]
    public struct RectangleF
    {
        public static readonly RectangleF Empty = new RectangleF(0, 0);

        [ScriptableMember(ScriptAlias = "x")]
        public float X { get; set; }
        [ScriptableMember(ScriptAlias = "y")]
        public float Y { get; set; }
        [ScriptableMember(ScriptAlias = "width")]
        public float Width { get; set; }
        [ScriptableMember(ScriptAlias = "height")]
        public float Height { get; set; }

        public RectangleF(float x, float y, float width = 0, float height = 0)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }


        public RectangleF(dynamic obj)
        {
            var dict = obj as System.Collections.Generic.IDictionary<string, object>;
            X = System.Convert.ToSingle(dict["x"]);
            Y = System.Convert.ToSingle(dict["y"]);
            Width = System.Convert.ToSingle(dict["width"]);
            Height = System.Convert.ToSingle(dict["height"]);
        }

        public override string ToString()
        {
            return $"Rect {{ X: {X} Y: {Y} Width: {Width} Height: {Height} }}";
        }
    }

    [ScriptableType]
    public struct Point
    {
        public static readonly Point Empty = new Point();

        [ScriptableMember(ScriptAlias = "x")]
        public int X { get; set; }
        [ScriptableMember(ScriptAlias = "y")]
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(dynamic obj)
        {
            var dict = obj as System.Collections.Generic.IDictionary<string, object>;
            X = System.Convert.ToInt32(dict["x"]);
            Y = System.Convert.ToInt32(dict["y"]);
        }

        public override string ToString()
        {
            return $"Point {{ X: {X} Y: {Y} }}";
        }
    }

    [ScriptableType]
    public struct PointF
    {
        public static readonly PointF Empty = new PointF();

        [ScriptableMember(ScriptAlias = "x")]
        public float X { get; set; }
        [ScriptableMember(ScriptAlias = "y")]
        public float Y { get; set; }

        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public PointF(dynamic obj)
        {
            var dict = obj as System.Collections.Generic.IDictionary<string, object>;
            X = System.Convert.ToSingle(dict["x"]);
            Y = System.Convert.ToSingle(dict["y"]);
        }

        public override string ToString()
        {
            return $"Point {{ X: {X} Y: {Y} }}";
        }
    }

    [ScriptableType]
    public struct DesktopCapturerSource
    {
        [ScriptableMember(ScriptAlias = "id")]
        public string Id { get; set; }
        [ScriptableMember(ScriptAlias = "name")]
        public string Name { get; set; }
        [ScriptableMember(ScriptAlias = "thumbnail")]
        public NativeImage Thumbnail { get; set; }

    }

    [ScriptableType]
    public class Error
    {
        [ScriptableMember(ScriptAlias = "name")]
        public string Name { get; set; }
        [ScriptableMember(ScriptAlias = "message")]
        public string Message { get; set; }
        [ScriptableMember(ScriptAlias = "stack")]
        public string Stack { get; set; }
    }

    [ScriptableType]
    public class Event
    {
        [ScriptableMember(ScriptAlias = "preventDefault")]
        public Func<object, Task<object>> PreventDefault { get; set; }
        [ScriptableMember(ScriptAlias = "sender")]
        public NodeJS.EventEmitter Sender { get; set; }

    }

}
