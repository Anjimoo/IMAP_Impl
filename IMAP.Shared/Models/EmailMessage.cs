using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP.Shared.Models
{
    public class EmailMessage
    {
        public int MsgNum { get; set; }
        public int UniqueID { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Dictionary<string, bool> Flags {get;set;}  = new Dictionary<string, bool>();



        public EmailMessage()
        {
            InitFlags();
        }

        private void InitFlags()
        {
            Flags.Add(@"\Answered", false);
            Flags.Add(@"\Flagged", false);
            Flags.Add(@"\Deleted", false);
            Flags.Add(@"\Seen", false);
            Flags.Add(@"\Draft", false);
            Flags.Add(@"\Recent", false);
        }

        public void RaiseFlag(string flagName)
        {
            Flags[flagName] = true;
        }

        public void LowerFlag(string flagName)
        {
            Flags[flagName] = false;
        }
    }
}
