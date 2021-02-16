using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Server.Services
{
    public static class ServerCapabilities
    {
        //Here is the list of what the server is able to do.
        public static List<string> CapabilitiesAny { get; private set; } = new List<string>() 
        {
            "IMAP4rev1",
            "Add more capabilities here" //What the string says, see here for a list: https://k9mail.app/documentation/development/imapExtensions.html
        };

        //**************We might add more types of capabilities here, we'll need to see how it's implemented.

    }
}
