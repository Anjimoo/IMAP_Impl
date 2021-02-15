using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMAP_Server.Interfaces;

namespace IMAP_Server.CommandModels
{
    public class SelectCommand : ICommand
    {
        public string Tag { get; set; }
        public int CommandSplits { get; set; }
        public string[] CommandContent { get; set; }
        public bool Validated { get; set; }

        public string GetResponse()
        {
            return "";
        }

        public void ValidateCommand()
        {
            
        }
    }
}
