using IMAP.Shared;
using IMAP_Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Server.CommandModels
{
    public static class SelectStateCommands
    {
        public static void Check(string[] command, Connection connectionState)
        {
            
        }
        public static void Close(string[] command, Connection connectionState)
        {
            
        }
        public static void Copy(string[] command, Connection connectionState)
        {
            
        }
        public static void Expunge(string[] command, Connection connectionState)
        {
            
        }
        public static void Fetch(string[] command, Connection connectionState)
        {
            
        }
        /// <summary>
        /// Searches messages in mail box by specific criteria
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connectionState"></param>
        public static void Search(string[] command, Connection connectionState)
        {
            if(command.Length > 2)
            {
                var messages = IMAP_Search.Search(command.Skip(2).ToArray(), connectionState);
                string response = "";
                foreach(var message in messages)
                {
                    response += $"{message}";
                }

                connectionState.SendToStream($"* SEARCH {response}");
                connectionState.SendToStream($"{command[0]} OK SEARCH completed");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - no search criteria specified");
            }
            
        }
        public static void Store(string[] command, Connection connectionState)
        {

        }
        public static void UID(string[] command, Connection connectionState)
        {

        }
    }
}
