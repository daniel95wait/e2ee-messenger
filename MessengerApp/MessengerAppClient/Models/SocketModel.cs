﻿using MessengerAppShared.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MessengerAppShared.Models
{
    public class SocketModel
    {
        // Constants
        public static readonly int PORT = 31416;
        public static readonly int BACKLOG = 10;
        public static readonly int BUFFER_SIZE = 2048;

        public AutoResetEvent stopWaitHandle = new AutoResetEvent(false);

        // Fields
        public Socket Socket;

        // Properties
        public static IPAddress Address { get; set; } = IPAddress.Loopback;
        public byte[] Buffer { get; set; } = new byte[BUFFER_SIZE];
        public bool Connected
        {
            get
            {
                // Tries to get Connected property, if Socket is null return false
                return (Socket?.Connected ?? false);
            }
        }

        // Creates socket object
        public void CreateSocket()
        {
            // Creates socket object
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        //// Connects to network
        //public void Connect()
        //{
        //    // When socket is already conneted
        //    if (Connected) { return; }

        //    CreateSocket();

        //    // For counting connection attemps
        //    int attempts = 0;
        //    // Repeat while socket is not connected for a max of 3 times
        //    while (!Socket.Connected && attempts <= 3)
        //    {
        //        // Error thrown when socket cannot connect
        //        try
        //        {
        //            attempts++;
        //            // Connects to the current machine
        //            Socket.Connect(Address, PORT);
        //        }
        //        catch (SocketException) { }
        //    }
        //}

        //// Disconnects from network
        //public void DisconnectFromServer()
        //{
        //    if (Socket != null)
        //    {
        //        // Ends connection
        //        Socket.Shutdown(SocketShutdown.Both);
        //        Socket.Close();
        //    }
        //}

        //// Sends message to server
        //public void SendSync(Socket socketReciever, string text)
        //{
        //    // When socket hasnt been created / is destroyed
        //    if (socketReciever == null) { Console.WriteLine("Null socket"); return; }

        //    // Creates transmission object
        //    Transmission message;
        //    try { message = new Transmission(text); Console.WriteLine("Message created"); }
        //    catch (ArgumentNullException) { Console.WriteLine("Invalid message"); return; }

        //    // Sends the byte array
        //    try { socketReciever.Send(message.Data, message.Data.Length, SocketFlags.None); Console.WriteLine("Message sent"); }
        //    catch (SocketException e) { Console.WriteLine("Socket error " + e.ErrorCode.ToString()); return; }
        //}

        //// Receive message from server
        //public string ReceiveSync()
        //{
        //    // When socket hasnt been created / is destroyed
        //    if (Socket == null) { return String.Empty; }

        //    // Reads response in and gets its length
        //    int amount;
        //    try { amount = Socket.Receive(Buffer, SocketFlags.None); }
        //    catch (SocketException) { return String.Empty; }

        //    // When nothing was recieved
        //    if (amount == 0) { return String.Empty; }

        //    byte[] received = new byte[amount];
        //    // Takes part of Buffer that has received data
        //    Array.Copy(Buffer, received, amount);
        //    // Creates transmisson object for data recieved
        //    Transmission message = new Transmission(received);
        //    // Returns the text
        //    return message.Content;
        //}



        // Async tests

        public void ConnectToServer(IPAddress address, int port, Socket clientSocket)
        {
            try
            {
                if (!clientSocket.Connected)
                {
                    IAsyncResult result = clientSocket.BeginConnect(address, port, new AsyncCallback(ConnectToServerCallback), clientSocket);
                }
            }
            catch (Exception) { }
        }

        private void ConnectToServerCallback(IAsyncResult asyncResult)
        {
            try
            {
                // Retrieve socket from state object
                Socket clientSocket = (Socket)asyncResult.AsyncState;
                // Comeplets connect
                clientSocket.EndConnect(asyncResult);
            }
            catch(Exception) { }

            stopWaitHandle.Set();
        }

        public static void Send(byte[] data, Socket socket)
        {
            try
            {
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            }
            catch (Exception) { }
        }

        private static void SendCallback(IAsyncResult asyncResult)
        {
            Socket socket = (Socket)asyncResult.AsyncState;

            int amountSend = socket.EndSend(asyncResult);
        }
    }
}
