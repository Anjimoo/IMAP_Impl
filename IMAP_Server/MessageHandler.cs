using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using IMAP.Shared;
using IMAP.Shared.Models;
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
        public void HandleMessage(string _message, string currentConnection, NetworkStream stream)
        {
           
            string[] tempMessage = _message.Split(' ');

            var command = tempMessage[1];
            var tag = tempMessage[0];

            switch (command)
            {
                //Any state commands
                case "CAPABILITY":
                    AnyStateCommands.Capability(tempMessage, _connections[currentConnection], stream);
                    break;
                case "LOGOUT":
                    AnyStateCommands.Logout(tempMessage, _connections[currentConnection], stream);
                    break;
                case "NOOP":
                    AnyStateCommands.NOOP(tempMessage, _connections[currentConnection], stream);
                    break;
                //No Auth state commands    

                case "AUTHENTICATE":
                    NonAuthStateCommands.Authenticate(tempMessage, _connections[currentConnection], stream);
                    break;
                case "LOGIN":
                    NonAuthStateCommands.Login(tempMessage, _connections[currentConnection], stream);
                    break;
                case "STARTTLS":
                    NonAuthStateCommands.StartTLS(tempMessage, _connections[currentConnection], stream);
                    break;

                //Auth state commands
                case "APPEND":
                    AuthStateCommands.Append(tempMessage,_connections[currentConnection], stream);
                    break;
                case "CREATE":
                    AuthStateCommands.Create(tempMessage,_connections[currentConnection], stream);
                    break;

                case "DELETE":
                    AuthStateCommands.Create(tempMessage,_connections[currentConnection], stream);
                    break;

                case "EXAMINE":
                    AuthStateCommands.Examine(tempMessage,_connections[currentConnection], stream); ;
                    break;

                case "LIST":
                    AuthStateCommands.List(tempMessage,_connections[currentConnection], stream);
                    break;

                case "LSUB": 
                    AuthStateCommands.Lsub(tempMessage,_connections[currentConnection], stream);
                    break;

                case "RENAME": 
                    AuthStateCommands.Rename(tempMessage,_connections[currentConnection], stream); 
                    break;
                case "SELECT":
                    AuthStateCommands.Select(tempMessage, _connections[currentConnection], stream);
                    break;
                case "STATUS":
                    AuthStateCommands.Status(tempMessage, _connections[currentConnection], stream);
                    break;
                case "SUBSCRIBE":
                    AuthStateCommands.Subscribe(tempMessage, _connections[currentConnection], stream);
                    break;
                case "UNSUBSCRIBE":
                    AuthStateCommands.Unsubscribe(tempMessage, _connections[currentConnection], stream);
                    break;

                //Select state commands
                case "CHECK":
                    SelectStateCommands.Check(tempMessage, _connections[currentConnection], stream);
                    break;
                case "CLOSE":
                    SelectStateCommands.Close(tempMessage,_connections[currentConnection], stream);
                    break;
                case "COPY":
                    SelectStateCommands.Copy(tempMessage,_connections[currentConnection], stream);
                    break;
                case "Expunge":
                    SelectStateCommands.Expunge(tempMessage,_connections[currentConnection], stream);
                    break;
                case "FETCH":
                    SelectStateCommands.Fetch(tempMessage,_connections[currentConnection], stream);
                    break;
                case "SEARCH":
                    SelectStateCommands.Search(tempMessage,_connections[currentConnection], stream);
                    break;
               case "STORE":
                    SelectStateCommands.Store(tempMessage, _connections[currentConnection], stream);
                    break;
                case "UID":
                    SelectStateCommands.UID(tempMessage,_connections[currentConnection], stream);
                    break;
               

                //case "CONNECT":
                //    response = $"{tag} OK greetings";
                //    break;
                //case "LOGIN":
                //    var loginCommand = new LoginCommand(tempMessage, _connections[currentConnection]);
                //    response = loginCommand.GetResponse();
                //    if (loginCommand.LoginSucceeded)
                //    {
                //        _connections[currentConnection].Authentificated = true;
                //    }
                //    break;
                //case "LOGOUT":
                //    response = $"{tag} BYE IMAP4rev1 Server logging out";
                //    _connections[currentConnection].Authentificated = false;
                //    break;
                //case "SELECT":
                //    var selectCommand = new SelectCommand(tempMessage, _connections[currentConnection]);
                //    break;

                default:
                    break;
            }
        }

        
    }
}
