using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Net;
using System.Text;

namespace PepperSharp
{
    public class WebSocket : Resource
    {

        const string UriSchemeWs = "ws";
        const string UriSchemeWss = "wss";

        PPVar receiveVar = new PPVar();

        /// <summary>
        /// Event raised when the websocket issues a Connect to the remote URI.
        /// </summary>
        public event EventHandler<PPError> Connection;

        /// <summary>
        /// Event raised when the websocket issues a Close to a WebSocket instance.
        /// </summary>
        public event EventHandler<PPError> Closed;

        /// <summary>
        /// Event raised when the websocket Receives data.
        /// </summary>
        public event EventHandler<WebSocketReceiveResult> ReceiveData;

        /// <summary>
        /// Constructs a WebSocket object.
        /// </summary>
        /// <param name="instance">The instance with which this resource will be
        /// associated.</param>
        public WebSocket(Instance instance)
        {
            handle = PPBWebSocket.Create(instance);
        }

        #region Implement IDisposable.
        
        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    Connection = null;
                    Closed = null;
                    ReceiveData = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        /// <summary>
        /// Connects to the specified WebSocket server
        /// </summary>
        /// <param name="url"></param>
        /// <param name="protocols"></param>
        public void Connect(Uri url, string[] protocols = null )
        {
            if (PPBWebSocket.IsWebSocket(this) != PPBool.True)
                throw new PlatformNotSupportedException("Websocket not supported");

            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }
            if (!url.IsAbsoluteUri)
            {
                throw new ArgumentException("Not Absolute URI", nameof(url));
            }
            if (url.Scheme.ToLower() != UriSchemeWs && url.Scheme.ToLower() != UriSchemeWss)
            {
                throw new ArgumentException("Scheme invalid", nameof(url));
            }

            PPVar[] varProtocols = null;

            if (protocols != null)
            {
                varProtocols = new PPVar[protocols.Length];

                for (int p = 0; p < protocols.Length; p++)
                {
                    varProtocols[p] = new Var(protocols[p]);
                }
            }

            PPBWebSocket.Connect(this, new Var(url.AbsoluteUri),
                varProtocols, varProtocols == null ? 0 : (uint)varProtocols.Length,
                new CompletionCallback(OnConnect)
                );
        }

        protected void OnConnect(PPError error)
        {
            Connection?.Invoke(this, error);
        }

        /// <summary>
        /// Closes the connection to the WebSocket server
        /// </summary>
        /// <param name="closeCode"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public PPError Close(WebSocketCloseStatus closeCode, string reason = null)
        {
            ThrowIfNotConnected();

            return (PPError)PPBWebSocket.Close(this, (ushort)closeCode,
                string.IsNullOrEmpty(reason) ? null : new Var(reason),
                new CompletionCallback(OnClose));
        }

        protected void OnClose(PPError error)
        {
            Closed?.Invoke(this, error);
        }

        /// <summary>
        /// Returns the Close Status of the WebSocket.
        /// </summary>
        public WebSocketCloseStatus? CloseStatus
        {
            get
            {
                if (PPBWebSocket.IsWebSocket(this) == PPBool.True)
                    return (WebSocketCloseStatus)PPBWebSocket.GetCloseCode(this);

                return null;
            }
        }

        /// <summary>
        /// Returns the Description used when closing the WebSocket.
        /// </summary>
        public string CloseStatusDescription
        {
            get
            {
                var desc = (Var)PPBWebSocket.GetCloseReason(this);

                if (desc.IsString)
                    return desc.AsString();

                return null;
            }
        }

        /// <summary>
        /// Sends a message to the WebSocket server
        /// </summary>
        /// <param name="buffer">An ArraySegment of bytes that will be sent to the WebSocket server.</param>
        /// <param name="messageType">This is the message type to send Binary or Text</param>
        /// <returns>Ok or an error</returns>
        public PPError Send(ArraySegment<byte> buffer, WebSocketMessageType messageType)
        {
            ThrowIfNotConnected();

            if (messageType != WebSocketMessageType.Binary &&
                    messageType != WebSocketMessageType.Text)
            {
                throw new ArgumentException($"Invalid Message Type {messageType} in method Send - Valid values are {WebSocketMessageType.Binary}, {WebSocketMessageType.Text}",
                    "messageType");
            }

            ValidateArraySegment<byte>(buffer, "buffer");

            if (messageType == WebSocketMessageType.Text)
            {
                var varBuffer = new Var(Encoding.UTF8.GetString(buffer.Array));
                return (PPError)PPBWebSocket.SendMessage(this, varBuffer);
            }
            else
            {
                var size = (uint)buffer.Count;
                var arrayBuffer = new VarArrayBuffer(size);

                var data = arrayBuffer.Map();
                for (int i = 0; i < size; ++i)
                     data[i] = buffer.Array[i];
                arrayBuffer.Flush();
                arrayBuffer.Unmap();

                return (PPError)PPBWebSocket.SendMessage(this, arrayBuffer);
            }
        }

        /// <summary>
        /// Receive a message from the WebSocket server.
        /// </summary>
        /// <param name="buffer">An ArraySegment of byte that was returned</param>
        /// <returns></returns>
        public PPError Receive(ArraySegment<byte> buffer)
        {
            ThrowIfNotConnected();
            // |receiveVar| must be valid until |callback| is invoked.
            // Just use a member variable.
            return (PPError)PPBWebSocket.ReceiveMessage(this, out receiveVar, new CompletionCallback<ArraySegment<byte>>(OnReceiveData, buffer));
        }

        protected virtual void OnReceiveData(PPError error, ArraySegment<byte> buffer)
        {
           
            WebSocketReceiveResult receiveResult = null;
            var recVar = (Var)receiveVar;
            if (State == WebSocketState.Open)
            {
                if (recVar.IsArrayBuffer)
                {
                    var arrayBuffer = new VarArrayBuffer(receiveVar);
                    var size = (uint)Math.Min(buffer.Count, arrayBuffer.ByteLength);

                    int offs = 0;
                    var data = arrayBuffer.Map();
                    for (offs = 0; offs < size; offs++)
                    {
                        buffer.Array[offs] = data[offs];
                    }
                    arrayBuffer.Unmap();
                    receiveResult = new WebSocketReceiveResult(size, WebSocketMessageType.Binary, true);
                }
                else
                {
                    var msg = Encoding.UTF8.GetBytes(recVar.AsString());
                    var size = (uint)Math.Min(buffer.Count, msg.Length);

                    int offs = 0;
                    for (offs = 0; offs < size; offs++)
                    {
                        buffer.Array[offs] = msg[offs];
                    }
                    receiveResult = new WebSocketReceiveResult(size, WebSocketMessageType.Text, true);
                }
            }
            else
                receiveResult = new WebSocketReceiveResult(0, WebSocketMessageType.Close, true, CloseStatus, CloseStatusDescription);

            ReceiveData?.Invoke(this, receiveResult);
        }

        /// <summary>
        /// Returns the state of the WebSocket
        /// </summary>
        public WebSocketState State
        {
            get
            {
                switch (PPBWebSocket.GetReadyState(this))
                {
                    case PPWebSocketReadyState.Connecting:
                        return WebSocketState.Connecting;
                    case PPWebSocketReadyState.Open:
                        return WebSocketState.Open;
                    case PPWebSocketReadyState.Closing:
                        return WebSocketState.CloseSent;
                    case PPWebSocketReadyState.Closed:
                        return WebSocketState.Closed;
                    default:
                        return WebSocketState.None;
                }
                
            }
        }

        private void ThrowIfNotConnected()
        {
            if (handle == PPResource.Empty)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
            else if (State != WebSocketState.Open)
            {
                throw new InvalidOperationException("WebSocket is not connected");
            }
        }

        internal static void ValidateArraySegment<T>(ArraySegment<T> arraySegment, string parameterName)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(parameterName), "'parameterName' MUST NOT be NULL or string.Empty");

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
    }

    public enum WebSocketState
    {
        None = 0,
        Connecting = 1,
        Open = 2,
        CloseSent = 3, // WebSocket close handshake started form local endpoint
        CloseReceived = 4, // WebSocket close message received from remote endpoint. Waiting for app to call close
        Closed = 5,
        Aborted = 6,
    }

    public enum WebSocketCloseStatus
    {
        NormalClosure = 1000,
        EndpointUnavailable = 1001,
        ProtocolError = 1002,
        InvalidMessageType = 1003,
        Empty = 1005,
        //AbnormalClosure = 1006, // 1006 is reserved and should never be used by user
        InvalidPayloadData = 1007,
        PolicyViolation = 1008,
        MessageTooBig = 1009,
        MandatoryExtension = 1010,
        InternalServerError = 1011,

        // TLSHandshakeFailed = 1015, // 1015 is reserved and should never be used by user

        // 0 - 999 Status codes in the range 0-999 are not used.
        // 1000 - 1999 Status codes in the range 1000-1999 are reserved for definition by this protocol.
        // 2000 - 2999 Status codes in the range 2000-2999 are reserved for use by extensions.
        // 3000 - 3999 Status codes in the range 3000-3999 MAY be used by libraries and frameworks. The 
        //             interpretation of these codes is undefined by this protocol. End applications MUST 
        //             NOT use status codes in this range.     
        UserRegisteredMin = 3000,
        UserRegisteredMax = 3999,
        // 4000 - 4999 Status codes in the range 4000-4999 MAY be used by application code. The interpretaion
        //             of these codes is undefined by this protocol.
        UserPrivateMin = 4000,
        UserPrivateMax = 4999

    }

    public enum WebSocketMessageType
    {
        Text = 0,
        Binary = 1,
        Close = 2
    }

    public class WebSocketReceiveResult
    {
        public WebSocketReceiveResult(uint count, WebSocketMessageType messageType, bool endOfMessage)
            : this(count, messageType, endOfMessage, null, null)
        {
        }

        public WebSocketReceiveResult(uint count,
            WebSocketMessageType messageType,
            bool endOfMessage,
            Nullable<WebSocketCloseStatus> closeStatus,
            string closeStatusDescription)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            this.Count = count;
            this.EndOfMessage = endOfMessage;
            this.MessageType = messageType;
            this.CloseStatus = closeStatus;
            this.CloseStatusDescription = closeStatusDescription;
        }

        public uint Count { get; private set; }
        public bool EndOfMessage { get; private set; }
        public WebSocketMessageType MessageType { get; private set; }
        public Nullable<WebSocketCloseStatus> CloseStatus { get; private set; }
        public string CloseStatusDescription { get; private set; }

        internal WebSocketReceiveResult Copy(uint count)
        {
            System.Diagnostics.Debug.Assert(count >= 0, "'count' MUST NOT be negative.");
            System.Diagnostics.Debug.Assert(count <= this.Count, "'count' MUST NOT be bigger than 'this.Count'.");
            this.Count -= count;
            return new WebSocketReceiveResult(count,
                this.MessageType,
                this.Count == 0 && this.EndOfMessage,
                this.CloseStatus,
                this.CloseStatusDescription);
        }
    }
}
