using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMAP_Server.Interfaces;
using IMAP.Shared;

namespace IMAP_Server.CommandModels
{
    public class SelectCommand : ICommand
    {
        public string Tag { get; set; }
        public int CommandSplits { get; set; }
        public string[] CommandContent { get; set; }
        public bool Validated { get; set; }
        public ConnectionState Connection { get; set; }

        public SelectCommand(string[] message, ConnectionState connection)
        {
            CommandSplits = 3;
            CommandContent = message;
            Connection = connection;                  
            ValidateCommand();
            if (Validated)
            {
                if (Server.mailBoxes.TryGetValue(message[2], out var mailbox))
                {
                    Connection.SelectedMailBox = true;
                }
                //connection = 
            }
        }

        public string GetResponse()
        {
            if(Connection.SelectedMailBox == true)
            {
                return "";
            }
            return "";
        }

        public void ValidateCommand()
        {
            if(CommandContent.Length == CommandSplits)
            {
                Validated = true;
            }
        }

        async Task Action()
        {

        }
    }
}
