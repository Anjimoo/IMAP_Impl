using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP.Shared.Models
{
    public class EmailMessage
    {
        public int msgNum { get; set; }
        public int uniqueID { get; set; }
        public DateTime date { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string Content { get; set; }

        public enum State
        {
            Seen,
            Answered,
            Flagged,
            Deleted,
            Draft,
            Recent
        }
        public EmailMessage()
        {

        }
    }
}
