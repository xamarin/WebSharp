using System;
using System.Threading.Tasks;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class WebRequest : EventEmitter
    {

        protected override string ScriptProxy => string.Empty;

        protected override string Requires => string.Empty;


        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        private WebRequest() : base() { }
        private WebRequest(object obj) : base(obj) { }

        private WebRequest(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator WebRequest(ScriptObjectProxy sop)
        {
            return new WebRequest(sop);
        }

        public async Task OnBeforeRedirect(OnBeforeRedirectFilter filter, ScriptObjectCallback<OnBeforeRedirectDetails> listener)
        {
            await Invoke<object>("onBeforeRedirect", filter, listener);
        }

        public async Task OnBeforeRequest(OnBeforeRequestFilter filter, ScriptObjectCallback<OnBeforeRequestDetails> listener)
        {
            await Invoke<object>("onBeforeRequest", filter, listener);
        }

        public async Task OnBeforeSendHeaders(OnBeforeSendHeadersFilter filter, ScriptObjectCallback listener)
        {
            await Invoke<object>("onBeforeSendHeaders", filter, listener);
        }

        public async Task OnCompleted(OnCompletedFilter filter, ScriptObjectCallback<OnCompletedDetails> listener)
        {
            await Invoke<object>("onCompleted", filter, listener);
        }

        public async Task OnErrorOccurred(OnErrorOccurredFilter filter, ScriptObjectCallback<OnErrorOccurredDetails> listener)
        {
            await Invoke<object>("onErrorOccurred", filter, listener);
        }

        public async Task OnHeadersReceived(OnHeadersReceivedFilter filter, ScriptObjectCallback listener)
        {
            await Invoke<object>("onHeadersReceived", filter, listener);
        }

        public async Task OnResponseStarted(OnResponseStartedFilter filter, ScriptObjectCallback<OnResponseStartedDetails> listener)
        {
            await Invoke<object>("onResponseStarted", filter, listener);
        }

        public async Task OnSendHeaders(OnBeforeSendHeadersFilter filter, ScriptObjectCallback<OnSendHeadersDetails> listener)
        {
            await Invoke<object>("onSendHeaders", filter, listener);
        }

    }

    [ScriptableType]
    public struct OnBeforeRedirectFilter
    {
        [ScriptableMember(ScriptAlias = "urls")]
        public string[] Urls { get; set; }
    }

    [ScriptableType]
    public class OnBeforeRedirectDetails
    {
        [ScriptableMember(ScriptAlias = "id")]
        public string Id { get; set; }
        [ScriptableMember(ScriptAlias = "url")]
        public string Url { get; set; }
        [ScriptableMember(ScriptAlias = "method")]
        public string Method { get; set; }
        [ScriptableMember(ScriptAlias = "resourceType")]
        public string ResourceType { get; set; }
        [ScriptableMember(ScriptAlias = "timestamp")]
        public double Timestamp { get; set; }
        [ScriptableMember(ScriptAlias = "redirectURL")]
        public string RedirectURL { get; set; }
        [ScriptableMember(ScriptAlias = "statusCode")]
        public int StatusCode { get; set; }
        [ScriptableMember(ScriptAlias = "ip")]
        public string Ip { get; set; }
        [ScriptableMember(ScriptAlias = "fromCache")]
        public bool FromCache { get; set; }
        [ScriptableMember(ScriptAlias = "responseHeaders")]
        public object ResponseHeaders { get; set; }
    }

    [ScriptableType]
    public struct OnBeforeRequestFilter
    {
        [ScriptableMember(ScriptAlias = "urls")]
        public string[] Urls { get; set; }
    }

    [ScriptableType]
    public class OnBeforeRequestDetails
    {
        [ScriptableMember(ScriptAlias = "id")]
        public string Id { get; set; }
        [ScriptableMember(ScriptAlias = "url")]
        public string Url { get; set; }
        [ScriptableMember(ScriptAlias = "method")]
        public string Method { get; set; }
        [ScriptableMember(ScriptAlias = "resourceType")]
        public string ResourceType { get; set; }
        [ScriptableMember(ScriptAlias = "timestamp")]
        public double Timestamp { get; set; }
        [ScriptableMember(ScriptAlias = "uploadData")]
        public UploadData[] UploadData { get; set; }
    }

    [ScriptableType]
    public struct OnBeforeSendHeadersFilter
    {
        [ScriptableMember(ScriptAlias = "urls")]
        public string[] Urls { get; set; }
    }

    [ScriptableType]
    public struct OnCompletedFilter
    {
        [ScriptableMember(ScriptAlias = "urls")]
        public string[] Urls { get; set; }
    }

    [ScriptableType]
    public struct OnCompletedDetails
    {
        [ScriptableMember(ScriptAlias = "id")]
        public string Id { get; set; }
        [ScriptableMember(ScriptAlias = "url")]
        public string Url { get; set; }
        [ScriptableMember(ScriptAlias = "method")]
        public string Method { get; set; }
        [ScriptableMember(ScriptAlias = "resourceType")]
        public string ResourceType { get; set; }
        [ScriptableMember(ScriptAlias = "timestamp")]
        public double Timestamp { get; set; }
        [ScriptableMember(ScriptAlias = "responseHeaders")]
        public object ResponseHeaders { get; set; }
        [ScriptableMember(ScriptAlias = "fromCache")]
        public bool FromCache { get; set; }
        [ScriptableMember(ScriptAlias = "statusCode")]
        public int StatusCode { get; set; }
        [ScriptableMember(ScriptAlias = "statusLine")]
        public string StatusLine { get; set; }
    }

    [ScriptableType]
    public struct OnErrorOccurredFilter
    {
        [ScriptableMember(ScriptAlias = "urls")]
        public string[] Urls { get; set; }
    }

    [ScriptableType]
    public struct OnErrorOccurredDetails
    {
        [ScriptableMember(ScriptAlias = "id")]
        public string Id { get; set; }
        [ScriptableMember(ScriptAlias = "url")]
        public string Url { get; set; }
        [ScriptableMember(ScriptAlias = "method")]
        public string Method { get; set; }
        [ScriptableMember(ScriptAlias = "resourceType")]
        public string ResourceType { get; set; }
        [ScriptableMember(ScriptAlias = "timestamp")]
        public double Timestamp { get; set; }
        [ScriptableMember(ScriptAlias = "fromCache")]
        public bool FromCache { get; set; }
        [ScriptableMember(ScriptAlias = "error")]
        public string Error { get; set; }
    }

    [ScriptableType]
    public struct OnHeadersReceivedFilter
    {
        [ScriptableMember(ScriptAlias = "urls")]
        public string[] Urls { get; set; }
    }

    [ScriptableType]
    public struct OnResponseStartedFilter
    {
        [ScriptableMember(ScriptAlias = "urls")]
        public string[] Urls { get; set; }
    }

    [ScriptableType]
    public struct OnResponseStartedDetails
    {
        [ScriptableMember(ScriptAlias = "id")]
        public string Id { get; set; }
        [ScriptableMember(ScriptAlias = "url")]
        public string Url { get; set; }
        [ScriptableMember(ScriptAlias = "method")]
        public string Method { get; set; }
        [ScriptableMember(ScriptAlias = "resourceType")]
        public string ResourceType { get; set; }
        [ScriptableMember(ScriptAlias = "timestamp")]
        public double Timestamp { get; set; }
        [ScriptableMember(ScriptAlias = "responseHeaders")]
        public object ResponseHeaders { get; set; }
        [ScriptableMember(ScriptAlias = "fromCache")]
        public bool FromCache { get; set; }
        [ScriptableMember(ScriptAlias = "statusCode")]
        public int StatusCode { get; set; }
        [ScriptableMember(ScriptAlias = "statusLine")]
        public string StatusLine { get; set; }
    }

    [ScriptableType]
    public struct OnSendHeadersFilter
    {
        [ScriptableMember(ScriptAlias = "urls")]
        public string[] Urls { get; set; }
    }

    [ScriptableType]
    public class OnSendHeadersDetails
    {
        [ScriptableMember(ScriptAlias = "id")]
        public string Id { get; set; }
        [ScriptableMember(ScriptAlias = "url")]
        public string Url { get; set; }
        [ScriptableMember(ScriptAlias = "method")]
        public string Method { get; set; }
        [ScriptableMember(ScriptAlias = "resourceType")]
        public string ResourceType { get; set; }
        [ScriptableMember(ScriptAlias = "timestamp")]
        public double Timestamp { get; set; }
        [ScriptableMember(ScriptAlias = "requestHeaders")]
        public object RequestHeaders { get; set; }
    }




}