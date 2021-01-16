using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP.Shared
{
    public class ConnectionState
    {
        public bool Connected { get; set; }
        public bool Authentificated { get; set; }
        public bool SelectedMail { get; set; }
    }
}
