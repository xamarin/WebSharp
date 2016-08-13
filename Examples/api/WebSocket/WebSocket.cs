using System;
using System.Text;

using PepperSharp;

namespace WebSocket
{
    public class WebSocket : Instance
    {

        PPResource websocket_;
        PPVar receive_var_ = new PPVar();

        public WebSocket(IntPtr handle) : base(handle)
        {
            ReceiveMessage += OnReceiveMessage;
        }

        private void OnReceiveMessage(object sender, Var var_message)
        {

            if (!var_message.IsString)
                return;

            var message = var_message.AsString();
            // This message must contain a command character followed by ';' and
            // arguments like "X;arguments".
            if (message.Length < 2 || message[1] != ';')
                return;

            switch (message[0])
            {
                case 'o':
                    // The command 'o' requests to open the specified URL.
                    // URL is passed as an argument like "o;URL".
                    Open(message.Substring(2));
                    break;
                case 'c':
                    // The command 'c' requests to close without any argument like "c;"
                    Close();
                    break;
                case 'b':
                    // The command 'b' requests to send a message as a binary frame. The
                    // message is passed as an argument like "b;message".
                    SendAsBinary(message.Substring(2));
                    break;
                case 't':
                    // The command 't' requests to send a message as a text frame. The message
                    // is passed as an argument like "t;message".
                    SendAsText(message.Substring(2));
                    break;
            }
        }

        void Open(string url)
        {
            websocket_ = PPBWebSocket.Create(this);
            if (PPBWebSocket.IsWebSocket(websocket_) != PPBool.True)
                return;
            PPBWebSocket.Connect(websocket_, new Var(url), null, 0, new CompletionCallback<WebSocket>(OnConnectCompletionCallback, this).Callback);
            PostMessage("connecting...");
        }

        void OnConnectCompletionCallback(PPError result, WebSocket instance)
        {
            instance.OnConnectCompletion((PPError)result);
        }

        void OnConnectCompletion(PPError result)
        {
            if (result != PPError.Ok)
            {
                PostMessage("connection failed");
                return;
            }
            PostMessage("connected");
            Receive();
        }
 

        void Receive()
        {
            // |receive_var_| must be valid until |callback| is invoked.
            // Just use a member variable.
            PPBWebSocket.ReceiveMessage(websocket_, out receive_var_, new CompletionCallback<WebSocket>(OnReceiveCompletionCallback, this));
        }

        void OnReceiveCompletionCallback(PPError result, WebSocket instance)
        {
            instance.OnReceiveCompletion((PPError)result);
        }

        void OnReceiveCompletion(PPError result)
        {
            if (result == PPError.Ok)
            {
                if (((Var)receive_var_).IsArrayBuffer)
                {

                    var arrayBuffer = new VarArrayBuffer(receive_var_);
                    var messageText = ArrayToString(arrayBuffer);
                    PostMessage($"receive (binary): {messageText}");
                }
                else
                {
                    PostMessage("receive (text): " + (Var)receive_var_);
                }
            }
            Receive();
        }

        bool IsConnected()
        {
            if (websocket_.ppresource > 0 && PPBWebSocket.IsWebSocket(websocket_) != PPBool.True)
                return false;
            if (PPBWebSocket.GetReadyState(websocket_) != PPWebSocketReadyState.Open)
                return false;
            return true;
        }

        void Close()
        {
            if (!IsConnected())
                return;
            PPBWebSocket.Close(websocket_, (int)PPWebSocketCloseCode.WebsocketstatuscodeNormalClosure,
                            new Var("bye"), new CompletionCallback<WebSocket>(OnCloseCompletionCallback, this));
        }

        void OnCloseCompletionCallback(PPError result, WebSocket instance)
        {
            instance.OnCloseCompletion((PPError)result);
        }

        void OnCloseCompletion(PPError result)
        {
            PostMessage(new Var(PPError.Ok == result ? "closed" : "abnormally closed"));
        }

        void SendAsText(string message)
        {
            if (!IsConnected())
                return;
            PPBWebSocket.SendMessage(websocket_, new Var(message));
            PostMessage("send (text): " + message);
        }

        const int MAX_TO_CONVERT = 8;
        const int BYTES_PER_CHAR = 4;
        const int TAIL_AND_NUL_SIZE = 4;

        static string ArrayToString(VarArrayBuffer array)
        {
            var hexString = new StringBuilder();

            int offs = 0;
            var data = array.Map();
            var size = 0;
            for (offs = 0; offs < array.ByteLength && offs < MAX_TO_CONVERT; offs++, size++)
            {
                hexString.Append(data[offs].ToString("x").ToUpper());
                hexString.Append("h ");
            }

            hexString.Append("...");

            array.Unmap();
            return hexString.ToString();
        }

        void SendAsBinary(string message)
        {
            if (!IsConnected())
                return;

            var size = (uint)message.Length;
            var arrayBuffer = new VarArrayBuffer(size);

            var data = arrayBuffer.Map();
            for (int i = 0; i < size; ++i)
                data[i] = (byte)message[i];
            arrayBuffer.Flush();
            arrayBuffer.Unmap();
            PPBWebSocket.SendMessage(websocket_, arrayBuffer);
            var messageText = ArrayToString(arrayBuffer);
            PostMessage($"send (binary): {messageText}");
        }
    }

}
