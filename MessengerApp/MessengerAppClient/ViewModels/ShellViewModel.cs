using Caliburn.Micro;
using MessengerAppClient.Model;
using System;
using System.Collections.Generic;
using System.Text;


namespace MessengerAppClient.ViewModels
{
    class ShellViewModel : Screen
    {
        public ClientSocket socket = new ClientSocket();

        public ShellViewModel()
        {
            // TODO: Wire-up buttons
            // TODO: Dynamic add TextBlocks to StackPanel
        }
    }
}
