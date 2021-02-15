using System;
using System.Collections.Generic;
using System.Text;
using IMAP.Shared;

namespace IMAP_Server
{
    public class MessageHandler
    {
        private Message message;
        private ConnectionState connectionState;
        private string response;
        public MessageHandler()
        {
            connectionState = new ConnectionState();
        }
        public string HandleMessage(string _message)
        {
            message = new Message();
            message.ParseMessage(_message);

            if (!connectionState.Connected && message.Content == "CONNECT")
            {
                connectionState.Connected = true;
                response = message.Response;     
            }
            else if(!connectionState.Authentificated && message.Content == "LOGIN")
            {
                if(message.Arguments[ArgumentType.USERNAME] == "Anton" && message.Arguments[ArgumentType.PASSWORD] == "12345")
                {
                    response = "OK LOGIN Complited: now in authentificated state";
                }
                else
                {
                    response = "NO LOGIN Failure: username or password rejected";
                }
            }
            else if(connectionState.Authentificated)
            {

            }
            else
            {
                response = "BAD";
            }



            return response;
        }

        
    }
}
