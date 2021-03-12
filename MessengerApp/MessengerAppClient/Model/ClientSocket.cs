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
        public List<string> ReceiveMessages = new List<string>();

        public void Connect()
        {
            Socket.Connect(EndPoint);
        }

        // TODO: Receive message from server
        public override void ReceiveCallback(IAsyncResult asyncResult)
        {
            // Gets socket from the async result
            Socket clientSocket = (Socket)asyncResult.AsyncState;
            // The amount of data received
            int received = clientSocket.EndReceive(asyncResult);

            byte[] dataBuffer = new byte[received];
            // Copy received bytes to data buffer
            Array.Copy(Buffer, dataBuffer, received);
            // Converts received byte[] to string
            string text = new Protocol(dataBuffer).Text;

            // TODO: Change this to a queue
            ReceiveMessages.Add(text);
        }
    }
}
