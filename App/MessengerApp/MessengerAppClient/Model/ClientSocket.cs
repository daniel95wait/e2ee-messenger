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
            // TODO: Connect socket to server
        }

        public void Send()
        {
            // TODO: Send message to server
        }

        public void Receive()
        {
            // TODO: Receive message from server
        }

        public override void ReceiveCallback(IAsyncResult asyncResult)
        {
            // TODO: Implement ReceiveCallback

            throw new NotImplementedException();
        }
    }
}
