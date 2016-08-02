using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using PepperSharp;

namespace Socket
{
    public class EchoServer
    {
        Instance instance;
        PPResource listeningSocket;
        PPResource incomingSocket;

        static readonly int BUFFER_SIZE = 1024;

        // Number of connections to queue up on the listening
        // socket before new ones get "Connection Refused"
        static readonly int BACK_LOG = 10;

        byte[] receiveBuffer= new byte[BUFFER_SIZE];

        public EchoServer(Instance instance, short port)
        {
            this.instance = instance;
            Start(port);
        }

        void Start(short port)
        {
            
            listeningSocket = PPBTCPSocket.Create(instance);
            if (PPBTCPSocket.IsTCPSocket(listeningSocket) == PPBool.False)
            {
                instance.PostMessage("Error creating TCPSocket.");
                return;
            }

            instance.PostMessage($"Starting server on port: {port}");

            // Attempt to listen on all interfaces (0.0.0.0)
            // on the given port number.
            var ipv4_addr = new PPNetAddressIPv4((ushort)IPAddress.HostToNetworkOrder(port));
            var addr = PPBNetAddress.CreateFromIPv4Address(instance, ipv4_addr);
            var rtn = (PPError)PPBTCPSocket.Bind(listeningSocket, addr, new CompletionCallback(OnBindCompletion));
            if (rtn != PPError.OkCompletionpending)
            {
                instance.PostMessage("Error binding listening socket.");
                return;
            }
        }

        private void OnBindCompletion(PPError result)
        {
            if (result != PPError.Ok)
            {
                instance.PostMessage($"server: Bind failed with: {result}");
                return;
            }

            var rtn = (PPError)PPBTCPSocket.Listen(listeningSocket, 
                BACK_LOG, 
                new CompletionCallback(OnListenCompletion));

            if (rtn != PPError.OkCompletionpending)
            {
                instance.PostMessage("server: Error listening on server socket.");
                return;
            }
        }

        private void OnListenCompletion(PPError result)
        {
            var status = string.Empty;
            if (result != PPError.Ok)
            {
                instance.PostMessage($"server: Listen failed with: {result}");
                return;
            }

            var addr = PPBTCPSocket.GetLocalAddress(listeningSocket);
            
            instance.PostMessage($"server: Listening on: {((Var)PPBNetAddress.DescribeAsString(addr, PPBool.True)).AsString()}");

            TryAccept();
        }

        void TryAccept()
        {
            var onAcceptCompletionCallback = new CompletionCallbackWithOutput<PPResource>(OnAcceptCompletion);
            PPBTCPSocket.Accept(listeningSocket, 
                out onAcceptCompletionCallback.OutputAdapter.output, 
                onAcceptCompletionCallback);
        }

        private void OnAcceptCompletion(PPError result, PPResource socket)
        {

            if (result != PPError.Ok)
            {
                instance.PostMessage($"server: Accept failed: {result}");
                return;
            }

            var addr = PPBTCPSocket.GetLocalAddress(socket);
            instance.PostMessage($"server: New connection from: {((Var)PPBNetAddress.DescribeAsString(addr, PPBool.True)).ToString()}");
            incomingSocket = new PPResource(socket);

            TryRead();
        }

        void TryRead()
        {
            PPBTCPSocket.Read(incomingSocket, 
                receiveBuffer, 
                BUFFER_SIZE, 
                new CompletionCallback(OnReadCompletion));
        }

        private void OnReadCompletion(PPError result)
        {
            var status = string.Empty;
            if ((int)result <= 0)
            {
                if ((int)result == 0)
                {
                    status = "server: client disconnected";
                }
                else
                {
                    status = $"server: Read failed: {result}";
                }
                instance.PostMessage(status);

                // Remove the current incoming socket and try
                // to accept the next one.
                PPBTCPSocket.Close(incomingSocket);
                incomingSocket.Dispose();
                TryAccept();
                return;
            }

            status = $"server: Read {(int)result} bytes";
            instance.PostMessage(status);

            // Echo the bytes back to the client
            result = (PPError)PPBTCPSocket.Write(incomingSocket,
                receiveBuffer,
                (int)result,
                new CompletionCallback(OnWriteCompletion));

            if (result != PPError.OkCompletionpending)
            {
                instance.PostMessage($"server: Write failed: {result}");
            }
        }

        private void OnWriteCompletion(PPError result)
        {
            if (result < 0)
            {
                instance.PostMessage($"server: Write failed: {result}");
                return;
            }

            instance.PostMessage($"server: Wrote {(int)result} bytes");

            // Try and read more bytes from the client
            TryRead();
        }
    }
}
