using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MessengerAppClient.ViewModels
{
    public class ShellViewModel : Screen
    {
        private static readonly Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int PORT = 31416;
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        private string _connectionStatus = "Not connected";
        private string _messageIn = "";
        private string _messageOut = "";

        // Controls state of buttons
        public bool CanConnect => !socket.Connected;
        public bool CanSend => (socket.Connected && !string.IsNullOrWhiteSpace(MessageOut));

        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                NotifyOfPropertyChange(() => ConnectionStatus);
            }
        }

        public string MessageIn
        {
            get { return _messageIn; }
            set
            {
                _messageIn = value;
                NotifyOfPropertyChange(() => MessageIn);
            }
        }

        public string MessageOut
        {
            get { return _messageOut; }
            set
            {
                _messageOut = value;
                NotifyOfPropertyChange(() => MessageOut);
                NotifyOfPropertyChange(() => CanSend);
            }
        }

        // Connects to the server
        public void Connect()
        {
            ConnectLoop();
            this.NotifyOfPropertyChange(nameof(CanSend));
            this.NotifyOfPropertyChange(nameof(CanConnect));
        }

        // Send message to the server and listens for response
        public void Send()
        {
            SendMessage(MessageOut);
            ReceiveResponse();
        }

        // Connects to server
        public void ConnectLoop()
        {
            // For counting connection attemps
            int attempts = 0;

            // Repeat while socket is not connected for a max of 5 times
            while (!socket.Connected && attempts <= 5)
            {
                // Error thrown when socket cannot connect
                try
                {
                    attempts++;
                    // Connects to the current machine
                    socket.Connect(IPAddress.Parse("127.0.0.1"), PORT);
                }
                catch (SocketException) { }
            }

            if (socket.Connected)
            {
                ConnectionStatus = "Connected";
            }
            else
            {
                ConnectionStatus = "Not connected";
            }

        }

        // Sends message to server
        public void SendMessage(string text)
        {
            // String to Unicode Bytes
            byte[] bufferOut = Encoding.Unicode.GetBytes(text);
            // Synchronous send to server (change to async in future)
            socket.Send(bufferOut, 0, bufferOut.Length, SocketFlags.None);
        }

        // Listens for response from server
        public void ReceiveResponse()
        {
            // Reads response to buffer and gets length of response
            int recieved = socket.Receive(buffer, SocketFlags.None);
            // When nothing has been recieved
            if (recieved == 0) { return; }
            // Creates array the length of response
            byte[] data = new byte[recieved];
            // Puts the part of buffer containing response into data
            Array.Copy(buffer, data, recieved);
            // Unicode Bytes to String
            MessageIn = Encoding.Unicode.GetString(data);
        }
    }
}
