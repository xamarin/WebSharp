//
// DragNDrop.cs
//


using System;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{
    [ScriptableType]
    public class DataTransfer
    {
        internal Action<string> DropEffectAction { get; set; }
        string dropEffect { get; set; } = "none";

        [ScriptableMember(ScriptAlias = "dropEffect")]
        internal string DropEffectValue
        {
            get
            {
                
                return dropEffect;
            }
            set
            {
                if (dropEffect != value)
                {
                    dropEffect = value;
                    DropEffectAction?.Invoke(dropEffect.ToLower());
                }
            }

        }

        public DropEffect DropEffect
        {
            get
            {
                var p = DropEffect.None;
                if (Enum.TryParse(dropEffect, true, out p))
                {
                    return p;
                }
                else
                    return DropEffect.None;
            }
            set
            {
                DropEffectValue = value.ToString();
            }
        }

        [ScriptableMember(ScriptAlias = "files")]
        public HTML5File[] Files { get; internal set; }

        [ScriptableMember(ScriptAlias = "types")]
        public object[] Types { get; internal set; }

    }

    [ScriptableType]
    public class HTML5File
    {
        [ScriptableMember(ScriptAlias = "lastModified")]
        public double LastModified { get; internal set; }
        [ScriptableMember(ScriptAlias = "lastModifiedDate")]
        public DateTime LastModifiedDate { get; internal set; }
        [ScriptableMember(ScriptAlias = "name")]
        public string Name { get; internal set; }
        [ScriptableMember(ScriptAlias = "size")]
        public long Size { get; internal set; }
        [ScriptableMember(ScriptAlias = "type")]
        public string Type { get; internal set; }

        // Electron has added a path attribute to the File interface which exposes the file’s real path on filesystem
        [ScriptableMember(ScriptAlias = "path")]
        public string Path { get; internal set; }
    }

    public enum DropEffect
    {
        Copy,
        Move,
        Link,
        None
    }
}
