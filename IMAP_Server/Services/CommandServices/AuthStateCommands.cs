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

        /*UNFINISHED*/
        public static void Append(string[] command, Connection connectionState)
        {
            if ((command.Length >= APPEND_MIN_SPLIT && command.Length <= APPEND_MAX_SPLIT) && connectionState.Authentificated)
            {

            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        /*
         The CREATE command creates a mailbox with the given name. An OK
        response is returned only if a new mailbox with that name has been
        created. It is an error to attempt to create INBOX or a mailbox
        with a name that refers to an extant mailbox. Any error in
        creation will return a tagged NO response.
         */
        public static void Create(string[] command, Connection connectionState)
        {
            if (command.Length == CREATE_SPLIT && connectionState.Authentificated)
            {
                bool created = CreateRecursive(command[2], connectionState);
                if (created)
                {
                    connectionState.SendToStream($"{command[0]} OK CREATE Completed: {command[2]} Successfully created");
                }
                else
                {
                    connectionState.SendToStream($"{command[0]} NO CREATE: Mailbox already exists in that path");
                }
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        /*
         The DELETE command permanently removes the mailbox with the given
        name. A tagged OK response is returned only if the mailbox has
        been deleted. It is an error to attempt to delete INBOX or a
        mailbox name that does not exist.
         */
        public static void Delete(string[] command, Connection connectionState)
        {
            if (command.Length == DELETE_SPLIT && connectionState.Authentificated)
            {
                if (Server.mailBoxes.TryGetValue(command[2], out var mb))
                {
                    if (mb.AllowedUsers.Contains(connectionState.Username))
                    {
                        Server.mailBoxes.Remove(mb.Path);
                        connectionState.SendToStream($"{command[0]} OK DELETE Completed: {mb.mailboxName} Successfully removed");
                        return;
                    }
                    else
                    {
                        connectionState.SendToStream($"{command[0]} NO DELETE: Access denided for the username {connectionState.Username}!");
                        return;

                    }
                }
                connectionState.SendToStream($"{command[0]} NO DELETE: Mailbox was not found");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        /*
         The EXAMINE command is identical to SELECT and returns the same
        output; however, the selected mailbox is identified as read-only.
        No changes to the permanent state of the mailbox, including
        per-user state, are permitted; in particular, EXAMINE MUST NOT
        cause messages to lose the \Recent flag.
        The text of the tagged OK response to the EXAMINE command MUST
        begin with the "[READ-ONLY]" response code.
         */
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
        /*
         The LIST command returns a subset of names from the complete set
        of all names available to the client. Zero or more untagged LIST
        replies are returned, containing the name attributes, hierarchy
        delimiter, and name; see the description of the LIST reply for more detail.
         */
        /*DOES NOT WORK AS INTENDED*/
        public static void List(string[] command, Connection connectionState)
        {
            if (command.Length == LIST_SPLIT && connectionState.Authentificated)
            {
                //    if (command[2] == "\"\"" && command[3] == "\"\"")
                //    {
                //        foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                //        {
                //            connectionState.SendToStream($"* LIST () " + mb.Key);
                //        }
                //        connectionState.SendToStream($"{command[0]} OK LIST completed");
                //    }
                //    else if (command[2] == "\"\"" && command[3] != "\"\"")
                //    {
                //        foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                //        {
                //            if (mb.Key == command[3] || mb.Key.Contains(command[3])||mb.Value.mailboxName.Contains(command[3]))
                //            {
                //                connectionState.SendToStream($"* LIST () " + command[3]);
                //                connectionState.SendToStream($"{command[0]} OK LIST completed");
                //                break;
                //            }
                //            else
                //            {
                //                connectionState.SendToStream($"{command[0]} NO - list failure: can't list that reference or name111");
                //                break;
                //            }
                //        }
                //    }
                //    else if (command[2] != "\"\"" && (command[3] == "*" || command[3] == "%"))
                //    {
                //        bool found = false;
                //        foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                //        {
                //            if (mb.Key.Contains(command[2]))
                //            {
                //                found = true;
                //                string[] trimmed = mb.Key.Split("/");
                //                if (trimmed[0] == command[2])
                //                {
                //                    string repath = string.Join('/', trimmed);
                //                    connectionState.SendToStream($"* LIST () " + repath);
                //                }
                //                else
                //                {
                //                    int sindex = 0;
                //                    for (int i = 0; i < trimmed.Length; i++)
                //                    {
                //                        if (trimmed[i] == command[2])
                //                        {
                //                            sindex = i;
                //                            break;
                //                        }
                //                    }
                //                    if (sindex != 0)
                //                    {
                //                        if (command[3] == "*")
                //                            trimmed = trimmed.Where((item, index) => index >= sindex).ToArray();
                //                        else
                //                            trimmed = trimmed.Where((item, index) => (index >= sindex && index <= sindex + 1)).ToArray();
                //                        string repath = string.Join('/', trimmed);
                //                        connectionState.SendToStream($"* LIST () " + repath);
                //                    }
                //                }
                //            }
                //            //else
                //            //{
                //            //    connectionState.SendToStream($"{command[0]} NO - list failure: can't list that reference or name22222");
                //            //}
                //        }
                //        if (found)
                //            connectionState.SendToStream($"{command[0]} OK LIST completed");
                //        else
                //            connectionState.SendToStream($"{command[0]}NO - list failure: can't list that reference or name22222");
                //    }
                //    else
                //    {
                //        bool found = false, neighbours = false;
                //        foreach (KeyValuePair<string, Mailbox> mb in Server.mailBoxes)
                //        {
                //            if (mb.Key.Contains(command[2])&&mb.Key.Contains(command[3]))
                //            {
                //                found = true;
                //                string[] trimmed = mb.Key.Split("/");
                //                int index = 0, nextIndex = 0;
                //                for(int i=0;i<trimmed.Length;i++)
                //                {
                //                    if(trimmed[i]==command[2])
                //                    {
                //                        if(trimmed[i+1]==command[3])
                //                        {
                //                            neighbours = true;
                //                            index = i;
                //                            nextIndex = i + 1;
                //                            break;
                //                        }
                //                    }
                //                }
                //                if(index==nextIndex)
                //                {
                //                    connectionState.SendToStream($"{command[0]} NO - list failure: can't list that reference or name333");
                //                    break;
                //                }
                //                else
                //                {
                //                    string repath = string.Join('/', trimmed[index], trimmed[nextIndex]);
                //                    connectionState.SendToStream($"* LIST () " + repath);
                //                    break;
                //                }
                //            }
                //        }
                //        if(found&&neighbours)
                //        {
                //            connectionState.SendToStream($"{command[0]} OK LIST completed");
                //        }
                //    }
            }
            else
            {
                //Dummy message so the server could be accessed by firebird client. 
                //connectionState.SendToStream($"* LSUB () / jimoo");
                //connectionState.SendToStream($"* LSUB () / INBOX");
                //connectionState.SendToStream($"{command[0]} OK LIST completed");

                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        /*DOES NOT WORK PROPERLY*/
        /*
         The LSUB command returns a subset of names from the set of names
        that the user has declared as being "active" or "subscribed".
        Zero or more untagged LSUB replies are returned. The arguments to
        LSUB are in the same form as those for LIST.
         */
        public static void Lsub(string[] command, Connection connectionState)
        {
            if (command.Length == LSUB_SPLIT && connectionState.Authentificated)
            {

                foreach (Mailbox mb in Server.subscriberMailboxes)
                {
                    connectionState.SendToStream($"* LIST () " + mb.mailboxName);
                }
                connectionState.SendToStream($"{command[0]} OK LIST completed");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        /*
         The RENAME command changes the name of a mailbox. A tagged OK
        response is returned only if the mailbox has been renamed. It is
        an error to attempt to rename from a mailbox name that does not
        exist or to a mailbox name that already exists. Any error in
        renaming will return a tagged NO response.
         */
        public static void Rename(string[] command, Connection connectionState)
        {
            if (command.Length == RENAME_SPLIT && connectionState.Authentificated)
            {
                if (Server.mailBoxes.TryGetValue(command[3], out _))
                {
                    connectionState.SendToStream($"{command[0]} NO - Folder already exists");
                    return;
                }
                if (Server.mailBoxes.TryGetValue(command[2], out var mailbox))
                {
                    if (RenameRecursive(command[2], command[3], connectionState))
                    {
                        connectionState.SendToStream($"{command[0]} OK - rename completed");
                    }
                    else
                    {
                        connectionState.SendToStream($"{command[0]} NO - Renaming Failed");
                    }
                }
                else
                {
                    connectionState.SendToStream($"{command[0]} NO - Source folder doesn't exists");
                }

            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        /*
         RENAMES NAMES OF ALL SUBFOLDERS IN FOLDER
         */
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
        /*
         The SELECT command selects a mailbox so that messages in the
        mailbox can be accessed. Before returning an OK to the client,
        the server MUST send the following untagged data to the client.
        Note that earlier versions of this protocol only required the
        FLAGS, EXISTS, and RECENT untagged data; consequently, client
        implementations SHOULD implement default behavior for missing data
        as discussed with the individual item.
         */
        public async static void Select(string[] command, Connection connectionState)
        {
            command[2] = command[2].Replace("\"", "");
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
        /*
         The STATUS command requests the status of the indicated mailbox.
        It does not change the currently selected mailbox, nor does it
        affect the state of any messages in the queried mailbox (in
        particular, STATUS MUST NOT cause messages to lose the \Recent
        flag).
         */
        public static void Status(string[] command, Connection connectionState)
        {
            if (command.Length >= STATUS_SPLIT && connectionState.Authentificated)
            {
                string statusToShow = IMAP_Status.GetStatus(command, connectionState);
                if (statusToShow == "BAD")
                {
                    connectionState.SendToStream($"{command[0]} NO - status failure: no status for that name");
                    return;
                }
                else if (statusToShow == "UNKNOWN_PARAMS")
                {
                    connectionState.SendToStream($"{command[0]} NO - status failure: unknown parameters");
                    return;
                }
                connectionState.SendToStream($"* STATUS {command[2]} ({statusToShow})");
                connectionState.SendToStream($"{command[0]} OK - STATUS completed.");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        /*
         The SUBSCRIBE command adds the specified mailbox name to the
        server’s set of "active" or "subscribed" mailboxes as returned by
        the LSUB command. This command returns a tagged OK response only
        if the subscription is successful.
        A server MAY validate the mailbox argument to SUBSCRIBE to verify
        that it exists. However, it MUST NOT unilaterally remove an
        existing mailbox name from the subscription list even if a mailbox
        by that name no longer exists.
         */
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
        /*
         The UNSUBSCRIBE command removes the specified mailbox name from
        the server’s set of "active" or "subscribed" mailboxes as returned
        by the LSUB command. This command returns a tagged OK response
        only if the unsubscription is successful.
         */
        public static void Unsubscribe(string[] command, Connection connectionState)
        {
            if (command.Length == UNSUBSCRIBE_SPLIT && connectionState.Authentificated)
            {
                if (Server.mailBoxes.TryGetValue(command[2], out var mb))
                {
                    if (Server.subscriberMailboxes.Remove(mb))
                        connectionState.SendToStream($"{command[0]} OK - {mb.Path} unsubscribed");
                    else
                        connectionState.SendToStream($"{command[0]} NO - Folder not found");
                    return;
                }
                connectionState.SendToStream($"{command[0]} NO - Folder not found");
            }
            else
            {
                connectionState.SendToStream($"{command[0]} BAD - command unknown or arguments invalid");
            }
        }
        /*CREATES FOLDERS HIERARCHICALLY*/
        private static bool CreateRecursive(string path, Connection connectionState)
        {
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
            Server.mailBoxes.Add(mailbox.Path, mailbox);

            return true;
        }


    }
}
