using IMAP.Shared;
using IMAP.Shared.Services;
using Serilog;
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
        private const int LOGIN_SPLIT = 4;
        private const int AUTHENTICATE_SPLIT = 2;
        private const int STARTTLS_SPLIT = 2;


        public static void Authenticate(string[] command, Connection connectionState)
        {
            string tag = command[0];
            string cmd = command[1];
            Log.Logger.Information($"{cmd} request by {connectionState.Ip}/{connectionState.Username}");
            if (!connectionState.Authentificated) //Only non-authenticated may use these commands.
            {
                if (command.Length == AUTHENTICATE_SPLIT)
                {
                    string response = $"{tag} NO - AUTHENTICATE is not available on this server";
                    connectionState.SendToStream(response);
                }
                else
                {
                    connectionState.SendToStream($"{tag} BAD - command unknown or arguments invalid");
                }
            }
            else
            {
                connectionState.SendToStream($"{tag} BAD - command unknown or arguments invalid");
            }
            Log.Logger.Information($"{cmd} response sent to {connectionState.Ip}/{connectionState.Username}");
        }

        public static void Login(string[] command, Connection connectionState)
        {
            string tag = command[0];
            string cmd = command[1];
            string username = command[2].Replace("\"", "");
            if (username.EndsWith("@localhost"))
            {
                username = username.Split("@")[0];
            }
            username = username.ToLower();
            string password = command[3].Replace("\"", "");

            Log.Logger.Information($"{cmd} request by {connectionState.Ip}/{connectionState.Username}");

            string ok = $"{tag} OK - login completed, now in authenticated state";
            string no = $"{tag} NO - login failure: user name or password rejected";
            string bad = $"{tag} BAD - command unknown or arguments invalid";


            if (!connectionState.Authentificated) //Only non-authenticated may use these commands.
            {
                if (!ValidationService.ValidateUsername(username) || command.Length != LOGIN_SPLIT)
                {
                    connectionState.SendToStream(bad);
                }
                else if (Server.users.TryGetValue(username, out var user))
                {
                    if (user.Password == password)
                    {
                        connectionState.Authentificated = true;
                        connectionState.Username = username;
                        connectionState.SendToStream(ok);
                    }
                    else //Wrong password
                    {
                        connectionState.SendToStream(no);
                    }
                }
                else //Wrong username
                {
                    connectionState.SendToStream(no);
                }
            }
            else
            {
                connectionState.SendToStream(no);
            }

            Log.Logger.Information($"{cmd} response sent to {connectionState.Ip}/{connectionState.Username}");
        }

        public static void StartTLS(string[] command, Connection connectionState)
        {
            string tag = command[0];
            string cmd = command[1];
            Log.Logger.Information($"{cmd} request by {connectionState.Ip}/{connectionState.Username}");
            if (!connectionState.Authentificated) //Only non-authenticated may use these commands.
            {
                if (command.Length == STARTTLS_SPLIT)
                {
                    string response = $"{tag} NO - STARTTLS is not available on this server";
                    connectionState.SendToStream(response);
                }
                else
                {
                    connectionState.SendToStream($"{tag} BAD - command unknown or arguments invalid");
                }
            }
            else
            {
                connectionState.SendToStream($"{tag} BAD - command unknown or arguments invalid");
            }
            Log.Logger.Information($"{cmd} response sent to {connectionState.Ip}/{connectionState.Username}");
        }
    }
}
