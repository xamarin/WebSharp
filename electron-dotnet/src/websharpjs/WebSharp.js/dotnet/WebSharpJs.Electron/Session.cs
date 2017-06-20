using System.Threading.Tasks;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class Session : EventEmitter
    {
        static readonly string sessionScriptProxy = @"function () { 

                            if (options && options.length > 0)
                            {
                                if (options[0] === 'session.defaultSession')
                                    return session.defaultSession;

                                if (options[0] === 'session.fromPartition')
                                    return session.fromPartition(options[1], options[2]);

                            }
                            else
                                return session.defaultSession;
                        }()";

        static readonly string sessionRequires = @"const {session} = websharpjs.IsRenderer() ? require('electron').remote : require('electron');";

        protected override string ScriptProxy => sessionScriptProxy;

        protected override string Requires => sessionRequires;


        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<Session> DefaultSession()
        {
            var proxy = new Session();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize();
            return proxy;

        }

        public static async Task<Session> FromPartition(string partition, FromPartitionOptions options)
        {
            var proxy = new Session();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize("session.fromPartition", partition, options);
            return proxy;

        }

        private Session() : base() { }
        private Session(object obj) : base(obj) { }

        private Session(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Session(ScriptObjectProxy sop)
        {
            return new Session(sop);
        }

        public async Task GetCacheSize(ScriptObjectCallback<int> callback)
        {
            await Invoke<object>("getCacheSize", callback);
        }

        public async Task ClearCache(ScriptObjectCallback callback)
        {
            await Invoke<object>("clearCache", callback);
        }

        public async Task FlushStorageData()
        {
            await Invoke<object>("flushStorageData");
        }

        public async Task SetDownloadPath(string path)
        {
            await Invoke<object>("setDownloadPath", path);
        }

        public async Task DisableNetworkEmulation()
        {
            await Invoke<object>("disableNetworkEmulation");
        }

        public async Task ClearHostResolverCache(ScriptObjectCallback callback = null)
        {
            await Invoke<object>("clearHostResolverCache", callback);
        }

        public async Task AllowNTLMCredentialsForDomains(string domains)
        {
            await Invoke<object>("allowNTLMCredentialsForDomains", domains);
        }

        public async Task<string> GetUserAgent()
        {
            return await Invoke<string>("getUserAgent");
        }

        public async Task<byte[]> getBlobData(string identifier)
        {
            return await Invoke<byte[]>("getBlobData");
        }

        public async Task getBlobData(string identifier, ScriptObjectCallback<byte[]> callback)
        {
            await Invoke<object>("getBlobData", callback);
        }
    }

    [ScriptableType]
    public struct FromPartitionOptions
    {
        [ScriptableMember(ScriptAlias = "cache")]
        public bool? Cache { get; set; }
  }
}
