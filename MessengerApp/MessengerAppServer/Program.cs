using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MessengerAppServer
{
    class Program
    {
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int PORT = 31416;
        private const int BACKLOG_MAX = 100;  // Maximum length connection 'queue' of clients
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        static void Main()
        {
            SetupServer();
            Console.ReadLine();  // Stops server closing after startup
        }

        // Starts socketing listening for client connections
        private static void SetupServer()
        {
            Console.WriteLine("Starting server...");

            // Tells the socket where to listen
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));

            // Starts listening
            serverSocket.Listen(BACKLOG_MAX);

            // Incomming connection request
            serverSocket.BeginAccept(AcceptCallback, null);

            Console.WriteLine("Setup complete");
        }

        // Adds client when connection request is recieved and begins to receive data
        private static void AcceptCallback(IAsyncResult asyncResult)
        {
            // Creates socket for the client
            Socket socket = serverSocket.EndAccept(asyncResult);

            // Adds client to connected clients list
            clientSockets.Add(socket);

            Console.WriteLine(socket.RemoteEndPoint.ToString() + " has connected");

            // Writes received message to buffer
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, socket);

            // Start accepting the next client
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        // Receives the data from the client and echoes it back to them
        private static void ReceiveCallback(IAsyncResult asyncResult)
        {
            // Recreates the same socket for the client
            Socket socket = (Socket)asyncResult.AsyncState;

            int received;

            // Fails if client disconnects
            try
            {
                // Ends read and returns length of received message
                received = socket.EndReceive(asyncResult);
            }
            catch (SocketException)
            {
                Console.WriteLine(socket.RemoteEndPoint.ToString() + " has disconnected");
                
                // Ends the connection
                socket.Close();

                // Removes from list of connected clients
                clientSockets.Remove(socket);

                return;
            }

            // Array to hold received message
            byte[] data = new byte[received];

            // Copies only the part of buffer that holds received message to data
            Array.Copy(buffer, data, received);

            // Bytes to Unicode String
            string text = Encoding.Unicode.GetString(data);

            Console.WriteLine(socket.RemoteEndPoint.ToString() + " says " + text);

            // Echoes back what client sent
            SendText(socket, text);

            // Starts the next receive
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }

        // Encodes and sends text to the client
        private static void SendText(Socket socket, string message)
        {
            // String to Unicode Bytes
            byte[] data = Encoding.Unicode.GetBytes(message);

            // Sends message back
            socket.Send(data, data.Length, SocketFlags.None);
        }

    }
}
