using IMAP.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Server.CommandModels
{
    public static class AuthStateCommands
    {
        private static readonly int SELECT_SPLIT = 3;
        private const int RENAME_SPLIT = 4;
        public static void Append(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
             
        }

        public static void Create(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }

        public static void Delete(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }

        public static void Examine(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }

        public static void List(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }

        public static void Lsub(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }

        public static void Rename(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            if (command.Length == RENAME_SPLIT)
            {
                if (Server.mailBoxes.TryGetValue(command[2], out var mailbox))
                {
                    mailbox.mailboxName = command[3];
                    Server.mailBoxes.Remove(command[2]);
                    Server.mailBoxes.Add(command[3], mailbox);
                    AnyStateCommands.SendResponse(stream, command[0] + "OK - rename completed");
                }
            }
            else
            {
                AnyStateCommands.SendResponse(stream, command[0] + "  BAD - command unknown or arguments invalid");
            }
        }

        public static void Select(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }     

        public static void Status(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }

        public static void Subscribe(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }

        public static void Unsubscribe(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }


    }
}
