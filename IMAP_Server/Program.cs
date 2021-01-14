using System;

namespace IMAP_Server
{
    class Program
    {
        static void Main(string[] args)
        {

            Server server = new Server();
            server.StartListening();

        }
    }
}
