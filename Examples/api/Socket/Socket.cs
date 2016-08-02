using System;
using System.Text;

using PepperSharp;

namespace Socket
{
    public class Socket : Instance
    {

        static readonly int kBufferSize = 1024;

        const char MSG_CREATE_TCP = 't';
        const char MSG_CREATE_UDP = 'u';
        const char MSG_SEND = 's';
        const char MSG_CLOSE = 'c';
        const char MSG_LISTEN = 'l';

        bool send_outstanding_;

        byte[] receive_buffer_ = new byte[kBufferSize];

        PPResource tcp_socket_;
        PPResource udp_socket_;
        PPResource resolver_;
        PPResource remote_host_;

        EchoServer echoServer;

        public Socket(IntPtr handle) : base(handle) { }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            LogToConsoleWithSource(PPLogLevel.Log, "Socket", "There be dragons here.");
            return base.Init(argc, argn, argv);
        }

        public override void HandleMessage(PPVar handleMessage)
        {
            var var_message = (Var)handleMessage;
            if (!var_message.IsString)
                return;
            var message = var_message.AsString();
            // This message must contain a command character followed by ';' and
            // arguments like "X;arguments".
            if (message.Length < 2 || message[1] != ';')
                return;
            switch (message[0])
            {
                case MSG_CREATE_UDP:
                    // The command 'b' requests to create a UDP connection the
                    // specified HOST.
                    // HOST is passed as an argument like "t;HOST".
                    Connect(message.Substring(2), false);
                    break;
                case MSG_CREATE_TCP:
                    // The command 'o' requests to connect to the specified HOST.
                    // HOST is passed as an argument like "u;HOST".
                    Connect(message.Substring(2), true);
                    break;
                case MSG_CLOSE:
                    // The command 'c' requests to close without any argument like "c;"
                    Close();
                    break;
                case MSG_LISTEN:
                    // The command 'l' starts a listening socket (server).
                    short port = 0;
                    short.TryParse(message.Substring(2), out port);
                    echoServer = new EchoServer(this, port);
                    break;
                case MSG_SEND:
                    // The command 't' requests to send a message as a text frame. The
                    // message passed as an argument like "t;message".
                    Send(message.Substring(2));
                    break;
                default:
                    var status = $"Unhandled message from JavaScript: {message}";
                    PostMessage(status);
                    break;
            }
        }

        bool IsConnected
        {
            get
            {
                if (!tcp_socket_.IsEmpty)
                    return true;
                if (!udp_socket_.IsEmpty)
                    return true;

                return false;
            }
        }

        bool IsUDP
        {
            get
            {
                return !udp_socket_.IsEmpty;
            }
        }

        void Connect(string host, bool tcp)
        {
            if (IsConnected)
            {
                PostMessage("Already connected.");
                return;
            }

            if (tcp)
            {

                tcp_socket_ = PPBTCPSocket.Create(this);
                if (PPBTCPSocket.IsTCPSocket(tcp_socket_) == PPBool.False)
                {
                    PostMessage("Error creating TCPSocket.");
                    return;
                }
            }
            else
            {

                udp_socket_ = PPBUDPSocket.Create(this);
                if (PPBUDPSocket.IsUDPSocket(udp_socket_) == PPBool.False)
                {
                    PostMessage("Error creating UDPSocket.");
                    return;
                }
            }


            resolver_ = PPBHostResolver.Create(this);
            if (PPBHostResolver.IsHostResolver(resolver_) == PPBool.False)
            {
                PostMessage("Error creating HostResolver.");
                return;

            }

            ushort port = 80;
            var hostname = host;
            int pos = host.IndexOf(':');
            if (pos > 0)
            {
                hostname = host.Substring(0, pos);
                port = ushort.Parse(host.Substring(pos + 1));
            }

            var hint = new PPHostResolverHint() { family = PPNetAddressFamily.Unspecified, flags = 0 };
            PPBHostResolver.Resolve(resolver_, 
                hostname, 
                port, 
                hint, 
                new CompletionCallback(OnResolveCompletion));

            PostMessage("Resolving ...");

        }

        void OnResolveCompletion(PPError result)
        {
            if (result != PPError.Ok)
            {
                PostMessage("Resolve failed.");
                return;
            }

            var addr = PPBHostResolver.GetNetAddress(resolver_, 0);
            PostMessage($"Resolved: {(Var)PPBNetAddress.DescribeAsString(addr, PPBool.True)}");

            var callback = new CompletionCallback(OnConnectCompletion);
            if (IsUDP)
            {
                PostMessage("Binding ...");
                remote_host_ = new PPResource(addr);
                var ipv4_addr = new PPNetAddressIPv4(0);
                PPBUDPSocket.Bind(udp_socket_, addr, callback);
            }
            else
            {
                PostMessage("Connecting ...");
                PPBTCPSocket.Connect(tcp_socket_, addr, callback);
            }
        }

        private void OnConnectCompletion(PPError result)
        {
            if (result != PPError.Ok)
            {
                var status = $"Connection failed: {result}";
                PostMessage(status);
                return;
            }

            if (IsUDP)
            {
                var addr = PPBUDPSocket.GetBoundAddress(udp_socket_);
                PostMessage($"Bound to: {((Var)PPBNetAddress.DescribeAsString(addr, PPBool.True)).AsString()}");
            }
            else
            {
                PostMessage("Connected");
            }

            Receive();
        }

        void Receive()
        {
            if (IsUDP)
            {
                Array.Clear(receive_buffer_, 0, receive_buffer_.Length);
                var OnReceiveFromCompletionCallback = new CompletionCallbackWithOutput<PPResource>(OnReceiveFromCompletion);
                PPBUDPSocket.RecvFrom(udp_socket_, receive_buffer_, kBufferSize, out OnReceiveFromCompletionCallback.OutputAdapter.output, OnReceiveFromCompletionCallback);
            }
            else
            {
                Array.Clear(receive_buffer_, 0, receive_buffer_.Length);
                PPBTCPSocket.Read(tcp_socket_, receive_buffer_, kBufferSize, new CompletionCallback(OnReceiveCompletion));
            }
        }

        private void OnReceiveFromCompletion(PPError result, PPResource source)
        {
            OnReceiveCompletion(result);
        }

        private void OnReceiveCompletion(PPError result)
        {
            
            if ((int)result < 0)
            {
                PostMessage($"Receive failed with: {result}");
                return;
            }

            PostMessage($"Received: {UTF8Encoding.UTF8.GetString(receive_buffer_).TrimEnd('\0')}");
            Receive();
        }

        void Send(string message)
        {
            if (!IsConnected)
            {
                PostMessage("Not connected.");
                return;
            }

            if (send_outstanding_)
            {
                PostMessage("Already sending.");
                return;
            }

            
            var data = Encoding.UTF8.GetBytes(message);
            var size = data.Length;
            var callback = new CompletionCallback(OnSendCompletion);
            int result = 0;

            if (IsUDP)
            {
                result = PPBUDPSocket.SendTo(udp_socket_, data, size, remote_host_, callback);
            }
            else
            {
                result = PPBTCPSocket.Write(tcp_socket_, data, size, callback);
            }
            string status = string.Empty;
            if (result < 0)
            {
                if ((PPError)result == PPError.OkCompletionpending)
                {
                    status = $"Sending bytes: {size}";
                    PostMessage(status);
                    send_outstanding_ = true;
                }
                else
                {
                    status = $"Send returned error: {result}";
                    PostMessage(status);
                }
            }
            else
            {
                status = $"Sent bytes synchronously: {result}";
                PostMessage(status);
            }
        }

        private void OnSendCompletion(PPError result)
        {
            string status;
            if (result < 0)
            {
                status = $"Send failed with: {result}";
            }
            else
            {
                status = $"Sent bytes: {(int)result}";
            }
            send_outstanding_ = false;
            PostMessage(status);
        }

        void Close()
        {
            if (!IsConnected)
            {
                PostMessage("Not connected.");
                return;
            }

            if (tcp_socket_.IsEmpty)
            {
                PPBUDPSocket.Close(udp_socket_);
                udp_socket_.Dispose();
            }
            else
            {
                PPBTCPSocket.Close(tcp_socket_);
                tcp_socket_.Dispose();
            }

            PostMessage("Closed connection.");
        }
    }
}
