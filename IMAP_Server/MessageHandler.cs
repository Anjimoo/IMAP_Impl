using System;
using System.Collections.Generic;
using System.Text;
using IMAP.Shared;
using IMAP_Server.CommandModels;

namespace IMAP_Server
{
    //public enum ArgumentType //I don't remember what i wanted to do
    //{
    //    USERNAME,
    //    PASSWORD,
    //    MAILBOX,
    //    OLDMAILBOX,
    //    NEWMAILBOX,
    //    REFNAME,
    //    MAILBOXWILDCARDS,
    //    STATUSDATANAMES,
    //    FLAGLIST,
    //    DATETIME,
    //    SEARCHCRIT,
    //    SEQUENCE,
    //    DATAORMACRO,
    //    DATANAME,
    //    VALUEDATA,
    //    COMMANDNAME,
    //    COMMANDARGS
    //}
    public class MessageHandler
    {
        public Dictionary<string, ConnectionState> _connections;
        private string response;
        public MessageHandler()
        {
            _connections = new Dictionary<string, ConnectionState>();
        }
        public string HandleMessage(string _message, string currentConnection)
        {  
            ParseMessage(_message, currentConnection);

            //if (message.Command == "LOGOUT")
            //{
            //    response = message.Response;
            //    _connections[currentConnection].Authentificated = false;
            //    _connections[currentConnection].SelectedMailBox = false;
            //    return response;
            //}

            //if (!_connections[currentConnection].Connected && message.Command == "CONNECT")
            //{
            //    _connections[currentConnection].Connected = true;
            //    response = message.Response;     
            //}
            //else if(!_connections[currentConnection].Authentificated && message.Command == "LOGIN")
            //{
            //    //HandleLogin(currentConnection);
            //}
            //else if(_connections[currentConnection].Authentificated)
            //{

            //}
            //else
            //{
            //    response = $"{message.Tag} BAD";
            //}

            return response;
        }

        //private void HandleLogin(string currentConnection)
        //{
        //    if (message.Arguments[ArgumentType.USERNAME] == "Anton" && message.Arguments[ArgumentType.PASSWORD] == "12345")
        //    {
        //        _connections[currentConnection].Authentificated = true;
        //        response = $"{message.Tag} OK LOGIN Complited: now in authentificated state";
        //    }
        //    else
        //    {
        //        response = $"{message.Tag} NO LOGIN Failure: username or password rejected";
        //    }
        //}

        private void ParseMessage(string _message, string currentConnection)
        {
            string[] tempMessage = _message.Split(' ');

            var command = tempMessage[1];
            var tag = tempMessage[0];

            switch (command)
            {
                case "CONNECT":
                    response = $"{tag} OK greetings";
                    break;
                case "LOGIN":
                    var loginCommand = new LoginCommand(tempMessage);
                    response = loginCommand.GetResponse();
                    if (loginCommand.LoginSucceeded)
                    {
                        _connections[currentConnection].Authentificated = true;
                    }
                    break;
                case "LOGOUT":
                    response = $"{tag} BYE IMAP4rev1 Server logging out";
                    _connections[currentConnection].Authentificated = false;
                    break;
                default:
                    break;
            }
        }

        
    }
}
