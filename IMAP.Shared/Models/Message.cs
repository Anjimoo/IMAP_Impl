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
        public string Content { get; set; }
        public Dictionary<ArgumentType, string> Arguments { get; set; }
        public string Response { get; set; }

        public void ParseMessage(string _message)
        {
            string[] tempMessage = _message.Split(' ');
            Arguments = new Dictionary<ArgumentType, string>();
     
            Content = tempMessage[0];

            switch (Content)
            {
                case "CONNECT":
                    Response = "* OK greetings";
                    break;
                case "LOGIN":
                    Arguments.Add(ArgumentType.USERNAME, tempMessage[1]);
                    Arguments.Add(ArgumentType.PASSWORD, tempMessage[2]);
                    break;
                case "LOGOUT":
                    Response = "* BYE IMAP4rev1 Server logging out";
                    break;
                default:
                    break;
            }
        }
    }
}
