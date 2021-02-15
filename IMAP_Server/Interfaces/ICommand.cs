using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP_Server.Interfaces
{
    public interface ICommand
    {

        string Tag { get; }
        int CommandSplits { get; }
        string CommandContent { get; set; }
        public bool Validated { get; set; }
        public bool ValidateCommand();                  
    }
}
