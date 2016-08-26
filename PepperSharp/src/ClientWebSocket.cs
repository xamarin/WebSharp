using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.WebSockets;
using System.Threading;
using System.Net;
using System.Runtime.CompilerServices;
using System.IO;

namespace PepperSharp
{
    public class PepperWebSocket : WebSocket
    {
        const string UriSchemeWs = "ws";
        const string UriSchemeWss = "wss";

        private readonly PepperWebSocketOptions options;
        private WebSocket innerWebSocket;
        private readonly CancellationTokenSource cts;


        // Stages of this class. Interlocked doesn't support enums.
        private int state;
        private const int created = 0;
        private const int connecting = 1;
        private const int connected = 2;
        private const int disposed = 3;

        static PepperWebSocket()
        {
            // Register ws: and wss: with WebRequest.Register so that WebRequest.Create returns a 
            // WebSocket capable HttpWebRequest instance.
            WebSocket.RegisterPrefixes();
        }

        public PepperWebSocket()
        {
            //if (!WebSocketProtocolComponent.IsSupported)
            //{
            //    PepperWebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
            //}

            state = created;
            options = new PepperWebSocketOptions();
            cts = new CancellationTokenSource();

        }


        public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            if (!uri.IsAbsoluteUri)
            {
                throw new ArgumentException("Not Absolute URI", nameof(uri));
            }
            if (uri.Scheme.ToLower() != UriSchemeWs && uri.Scheme.ToLower() != UriSchemeWss)
            {
                throw new ArgumentException("Scheme invalid", nameof(uri));
            }

            // Check that we have not started already
            var priorState = (InternalState)Interlocked.CompareExchange(ref _state, (int)InternalState.Connecting, (int)InternalState.Created);
            if (priorState == InternalState.Disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
            else if (priorState != InternalState.Created)
            {
                throw new InvalidOperationException("WebSocket already started");
            }
            _options.SetToReadOnly();

            return ConnectAsyncCore(uri, cancellationToken);
        }
    }

    public sealed class PepperWebSocketOptions
    {
        private bool isReadOnly; // After ConnectAsync is called the options cannot be modified.
        private readonly IList<string> requestedSubProtocols;
        private readonly WebHeaderCollection requestHeaders;
        private TimeSpan keepAliveInterval;
        private int receiveBufferSize;
        private int sendBufferSize;
        private ArraySegment<byte>? buffer;
        private bool useDefaultCredentials;
        private ICredentials credentials;
        private IWebProxy proxy;
        //private X509CertificateCollection clientCertificates;
        private CookieContainer cookies;

        internal PepperWebSocketOptions()
        {
            requestedSubProtocols = new List<string>();
            requestHeaders = new WebHeaderCollection(WebHeaderCollectionType.HttpWebRequest);
            Proxy = WebRequest.DefaultWebProxy;
            receiveBufferSize = PepperWebSocketHelpers.DefaultReceiveBufferSize;
            sendBufferSize = PepperWebSocketHelpers.DefaultClientSendBufferSize;
            keepAliveInterval = WebSocket.DefaultKeepAliveInterval;
        }

        #region HTTP Settings

        // Note that some headers are restricted like Host.
        public void SetRequestHeader(string headerName, string headerValue)
        {
            ThrowIfReadOnly();
            // WebHeadersColection performs the validation
            requestHeaders.Set(headerName, headerValue);
        }

        internal WebHeaderCollection RequestHeaders { get { return requestHeaders; } }

        public bool UseDefaultCredentials
        {
            get
            {
                return useDefaultCredentials;
            }
            set
            {
                ThrowIfReadOnly();
                useDefaultCredentials = value;
            }
        }

        public ICredentials Credentials
        {
            get
            {
                return credentials;
            }
            set
            {
                ThrowIfReadOnly();
                credentials = value;
            }
        }

        public IWebProxy Proxy
        {
            get
            {
                return proxy;
            }
            set
            {
                ThrowIfReadOnly();
                proxy = value;
            }
        }

    
        public CookieContainer Cookies
        {
            get
            {
                return cookies;
            }
            set
            {
                ThrowIfReadOnly();
                cookies = value;
            }
        }

        #endregion HTTP Settings

        #region WebSocket Settings

        public void SetBuffer(int receiveBufferSize, int sendBufferSize)
        {
            ThrowIfReadOnly();
            PepperWebSocketHelpers.ValidateBufferSizes(receiveBufferSize, sendBufferSize);

            this.buffer = null;
            this.receiveBufferSize = receiveBufferSize;
            this.sendBufferSize = sendBufferSize;
        }

        public void SetBuffer(int receiveBufferSize, int sendBufferSize, ArraySegment<byte> buffer)
        {
            ThrowIfReadOnly();
            PepperWebSocketHelpers.ValidateBufferSizes(receiveBufferSize, sendBufferSize);
            PepperWebSocketHelpers.ValidateArraySegment(buffer, "buffer");
            PepperWebSocketBuffer.Validate(buffer.Count, receiveBufferSize, sendBufferSize, false);

            this.receiveBufferSize = receiveBufferSize;
            this.sendBufferSize = sendBufferSize;

            // Only full-trust applications can specify their own buffer to be used as the
            // internal buffer for the WebSocket object.  This is because the contents of the
            // buffer are used internally by the WebSocket as it marshals data with embedded
            // pointers to native code.  A malicious application could use this to corrupt
            // native memory.
            if (AppDomain.CurrentDomain.IsFullyTrusted)
            {
                this.buffer = buffer;
            }
            else
            {
                // We silently ignore the passed in buffer and will create an internal
                // buffer later.
                this.buffer = null;
            }
        }

        internal int ReceiveBufferSize { get { return receiveBufferSize; } }

        internal int SendBufferSize { get { return sendBufferSize; } }

        internal ArraySegment<byte> GetOrCreateBuffer()
        {
            if (!buffer.HasValue)
            {
                buffer = WebSocket.CreateClientBuffer(receiveBufferSize, sendBufferSize);
            }
            return buffer.Value;
        }

        public void AddSubProtocol(string subProtocol)
        {
            ThrowIfReadOnly();
            PepperWebSocketHelpers.ValidateSubprotocol(subProtocol);
            // Duplicates not allowed.
            foreach (string item in requestedSubProtocols)
            {
                if (string.Equals(item, subProtocol, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("No Duplicate subProtocol",
                        "subProtocol");
                }
            }
            requestedSubProtocols.Add(subProtocol);
        }

        internal IList<string> RequestedSubProtocols { get { return requestedSubProtocols; } }

        public TimeSpan KeepAliveInterval
        {
            get
            {
                return keepAliveInterval;
            }
            set
            {
                ThrowIfReadOnly();
                if (value < Timeout.InfiniteTimeSpan)
                {
                    throw new ArgumentOutOfRangeException("value", value,
                        $"Argument out of range {Timeout.InfiniteTimeSpan.ToString()}");
                }
                keepAliveInterval = value;
            }
        }

        #endregion WebSocket settings

        #region Helpers

        internal void SetToReadOnly()
        {
            //Contract.Assert(!isReadOnly, "Already set");
            isReadOnly = true;
        }

        private void ThrowIfReadOnly()
        {
            if (isReadOnly)
            {
                throw new InvalidOperationException("WebSocket already started");
            }
        }

        #endregion Helpers
    }

    internal static class PepperWebSocketHelpers
    {
        internal const string SecWebSocketKeyGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        internal const string WebSocketUpgradeToken = "websocket";
        internal const int DefaultReceiveBufferSize = 16 * 1024;
        internal const int DefaultClientSendBufferSize = 16 * 1024;
        internal const int MaxControlFramePayloadLength = 123;

        // RFC 6455 requests WebSocket clients to let the server initiate the TCP close to avoid that client sockets 
        // end up in TIME_WAIT-state
        //
        // After both sending and receiving a Close message, an endpoint considers the WebSocket connection closed and 
        // MUST close the underlying TCP connection.  The server MUST close the underlying TCP connection immediately; 
        // the client SHOULD wait for the server to close the connection but MAY close the connection at any time after
        // sending and receiving a Close message, e.g., if it has not received a TCP Close from the server in a 
        // reasonable time period.
        internal const int ClientTcpCloseTimeout = 1000; // 1s

        private const int CloseStatusCodeAbort = 1006;
        private const int CloseStatusCodeFailedTLSHandshake = 1015;
        private const int InvalidCloseStatusCodesFrom = 0;
        private const int InvalidCloseStatusCodesTo = 999;
        private const string Separators = "()<>@,;:\\\"/[]?={} ";

        private static readonly ArraySegment<byte> s_EmptyPayload = new ArraySegment<byte>(new byte[] { }, 0, 0);
        private static readonly Random s_KeyGenerator = new Random();
        private static volatile bool s_HttpSysSupportsWebSockets = ComNetOS.IsWin8orLater;

        internal static ArraySegment<byte> EmptyPayload
        {
            get { return s_EmptyPayload; }
        }

        internal static Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(HttpListenerContext context,
            string subProtocol,
            int receiveBufferSize,
            TimeSpan keepAliveInterval,
            ArraySegment<byte> internalBuffer)
        {
            //PepperWebSocketHelpers.ValidateOptions(subProtocol, receiveBufferSize, WebSocketBuffer.MinSendBufferSize, keepAliveInterval);
            //PepperWebSocketHelpers.ValidateArraySegment<byte>(internalBuffer, "internalBuffer");
            //WebSocketBuffer.Validate(internalBuffer.Count, receiveBufferSize, WebSocketBuffer.MinSendBufferSize, true);

            return AcceptWebSocketAsyncCore(context, subProtocol, receiveBufferSize, keepAliveInterval, internalBuffer);
        }

        private static async Task<HttpListenerWebSocketContext> AcceptWebSocketAsyncCore(HttpListenerContext context,
            string subProtocol,
            int receiveBufferSize,
            TimeSpan keepAliveInterval,
            ArraySegment<byte> internalBuffer)
        {
            HttpListenerWebSocketContext webSocketContext = null;

            try
            {
                //// get property will create a new response if one doesn't exist.
                //HttpListenerResponse response = context.Response;
                //HttpListenerRequest request = context.Request;
                //ValidateWebSocketHeaders(context);

                //string secWebSocketVersion = request.Headers[HttpKnownHeaderNames.SecWebSocketVersion];

                //// Optional for non-browser client
                //string origin = request.Headers[HttpKnownHeaderNames.Origin];

                //List<string> secWebSocketProtocols = new List<string>();
                //string outgoingSecWebSocketProtocolString;
                //bool shouldSendSecWebSocketProtocolHeader =
                //    PepperWebSocketHelpers.ProcessWebSocketProtocolHeader(
                //        request.Headers[HttpKnownHeaderNames.SecWebSocketProtocol],
                //        subProtocol,
                //        out outgoingSecWebSocketProtocolString);

                //if (shouldSendSecWebSocketProtocolHeader)
                //{
                //    secWebSocketProtocols.Add(outgoingSecWebSocketProtocolString);
                //    response.Headers.Add(HttpKnownHeaderNames.SecWebSocketProtocol,
                //        outgoingSecWebSocketProtocolString);
                //}

                //// negotiate the websocket key return value
                //string secWebSocketKey = request.Headers[HttpKnownHeaderNames.SecWebSocketKey];
                //string secWebSocketAccept = PepperWebSocketHelpers.GetSecWebSocketAcceptString(secWebSocketKey);

                //response.Headers.Add(HttpKnownHeaderNames.Connection, HttpKnownHeaderNames.Upgrade);
                //response.Headers.Add(HttpKnownHeaderNames.Upgrade, PepperWebSocketHelpers.WebSocketUpgradeToken);
                //response.Headers.Add(HttpKnownHeaderNames.SecWebSocketAccept, secWebSocketAccept);

                //response.StatusCode = (int)HttpStatusCode.SwitchingProtocols; // HTTP 101                
                //response.ComputeCoreHeaders();
                //ulong hresult = SendWebSocketHeaders(response);
                //if (hresult != 0)
                //{
                //    throw new WebSocketException((int)hresult,
                //        SR.GetString(SR.net_WebSockets_NativeSendResponseHeaders,
                //        PepperWebSocketHelpers.MethodNames.AcceptWebSocketAsync,
                //        hresult));
                //}

                //if (Logging.On)
                //{
                //    Logging.PrintInfo(Logging.WebSockets, string.Format("{0} = {1}",
                //        HttpKnownHeaderNames.Origin, origin));
                //    Logging.PrintInfo(Logging.WebSockets, string.Format("{0} = {1}",
                //        HttpKnownHeaderNames.SecWebSocketVersion, secWebSocketVersion));
                //    Logging.PrintInfo(Logging.WebSockets, string.Format("{0} = {1}",
                //        HttpKnownHeaderNames.SecWebSocketKey, secWebSocketKey));
                //    Logging.PrintInfo(Logging.WebSockets, string.Format("{0} = {1}",
                //        HttpKnownHeaderNames.SecWebSocketAccept, secWebSocketAccept));
                //    Logging.PrintInfo(Logging.WebSockets, string.Format("Request  {0} = {1}",
                //        HttpKnownHeaderNames.SecWebSocketProtocol,
                //        request.Headers[HttpKnownHeaderNames.SecWebSocketProtocol]));
                //    Logging.PrintInfo(Logging.WebSockets, string.Format("Response {0} = {1}",
                //        HttpKnownHeaderNames.SecWebSocketProtocol, outgoingSecWebSocketProtocolString));
                //}

                //await response.OutputStream.FlushAsync().SuppressContextFlow();

                //HttpResponseStream responseStream = response.OutputStream as HttpResponseStream;
                //Contract.Assert(responseStream != null, "'responseStream' MUST be castable to System.Net.HttpResponseStream.");
                //((HttpResponseStream)response.OutputStream).SwitchToOpaqueMode();
                //HttpRequestStream requestStream = new HttpRequestStream(context);
                //requestStream.SwitchToOpaqueMode();
                //WebSocketHttpListenerDuplexStream webSocketStream =
                //    new WebSocketHttpListenerDuplexStream(requestStream, responseStream, context);
                //WebSocket webSocket = WebSocket.CreateServerWebSocket(webSocketStream,
                //    subProtocol,
                //    receiveBufferSize,
                //    keepAliveInterval,
                //    internalBuffer);

                //webSocketContext = new HttpListenerWebSocketContext(
                //                                                    request.Url,
                //                                                    request.Headers,
                //                                                    request.Cookies,
                //                                                    context.User,
                //                                                    request.IsAuthenticated,
                //                                                    request.IsLocal,
                //                                                    request.IsSecureConnection,
                //                                                    origin,
                //                                                    secWebSocketProtocols.AsReadOnly(),
                //                                                    secWebSocketVersion,
                //                                                    secWebSocketKey,
                //                                                    webSocket);

                //if (Logging.On)
                //{
                //    Logging.Associate(Logging.WebSockets, context, webSocketContext);
                //    Logging.Associate(Logging.WebSockets, webSocketContext, webSocket);
                //}
            }
            catch (Exception ex)
            {
                //if (Logging.On)
                //{
                //    Logging.Exception(Logging.WebSockets, context, "AcceptWebSocketAsync", ex);
                //}
                throw;
            }
            finally
            {
                //if (Logging.On)
                //{
                //    Logging.Exit(Logging.WebSockets, context, "AcceptWebSocketAsync", "");
                //}
            }

            return webSocketContext;
        }

        [SuppressMessage("Microsoft.Cryptographic.Standard", "CA5354:SHA1CannotBeUsed",
            Justification = "SHA1 used only for hashing purposes, not for crypto.")]
        internal static string GetSecWebSocketAcceptString(string secWebSocketKey)
        {
            string retVal;

            // SHA1 used only for hashing purposes, not for crypto. Check here for FIPS compat.
            using (SHA1 sha1 = SHA1.Create())
            {
                string acceptString = string.Concat(secWebSocketKey, PepperWebSocketHelpers.SecWebSocketKeyGuid);
                byte[] toHash = Encoding.UTF8.GetBytes(acceptString);
                retVal = Convert.ToBase64String(sha1.ComputeHash(toHash));
            }

            return retVal;
        }

        internal static string GetTraceMsgForParameters(int offset, int count, CancellationToken cancellationToken)
        {
            return string.Format(
                "offset: {0}, count: {1}, cancellationToken.CanBeCanceled: {2}",
                offset,
                count,
                cancellationToken.CanBeCanceled);
        }

        // return value here signifies if a Sec-WebSocket-Protocol header should be returned by the server. 
        internal static bool ProcessWebSocketProtocolHeader(string clientSecWebSocketProtocol,
            string subProtocol,
            out string acceptProtocol)
        {
            acceptProtocol = string.Empty;
            if (string.IsNullOrEmpty(clientSecWebSocketProtocol))
            {
                // client hasn't specified any Sec-WebSocket-Protocol header
                if (subProtocol != null)
                {
                    // If the server specified _anything_ this isn't valid.
                    throw new WebSocketException(WebSocketError.UnsupportedProtocol,
                        $"Client does not accept protocols {subProtocol}");
                }
                // Treat empty and null from the server as the same thing here, server should not send headers. 
                return false;
            }

            // here, we know the client specified something and it's non-empty.

            if (subProtocol == null)
            {
                // client specified some protocols, server specified 'null'. So server should send headers.                 
                return true;
            }

            // here, we know that the client has specified something, it's not empty
            // and the server has specified exactly one protocol

            string[] requestProtocols = clientSecWebSocketProtocol.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries);
            acceptProtocol = subProtocol;

            // client specified protocols, serverOptions has exactly 1 non-empty entry. Check that 
            // this exists in the list the client specified. 
            for (int i = 0; i < requestProtocols.Length; i++)
            {
                string currentRequestProtocol = requestProtocols[i].Trim();
                if (string.Compare(acceptProtocol, currentRequestProtocol, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
            }

            throw new WebSocketException(WebSocketError.UnsupportedProtocol,
                $"Unsupported protocol {acceptProtocol}");
        }

        internal static ConfiguredTaskAwaitable SuppressContextFlow(this Task task)
        {
            // We don't flow the synchronization context within WebSocket.xxxAsync - but the calling application
            // can decide whether the completion callback for the task returned from WebSocket.xxxAsync runs
            // under the caller's synchronization context.
            return task.ConfigureAwait(false);
        }

        internal static ConfiguredTaskAwaitable<T> SuppressContextFlow<T>(this Task<T> task)
        {
            // We don't flow the synchronization context within WebSocket.xxxAsync - but the calling application
            // can decide whether the completion callback for the task returned from WebSocket.xxxAsync runs
            // under the caller's synchronization context.
            return task.ConfigureAwait(false);
        }

        internal static void ValidateBuffer(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset < 0 || offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            if (count < 0 || count > (buffer.Length - offset))
            {
                throw new ArgumentOutOfRangeException("count");
            }
        }

        private static unsafe ulong SendWebSocketHeaders(HttpListenerResponse response)
        {
            //return response.SendHeaders(null, null,
            //    UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_OPAQUE |
            //    UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_MORE_DATA |
            //    UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_BUFFER_DATA,
            //    true);
        }

        private static void ValidateWebSocketHeaders(HttpListenerContext context)
        {
            //EnsureHttpSysSupportsWebSockets();

            if (!context.Request.IsWebSocketRequest)
            {
                throw new WebSocketException(WebSocketError.NotAWebSocket, "Not a Web Socket");
            }

            //string secWebSocketVersion = context.Request.Headers[HttpKnownHeaderNames.SecWebSocketVersion];
            //if (string.IsNullOrEmpty(secWebSocketVersion))
            //{
            //    throw new WebSocketException(WebSocketError.HeaderError,
            //        SR.GetString(SR.net_WebSockets_AcceptHeaderNotFound,
            //        PepperWebSocketHelpers.MethodNames.ValidateWebSocketHeaders,
            //        HttpKnownHeaderNames.SecWebSocketVersion));
            //}

            //if (string.Compare(secWebSocketVersion, WebSocketProtocolComponent.SupportedVersion, StringComparison.OrdinalIgnoreCase) != 0)
            //{
            //    throw new WebSocketException(WebSocketError.UnsupportedVersion,
            //        SR.GetString(SR.net_WebSockets_AcceptUnsupportedWebSocketVersion,
            //        PepperWebSocketHelpers.MethodNames.ValidateWebSocketHeaders,
            //        secWebSocketVersion,
            //        WebSocketProtocolComponent.SupportedVersion));
            //}

            //if (string.IsNullOrWhiteSpace(context.Request.Headers[HttpKnownHeaderNames.SecWebSocketKey]))
            //{
            //    throw new WebSocketException(WebSocketError.HeaderError,
            //        SR.GetString(SR.net_WebSockets_AcceptHeaderNotFound,
            //        PepperWebSocketHelpers.MethodNames.ValidateWebSocketHeaders,
            //        HttpKnownHeaderNames.SecWebSocketKey));
            //}
        }

        internal static void PrepareWebRequest(ref HttpWebRequest request)
        {
            //request.Connection = HttpKnownHeaderNames.Upgrade;
            //request.Headers[HttpKnownHeaderNames.Upgrade] = WebSocketUpgradeToken;

            //byte[] keyBlob = new byte[16];
            //lock (s_KeyGenerator)
            //{
            //    s_KeyGenerator.NextBytes(keyBlob);
            //}

            //request.Headers[HttpKnownHeaderNames.SecWebSocketKey] = Convert.ToBase64String(keyBlob);
            //if (WebSocketProtocolComponent.IsSupported)
            //{
            //    request.Headers[HttpKnownHeaderNames.SecWebSocketVersion] = WebSocketProtocolComponent.SupportedVersion;
            //}
        }

        internal static void ValidateSubprotocol(string subProtocol)
        {
            if (string.IsNullOrWhiteSpace(subProtocol))
            {
                throw new ArgumentException("Invalid Empty protocol", "subProtocol");
            }

            char[] chars = subProtocol.ToCharArray();
            string invalidChar = null;
            int i = 0;
            while (i < chars.Length)
            {
                char ch = chars[i];
                if (ch < 0x21 || ch > 0x7e)
                {
                    invalidChar = string.Format("[{0}]", (int)ch);
                    break;
                }

                if (!char.IsLetterOrDigit(ch) &&
                    Separators.IndexOf(ch) >= 0)
                {
                    invalidChar = ch.ToString();
                    break;
                }

                i++;
            }

            if (invalidChar != null)
            {
                throw new ArgumentException($"Invalide char in protocol string {subProtocol}, {invalidChar}",
                    "subProtocol");
            }
        }

        internal static void ValidateCloseStatus(WebSocketCloseStatus closeStatus, string statusDescription)
        {
            if (closeStatus == WebSocketCloseStatus.Empty && !string.IsNullOrEmpty(statusDescription))
            {
                throw new ArgumentException($"Not null {statusDescription}, {WebSocketCloseStatus.Empty}",
                    "statusDescription");
            }

            int closeStatusCode = (int)closeStatus;

            if ((closeStatusCode >= InvalidCloseStatusCodesFrom &&
                closeStatusCode <= InvalidCloseStatusCodesTo) ||
                closeStatusCode == CloseStatusCodeAbort ||
                closeStatusCode == CloseStatusCodeFailedTLSHandshake)
            {
                // CloseStatus 1006 means Aborted - this will never appear on the wire and is reflected by calling WebSocket.Abort
                throw new ArgumentException($"Invalid Close Status Code {closeStatusCode}",
                    "closeStatus");
            }

            int length = 0;
            if (!string.IsNullOrEmpty(statusDescription))
            {
                length = UTF8Encoding.UTF8.GetByteCount(statusDescription);
            }

            if (length > PepperWebSocketHelpers.MaxControlFramePayloadLength)
            {
                throw new ArgumentException($"Invalid Close Status Description {statusDescription},{PepperWebSocketHelpers.MaxControlFramePayloadLength}",
                    "statusDescription");
            }
        }

        internal static void ValidateOptions(string subProtocol,
            int receiveBufferSize,
            int sendBufferSize,
            TimeSpan keepAliveInterval)
        {
            // We allow the subProtocol to be null. Validate if it is not null.
            if (subProtocol != null)
            {
                ValidateSubprotocol(subProtocol);
            }

            ValidateBufferSizes(receiveBufferSize, sendBufferSize);

            if (keepAliveInterval < Timeout.InfiniteTimeSpan) // -1
            {
                throw new ArgumentOutOfRangeException("keepAliveInterval", keepAliveInterval,
                    $"Too small {Timeout.InfiniteTimeSpan.ToString()}");
            }
        }

        internal static void ValidateBufferSizes(int receiveBufferSize, int sendBufferSize)
        {
            //if (receiveBufferSize < WebSocketBuffer.MinReceiveBufferSize)
            //{
            //    throw new ArgumentOutOfRangeException("receiveBufferSize", receiveBufferSize,
            //        SR.GetString(SR.net_WebSockets_ArgumentOutOfRange_TooSmall, WebSocketBuffer.MinReceiveBufferSize));
            //}

            //if (sendBufferSize < WebSocketBuffer.MinSendBufferSize)
            //{
            //    throw new ArgumentOutOfRangeException("sendBufferSize", sendBufferSize,
            //        SR.GetString(SR.net_WebSockets_ArgumentOutOfRange_TooSmall, WebSocketBuffer.MinSendBufferSize));
            //}

            //if (receiveBufferSize > WebSocketBuffer.MaxBufferSize)
            //{
            //    throw new ArgumentOutOfRangeException("receiveBufferSize", receiveBufferSize,
            //        SR.GetString(SR.net_WebSockets_ArgumentOutOfRange_TooBig,
            //            "receiveBufferSize",
            //            receiveBufferSize,
            //            WebSocketBuffer.MaxBufferSize));
            //}

            //if (sendBufferSize > WebSocketBuffer.MaxBufferSize)
            //{
            //    throw new ArgumentOutOfRangeException("sendBufferSize", sendBufferSize,
            //        SR.GetString(SR.net_WebSockets_ArgumentOutOfRange_TooBig,
            //            "sendBufferSize",
            //            sendBufferSize,
            //            WebSocketBuffer.MaxBufferSize));
            //}
        }

        internal static void ValidateInnerStream(Stream innerStream)
        {
            if (innerStream == null)
            {
                throw new ArgumentNullException("innerStream");
            }

            //if (!innerStream.CanRead)
            //{
            //    throw new ArgumentException(SR.GetString(SR.NotReadableStream), "innerStream");
            //}

            //if (!innerStream.CanWrite)
            //{
            //    throw new ArgumentException(SR.GetString(SR.NotWriteableStream), "innerStream");
            //}
        }

        internal static void ThrowIfConnectionAborted(Stream connection, bool read)
        {
            if ((!read && !connection.CanWrite) ||
                (read && !connection.CanRead))
            {
                throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely);
            }
        }

         internal static void ValidateArraySegment<T>(ArraySegment<T> arraySegment, string parameterName)
        {
            //Contract.Requires(!string.IsNullOrEmpty(parameterName), "'parameterName' MUST NOT be NULL or string.Empty");

            if (arraySegment.Array == null)
            {
                throw new ArgumentNullException(parameterName + ".Array");
            }

            if (arraySegment.Offset < 0 || arraySegment.Offset > arraySegment.Array.Length)
            {
                throw new ArgumentOutOfRangeException(parameterName + ".Offset");
            }
            if (arraySegment.Count < 0 || arraySegment.Count > (arraySegment.Array.Length - arraySegment.Offset))
            {
                throw new ArgumentOutOfRangeException(parameterName + ".Count");
            }
        }
 

        internal static class MethodNames
        {
            internal const string AcceptWebSocketAsync = "AcceptWebSocketAsync";
            internal const string ValidateWebSocketHeaders = "ValidateWebSocketHeaders";
        }
    }
}
