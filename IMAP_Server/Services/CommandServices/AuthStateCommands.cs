using IMAP.Shared;
using IMAP_Server.Services;
using IMAP_Server.Models;
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
        private const int APPEND_MIN_SPLIT = 4;
        private const int APPEND_MAX_SPLIT = 6;
        private const int LIST_SPLIT = 4;
        private const int LSUB_SPLIT = 4;
        private const int STATUS_SPLIT = 4;

        //TODO - Look for CATENATE extension
        public static void Append(string[] command, Connection connectionState)
        {
            if ((command.Length >= APPEND_MIN_SPLIT && command.Length <= APPEND_MAX_SPLIT) && connectionState.Authentificated)
            {
                // ******************************** TODO: UNFINISHED
                //***************************************************
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }

        public static void Create(string[] command, Connection connectionState)
        {
            if (command.Length == CREATE_SPLIT && connectionState.Authentificated)
            {
                bool created = CreateRecursive(command[2], connectionState);
                if (created)
                {
                    connectionState.SendToStream(command[0] + $"OK CREATE Completed: {command[2]} Successfully created");
                }
                else
                {
                    connectionState.SendToStream(command[0] + $"NO CREATE: Mailbox already exists in that path");
                }
                //foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                //{
                //    if (mb.Value.mailboxName == command[2])
                //    {
                //        connectionState.SendToStream(command[0] + $"NO CREATE: Mailbox already exists in that name");
                //        return;
                //    }
                //}
                //Mailbox mailbox;
                //mailbox = new Mailbox();
                //if (command[2].Contains('/'))
                //{
                //    string[] hierarchy = command[2].Split('/');
                //    mailbox.mailboxName = hierarchy[hierarchy.Length - 1];
                //    mailbox.Path = string.Join('/', mailbox.mailboxName);
                //    Array.Resize(ref hierarchy, hierarchy.Length - 1);
                //    string father = string.Join('/', hierarchy);
                //    command[2] = father;
                //    Create(command, connectionState);
                //}
                //else
                //{
                //    mailbox.mailboxName = command[2];
                //}
                //mailbox.mailboxSize = 50000;
                //mailbox.AllowedUsers.Add(connectionState.Username);
                //Server.mailBoxes.Add(mailbox.mailboxName, mailbox);
                //connectionState.SendToStream(command[0] + $"OK CREATE Completed: {mailbox.mailboxName} Successfully created");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }

        public static void Delete(string[] command, Connection connectionState)
        {
            if (command.Length == DELETE_SPLIT && connectionState.Authentificated)
            {
                //foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                //{
                    if (Server.mailBoxes.TryGetValue(command[2], out var mb))
                    {
                        if (mb.AllowedUsers.Contains(connectionState.Username))
                        {
                            Server.mailBoxes.Remove(mb.Path);
                            connectionState.SendToStream(command[0] + $"OK DELETE Completed: {mb.mailboxName} Successfully removed");
                            return;
                        }
                        else
                        {
                            connectionState.SendToStream(command[0] + $"NO DELETE: Access denided for the username {connectionState.Username}!");
                            return;

                        }
                    }
                //}
                connectionState.SendToStream($"NO DELETE: Mailbox was not found");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }

        public async static void Examine(string[] command, Connection connectionState)
        {
            if (command.Length == EXAMINE_SPLIT && connectionState.Authentificated) //check if command is legal
            {

                if (Server.mailBoxes.TryGetValue(command[2], out var mailbox)) //check if chosen mailbox is present
                {
                    connectionState.SendToStream($"* {mailbox.EmailMessages.Count} EXISTS");
                    int c = 0;
                    foreach (EmailMessage em in mailbox.EmailMessages)
                    {
                        if (em.Flags.TryGetValue(@"\Recent", out var recent))
                            if (recent)
                                c++;//Examine do the same as SELECT except that it does not lower Recent flags.
                    }
                    connectionState.SendToStream($"* {c} RECENT");
                    if (mailbox.EmailMessages.Count == 0)
                    {
                        connectionState.SendToStream($"* OK No message is first unseen");
                    }
                    else
                    {
                        connectionState.SendToStream($"* OK [UNSEEN {mailbox.EmailMessages.Last().MessageId}] is first unseen");
                    }
                    connectionState.SendToStream($"* OK {mailbox.uniqueIDValidityVal} UIDs valid");
                    connectionState.SendToStream($"* OK {mailbox.nextUniqueIDVal} Predicted next UID");
                    string permflaglist = "";
                    foreach (var flag in PermanentFlags.PermaFlags)
                    {
                        permflaglist += " " + flag;
                    }
                    connectionState.SendToStream($"* OK [PERMFLAGS {permflaglist}]");
                    string mailboxFlagList = "";
                    foreach (var flag in mailbox.supportedFlags)
                    {
                        mailboxFlagList += " " + flag;
                    }
                    connectionState.SendToStream($"* FLAGS {mailboxFlagList}");
                    await Task.Delay(500);
                    connectionState.SendToStream($"{command[0]} OK [READ-ONLY] EXAMINE completed");
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
            if (command.Length == LIST_SPLIT && connectionState.Authentificated)
            {
                // ******************************** TODO: UNFINISHED
                //***************************************************
                if (command[2] == "" && command[3] == "")
                {
                    foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                    {
                        connectionState.SendToStream($"* LIST () " + mb.Key);
                    }
                    connectionState.SendToStream($"{command[0]} OK LIST completed");
                }
                else if (command[2] == "" && command[3] != "")
                {
                    foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                    {
                        if (mb.Key == command[3] || mb.Key.Contains(command[3])||mb.Value.mailboxName.Contains(command[3]))
                        {
                            connectionState.SendToStream($"* LIST () " + command[3]);
                            connectionState.SendToStream($"{command[0]} OK LIST completed");
                            break;
                        }
                        else
                        {
                            connectionState.SendToStream($"{command[0]} NO - list failure: can't list that reference or name111");
                            break;
                        }
                    }
                }
                else if (command[2] != "" && (command[3] == "*" || command[3] == "%"))
                {
                    bool found = false;
                    foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                    {
                        if (mb.Key.Contains(command[2]))
                        {
                            found = true;
                            string[] trimmed = mb.Key.Split("/");
                            if (trimmed[0] == command[2])
                            {
                                string repath = string.Join('/', trimmed);
                                connectionState.SendToStream($"* LIST () " + repath);
                            }
                            else
                            {
                                int sindex = 0;
                                for (int i = 0; i < trimmed.Length; i++)
                                {
                                    if (trimmed[i] == command[2])
                                    {
                                        sindex = i;
                                        break;
                                    }
                                }
                                if (sindex != 0)
                                {
                                    if (command[3] == "*")
                                        trimmed = trimmed.Where((item, index) => index >= sindex).ToArray();
                                    else
                                        trimmed = trimmed.Where((item, index) => (index >= sindex && index <= sindex + 1)).ToArray();
                                    string repath = string.Join('/', trimmed);
                                    connectionState.SendToStream($"* LIST () " + repath);
                                }
                            }
                        }
                        //else
                        //{
                        //    connectionState.SendToStream($"{command[0]} NO - list failure: can't list that reference or name22222");
                        //}
                    }
                    if (found)
                        connectionState.SendToStream($"{command[0]} OK LIST completed");
                    else
                        connectionState.SendToStream($"{command[0]}NO - list failure: can't list that reference or name22222");
                }
                else
                {
                    bool found = false, neighbours = false;
                    foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                    {
                        if (mb.Key.Contains(command[2])&&mb.Key.Contains(command[3]))
                        {
                            found = true;
                            string[] trimmed = mb.Key.Split("/");
                            int index = 0, nextIndex = 0;
                            for(int i=0;i<trimmed.Length;i++)
                            {
                                if(trimmed[i]==command[2])
                                {
                                    if(trimmed[i+1]==command[3])
                                    {
                                        neighbours = true;
                                        index = i;
                                        nextIndex = i + 1;
                                        break;
                                    }
                                }
                            }
                            if(index==nextIndex)
                            {
                                connectionState.SendToStream($"{command[0]} NO - list failure: can't list that reference or name333");
                                break;
                            }
                            else
                            {
                                string repath = string.Join('/', trimmed[index], trimmed[nextIndex]);
                                connectionState.SendToStream($"* LIST () " + repath);
                                break;
                            }
                        }
                    }
                    if(found&&neighbours)
                    {
                        connectionState.SendToStream($"{command[0]} OK LIST completed");
                    }
                }
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }

        //TODO - Requires a param named "reference name" and I have no clue what exactly is it.
        public static void Lsub(string[] command, Connection connectionState)
        {
            if (command.Length == LSUB_SPLIT && connectionState.Authentificated)
            {
                // ******************************** TODO: UNFINISHED
                //***************************************************
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }

        public static void Rename(string[] command, Connection connectionState)
        {
            if (command.Length == RENAME_SPLIT && connectionState.Authentificated)
            {
                if (Server.mailBoxes.TryGetValue(command[3], out _))
                {
                    connectionState.SendToStream(command[0] + "NO - Folder already exists");
                    return;
                }
                if (Server.mailBoxes.TryGetValue(command[2], out var mailbox))
                {
                    if(RenameRecursive(command[2], command[3], connectionState))
                    {
                        connectionState.SendToStream(command[0] + "OK - rename completed");
                    }
                    else
                    {
                        connectionState.SendToStream(command[0] + "NO - Renaming Failed");
                    }
                    //    mailbox.Path = command[3];
                    //    if(!command[3].Contains('/'))
                    //    {
                    //        mailbox.mailboxName = command[3];
                    //    }
                    //    Server.mailBoxes.Remove(command[2]);
                    //    Server.mailBoxes.Add(command[3], mailbox);
                    //    connectionState.SendToStream(command[0] + "OK - rename completed");
                }
                else
                {
                    connectionState.SendToStream(command[0] + "NO - Folder already exists");
                }

            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        private static bool RenameRecursive(string oldName, string newName, Connection connectionState)
        {
            List<string> toRename = new List<string>();
            List<string> changedNames = new List<string>();
            try
            {
                foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                {
                    if (mb.Key.StartsWith(oldName))
                    {
                        toRename.Add(mb.Key);
                    }
                }
                foreach (var name in toRename)
                {
                    changedNames.Add(name.Replace(oldName, newName));
                }
                for (int i = 0; i < changedNames.Count; i++)
                {
                    if (Server.mailBoxes.TryGetValue(toRename[i], out var mailbox))
                    {
                        Server.mailBoxes.Remove(oldName);
                        mailbox.Path = changedNames[i];
                        if (changedNames[i].Contains('/'))
                        {
                            mailbox.mailboxName = changedNames[i].Split('/')[changedNames[i].Split().Length - 1];
                            string[] newNameSplitted = newName.Split('/');
                            Array.Resize(ref newNameSplitted, newNameSplitted.Length - 1);

                            CreateRecursive(string.Join('/', newNameSplitted), connectionState);
                        }
                        else
                        {
                            mailbox.mailboxName = changedNames[i];
                        }
                        Server.mailBoxes.Add(changedNames[i], mailbox);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async static void Select(string[] command, Connection connectionState)
        {
            if (command.Length == SELECT_SPLIT && connectionState.Authentificated) //check if command is legal
            {

                if (Server.mailBoxes.TryGetValue(command[2], out var mailbox)) //check if chosen mailbox is present
                {
                    connectionState.SelectedState = true;
                    connectionState.SelectedMailBox = mailbox;
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
                    connectionState.SendToStream($"* {c} RECENT");

                    if (mailbox.EmailMessages.Count == 0)
                    {
                        connectionState.SendToStream($"* OK No message is first unseen");
                    }
                    else
                    {
                        connectionState.SendToStream($"* OK [UNSEEN {mailbox.EmailMessages.Last().MessageId}] is first unseen");
                    }
                    connectionState.SendToStream($"* OK {mailbox.uniqueIDValidityVal} UIDs valid");
                    connectionState.SendToStream($"* OK {mailbox.nextUniqueIDVal} Predicted next UID");
                    string permflaglist = "";
                    foreach (var flag in PermanentFlags.PermaFlags)
                    {
                        permflaglist += " " + flag;
                    }
                    connectionState.SendToStream($"* OK [PERMFLAGS {permflaglist}]");
                    string mailboxFlagList = "";
                    foreach (var flag in mailbox.supportedFlags)
                    {
                        mailboxFlagList += " " + flag;
                    }
                    connectionState.SendToStream($"* FLAGS {mailboxFlagList}");
                    //await Task.Delay(500);
                    connectionState.SendToStream($"{command[0]} OK [READ-WRITE] SELECT completed");
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

        public static void Status(string[] command, Connection connectionState)
        {
            if (command.Length >= STATUS_SPLIT && connectionState.Authentificated)
            {
                string statusToShow = IMAP_Status.GetStatus(command, connectionState);
                if(statusToShow == "BAD")
                {
                    connectionState.SendToStream($"{command[0]} NO - status failure: no status for that name");
                    return;
                }
                else if(statusToShow == "UNKNOWN_PARAMS")
                {
                    connectionState.SendToStream($"{command[0]} NO - status failure: unknown parameters");
                    return;
                }
                connectionState.SendToStream($"* STATUS {command[2]} ({statusToShow})");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }

        public static void Subscribe(string[] command, Connection connectionState)
        {
            if (command.Length == SUBSCRIBE_SPLIT && connectionState.Authentificated)
            {
                    if (Server.mailBoxes.TryGetValue(command[2], out var mb))
                    {
                        if (Server.subscriberMailboxes.Add(mb))
                            connectionState.SendToStream($"{command[0]} OK - {mb.Path} subscribed");
                        else
                            connectionState.SendToStream($"{command[0]} NO - {mb.Path} already subscribed");
                        return;
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
            if (command.Length == UNSUBSCRIBE_SPLIT && connectionState.Authentificated)
            {
                //foreach (Mailbox mb in Server.subscriberMailboxes)
                //{
                    if (Server.mailBoxes.TryGetValue(command[2], out var mb))
                    {
                        Server.subscriberMailboxes.Remove(mb);
                        connectionState.SendToStream($"{command[0]} OK - {mb.Path} unsubscribed");
                        return;
                    }
                //}
                connectionState.SendToStream($"{command[0]} NO - Folder not found");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }

        private static bool CreateRecursive(string path, Connection connectionState)
        {

            //foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
            //{
            //    if (mb.Value.Path == path)
            //    {
            //        return false;
            //    }
            //}
            if (Server.mailBoxes.TryGetValue(path, out _))
            {
                return false;
            }
            Mailbox mailbox = new Mailbox();
            if (path.Contains('/'))
            {
                string[] hierarchy = path.Split('/');
                mailbox.mailboxName = hierarchy[hierarchy.Length - 1];
                mailbox.Path = path;
                Array.Resize(ref hierarchy, hierarchy.Length - 1);
                string father = string.Join('/', hierarchy);
                CreateRecursive(father, connectionState);
            }

            else
            {
                mailbox.mailboxName = path;
                mailbox.Path = path;
            }
            mailbox.mailboxSize = 50000;
            mailbox.AllowedUsers.Add(connectionState.Username);
            Server.mailBoxes.Add(mailbox.Path, mailbox);//works with mailboxPath but it breaks the rest of the commands (they need to refer to the full path)

            return true;
        }

        
    }
}
