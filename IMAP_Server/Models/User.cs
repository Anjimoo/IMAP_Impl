using IMAP.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Server.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } //Password in its Base64 form.
    }
}
