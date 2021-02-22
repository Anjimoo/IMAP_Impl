using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Server.Models
{
    public class EmailModel
    {
        public string Bcc_Name { get; set; }
        public string Bcc_Address { get; set; }
        public string Cc_Name { get; set; }
        public string Cc_Address { get; set; }
        public string From_Name { get; set; }
        public string From_Address { get; set; }
        public string To_Name { get; set; }
        public string To_Address { get; set; }
        public string TextBody { get; set; }
        public string Subject { get; set; }
    }
}
