using System;
using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class Shell : NodeJsObject
    {
        protected override string ScriptProxy => @"shell; ";
        protected override string Requires => @"const {shell} = require('electron');";

        static Shell proxy;

        public static async Task<Shell> Instance()
        {
            if (proxy == null)
            {
                proxy = new Shell();
                await proxy.Initialize();
            }
            return proxy;
        }

        protected Shell() : base() { }

        protected Shell(object scriptObject) : base(scriptObject)
        { }

        public Shell(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Shell(ScriptObjectProxy sop)
        {
            return new Shell(sop);
        }

        public async Task Beep()
        {
            await Invoke<object>("beep");
        }

        public async Task<bool> MoveItemToTrash(string fullPath)
        {
            return await Invoke<bool>("moveItemToTrash", fullPath);
        }

        public async Task<bool> OpenExternal(string url, OpenExternalOptions? options = null, ScriptObjectCallback<object> callback = null)
        {
            System.Console.WriteLine((options != null && options.HasValue) ? options.Value : (object)null);
            return await Invoke<bool>("openExternal", url,
                (options != null && options.HasValue) ? options.Value : (object)null,
                callback
                
                );
        }

        public async Task<bool> OpenItem(string fullPath)
        {
            return await Invoke<bool>("openItem", fullPath);
        }

        public async Task<bool> ShowItemInFolder(string fullPath)
        {
            return await Invoke<bool>("showItemInFolder", fullPath);
        }

        public async Task<ShortcutDetails> readShortcutLink(string shortcutPath)
        {
            return await Invoke<ShortcutDetails>("readShortcutLink", shortcutPath);
        }

        public async Task<bool> WriteShortcutLink(string shortcutPath, ShortcutLinkOperation operation, ShortcutLinkOperation options)
        {
            return await Invoke<bool>("writeShortcutLink", shortcutPath, operation.ToString().ToLower(), options);
        }
        public async Task<bool> WriteShortcutLink(string shortcutPath, ShortcutLinkOperation options)
        {
            return await Invoke<bool>("writeShortcutLink", shortcutPath, options);
        }
    }

    public enum ShortcutLinkOperation
    {
        Create,
        Update,
        Replace
    }

    [ScriptableType]
    public struct OpenExternalOptions
    {
        [ScriptableMember(ScriptAlias = "activate")]
        public bool? Activate { get; set; }
    }
}
