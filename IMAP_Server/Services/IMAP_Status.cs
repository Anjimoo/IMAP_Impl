using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMAP.Shared;
using IMAP_Server.Models;

namespace IMAP_Server.Services
{
    public static class IMAP_Status
    {
        //public const string MESSAGES = "MESSAGES";
        //public const string RECENT = "RECENT";
        //public const string UIDNEXT = "UIDNEXT";
        //public const string UIDVALIDITY = "UIDVALIDITY";
        //public const string UNSEEN = "UNSEEN";

        public static string GetStatus(string[] commands, Connection connectionState)
        {
            string statusForMailbox = "";

            if(Server.mailBoxes.TryGetValue(commands[2], out var mailbox))
            {
                string[] commandArray = commands.Where((item, index) => (index != 0&&index!=1&&index!=2)).ToArray();
                for (int i=0; i<commandArray.Length;i++)
                {
                    if(Enum.TryParse<StatusCommands>(commandArray[i], false, out var command))
                    switch(command)
                    {
                        case StatusCommands.MESSAGES:
                            statusForMailbox += "MESSAGES "+mailbox.EmailMessages.Count+" ";
                            break;
                        case StatusCommands.RECENT:
                            statusForMailbox += "RECENT " + mailbox.GetAllRecents() + " ";
                            break;
                        case StatusCommands.UIDNEXT:
                            statusForMailbox += "UIDNEXT " + mailbox.nextUniqueIDVal + " ";
                            break;
                        case StatusCommands.UIDVALIDITY:
                            statusForMailbox += "UIDVALID " + mailbox.uniqueIDValidityVal + " ";
                            break;
                        case StatusCommands.UNSEEN:
                            statusForMailbox += "UNSEEN " + mailbox.GetAllUnseen() + " ";
                            break;
                        default:
                            return "UNKNOWN_PARAMS";
                    }
                }
            }
            else
            {
                return "BAD";
            }
            return statusForMailbox;
        }
    }
}
