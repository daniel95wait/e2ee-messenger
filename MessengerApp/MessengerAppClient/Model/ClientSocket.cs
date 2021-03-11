using MessengerAppShared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MessengerAppClient.Model
{
    public class ClientSocket : SocketBase
    {
        public void Connect()
        {
            Socket.Connect(EndPoint);
        }

        // TODO: Receive message from server

        public override void ReceiveCallback(IAsyncResult asyncResult)
        {
            // TODO: Implement ReceiveCallback
            throw new NotImplementedException();
        }
    }
}
