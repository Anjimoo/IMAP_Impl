using IMAP_Client.UpdateEvents;
using IMAP_Client.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Client
{
    //Not sure if we do this here on in ViewModels.
    //This might just hold small functions that'll help the implementation in the ViewModel instead.
    //Will also hold the global outgoing and incoming Tags.
    public static class ResponseHandler
    {
        public static IEventAggregator _eventAggregator;

        public static void HandleResponse(string command, string response)
        {
            switch (command)
            {
                //Any state commands
                case "CONNECT":
                    Connect(response);
                    break;
                case "CAPABILITY":
                    break;
                case "LOGOUT":
                    break;
                case "NOOP":
                    break;

                //No Auth state commands    
                case "AUTHENTICATE":
                    break;
                case "LOGIN":
                    Login(response);
                    break;
                case "STARTTLS":
                    break;

                //Auth state commands
                case "APPEND":
                    break;
                case "CREATE":
                    break;

                case "DELETE":
                    break;

                case "EXAMINE":
                    break;

                case "LIST":
                    break;

                case "LSUB":
                    break;

                case "RENAME":
                    break;
                case "SELECT":
                    break;
                case "STATUS":
                    break;
                case "SUBSCRIBE":
                    break;
                case "UNSUBSCRIBE":
                    break;

                //Select state commands
                case "CHECK":
                    break;
                case "CLOSE":
                    break;
                case "COPY":
                    break;
                case "Expunge":
                    break;
                case "FETCH":
                    break;
                case "SEARCH":
                    break;
                case "STORE":
                    break;
                case "UID":
                    break;
            }
        }

        //Only add functions here if we also require to do something else besides getting information.

        static private void Connect(string response)
        {
            if (response.Split("\n")[response.Split("\n").Length - 1].Contains("OK"))
            {
                _eventAggregator.GetEvent<UpdateServerConnectionState>().Publish(true);
            }
        }

        static private void Login(string response)
        {
            if (response.Split("\n")[response.Split("\n").Length - 1].Contains("OK"))
            {
                _eventAggregator.GetEvent<UpdateAuthentificationState>().Publish(true);
            }
        }

        public static void Bye()
        {
            _eventAggregator.GetEvent<UpdateServerConnectionState>().Publish(false);
            _eventAggregator.GetEvent<UpdateAuthentificationState>().Publish(false);
        }



    }
}
