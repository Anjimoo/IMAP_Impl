using IMAP.Shared;
using IMAP.Shared.Models;
using IMAP.Shared.Services;
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
        private const int SELECT_SPLIT = 3;

        public static void Append(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }

        public static void Create(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            foreach(KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
            {
                if (mb.Value.mailboxName == ImapUTF7.Encode(command[2]))
                {
                    AnyStateCommands.SendResponse(stream, $"NO CREATE: Mailbox already exists in that name");
                    return;
                }
            }
            Mailbox mailbox = new Mailbox()
            {
                mailboxName = ImapUTF7.Encode(command[2]),
                mailboxSize = 50000
            };
            Server.mailBoxes.Add(mailbox.mailboxName, mailbox);
            AnyStateCommands.SendResponse(stream, $"OK CREATE Completed: {mailbox.mailboxName} Successfully removed");
        }

        public static void Delete(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            foreach(KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
            {
                if(mb.Value.mailboxName==ImapUTF7.Encode(command[2]))
                {
                    Server.mailBoxes.Remove(mb.Value.mailboxName);
                    AnyStateCommands.SendResponse(stream, $"OK DELETE Completed: {mb.Value.mailboxName} Successfully removed");
                    return;
                }
            }
            AnyStateCommands.SendResponse(stream, $"NO DELETE: Mailbox was not found");
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
            
        }

        public static void Select(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            if(command.Length == SELECT_SPLIT) //check if command is legal
            {
                
                if(Server.mailBoxes.TryGetValue(command[2], out var mailbox)) //check if chosen mailbox is present
                {
                    connectionState.SelectedMailBox = true;
                    AnyStateCommands.SendResponse(stream, $"* {mailbox.EmailMessages.Count} EXISTS");
                    AnyStateCommands.SendResponse(stream, $"* 2 RECENT");
                }
                else
                {
                    AnyStateCommands.SendResponse(stream, 
                        $"{command[0]} NO - select failure, now in authenticated state: no such mailbox, can’t access mailbox");
                }
            }
            else
            {
                AnyStateCommands.SendResponse(stream, $"{command[0]} BAD - command unknown or arguments invalid");
            }
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
