using IMAP.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Server.CommandModels
{
    public static class NonAuthStateCommands
    {
        public static void Authenticate(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
          
        }

        public static void Login(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
          
        }

        public static void StartTLS(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
          
        }
    }
}
