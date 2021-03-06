using Caliburn.Micro;
using MessengerAppShared.Models;
using System;
using System.Diagnostics;
using System.Threading;

namespace MessengerAppClient.ViewModels
{
    public class ShellViewModel : Screen
    {
        public SocketModel NetSocket = new SocketModel();

        public ShellViewModel()
        {
            NetSocket.CreateSocket();
            ConnectionStatus = "Not connected";
        }


        private string _messageToSend;
        public string MessageToSend
        {
            get { return _messageToSend; }
            set {
                _messageToSend = value;
                NotifyOfPropertyChange(() => MessageToSend);
            }
        }


        private string _messageRecieved;
        public string MessageRecieved
        {
            get { return _messageRecieved; }
            set { _messageRecieved = value; }
        }


        private string _connectionStatus;
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set {
                Debug.WriteLine($"Debug: status from {ConnectionStatus} to {value}");
                _connectionStatus = value;
                NotifyOfPropertyChange(() => ConnectionStatus);
            }
        }


        public bool CanConnect => ConnectionStatus != "Connected";
        public void Connect()
        {
            ConnectionStatus = "Connecting";
            NetSocket.ConnectToServer(SocketModel.Address, SocketModel.PORT, NetSocket.Socket);
            NetSocket.stopWaitHandle.WaitOne();
            if (NetSocket.Connected)
            {
                ConnectionStatus = "Connected";
                Debug.WriteLine(ConnectionStatus);
            }
            else
            {
                ConnectionStatus = "Connection failed";
            }
        }


        public bool CanDisconnect => ConnectionStatus == "Connected";
        public void Disconnect()
        {
            throw new NotImplementedException();
            
            // Make disconnect async
        }


        public bool CanSend => ConnectionStatus == "Connected";
        public void Send()
        {
            throw new NotImplementedException();

            // Make send async
        }

        //public static SocketModel NetSocket = new SocketModel();

        //// Bools to control when buttons are enabled
        //public bool CanConnect => !NetSocket.Connected;
        //public bool CanDisconnect => NetSocket.Connected;
        //public bool CanSend => NetSocket.Connected;

        //public enum ConnectionStatusEnum
        //{
        //    Connected,
        //    Connecting,
        //    Disconnected,
        //    Disconnecting
        //}

        //private ConnectionStatusEnum _status = ConnectionStatusEnum.Disconnected;

        //// Whether the socket is connected or not
        //public class Status
        //{
        //    set
        //    {
        //        _status = value;

        //        NotifyOfPropertyChange(() => Connected);

        //        NotifyOfPropertyChange(() => CanConnect);
        //        NotifyOfPropertyChange(() => CanDisconnect);
        //        NotifyOfPropertyChange(() => ConnectionStatus);
        //    }
        //    get
        //    {
        //        switch (_status)
        //        {
        //            case ConnectionStatusEnum.Connected:
        //                return "Connected";
        //            case ConnectionStatusEnum.Connecting:
        //                return "Connecting";
        //            case ConnectionStatusEnum.Disconnected:
        //                return "Disconnected";
        //            case ConnectionStatusEnum.Disconnecting:
        //                return "Disconnected";
        //            default:
        //                return "Unknown";
        //        }
        //    }
        //}

        //private bool _connected;
        //public bool Connected
        //{
        //    get { return _connected; }
        //    set
        //    {
        //        _connected = value;

        //    }
        //}

        //// Message server sends to client
        //private string _recieved = String.Empty;
        //public string Recieved
        //{
        //    get { return _recieved; }
        //    set
        //    {
        //        _recieved = value;
        //        NotifyOfPropertyChange(() => Recieved);
        //    }
        //}

        //// Message client sends to server
        //private string _messageToSend = String.Empty;
        //public string MessageToSend
        //{
        //    get { return _messageToSend; }
        //    set { _messageToSend = value; }
        //}

        ////Send message to the server and listens for response
        //public void Send()
        //{
        //    // Gets up to date message
        //    NotifyOfPropertyChange(() => MessageToSend);

        //    // Only sends message when its not blank
        //    if (!String.IsNullOrEmpty(MessageToSend))
        //    {
        //        NetSocket.SendSync(NetSocket.Socket, MessageToSend);
        //        Recieved = NetSocket.ReceiveSync();
        //    }
        //}

        //public void Disconnect()
        //{
        //    NetSocket.DisconnectFromServer();
        //    // Refresh properites

        //    Connected = false;
        //}

        //public void Connect()
        //{
        //    NetSocket.CreateSocket();
        //    SocketModel.ConnectToServer(SocketModel.Address, SocketModel.PORT, NetSocket.Socket);

        //    Connected = true;

        //    // NetSocket.ConnectToServer();
        //}
    }
}
