using System;
using System.Collections.Generic;
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
        private const int BACKLOG_MAX = 10;  // Maximum length connection 'queue' of clients
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        static void Main()
        {
            Console.Title = "MessengerApp Server";
            SetupServer();
            Console.ReadLine();  // Stops server closing after startup
        }

        // Starts socketing listening for client connections
        private static void SetupServer()
        {
            Console.WriteLine("Starting server...");

            // Tells the socket where to listen (connects to )
            serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, PORT));

            // Starts listening
            serverSocket.Listen(BACKLOG_MAX);

            // Incomming connection request (async)
            serverSocket.BeginAccept(ClientConnectLoop, null);

            Console.WriteLine("Setup complete");
        }

        // Loop adds clients
        private static void ClientConnectLoop(IAsyncResult asyncResult)
        {
            // Creates socket for the client
            Socket socket = serverSocket.EndAccept(asyncResult);

            // Adds client to connected clients list
            clientSockets.Add(socket);

            Console.WriteLine(socket.RemoteEndPoint.ToString() + " has connected");

            // Writes received message to buffer
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, socket);

            // Start accepting the next client
            serverSocket.BeginAccept(ClientConnectLoop, null);
        }

        // Receives the data from the client and echoes it back to them
        private static void ReceiveCallback(IAsyncResult asyncResult)
        {
            // Recreates the same socket for the client
            Socket socket = (Socket)asyncResult.AsyncState;

            // When socket has been disconnected
            if (!socket.Connected)
            {
                DisconnectSocket(socket);
                return;
            }

            int received;
            // Fails if client disconnects
            try
            {
                // Ends read and returns length of received message
                received = socket.EndReceive(asyncResult);
            }
            catch (SocketException)
            {
                DisconnectSocket(socket);
                return;
            }

            if (received != 0)
            {
                // Array to hold received message
                byte[] data = new byte[received];

                // Copies only the part of buffer that holds received message to data
                Array.Copy(buffer, data, received);

                // Unicode Bytes to String
                string text = Encoding.Unicode.GetString(data);

                Console.WriteLine(socket.RemoteEndPoint.ToString() + " says " + text);

                // Echoes back what client sent
                SendText(socket, text);
            }
            else
            {
                DisconnectSocket(socket);
                return;
            }

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

            Console.WriteLine("Sending \"" + message + "\" to " + socket.RemoteEndPoint.ToString());
        }

        private static void DisconnectSocket(Socket socket)
        {
            clientSockets.Remove(socket);
            Console.WriteLine(socket.RemoteEndPoint.ToString() + " has disconnected");

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

    }
}
