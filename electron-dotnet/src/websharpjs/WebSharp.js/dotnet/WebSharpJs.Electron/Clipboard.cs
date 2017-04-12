using System.Linq;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{

    public enum ClipboardType
    {
        None,
        Selection
    }

    [ScriptableType]
    public class ClipboardData
    {
        [ScriptableMember(ScriptAlias = "text")]
        public string Text { get; set; }
        [ScriptableMember(ScriptAlias = "html")]
        public string HTML { get; set; }
        [ScriptableMember(ScriptAlias = "image")]
        public object Image { get; set; }
        [ScriptableMember(ScriptAlias = "rtf")]
        public string RTF { get; set; }
        [ScriptableMember(ScriptAlias = "bookmark")]
        public string Bookmark { get; set; }
    }

    [ScriptableType]
    public class Bookmark
    {
        [ScriptableMember(ScriptAlias = "title")]
        public string Title { get; set; }
        [ScriptableMember(ScriptAlias = "url")]
        public string URL { get; set; }
    }

    public class Clipboard : NodeJsObject
    {
        protected override string ScriptProxy => @"clipboard; ";
        protected override string Requires => @"const {clipboard} = require('electron');";

        static Clipboard proxy;

        public static async Task<Clipboard> Instance()
        {
            if (proxy == null)
            {
                proxy = new Clipboard();
                await proxy.Initialize();
            }
            return proxy;
        }

        protected Clipboard() : base() { }

        protected Clipboard(object scriptObject) : base(scriptObject)
        { }

        public Clipboard(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Clipboard(ScriptObjectProxy sop)
        {
            return new Clipboard(sop);
        }

        public async Task<string> ReadText(ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<string>("readText", type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<object> WriteText(string text, ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<object>("writeText", text, type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<string> ReadHTML(ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<string>("readHTML", type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<object> WriteHTML(string markup, ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<object>("writeHTML", markup, type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<string> ReadRTF(ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<string>("readRTF", type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<object> WriteRTF(string markup, ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<object>("writeRTF", markup, type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<Bookmark> ReadBookmark(ClipboardType type = ClipboardType.None)
        {
            var result = await InvokeAsync<object>("readBookmark", type == ClipboardType.Selection ? "selection" : string.Empty);
            var bm = new Bookmark();
            if (result != null)
                ScriptObjectHelper.DictionaryToScriptableType((System.Collections.Generic.IDictionary<string, object>)result, bm);

            return bm;
        }

        public async Task<object> WriteBookmark(string title, string url, ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<object>("writeBookmark", title, url, type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<string> ReadFindText(ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<string>("readFindText", type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<object> WriteFindText(string text, ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<object>("writeFindText", text, type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<object> Clear(ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<object>("clear", type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<string[]> AvailableFormats(ClipboardType type = ClipboardType.None)
        {
            var result = await InvokeAsync<object>("availableFormats", type == ClipboardType.Selection ? "selection" : string.Empty);
            return (result == null ? new string[] { } : ((System.Collections.IEnumerable)result)
                                                          .Cast<object>()
                                                          .Select(x => x.ToString())
                                                          .ToArray());
        }

        public async Task<bool> Has(string format, ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<bool>("has", format, type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<string> Read(string format, ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<string>("read", format, type == ClipboardType.Selection ? "selection" : string.Empty);
        }

        public async Task<object> Write(ClipboardData data, ClipboardType type = ClipboardType.None)
        {
            return await InvokeAsync<object>("write", data, type == ClipboardType.Selection ? "selection" : string.Empty);
        }

    }
}
