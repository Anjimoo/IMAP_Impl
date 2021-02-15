using System;
using System.Collections.Generic;
using System.Text;
using IMAP.Shared;

namespace IMAP_Server
{
    public class MessageHandler
    {
        private Message message;      
        public Dictionary<string, ConnectionState> _connections;
        private string response;
        public MessageHandler()
        {
            _connections = new Dictionary<string, ConnectionState>();
        }
        public string HandleMessage(string _message, string currentConnection)
        {
            message = new Message();
            message.ParseMessage(_message);

            if (message.Command == "LOGOUT")
            {
                response = message.Response;
                _connections[currentConnection].Authentificated = false;
                _connections[currentConnection].SelectedMail = false;
                return response;
            }

            if (!_connections[currentConnection].Connected && message.Command == "CONNECT")
            {
                _connections[currentConnection].Connected = true;
                response = message.Response;     
            }
            else if(!_connections[currentConnection].Authentificated && message.Command == "LOGIN")
            {
                HandleLogin(currentConnection);
            }
            else if(_connections[currentConnection].Authentificated)
            {

            }
            else
            {
                response = $"{message.Tag} BAD";
            }



            return response;
        }

        private void HandleLogin(string currentConnection)
        {
            if (message.Arguments[ArgumentType.USERNAME] == "Anton" && message.Arguments[ArgumentType.PASSWORD] == "12345")
            {
                _connections[currentConnection].Authentificated = true;
                response = $"{message.Tag} OK LOGIN Complited: now in authentificated state";
            }
            else
            {
                response = $"{message.Tag} NO LOGIN Failure: username or password rejected";
            }
        }

        
    }
}
