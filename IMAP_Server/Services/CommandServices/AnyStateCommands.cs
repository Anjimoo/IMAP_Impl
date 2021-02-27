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
    /// <summary>
    /// A service class that holds handling functions for Any State client commands.
    /// </summary>
    public static class AnyStateCommands
    {
        //A list of the server's capabilities.
        public static List<string> capabilities = new List<string>() 
        { 
          "CAPABILITY",
          "IMAP4rev1",
          /*"AUTH=PLAIN", "LOGINDISABLED, "STARTTLS */ //Not supported.
          "CATENATE", 
          "WITHIN" 
        };

        /// <summary>
        /// Returns the list of server's capabilities.
        /// </summary>
        public static void Capability(string[] command, Connection connectionState)
        {
            string tag = command[0];
            string cmd = command[1];
            Log.Logger.Information($"{cmd} request by {connectionState.Ip}/{connectionState.Username}");

            if (command.Length == 2)
            {
                string response = "*";

                foreach (var cap in capabilities)
                {
                    response += $" {cap}";
                }

                connectionState.SendToStream(response);
                connectionState.SendToStream(OK(tag, cmd));
                Log.Logger.Information($"{cmd} list sent to {connectionState.Ip}/{connectionState.Username}");
            }
            else
            {
                connectionState.SendToStream(BAD(tag));
            }
        }

        /// <summary>
        /// Performs a graceful logout, where both user asks the servrer to log out (and not a forced
        /// shut-down of the connection.)
        /// </summary>
        public static void Logout(string[] command, Connection connectionState)
        {
            string tag = command[0];
            string cmd = command[1];
            Log.Logger.Information($"{cmd} request by {connectionState.Ip}/{connectionState.Username}");

            if (command.Length == 2)
            {
                connectionState.Authentificated = false;

                connectionState.SendToStream("* BYE IMAP4rev1 Server logging out");
                connectionState.SendToStream($"{tag} OK - LOGOUT completed");
                connectionState.CloseConnection();
                Log.Logger.Information($"{cmd} sent to {connectionState.Ip}/{connectionState.Username}");

            }
            else
            {
                connectionState.SendToStream(BAD(tag));
            }
        }

        public static void NOOP(string[] command, Connection connectionState)
        {
            string tag = command[0];
            string cmd = command[1];
            Log.Logger.Information($"{cmd} request by {connectionState.Ip}");

            if (command.Length == 2)
            {
                connectionState.ResetTimeout();
                connectionState.SendToStream($"{tag} OK - NOOP completed. Timeout countdown is now back to 10 minutes.");
                Log.Logger.Information($"{cmd} request by {connectionState.Ip} was approved, connection timeout countdown has been reset.");
            }
            else
            {
                connectionState.SendToStream(BAD(tag));
            }

        }

        /// <summary>
        /// The default server response for any unrecognized command.
        /// </summary>
        public static void Default(string[] command, Connection connectionState)
        {
            string tag = command[0];
            connectionState.SendToStream(BAD(tag));
        }

        public static void Empty(string[] command, Connection connectionState)
        {
            string tag = command[0];
            connectionState.SendToStream("");
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
