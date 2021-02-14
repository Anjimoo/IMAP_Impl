using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP.Shared.Interfaces
{
    public interface ICommand
    {
        public string CommandContent { get; set; }
        public bool Validated { get; set; }
        public bool ValidateCommand();

                  
    }
}
