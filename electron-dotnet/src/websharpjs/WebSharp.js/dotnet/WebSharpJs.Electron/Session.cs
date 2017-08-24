using System;
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

        public static async Task<Session> FromPartition(string partition, FromPartitionOptions? options = null)
        {
            var proxy = new Session();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize("session.fromPartition", partition, 
                            (options != null && options.HasValue) ? options.Value : (object)null
                );

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

        public async Task ClearStorageData(ClearStorageDataOptions? options = null, ScriptObjectCallback callback = null)
        {

            await Invoke<object>("clearStorageData", 
                (options != null && options.HasValue) ? options.Value : (object)null,
                callback);
        }

        public async Task FlushStorageData()
        {
            await Invoke<object>("flushStorageData");
        }

        public async Task SetProxy(Config config, ScriptObjectCallback callback = null)
        {
            await Invoke<object>("setProxy", config, callback);
        }

        public async Task ResolveProxy(string url, ScriptObjectCallback<string> callback = null)
        {
            await Invoke<object>("setProxy", url, callback);
        }

        public async Task SetDownloadPath(string path)
        {
            await Invoke<object>("setDownloadPath", path);
        }

        public async Task EnableNetworkEmulation(EnableNetworkEmulationOptions options)
        {
            await Invoke<object>("enableNetworkEmulation", options);
        }

        public async Task DisableNetworkEmulation()
        {
            await Invoke<object>("disableNetworkEmulation");
        }

        public async Task SetPermissionRequestHandler(ScriptObjectCallback<WebContents, string, Func<object, Task<object>>> handler)
        {
            await Invoke<object>("setPermissionRequestHandler", handler);
        }

        public async Task SetCertificateVerifyProc(ScriptObjectCallback<CertificateVerifyProcRequest, Func<object, Task<object>>> handler)
        {
            await Invoke<object>("setCertificateVerifyProc", handler);
        }


        public async Task ClearHostResolverCache(ScriptObjectCallback callback = null)
        {
            await Invoke<object>("clearHostResolverCache", callback);
        }

        public async Task AllowNTLMCredentialsForDomains(string domains)
        {
            await Invoke<object>("allowNTLMCredentialsForDomains", domains);
        }

        public async Task SetUserAgent(string userAgent, string acceptLanguages = null)
        {
            if (acceptLanguages == null)
                await Invoke<object>("setUserAgent", userAgent);
            else
                await Invoke<object>("setUserAgent", userAgent, acceptLanguages); 
        }


        public async Task<string> GetUserAgent()
        {
            return await Invoke<string>("getUserAgent");
        }

        public async Task<byte[]> GetBlobData(string identifier)
        {
            return await Invoke<byte[]>("getBlobData");
        }

        public async Task GetBlobData(string identifier, ScriptObjectCallback<byte[]> callback)
        {
            await Invoke<object>("getBlobData", callback);
        }

        public async Task CreateInterruptedDownload(CreateInterruptedDownloadOptions options)
        {
            await Invoke<object>("createInterruptedDownload", options);
        }

        public async Task ClearAuthCache(RemoveClientCertificate options, ScriptObjectCallback<byte[]> callback = null)
        {
            if (callback != null)
                await Invoke<object>("clearAuthCache", options, callback);
            else
                await Invoke<object>("clearAuthCache", options);
        }

        public async Task ClearAuthCache(RemovePassword options, ScriptObjectCallback<byte[]> callback = null)
        {
            if (callback != null)
                await Invoke<object>("clearAuthCache", options, callback);
            else
                await Invoke<object>("clearAuthCache", options);
        }

        public async Task<WebRequest> GetWebRequest()
        {
            return await GetProperty<WebRequest>("webRequest");
        }

        public async Task SetWebRequest(WebRequest webRequest)
        {
            await SetProperty("webRequest", webRequest);
        }

        public async Task<Cookies> Cookies()
        {
            return await GetProperty<Cookies>("cookies");
        }
    }

    [ScriptableType]
    public struct FromPartitionOptions
    {
        [ScriptableMember(ScriptAlias = "cache")]
        public bool? Cache { get; set; }
    }

    [ScriptableType]
    public struct ClearStorageDataOptions
    {
        [ScriptableMember(ScriptAlias = "origin")]
        public string Origin { get; set; }
        [ScriptableMember(ScriptAlias = "storages")]
        public string[] Storages { get; set; }
        [ScriptableMember(ScriptAlias = "quotas")]
        public string[] Quotas { get; set; }
    }

    [ScriptableType]
    public struct Config
    {
        [ScriptableMember(ScriptAlias = "pacScript")]
        public string PacScript { get; set; }
        [ScriptableMember(ScriptAlias = "proxyRules")]
        public string ProxyRules { get; set; }
        [ScriptableMember(ScriptAlias = "proxyBypassRules")]
        public string ProxyBypassRules { get; set; }

    }

    [ScriptableType]
    public struct EnableNetworkEmulationOptions
    {
        [ScriptableMember(ScriptAlias = "offline")]
        public bool? Offline { get; set; }
        [ScriptableMember(ScriptAlias = "latency")]
        public int? Latency { get; set; }
        [ScriptableMember(ScriptAlias = "downloadThroughput")]
        public int? DownloadThroughput { get; set; }
        [ScriptableMember(ScriptAlias = "uploadThroughput")]
        public int? UploadThroughput { get; set; }

    }

    [ScriptableType]
    public struct CreateInterruptedDownloadOptions
    {
        [ScriptableMember(ScriptAlias = "path")]
        public string Path { get; set; }
        [ScriptableMember(ScriptAlias = "urlChain")]
        public string[] UrlChain { get; set; }
        [ScriptableMember(ScriptAlias = "mimeType")]
        public string MimeType { get; set; }
        [ScriptableMember(ScriptAlias = "offset")]
        public int? Offset { get; set; }
        [ScriptableMember(ScriptAlias = "length")]
        public int? Length { get; set; }
        [ScriptableMember(ScriptAlias = "lastModified")]
        public string LastModified { get; set; }
        [ScriptableMember(ScriptAlias = "eTag")]
        public string ETag { get; set; }
        [ScriptableMember(ScriptAlias = "startTime")]
        public int? StartTime { get; set; }
    }

    [ScriptableType]
    public class CertificateVerifyProcRequest
    {
        [ScriptableMember(ScriptAlias = "hostname")]
        public string Hostname { get; set; }
        [ScriptableMember(ScriptAlias = "certificate")]
        public Certificate Certificate { get; set; }
        [ScriptableMember(ScriptAlias = "error")]
        public string Error { get; set; }

    }

    public class CertificateVerifyProcResult : ScriptObjectCallbackResult
    {

        public CertificateVerifyProcRequest CertificateVerifyProcRequest { get; private set; }
        public Func<object, Task<object>> Callback { get; private set; }

        public CertificateVerifyProcResult(object state) : base(state)
        {

            var result = state as object[];
            if (result != null)
            {
                CertificateVerifyProcRequest = result[0] as CertificateVerifyProcRequest;
                Callback = result[1] as Func<object, Task<object>>;
            }
        }

        public CertificateVerifyProcResult(ICallbackResult result) : this(result.CallbackState)
        { }

        public async Task<bool> SendVerificationResult(int response)
        {
            if (Callback == null)
                return false;

            await Callback(response);

            return true;
        }

    }

    public enum Permission
    {
        Undefined,
        Media,
        GeoLocation,
        Notifications,
        MidiSysex,
        PointerLock,
        FullScreen,
        OpenExternal
    }

	public class PermissionRequestResult : ScriptObjectCallbackResult
	{

		public WebContents WebContents { get; private set; }
        public string PermissionValue { get; private set; }
        public Func<object, Task<object>> Callback { get; private set; }

		public PermissionRequestResult(object state) : base(state)
		{

			var result = state as object[];
			if (result != null)
			{
				WebContents = result[0] as WebContents;
                PermissionValue = result[1].ToString();
				Callback = result[2] as Func<object, Task<object>>;
			}
		}

		public PermissionRequestResult(ICallbackResult result) : this(result.CallbackState)
		{ }

		public async Task<bool> GrantPermission(bool granted)
		{
			if (Callback == null)
				return false;

			await Callback(granted);

			return true;
		}

        public Permission Permission
        {
            get
            {
                var permission = Permission.Undefined;
                Enum.TryParse<Permission>(PermissionValue, true, out permission);
                return permission;
            }
        }
            


    }

    public class WillDownloadResult : ScriptObjectCallbackResult
    {

        public WebContents WebContents { get; private set; }
        public Event Event { get; private set; }
        public DownloadItem DownloadItem { get; private set; }

        public WillDownloadResult(object state) : base(state)
        {

            var result = state as object[];
            if (result != null)
            {
                Event = result[0] as Event;
                DownloadItem = result[1] as DownloadItem;
                WebContents = result[2] as WebContents;
            }
        }

        public WillDownloadResult(ICallbackResult result) : this(result.CallbackState)
        { }
    }

}
