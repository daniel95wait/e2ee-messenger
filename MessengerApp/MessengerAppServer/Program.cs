using System;

namespace MessengerAppServer
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Messenger Server";

            ServerSocket serverSocket = new ServerSocket();
            serverSocket.Start();

            // TODO: Fix pos of cursor after enter for pretty console output
            //Console.ReadLine();
            //Console.SetCursorPosition(0, Console.CursorTop - 1);

            // User presses enter to close the server
            serverSocket.Stop();

            // TODO: Add commands from server console
        }
    }
}
