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
        public static List<string> capabilities = new List<string>() { "CAPABILITY", "IMAP4rev1", /*"AUTH=PLAIN", "LOGINDISABLED, "STARTTLS */ "CATENATE", "WITHIN" };

        public static void Capability(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            Log.Logger.Information($"Capability request by {connectionState.Ip}");
            string tag = command[0];
            string ok = $"{tag} OK - capability completed";
            string bad = $"{tag} BAD - command unknown or arguments invalid";
        }

        public static void Logout(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
           
        }

        public static void NOOP(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
           
        }

        public static void Default(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            string tag = command[0];
            string bad = $"{tag} BAD - command unknown or arguments invalid";

            SendResponse(stream, bad);
        }

        public static void SendResponse(NetworkStream stream, string response)
        {
            Byte[] reply = System.Text.Encoding.UTF8.GetBytes(response);
            stream.Write(reply, 0, reply.Length);
            Log.Logger.Information($"SENT : {response}");
        }

    }
}
