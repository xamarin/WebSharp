using System;

using PepperSharp;

namespace WebSocket
{
    public class WebSocket : Instance
    {

        PPResource websocket_;
        PPVar receive_var_ = new PPVar();

        public override void HandleMessage(PPVar msg)
        {
            var var_message = (Var)msg;

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
                    //SendAsBinary(message.substr(2));
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
            var callback = new PPCompletionCallback(OnConnectCompletionCallback, this);
            websocket_ = PPBWebSocket.Create(this);
            if (PPBWebSocket.IsWebSocket(websocket_) != PPBool.True)
                return;
            PPBWebSocket.Connect(websocket_, new Var(url), null, 0, callback);
            PostMessage("connecting...");
        }

        void OnConnectCompletionCallback(IntPtr userData, int result)
        {
            var instance = PPCompletionCallback.GetUserData<WebSocket>(userData);
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
            var receiveCallback = new PPCompletionCallback(OnReceiveCompletionCallback, this);
            // |receive_var_| must be valid until |callback| is invoked.
            // Just use a member variable.
            PPBWebSocket.ReceiveMessage(websocket_, out receive_var_, receiveCallback);
        }

        void OnReceiveCompletionCallback(IntPtr user_data, int result)
        {
            var instance = PPCompletionCallback.GetUserData<WebSocket>(user_data);
            instance.OnReceiveCompletion((PPError)result);
        }

        void OnReceiveCompletion(PPError result)
        {
            if (result == PPError.Ok)
            {
                if (((Var)receive_var_).IsArrayBuffer)
                {
                    // TODO: implement array buffer receive
                    //    pp::VarArrayBuffer array_buffer(receive_var_);
                    //    std::string message_text = ArrayToString(array_buffer);
                    //    PostMessage("receive (binary): " + message_text);
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
            var closeCallback = new PPCompletionCallback(OnCloseCompletionCallback, this);
            PPBWebSocket.Close(websocket_, (int)PPWebSocketCloseCode.WebsocketstatuscodeNormalClosure,
                            new Var("bye"), closeCallback);
        }

        void OnCloseCompletionCallback(IntPtr user_data, int result)
        {
            var instance = PPCompletionCallback.GetUserData<WebSocket>(user_data);
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
    }

}
