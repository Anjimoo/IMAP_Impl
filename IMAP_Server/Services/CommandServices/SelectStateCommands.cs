using IMAP.Shared;
using IMAP_Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IMAP_Server.Models;

namespace IMAP_Server.CommandModels
{
    public static class SelectStateCommands
    {
        private const int SEARCH_MINSPLIT = 2;
        private const int CLOSE_SPLIT = 2;
        private const int CHECK_SPLIT = 2;
        private const int STORE_SPLIT = 4;
        private const int UID_SPLIT = 4;
        private const int COPY_SPLIT = 4;
        private const int EXPUNGE_SPLIT = 2;
        private const int FETCH_SPLIT = 4;


        public static void Check(string[] command, Connection connectionState)
        {
            if (connectionState.SelectedState && command.Length == CHECK_SPLIT)
            {
                // ******************************** TODO: UNFINISHED
                //***************************************************
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }

        }
        public static void Close(string[] command, Connection connectionState)
        {
            if (connectionState.SelectedState && command.Length == CLOSE_SPLIT)
            {

                if (connectionState.SelectedMailBox.EmailMessages.Count != 0)
                {
                    List<EmailMessage> emailsToDelete = new List<EmailMessage>();
                    foreach (EmailMessage em in connectionState.SelectedMailBox.EmailMessages)
                    {
                        if (em.Flags.TryGetValue(@"\Deleted", out var deleted))
                        {
                            if (deleted)
                            {

                                emailsToDelete.Add(em);                    
                            }
                        }
                    }
                    //deleting emails
                    foreach (var email in emailsToDelete)
                    {
                        connectionState.SelectedMailBox.EmailMessages.Remove(email);
                        foreach (var mail in connectionState.SelectedMailBox.EmailMessages)
                        {
                            //decrease message number for every email that have higher message number then deleted message's number
                            if (mail.MsgNum > email.MsgNum)
                            {
                                mail.MsgNum--;
                            }
                        }
                    }
                }
                

                connectionState.SelectedMailBox = null;
                connectionState.SelectedState = false;
                connectionState.SendToStream($"{command[0]} OK CLOSE completed");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        public static void Copy(string[] command, Connection connectionState)
        {
            if (connectionState.SelectedState && command.Length == COPY_SPLIT)
            {
                if (command.Length > 2)
                {
                    if (connectionState.SelectedMailBox.EmailMessages.Count > 0)
                    {
                        string[] emailsNumbers = command[2].Split(':');
                        List<int> numbers = new List<int>();
                        foreach (var number in emailsNumbers)
                        {
                            numbers.Add(Int32.Parse(number));
                        }
                        foreach (var email in connectionState.SelectedMailBox.EmailMessages)
                        {
                            foreach (var number in numbers)
                            {
                                if (number == email.MsgNum)
                                {
                                    int count = Server.mailBoxes[command[3]].EmailMessages.Count();
                                    Server.mailBoxes[command[3]].EmailMessages.Add(email.GetCopy(++count));      
                                }
                            }
                        }
                        connectionState.SendToStream($"{command[0]} OK - UID command completed");
                    }
                }
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }

        }
        public static void Expunge(string[] command, Connection connectionState)
        {
            if (connectionState.SelectedState && command.Length == EXPUNGE_SPLIT)
            {

                if (connectionState.SelectedMailBox.EmailMessages.Count != 0)
                {
                    List<EmailMessage> emailsToDelete = new List<EmailMessage>();
                    //check which emails to delete
                    foreach (EmailMessage em in connectionState.SelectedMailBox.EmailMessages)
                    {
                        em.Flags[Flags.DELETED] = true;
                        if (em.Flags.TryGetValue(@"\Deleted", out var deleted))
                        {
                            if (deleted)
                            {                               
                                emailsToDelete.Add(em);
                            }
                        }
                    }
                    //deleting emails
                    foreach (var email in emailsToDelete)
                    {
                        connectionState.SelectedMailBox.EmailMessages.Remove(email);
                        foreach(var mail in connectionState.SelectedMailBox.EmailMessages)
                        {
                            //decrease message number for every email that have higher message number then deleted message's number
                            if (mail.MsgNum > email.MsgNum)
                            {
                                mail.MsgNum--;
                            }
                        }
                        connectionState.SendToStream($"{command[0]} {email.MessageId} EXPUNGE");
                    }
                }
                connectionState.SendToStream($"{command[0]} OK EXPUNGE completed");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }



        }
        public static void Fetch(string command, Connection connectionState)
        {
            var tag = command.Split(' ').First();
            if (connectionState.SelectedState)
            {
                if(IMAP_Fetch.TryFetch(command, connectionState))
                {

                    connectionState.SendToStream($"{tag} OK FETCH completed");
                }
                else
                {
                    connectionState.SendToStream($"{tag} NO - fetch error: can’t fetch that data");
                }
            }
            else
            {
                connectionState.SendToStream($"{tag} BAD - command unknown or arguments invalid");
            }
        }
        /// <summary>
        /// Searches messages in mail box by specific criteria
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connectionState"></param>
        public static void Search(string[] command, Connection connectionState)
        {
            if (connectionState.SelectedState && command.Length > SEARCH_MINSPLIT)
            {
                var messages = IMAP_Search.Search(command.Skip(2).ToArray(), connectionState);
                if (messages.First() != -1)
                {
                    string response = "";
                    foreach (var message in messages)
                    {
                        response += $"{message} ";
                    }

                    connectionState.SendToStream($"* SEARCH {response}");
                    connectionState.SendToStream($"{command[0]} OK SEARCH completed");
                }
                else
                {
                    connectionState.SendToStream($"{command[0]} BAD - this command is not supported");
                }
                
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - no search criteria specified");
            }

        }
        public static void Store(string[] command, Connection connectionState)
        {

            if (connectionState.SelectedState && command.Length == STORE_SPLIT)
            {
                // ******************************** TODO: UNFINISHED
                //***************************************************
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        public static void UID(string[] command, Connection connectionState)
        {

            if (connectionState.SelectedState && command.Length == UID_SPLIT)
            {
                // ******************************** TODO: UNFINISHED
                //***************************************************
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
    }
}
