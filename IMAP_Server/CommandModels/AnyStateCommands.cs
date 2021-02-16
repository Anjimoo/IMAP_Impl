using System;
using IMAP.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace IMAP_Server.CommandModels
{
    public static class AnyStateCommands
    {

        public static void Capability(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
           
        }

        public static void Logout(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
           
        }

        public static void NOOP(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
           
        }

        public static void SendResponse(NetworkStream stream, string response)
        {
            Byte[] reply = System.Text.Encoding.UTF8.GetBytes(response);
            stream.Write(reply, 0, reply.Length);
            Log.Logger.Information($"SENT : {response}");
        }

    }
}
