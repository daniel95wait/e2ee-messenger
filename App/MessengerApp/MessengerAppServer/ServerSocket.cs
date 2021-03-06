using MessengerAppShared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MessengerAppServer
{
    public class ServerSocket : SocketBase
    {
        // Dict of identifier and socket
        public Dictionary<string, Socket> ClientSockets = new Dictionary<string, Socket>();
        public const int BACKLOG = 5;

        // Starts listening
        public void Start()
        {
            PrintMessage("Starting server...");

            Socket.Bind(EndPoint);
            Socket.Listen(BACKLOG);

            PrintMessage("Server started");

            Socket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        // Disconnects socket to end server
        public void Stop()
        {
            PrintMessage("Stopping server...");

            try
            {
                // Disables sending and receiving
                Socket.Shutdown(SocketShutdown.Both);
                // Ends connection
                Socket.Close();
            }
            // TODO: Find cause of exception causing clients to disconnect
            catch (Exception e) { PrintMessage("Error: " + e.Message); }

            PrintMessage("Server stopped");
        }

        // Accepts incoming client connection
        public void AcceptCallback(IAsyncResult asyncResult)
        {
            Socket clientSocket = Socket.EndAccept(asyncResult);
            ClientSockets.Add(clientSocket.RemoteEndPoint.ToString(), clientSocket);

            PrintMessage($"Client {clientSocket.RemoteEndPoint} connected");

            // Starts listening for data from the client just connected
            Receive(clientSocket);

            // Starts accepting the next client
            Socket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        // Receives message and echoes it back to client
        public override void ReceiveCallback(IAsyncResult asyncResult)
        {
            // Gets socket from the async result
            Socket clientSocket = (Socket)asyncResult.AsyncState;
            // The amount of data received
            int received = clientSocket.EndReceive(asyncResult);

            byte[] dataBuffer = new byte[received];
            // Copy received bytes to data buffer
            Array.Copy(Buffer, dataBuffer, received);
            // Converts received byte[] to string
            string text = new Protocol(dataBuffer).Text;

            PrintMessage($"Client {clientSocket.RemoteEndPoint} says \"{text}\"");

            // Loops back to start
            Receive(clientSocket);
        }

        // Neatly output messages to the server console
        public static void PrintMessage(string message)
        {
            Console.WriteLine(message);

            /* TODO: Fix pretty console output
                        
            //Error may be thrown by 'hitting' top of screen
            try
            {
                // Ensures blank line above is actually clear
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));

                // Clears closing instruction message
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                Console.Write(new string(' ', Console.WindowWidth));

                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            catch (Exception) { }

            // Prefix is the time padded into set length string
            string prefix = $"[{DateTime.Now.Hour:00}:{DateTime.Now.Minute:00}:{DateTime.Now.Second:00}] ";

            int writeable = Console.WindowWidth - prefix.Length;
            if (message.Length > writeable)
            {
                // Prints lines that exceed window width
                for (int line = 1; message.Length > writeable; ++line)
                {
                    // For the first line
                    if (line == 1) { Console.WriteLine(prefix + message.Substring(0, writeable)); }
                    // All other lines need indentations
                    else {
                        Console.SetCursorPosition(prefix.Length, Console.CursorTop);
                        Console.WriteLine(message.Substring(0, writeable));
                    }

                    // Removes printed section
                    message = message.Remove(0, writeable);
                }

                // Prints remainer
                Console.SetCursorPosition(prefix.Length, Console.CursorTop);
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine(prefix + message);
            }
                       

            Console.Write("\n[Enter to close server]");
            /**/
        }
    }
}
