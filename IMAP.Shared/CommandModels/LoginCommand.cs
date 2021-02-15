using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using IMAP.Shared.Interfaces;

namespace IMAP.Shared.CommandModels
{

    //Prototype only. Possibility for commands to NOT look like this.
    //This is in shared since server might possiblity want to use the same validations as the client.
    //We may move this to server later on.

    public class LoginCommand : ICommand

    {

        public string CommandContent { get; set; }
        public bool Validated { get; set; }


        public LoginCommand(string command)
        {
            this.CommandContent = command;
            this.Validated = ValidateCommand();
        }

        public bool ValidateCommand()
        {
            string[] splitCommand = CommandContent.Split(' ');
            if (splitCommand.Length > 2)
                return false;
            string username = splitCommand[0];
            string password = splitCommand[1];

            var match = Regex.Match(username, @"^(?=[a-zA-Z])[-\w.]{0,23}([a-zA-Z\d]|(?<![-.])_)$", RegexOptions.IgnoreCase);

            if (match.Success)
                return true;
            
            return false;
        }
    }
}
