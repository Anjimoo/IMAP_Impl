using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IMAP.Shared;
using IMAP_Server.CommandModels;

namespace IMAP_Server
{
    //This class handles messages from the client, also holds the current active connections dictionary.
    public class MessageHandler
    {
        public Dictionary<string, Connection> _connections;
        
        public MessageHandler()
        {
            _connections = new Dictionary<string, Connection>();
        }

        //Receives messages and assigns them to the corresponding handling function.
        public async Task HandleMessage(string _message, string currentConnection)
        {
            
            _message = _message.Trim();
            string[] tempMessage = _message.Split(' ');
            string command;

            if (tempMessage.Length>1)
            {
                command = tempMessage[1];
            }
            else 
            {
                command = _message;
            }

            switch (command)
            {
                //Any state commands
                case "CAPABILITY":
                    AnyStateCommands.Capability(tempMessage, _connections[currentConnection]);
                    break;
                case "LOGOUT":
                    AnyStateCommands.Logout(tempMessage, _connections[currentConnection]);
                    break;
                case "NOOP":
                    AnyStateCommands.NOOP(tempMessage, _connections[currentConnection]);
                    break;

                //No Auth state commands    
                case "AUTHENTICATE":
                    NonAuthStateCommands.Authenticate(tempMessage, _connections[currentConnection]);
                    break;
                case "LOGIN":
                    NonAuthStateCommands.Login(tempMessage, _connections[currentConnection]);
                    break;
                case "STARTTLS":
                    NonAuthStateCommands.StartTLS(tempMessage, _connections[currentConnection]);
                    break;

                //Auth state commands
                case "APPEND":
                    AuthStateCommands.Append(tempMessage,_connections[currentConnection]);
                    break;
                case "CREATE":
                    AuthStateCommands.Create(tempMessage,_connections[currentConnection]);
                    break;

                case "DELETE":
                    AuthStateCommands.Delete(tempMessage,_connections[currentConnection]);
                    break;

                case "EXAMINE":
                    AuthStateCommands.Examine(tempMessage,_connections[currentConnection]); ;
                    break;

                case "LIST":
                    AuthStateCommands.List(tempMessage,_connections[currentConnection]);
                    break;

                case "LSUB": 
                    AuthStateCommands.Lsub(tempMessage,_connections[currentConnection]);
                    break;

                case "RENAME": 
                    AuthStateCommands.Rename(tempMessage,_connections[currentConnection]); 
                    break;
                case "SELECT":
                    AuthStateCommands.Select(tempMessage, _connections[currentConnection]);
                    break;
                case "STATUS":
                    AuthStateCommands.Status(tempMessage, _connections[currentConnection]);
                    break;
                case "SUBSCRIBE":
                    AuthStateCommands.Subscribe(tempMessage, _connections[currentConnection]);
                    break;
                case "UNSUBSCRIBE":
                    AuthStateCommands.Unsubscribe(tempMessage, _connections[currentConnection]);
                    break;

                //Select state commands
                case "CHECK":
                    SelectStateCommands.Check(tempMessage, _connections[currentConnection]);
                    break;
                case "CLOSE":
                    SelectStateCommands.Close(tempMessage,_connections[currentConnection]);
                    break;
                case "COPY":
                    SelectStateCommands.Copy(tempMessage,_connections[currentConnection]);
                    break;
                case "EXPUNGE":
                    SelectStateCommands.Expunge(tempMessage,_connections[currentConnection]);
                    break;
                case "FETCH":
                    SelectStateCommands.Fetch(tempMessage,_connections[currentConnection]);
                    break;
                case "SEARCH":
                    SelectStateCommands.Search(tempMessage,_connections[currentConnection]);
                    break;
               case "STORE":
                    SelectStateCommands.Store(tempMessage, _connections[currentConnection]);
                    break;
                case "UID":
                    SelectStateCommands.UID(tempMessage,_connections[currentConnection]);
                    break;

                //Any other unrecognized command/message sent to the server.
                case "":
                    AnyStateCommands.Empty(tempMessage, _connections[currentConnection]);
                    break;
                default:
                    AnyStateCommands.Default(tempMessage, _connections[currentConnection]);
                    break;
            }
        }

        
    }
}
