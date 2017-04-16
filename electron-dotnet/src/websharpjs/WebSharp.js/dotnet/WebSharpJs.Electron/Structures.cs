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

}
