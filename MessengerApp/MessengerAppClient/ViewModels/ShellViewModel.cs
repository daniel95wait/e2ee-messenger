using Caliburn.Micro;
using MessengerAppShared.Models;
using System;

namespace MessengerAppClient.ViewModels
{
    public class ShellViewModel : Screen
    {
        public static NetworkSocket socket = new NetworkSocket();

        // Bools to control when buttons are enabled
        public bool CanReconnect => !socket.Connected;
        public bool CanDisconnect => socket.Connected;
        public bool CanSend => socket.Connected;

        // Whether the socket is connected or not
        public string ConnectionStatus = "No connection";
        public bool Connected
        {
            get { return Connected; }
            set
            {
                Connected = value;
                ConnectionStatus = Connected ? "Connected" : "No connection";

                NotifyOfPropertyChange(() => Connected);
                NotifyOfPropertyChange(() => CanReconnect);
                NotifyOfPropertyChange(() => CanDisconnect);
            }
        }

        // Message server sends to client
        private string _recieved = "";
        public string Recieved
        {
            get { return _recieved; }
            set
            {
                _recieved = value;
                NotifyOfPropertyChange(() => Recieved);
            }
        }

        // Message client sends to server
        private string _messageToSend = "";
        public string MessageToSend
        {
            get { return _messageToSend; }
            set { _messageToSend = value; }
        }

        //Send message to the server and listens for response
        public void Send()
        {
            // Gets up to date message
            NotifyOfPropertyChange(() => MessageToSend);

            // Only sends message when its not blank
            if (!String.IsNullOrEmpty(MessageToSend))
            {
                socket.SendSync(MessageToSend);
                Recieved = socket.ReceiveSync();
            }
        }

        public void Disconnect()
        {
            socket.Disconnect();
            // Refresh properites
            NotifyOfPropertyChange(() => CanSend);
            NotifyOfPropertyChange(() => CanReconnect);
            NotifyOfPropertyChange(() => CanDisconnect);
        }

        public void Reconnect()
        {
            socket.Connect();
            // Refresh properites
            NotifyOfPropertyChange(() => CanSend);
            NotifyOfPropertyChange(() => CanDisconnect);
            NotifyOfPropertyChange(() => CanReconnect);
        }
    }
}
