using System;
using System.Text;

using PepperSharp;
using System.Linq;

namespace WebSocket
{
    public class WebSocket : Instance
    {

        PepperSharp.WebSocket webSocket2;
        ArraySegment<byte> rcvBuffer = new ArraySegment<byte>();


        public WebSocket(IntPtr handle) : base(handle)
        {
            HandleMessage += OnReceiveMessage;
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
                    Send(message.Substring(2), WebSocketMessageType.Binary);
                    break;
                case 't':
                    // The command 't' requests to send a message as a text frame. The message
                    // is passed as an argument like "t;message".
                    Send(message.Substring(2), WebSocketMessageType.Text);
                    break;
            }
        }

         void Open(string url)
        {
            webSocket2 = new PepperSharp.WebSocket(this);

            webSocket2.Connection += HandleConnection;
            webSocket2.Closed += HandleClosed;
            webSocket2.ReceiveData += HandleReceiveData;

            PostMessage("connecting...");
            webSocket2.Connect(new Uri(url));
        }

         private void HandleConnection(object sender, PPError result)
        {
            var ws = sender as PepperSharp.WebSocket;
            if (ws == null || ws.State != PepperSharp.WebSocketState.Open)
            {
                PostMessage($"connection failed {result}");
                return;
            }

            PostMessage("connected");
            Receive();
        }

        void Receive()
        {
            var rcvBytes = new byte[128];
            rcvBuffer = new ArraySegment<byte>(rcvBytes);
            var receiveResult = webSocket2.Receive(rcvBuffer);
            if (receiveResult != PPError.OkCompletionpending)
                PostMessage($"receive failed {receiveResult}");
        }

        private void HandleReceiveData(object sender, PepperSharp.WebSocketReceiveResult rcvResult)
        {
            if (rcvResult.MessageType == PepperSharp.WebSocketMessageType.Close)
                return;

            if (rcvResult.MessageType == PepperSharp.WebSocketMessageType.Text)
            {
                byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take((int)rcvResult.Count).ToArray();
                string rcvMsg = Encoding.UTF8.GetString(msgBytes);
                PostMessage($"receive (text): {rcvMsg}");
            }
            else if (rcvResult.MessageType == PepperSharp.WebSocketMessageType.Binary)
            {

                byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take((int)rcvResult.Count).ToArray();
                var messageText = ArrayToString(msgBytes);
                PostMessage($"receive (binary): {messageText}");
            }

            Receive();
        }


        bool IsConnected()
        {
            if (webSocket2.State == PepperSharp.WebSocketState.Open)
                return true;

            if (webSocket2 != null)
                Console.WriteLine($"Socket is closed {webSocket2.CloseStatus} / {webSocket2.CloseStatusDescription}");

            return false;
        }

        void Close()
        {
            if (!IsConnected())
                return;

            webSocket2.Close(PepperSharp.WebSocketCloseStatus.NormalClosure, "Bye");
        }

        private void HandleClosed(object sender, PPError e)
        {

            if (e != PPError.Ok)
                PostMessage($"error closing {e} - {webSocket2.CloseStatus} / {webSocket2.CloseStatusDescription}");

            PostMessage(webSocket2.State == PepperSharp.WebSocketState.Closed 
                ? $"closed {webSocket2.CloseStatus} / {webSocket2.CloseStatusDescription}" 
                : $"abnormally closed {webSocket2.CloseStatus} / {webSocket2.CloseStatusDescription}");
        }
        
        void Send(string message, PepperSharp.WebSocketMessageType messageType)
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            var sendBuffer = new ArraySegment<byte>(sendBytes);

            var sendResult = webSocket2.Send(sendBuffer, messageType);

            var msgBytes = (messageType == PepperSharp.WebSocketMessageType.Text) ? message : ArrayToString(sendBytes);
            if (sendResult != PPError.Ok)
                PostMessage($"send failed ({messageType}) - {sendResult}: {msgBytes}");
            else
                PostMessage($"send ({messageType}): {msgBytes}");
        }

        const int MAX_TO_CONVERT = 8;
        const int BYTES_PER_CHAR = 4;
        const int TAIL_AND_NUL_SIZE = 4;

        static string ArrayToString(byte[] array)
        {
            var hexString = new StringBuilder();

            int offs = 0;
            var size = 0;
            for (offs = 0; offs < array.Length && offs < MAX_TO_CONVERT; offs++, size++)
            {
                hexString.Append(array[offs].ToString("x").ToUpper());
                hexString.Append("h ");
            }

            hexString.Append("...");

            return hexString.ToString();
        }

    }
    
}
