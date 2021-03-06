using System;
using System.Net;
using System.Net.Sockets;

namespace MessengerAppShared.Models
{
    public class NetworkSocket
    {
        // Constants
        public static readonly int PORT = 31416;
        public static readonly int BACKLOG = 10;
        public static readonly int BUFFER_SIZE = 2048;

        // Fields
        public Socket Socket;

        // Properties
        public IPAddress Address { get; set; } = IPAddress.Loopback;
        public byte[] Buffer { get; set; } = new byte[BUFFER_SIZE];
        public bool Connected
        {
            get
            {
                if (Socket == null) { return false; }
                else { return Socket.Connected; }
            }
        }

        // Creates socket object
        public void CreateSocket()
        {
            // Creates socket object
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        // Connects to network
        public void ConnectToServer()
        {
            // When socket is already conneted
            if (Connected) { return; }

            CreateSocket();

            // For counting connection attemps
            int attempts = 0;
            // Repeat while socket is not connected for a max of 3 times
            while (!Socket.Connected && attempts <= 3)
            {
                // Error thrown when socket cannot connect
                try
                {
                    attempts++;
                    // Connects to the current machine
                    Socket.Connect(Address, PORT);
                }
                catch (SocketException) { }
            }
        }

        // Disconnects from network
        public void DisconnectFromServer()
        {
            if (Socket != null)
            {
                // Ends connection
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
        }

        // Sends message to server
        public void SendSync(Socket socketReciever, string text)
        {
            // When socket hasnt been created / is destroyed
            if (socketReciever == null) { Console.WriteLine("Null socket"); return; }

            // Creates transmission object
            Transmission message;
            try { message = new Transmission(text); Console.WriteLine("Message created"); }
            catch (ArgumentNullException) { Console.WriteLine("Invalid message"); return; }

            // Sends the byte array
            try { socketReciever.Send(message.Data, message.Data.Length, SocketFlags.None); Console.WriteLine("Message sent"); }
            catch (SocketException e) { Console.WriteLine("Socket error " + e.ErrorCode.ToString()); return; }
        }

        // Receive message from server
        public string ReceiveSync()
        {
            // When socket hasnt been created / is destroyed
            if (Socket == null) { return String.Empty; }

            // Reads response in and gets its length
            int amount;
            try { amount = Socket.Receive(Buffer, SocketFlags.None); }
            catch (SocketException) { return String.Empty; }

            // When nothing was recieved
            if (amount == 0) { return String.Empty; }

            byte[] received = new byte[amount];
            // Takes part of Buffer that has received data
            Array.Copy(Buffer, received, amount);
            // Creates transmisson object for data recieved
            Transmission message = new Transmission(received);
            // Returns the text
            return message.Content;
        }
    }
}
