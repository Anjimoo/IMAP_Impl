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
        private const int CREATE_SPLIT = 3;
        private const int DELETE_SPLIT = 3;
        private const int SELECT_SPLIT = 3;
        private const int EXAMINE_SPLIT = 3;
        private const int RENAME_SPLIT = 4;
        private const int SUBSCRIBE_SPLIT = 3;
        private const int UNSUBSCRIBE_SPLIT = 3;

        //TODO - Look for CATENATE extension
        public static void Append(string[] command, Connection connectionState)
        {

        }

        public static void Create(string[] command, Connection connectionState)
        {
            if (command.Length == CREATE_SPLIT)
            {
                foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                {
                    if (mb.Value.mailboxName == command[2])
                    {
                        connectionState.SendToStream($"NO CREATE: Mailbox already exists in that name");
                        return;
                    }
                }
                Mailbox mailbox;
                if (command[2].Contains('/'))
                {
                    string[] hierarchy = command[2].Split('/');
                    for (int i = 0; i < hierarchy.Length; i++)
                    {
                        if (Server.mailBoxes.TryGetValue(hierarchy[i], out var parentMailbox))
                        {
                            mailbox = new Mailbox(parentMailbox);
                            mailbox.mailboxName = parentMailbox.mailboxName + "/" + hierarchy[i + 1];
                            Server.mailBoxes.Add(mailbox.mailboxName, mailbox);
                            connectionState.SendToStream($"OK CREATE Completed: {mailbox.mailboxName} Successfully created");
                            return;
                        }
                        else
                        {
                            mailbox = new Mailbox(new Mailbox() { mailboxName = hierarchy[i] });
                            mailbox.mailboxName = parentMailbox.mailboxName + "/" + hierarchy[i + 1];
                            Server.mailBoxes.Add(mailbox.mailboxName, mailbox);
                            connectionState.SendToStream($"OK CREATE Completed: {mailbox.mailboxName} Successfully created");
                            return;
                        }
                    }
                }
                else
                {
                    mailbox = new Mailbox();
                    mailbox.mailboxName = command[2];
                    mailbox.mailboxSize = 50000;
                    mailbox.AllowedUsers.Add(connectionState.Username);
                    Server.mailBoxes.Add(mailbox.mailboxName, mailbox);
                    connectionState.SendToStream($"OK CREATE Completed: {mailbox.mailboxName} Successfully created");
                }
            }
            else
            {
                connectionState.SendToStream(command[0] + "  BAD - command unknown or arguments invalid");
            }
        }

        public static void Delete(string[] command, Connection connectionState)
        {
            if (command.Length == DELETE_SPLIT)
            {
                foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                {
                    if (mb.Value.AllowedUsers.Contains(connectionState.Username))
                    {
                        if (mb.Value.mailboxName == command[2])
                        {
                            Server.mailBoxes.Remove(mb.Value.mailboxName);
                            connectionState.SendToStream($"OK DELETE Completed: {mb.Value.mailboxName} Successfully removed");
                            return;
                        }
                    }
                    else
                    {
                        connectionState.SendToStream($"NO DELETE: Access denided for the username {connectionState.Username}!");
                        return;
                    }
                }
                connectionState.SendToStream($"NO DELETE: Mailbox was not found");
            }
            else
            {
                connectionState.SendToStream(command[0] + "  BAD - command unknown or arguments invalid");
            }
        }

        public static void Examine(string[] command, Connection connectionState)
        {
            if (command.Length == EXAMINE_SPLIT) //check if command is legal
            {

                if (Server.mailBoxes.TryGetValue(command[2], out var mailbox)) //check if chosen mailbox is present
                {
                    connectionState.SelectedState = true;
                    connectionState.SendToStream($"* {mailbox.EmailMessages.Count} EXISTS");
                    int c = 0;
                    foreach (EmailMessage em in mailbox.EmailMessages)
                    {
                        if (em.Flags.TryGetValue(@"\Recent", out var recent))
                            if (recent)
                                c++;//Examine do the same as SELECT except that it does not lower Recent flags.
                    }
                    connectionState.SendToStream($"* {c} RECENT");
                }
                else
                {
                    connectionState.SendToStream($"{command[0]} NO - EXAMINE failure, now in authenticated state: no such mailbox, can’t access mailbox");
                }
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }

        }

        //TODO - Requires a param named "reference name" and I have no clue what exactly is it.
        public static void List(string[] command, Connection connectionState)
        {

        }

        //TODO - Requires a param named "reference name" and I have no clue what exactly is it.
        public static void Lsub(string[] command, Connection connectionState)
        {

        }

        public static void Rename(string[] command, Connection connectionState)
        {
            if (command.Length == RENAME_SPLIT)
            {
                if (Server.mailBoxes.TryGetValue(command[2], out var mailbox))
                {
                    mailbox.mailboxName = command[3];
                    Server.mailBoxes.Remove(command[2]);
                    Server.mailBoxes.Add(command[3], mailbox);
                    connectionState.SendToStream(command[0] + "OK - rename completed");
                }
            }
            else
            {
                connectionState.SendToStream(command[0] + "  BAD - command unknown or arguments invalid");
            }
        }

        public static void Select(string[] command, Connection connectionState)
        {
            if (command.Length == SELECT_SPLIT) //check if command is legal
            {

                if (Server.mailBoxes.TryGetValue(command[2], out var mailbox)) //check if chosen mailbox is present
                {
                    connectionState.SelectedState = true;
                    mailbox.uniqueIDValidityVal++;
                    connectionState.SendToStream($"* {mailbox.EmailMessages.Count} EXISTS");
                    int c = 0;
                    foreach (EmailMessage em in mailbox.EmailMessages)
                    {
                        if (em.Flags.TryGetValue(@"\Recent", out var recent))
                        {
                            if (recent)
                            {
                                c++;
                                em.LowerFlag(@"\Recent");
                            }
                        }
                    }
                    connectionState.SendToStream($"* {c} RECENT READED");
                }
                else
                {
                    connectionState.SendToStream(
                        $"{command[0]} NO - select failure, now in authenticated state: no such mailbox, can’t access mailbox");
                }
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }

        //TODO - Requires more than 2 params, just like Search but more lightweight
        public static void Status(string[] command, Connection connectionState)
        {

        }

        public static void Subscribe(string[] command, Connection connectionState)
        {
            if (command.Length == SUBSCRIBE_SPLIT)
            {
                foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                {
                    if (mb.Key == command[2])
                    {
                        if(Server.subscriberMailboxes.Add(mb.Value))
                            connectionState.SendToStream($"{command[0]} OK - {mb.Key} subscribed");
                        else
                            connectionState.SendToStream($"{command[0]} NO - {mb.Key} already subscribed");
                        return;
                    }
                }
                connectionState.SendToStream($"{command[0]} NO - Folder not found");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }

        public static void Unsubscribe(string[] command, Connection connectionState)
        {
            if (command.Length == SUBSCRIBE_SPLIT)
            {
                foreach (Mailbox mb in Server.subscriberMailboxes)
                {
                    if (mb.mailboxName == command[2])
                    {
                        Server.subscriberMailboxes.Remove(mb);
                        connectionState.SendToStream($"{command[0]} OK - {mb.mailboxName} unsubscribed");
                        return;
                    }
                }
                connectionState.SendToStream($"{command[0]} NO - Folder not found");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
    }
}
