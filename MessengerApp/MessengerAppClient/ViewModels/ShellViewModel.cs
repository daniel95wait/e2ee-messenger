using Caliburn.Micro;
using MessengerAppClient.Model;
using MessengerAppShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace MessengerAppClient.ViewModels
{
    class ShellViewModel : Screen
    {
        public ClientSocket socket = new ClientSocket();

        private string _messageToSend;
        public string MessageToSend
        {
            get { return _messageToSend; }
            set { _messageToSend = value; }
        }

        public ShellViewModel()
        {
            // TODO: Wire-up buttons
            // TODO: Dynamic add TextBlocks to StackPanel
        }

        public void Connect()
        {
            socket.Connect();
        }

        public void SendMessage()
        {
            socket.Send(MessageToSend);
        }
    }
}
