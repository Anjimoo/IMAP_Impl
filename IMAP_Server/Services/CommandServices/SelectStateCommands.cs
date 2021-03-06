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

        /* UNFINISHED */
        public static void Check(string[] command, Connection connectionState)
        {
            if (connectionState.SelectedState && command.Length == CHECK_SPLIT)
            {
                  
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }

        }
        /*
         * The CLOSE command permanently removes all messages that have the
         * \Deleted flag set from the currently selected mailbox, and returns
         * to the authenticated state from the selected state. No untagged
         * EXPUNGE responses are sent.
         * No messages are removed, and no error is given, if the mailbox is
         * selected by an EXAMINE command or is otherwise selected read-only.
         * Even if a mailbox is selected, a SELECT, EXAMINE, or LOGOUT
         * command MAY be issued without previously issuing a CLOSE command.
         * The SELECT, EXAMINE, and LOGOUT commands implicitly close the
         * currently selected mailbox without doing an expunge. However,
         * when many messages are deleted, a CLOSE-LOGOUT or CLOSE-SELECT
         * sequence is considerably faster than an EXPUNGE-LOGOUT or
         * EXPUNGE-SELECT because no untagged EXPUNGE responses (which the
         * client would probably ignore) are sent.
         */
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
        /*
        The COPY command copies the specified message(s) to the end of the
        specified destination mailbox. The flags and internal date of the
        message(s) SHOULD be preserved, and the Recent flag SHOULD be set,
        in the copy.
        If the destination mailbox does not exist, a server SHOULD return
        an error. It SHOULD NOT automatically create the mailbox. Unless
        it is certain that the destination mailbox can not be created, the
        server MUST send the response code "[TRYCREATE]" as the prefix of
        the text of the tagged NO response. This gives a hint to the
        the CREATE is successful.
        If the COPY command is unsuccessful for any reason, server
        implementations MUST restore the destination mailbox to its state
        before the COPY attempt.
         */
        public static void Copy(string[] command, Connection connectionState)
        {
            if (connectionState.SelectedState && command.Length == COPY_SPLIT)
            {
                if(!Server.mailBoxes.TryGetValue(command[3], out var mailBox))
                {
                    connectionState.SendToStream($"{command[0]} NO - copy error: can’t copy those messages or to that name");
                    return;
                }
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
                        if(connectionState.SelectedMailBox.EmailMessages.Count < numbers.Last())
                        { 
                            connectionState.SendToStream($"{command[0]} NO - copy error: can’t copy those messages or to that name");
                            return;
                        }
                        foreach (var email in connectionState.SelectedMailBox.EmailMessages)
                        {
                            
                            foreach (var number in numbers)
                            {
                                if (number == email.MsgNum)
                                {
                                    int count = Server.mailBoxes[command[3]].EmailMessages.Count();
                                    Server.mailBoxes[command[3]].EmailMessages.Add(email.GetCopy(++count));
                                    break;
                                }
                            }
                        }
                        connectionState.SendToStream($"{command[0]} OK - copy completed");
                    }
                    
                }
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }

        }
        /*
         The EXPUNGE command permanently removes all messages that have the
        \Deleted flag set from the currently selected mailbox. Before
        returning an OK to the client, an untagged EXPUNGE response is
        sent for each message that is removed.
         */
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
        /*
         The FETCH command retrieves data associated with a message in the
        mailbox. The data items to be fetched can be either a single atom
        or a parenthesized list.
        Most data items, identified in the formal syntax under the
        msg-att-static rule, are static and MUST NOT change for any
        particular message. Other data items, identified in the formal
        syntax under the msg-att-dynamic rule, MAY change, either as a
        result of a STORE command or due to external events.
        For example, if a client receives an ENVELOPE for a
        message when it already knows the envelope, it can
        safely ignore the newly transmitted envelope.
        There are three macros which specify commonly-used sets of data
        items, and can be used instead of data items. A macro must be
        used by itself, and not in conjunction with other macros or data
        items.
         */
        public static void Fetch(string command, Connection connectionState)
        {
            var tag = command.Split(' ').First();
            if (connectionState.SelectedState)
            {
                if(IMAP_Fetch.TryFetch(command, connectionState, false))
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
        /*
         The SEARCH command searches the mailbox for messages that match
        the given searching criteria. Searching criteria consist of one
        or more search keys. The untagged SEARCH response from the server
        contains a listing of message sequence numbers corresponding to
        those messages that match the searching criteria.
         */
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
                    connectionState.SendToStream($"{command[0]} NO - search error: can’t search that [CHARSET] or criteria");
                }
                
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - no search criteria specified");
            }

        }
        /*UNFINISHED*/
        public static void Store(string[] command, Connection connectionState)
        {

            if (connectionState.SelectedState && command.Length == STORE_SPLIT)
            {
                
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        /*
         The UID command has two forms. In the first form, it takes as its
        arguments a COPY, FETCH, or STORE command with arguments
        appropriate for the associated command. However, the numbers in
        the sequence set argument are unique identifiers instead of
        message sequence numbers. Sequence set ranges are permitted, but
        there is no guarantee that unique identifiers will be contiguous...

        In the second form, the UID command takes a SEARCH command with
        SEARCH command arguments. The interpretation of the arguments is
        the same as with SEARCH; however, the numbers returned in a SEARCH
        response for a UID SEARCH command are unique identifiers instead of message sequence numbers.
         */
        public static void UID(string command, Connection connectionState)
        {
            var tag = command.Split(' ').First();
            if (connectionState.SelectedState )
            {
                if (command.ToLower().Contains("fetch"))
                {
                    if(IMAP_Fetch.TryFetch(command, connectionState, true))
                    {
                        connectionState.SendToStream($"{tag} OK UID FETCH completed");
                    }
                }else if (command.ToLower().Contains("search"))
                {
                    var commandParams = IMAP_Fetch.ParseFetchCommand(command);
                    var messagesNums = IMAP_Search.Search(commandParams.Skip(1).ToArray(), connectionState);
                    if (messagesNums.First() != -1)
                    {

                        string response = "";
                        foreach (var messageNum in messagesNums)
                        {
                            foreach(var message in connectionState.SelectedMailBox.EmailMessages)
                            {
                                if(message.MsgNum == messageNum)
                                {
                                    response += $"{message.MessageId} ";
                                    break;
                                }
                            }
                            
                        }

                        connectionState.SendToStream($"* SEARCH {response}");
                        connectionState.SendToStream($"{command[0]} OK UID completed");
                    }
                    else
                    {
                        connectionState.SendToStream($"{command[0]} BAD - this command is not supported");
                    }
                }
            }
            else
            {
                connectionState.SendToStream($"{tag} BAD - command unknown or arguments invalid");
            }
        }
    }
}
