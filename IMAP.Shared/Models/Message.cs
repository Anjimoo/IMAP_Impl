using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP.Shared
{
    public enum ArgumentType{
        USERNAME,
        PASSWORD,
        MAILBOX,
        OLDMAILBOX,
        NEWMAILBOX,
        REFNAME,
        MAILBOXWILDCARDS,
        STATUSDATANAMES,
        FLAGLIST,
        DATETIME,
        SEARCHCRIT,
        SEQUENCE,
        DATAORMACRO,
        DATANAME,
        VALUEDATA,
        COMMANDNAME,
        COMMANDARGS
    }
    public class Message
    {
        public string Command { get; set; }
        public string Tag { get; set; }
        public Dictionary<ArgumentType, string> Arguments { get; set; }
        public string Response { get; set; }

        public void ParseMessage(string _message)
        {
            string[] tempMessage = _message.Split(' ');
            Arguments = new Dictionary<ArgumentType, string>();
     
            Command = tempMessage[1];
            Tag = tempMessage[0];

            switch (Command)
            {
                case "CONNECT":
                    Response = $"{Tag} OK greetings";
                    break;
                case "LOGIN":
                    Arguments.Add(ArgumentType.USERNAME, tempMessage[2]);
                    Arguments.Add(ArgumentType.PASSWORD, tempMessage[3]);

                    break;
                case "LOGOUT":
                    Response = $"{Tag} BYE IMAP4rev1 Server logging out";
                    break;
                default:
                    break;
            }
        }
    }
}
