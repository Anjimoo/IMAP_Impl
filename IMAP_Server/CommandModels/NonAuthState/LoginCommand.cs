using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using IMAP_Server.Interfaces;
using System.Threading.Tasks;
using IMAP.Shared;

namespace IMAP_Server.CommandModels
{

    //Prototype only. Possibility for commands to NOT look like this.
    //This is in shared since server might possiblity want to use the same validations as the client.
    //We may move this to server later on.

    //I don't know whether this will be used on the server (handler side) or on the client yet.

    public class LoginCommand : ICommand

    {
        public string Tag { get; private set; }
        public int CommandSplits { get; private set; } = 4;
        public string[] CommandContent { get; set; }
        public bool Validated { get; set; }
        public bool LoginSucceeded { get; set; }
        public ConnectionState Connection { get; set; }

        public LoginCommand(string[] command, ConnectionState connection)
        {
            Connection = connection;
            Tag = command[0];
            CommandContent = command;
            ValidateCommand();
            if (Validated)
            {
                ValidateLogin();
            }

            //Example for general use of "Action" function. In login there's probably not much to do.
            if (LoginSucceeded)
            {
                Task.Run(() => Action()); //Run what the command is supposed to do in a different thread (I don't know yet 
                                            //if we need async here or not.)
            }       
        }

        public void ValidateCommand()
        {
            /***********************************/
            //TODO: Finish writing this function.
            /***********************************/
            if(CommandContent.Length != CommandSplits)
            {
                Validated = false;
            }
            Validated = true;
        }

        public string GetResponse()
        {
            if(Validated == false)
            {
                return $"{Tag} BAD - command unknown or arguments invalid";
            }
            
            if(LoginSucceeded)
            {
                return $"{Tag} OK - login completed, now in authenticated state";
            }
            
            return $"{Tag} NO - login failure: user name or password rejected";
        }

        public void ValidateLogin()
        {
            if(CommandContent[2] == "Anton" && CommandContent[3] == "12345")
            {
                LoginSucceeded = true;
            }
            else
            {
                LoginSucceeded = false;
            }
        }

        async Task Action()
        {

        }
    }
}
