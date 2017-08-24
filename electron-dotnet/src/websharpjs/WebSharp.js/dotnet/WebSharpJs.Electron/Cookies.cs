using System;
using System.Threading.Tasks;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class Cookies : EventEmitter
    {
        //static readonly string cookiesScriptProxy = @"function () { 

        //                    if (options && options.length > 0)
        //                    {
        //                        if (options[0] === 'session.defaultCookies')
        //                            return session.defaultCookies;

        //                        if (options[0] === 'session.fromPartition')
        //                            return session.fromPartition(options[1], options[2]);

        //                    }
        //                    else
        //                        return session.defaultCookies;
        //                }()";

        //static readonly string sessionRequires = @"const {session} = websharpjs.IsRenderer() ? require('electron').remote : require('electron');";

        //protected override string ScriptProxy => cookiesScriptProxy;

        //protected override string Requires => sessionRequires;


        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        private Cookies() : base() { }
        private Cookies(object obj) : base(obj) { }

        private Cookies(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Cookies(ScriptObjectProxy sop)
        {
            return new Cookies(sop);
        }

        public async Task Get(Filter filter, ScriptObjectCallback<Error, Cookie[]> callback)
        {
            await Invoke<object>("get", filter, callback);
        }

        public async Task Set(Details details, ScriptObjectCallback<Error> callback)
        {
            await Invoke<object>("set", details, callback);
        }

        public async Task Remove(string url, string name, ScriptObjectCallback callback)
        {
            await Invoke<object>("remove", url, name, callback);
        }

        public async Task FlushStore(ScriptObjectCallback callback)
        {
            await Invoke<object>("flushStore", callback);
        }

    }

    [ScriptableType]
    public struct Details
    {

        [ScriptableMember(ScriptAlias = "url")]
        public string Url { get; set; }
        [ScriptableMember(ScriptAlias = "name")]
        public string Name { get; set; }
        [ScriptableMember(ScriptAlias = "value")]
        public string Value { get; set; }
        [ScriptableMember(ScriptAlias = "domain")]
        public string Domain { get; set; }
        [ScriptableMember(ScriptAlias = "path")]
        public string Path { get; set; }
        [ScriptableMember(ScriptAlias = "secure")]
        public bool? Secure { get; set; }
        [ScriptableMember(ScriptAlias = "httpOnly")]
        public bool? HttpOnly { get; set; }
        [ScriptableMember(ScriptAlias = "expirationDate")]
        public double? expirationDate { get; set; }
    }

    [ScriptableType]
    public struct Filter
    {
        [ScriptableMember(ScriptAlias = "url")]
        public string Url { get; set; }
        [ScriptableMember(ScriptAlias = "name")]
        public string Name { get; set; }
        [ScriptableMember(ScriptAlias = "domain")]
        public string Domain { get; set; }
        [ScriptableMember(ScriptAlias = "path")]
        public string Path { get; set; }
        [ScriptableMember(ScriptAlias = "secure")]
        public bool? Secure { get; set; }
        [ScriptableMember(ScriptAlias = "session")]
        public bool? Session { get; set; }
  }

}
