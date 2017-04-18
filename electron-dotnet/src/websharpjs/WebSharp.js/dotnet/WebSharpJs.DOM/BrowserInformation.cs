
using WebSharpJs.Script;

namespace WebSharpJs.DOM
{
    [ScriptableType]
    public class BrowserInformation
    {
        [ScriptableMember(ScriptAlias = "appName")]
        public string Name { get; internal set; }
        [ScriptableMember(ScriptAlias = "appVersion")]
        public string BrowserVersion { get; internal set; }
        [ScriptableMember(ScriptAlias = "userAgent")]
        public string UserAgent { get; internal set; }
        [ScriptableMember(ScriptAlias = "platform")]
        public string Platform { get; internal set; }
        [ScriptableMember(ScriptAlias = "cookieEnabled")]
        public bool CookiesEnabled { get; internal set; }
        [ScriptableMember(ScriptAlias = "appCodeName")]
        public string ProductName { get; internal set; }
        [ScriptableMember(ScriptAlias = "productVersion")]
        public string ProductVersion { get; internal set; }

    }
}
