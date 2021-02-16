﻿using IMAP.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Server.Interfaces
{
    public interface ICommand
    {

        string Tag { get; }
        int CommandSplits { get; }
        string[] CommandContent { get; set; }
        bool Validated { get; set; }
        public ConnectionState Connection { get; }

        void ValidateCommand();
        string GetResponse();
        async Task Action() { }
    }
}
