using IMAP.Shared;
using IMAP.Shared.Models;
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
        private const int SEARCH_MINSPLIT = 2;

        public static void Check(string[] command, Connection connectionState)
        {
            
        }
        public static void Close(string[] command, Connection connectionState)
        {
            if (connectionState.SelectedMailBox.EmailMessages.Count != 0)
            {
                foreach (EmailMessage em in connectionState.SelectedMailBox.EmailMessages)
                {
                    if (em.Flags.TryGetValue(@"\Deleted", out var deleted))
                    {
                        if (deleted)
                        {
                            int deletedUID = em.UniqueID;
                            connectionState.SelectedMailBox.EmailMessages.Remove(em);
                        }
                    }
                }
            }
            connectionState.SelectedMailBox = null;
            connectionState.SelectedState = false;
            connectionState.SendToStream($"{command[0]} OK CLOSE completed");
        }
        public static void Copy(string[] command, Connection connectionState)
        {
            
        }
        public static void Expunge(string[] command, Connection connectionState)
        {
            if (connectionState.SelectedMailBox.EmailMessages.Count != 0)
            {

                foreach (EmailMessage em in connectionState.SelectedMailBox.EmailMessages)
                {
                    if (em.Flags.TryGetValue(@"\Deleted", out var deleted))
                    {
                        if (deleted)
                        {
                            int deletedUID = em.UniqueID;
                            connectionState.SelectedMailBox.EmailMessages.Remove(em);
                            connectionState.SendToStream($"{command[0]} {deletedUID} EXPUNGE");
                        }
                    }
                }
            }
            connectionState.SendToStream($"{command[0]} OK EXPUNGE completed");
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
            if(command.Length > SEARCH_MINSPLIT)
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
