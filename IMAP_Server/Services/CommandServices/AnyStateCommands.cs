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

        public static void Capability(string[] command, Connection connectionState)
        {
            string tag = command[0];
            string cmd = command[1];
            Log.Logger.Information($"{cmd} request by {connectionState.Ip}/{connectionState.Username}");

            string response = "*";

            foreach (var cap in capabilities)
            {
                response+=$" {cap}";
            }

            connectionState.SendToStream(response);
            connectionState.SendToStream(OK(tag, cmd));
            Log.Logger.Information($"{cmd} list sent to {connectionState.Ip}/{connectionState.Username}");
        }

        public static void Logout(string[] command, Connection connectionState)
        {
            string tag = command[0];
            string cmd = command[1];
            Log.Logger.Information($"{cmd} request by {connectionState.Ip}/{connectionState.Username}");

            connectionState.Authentificated = false;

            Log.Logger.Information($"{cmd} sent to {connectionState.Ip}/{connectionState.Username}");
        }

        public static void NOOP(string[] command, Connection connectionState)
        {
            string tag = command[0];
            string cmd = command[1];
            Log.Logger.Information($"{cmd} request by {connectionState.Ip}");


        }

        public static void Default(string[] command, Connection connectionState)
        {
            string tag = command[0];
            connectionState.SendToStream(BAD(tag));
        }


        public static string OK(string tag, string command)
        {
            return $"{tag} OK - {command} completed";
        }

        public static string BAD(string tag)
        {
            return $"{tag} BAD - command unknown or arguments invalid";
        }
    }
}
