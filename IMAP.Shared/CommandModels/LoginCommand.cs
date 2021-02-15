using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using IMAP.Shared.Interfaces;
using IMAP.Shared.Services;

namespace IMAP.Shared.CommandModels
{

    //Prototype only. Possibility for commands to NOT look like this.
    //This is in shared since server might possiblity want to use the same validations as the client.
    //We may move this to server later on.

    //I don't know whether this will be used on the server (handler side) or on the client yet.

    public class LoginCommand : ICommand

    {
        public const int commandSplits = 2;
        public string CommandContent { get; set; }
        public bool Validated { get; set; }


        public LoginCommand(string command)
        {
            this.CommandContent = command;
            //this.Validated = ValidateCommand();
        }

       public bool ValidateCommand()
        {
            /***********************************/
            //TODO: Finish writing this function.
            /***********************************/
            return true;
        }

        //public bool ValidateCommand()
        //{
        //    string[] splitCommand = CommandContent.Split(' ');
        //    if (splitCommand.Length != commandSplits)
        //    {
        //        return false;
        //    }

        //    string username = splitCommand[0];
        //    string password = splitCommand[1];

        //    return ValidationService.ValidateUsername(username) && ValidationService.ValidatePassword(password);
        //}
    }
}
