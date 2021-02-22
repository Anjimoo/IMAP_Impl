using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP_Server.Models
{
    public static class Flags
    {
        public const string ANSWERED = @"\Answered";
        public const string FLAGGED = @"\Flagged";
        public const string DELETED = @"\Deleted";
        public const string SEEN = @"\Seen";
        public const string DRAFT = @"\Draft";
        public const string RECENT = @"\Recent";
    }
}
