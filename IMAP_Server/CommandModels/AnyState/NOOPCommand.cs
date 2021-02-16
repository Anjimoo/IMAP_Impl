﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMAP_Server.Interfaces;
using IMAP.Shared;

namespace IMAP_Server.CommandModels
{
    public class NOOPCommand : ICommand
    {
        public string Tag { get; set; }
        public int CommandSplits { get; set; }
        public string[] CommandContent { get; set; }
        public bool Validated { get; set; }                
        public ConnectionState Connection { get; set; }


    public NOOPCommand(string[] command)
        {
            
        }


        public string GetResponse()
        {
            return "";
        }

        public void ValidateCommand()
        {
            
        }

        async Task Action()
        {
            
        }
    }
}
