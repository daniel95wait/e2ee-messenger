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

        private string _messageReceived;
        public string MessageReceived
        {
            get { return _messageReceived; }
            set { _messageReceived = value; }
        }

        public ShellViewModel()
        {
            // TODO: Wire-up buttons
            // TODO: Dynamic add TextBlocks to StackPanel
        }

        public void Connect()
        {
            socket.Connect();

            // TODO: Begins a loop to receive
        }

        public void SendMessage()
        {
            socket.Send(MessageToSend, socket.Socket);
            socket.Receive(socket.Socket);

            // Display the received message
            // TODO: Wait for ReceiveCallback return before running this
            if (socket.ReceiveMessages.Count != 0)
            {
                MessageReceived = socket.ReceiveMessages[socket.ReceiveMessages.Count - 1];
                NotifyOfPropertyChange(() => MessageReceived);
            }

        }
    }
}
