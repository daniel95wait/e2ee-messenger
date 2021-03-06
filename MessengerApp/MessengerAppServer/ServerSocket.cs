using MessengerAppShared.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace MessengerAppServer
{
    public class ServerSocket : NetworkSocket
    {
        public List<Socket> ClientSockets = new List<Socket>();
        public bool Active;

        public void StartServer()
        {
            Console.WriteLine("Starting server...");

            Active = true;
            // Binds to local address
            Socket.Bind(new IPEndPoint(Address, PORT));
            // Send socket to listen
            Socket.Listen(BACKLOG);

            Socket.BeginAccept(ClientConnectLoopAsync, null);

            Console.WriteLine("Setup complete");
        }

        // (Loop) Accepts new clients and passes them on to receive loop
        public void ClientConnectLoopAsync(IAsyncResult asyncResult)
        {
            // Breaks all loops when Actice is false
            if (!Active) { return; }

            // Accepts new client
            Socket newSocket = Socket.EndAccept(asyncResult);
            ClientSockets.Add(newSocket);

            Console.WriteLine(newSocket.RemoteEndPoint.ToString() + " connected");

            // Starts client's recieving loop
            newSocket.BeginReceive
                (Buffer, 0, BUFFER_SIZE, SocketFlags.None, ClientReceiveLoopAsync, newSocket);

            // Goes back to start of loop
            Socket.BeginAccept(ClientConnectLoopAsync, null);
        }

        // (Loop) Listens for messages from client
        public void ClientReceiveLoopAsync(IAsyncResult asyncResult)
        {
            // Breaks all loops when Active is false
            if (!Active) { return; }

            // Recreates the same socket for client
            Socket clientSocket = (Socket)asyncResult.AsyncState;

            // Tries to receive message
            int amount;
            try { amount = clientSocket.EndReceive(asyncResult); }
            catch (SocketException)
            {
                // Disconnects when error with socket
                DisconnectClient(clientSocket);
                return;  // Breaks loop
            }

            // When nothing is comming from socket, disconnect
            if (amount == 0)
            {
                DisconnectClient(clientSocket);
                return; // Breaks loop
            }

            // Writes received data to a Transmission object
            byte[] received = new byte[amount];
            Array.Copy(Buffer, received, amount);
            Transmission message = new Transmission(received);

            Console.WriteLine(clientSocket.RemoteEndPoint.ToString() + " says \"" + message.Content + "\"");

            // Echoes what the client sent back to client
            SendSync(clientSocket, message.Content);

            // Goes back to start of loop
            clientSocket.BeginReceive
                (Buffer, 0, BUFFER_SIZE, SocketFlags.None, ClientReceiveLoopAsync, clientSocket);
        }

        // Disconnects a socket
        public void DisconnectClient(Socket socket)
        {
            // Gets the socket's IP
            string identifier = socket.RemoteEndPoint.ToString();

            // Removes from list of connected sockets
            ClientSockets.Remove(socket);
            // Only try methods when object exists
            if (socket != null)
            {
                // Ends sending and receiving for socket
                socket.Shutdown(SocketShutdown.Both);
                // Ends connection
                socket.Close();
            }

            Console.WriteLine(identifier + " disconnected");
        }
    }
}
