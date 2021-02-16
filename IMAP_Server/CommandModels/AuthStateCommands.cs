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
        private static readonly int SELECT_SPLIT = 3;

        public static void Append(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            
        }

        public static void Create(string[] command, ConnectionState connectionState, NetworkStream stream)
        {
            foreach(KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
            {
                if (mb.Value.mailboxName == ImapUTF7.Encode(command[2]))
                {
                    //send "NO" response then break
                    return;
                }
            }
            Mailbox mailbox = new Mailbox()
            {
                mailboxName = ImapUTF7.Encode(command[2]),
                mailboxSize = 50000
            };
            Server.mailBoxes.Add("Jimoo@gmail.com", mailbox);
            //Send OK message
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
