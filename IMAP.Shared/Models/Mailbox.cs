using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP.Shared.Models
{
    /// <summary>
    /// Represents a mailbox folder.
    /// </summary>
    public class Mailbox
    {
        public List<string> AllowedUsers { get; set; }
        public string mailboxName { get; set; } //Should use UTF-7 string instead since IMAP supports that only? Not sure how 
        public int mailboxSize { get; set; }
        public bool nextUniqueIDVal { get; set; } //MUST NOT change unless new messages are added to the mailbox; and second, the next unique identifier value MUST change whenever new messages are added to the mailbox, even if those new messages are subsequently expunged.
        public long uniqueIDValidityVal { get; set; } //containing the UIDVALIDITY of the currently selected folder, or 0 if no folder is selected. (The client needs to check this value for each folder every session)
        public List<EmailMessage> EmailMessages { get; set; }
        public Mailbox Parent { get; set; } //can be null or have a mailbox - it's the hierarchy property

        public Mailbox()
        {
            Parent = null;
            AllowedUsers = new List<string>();
            EmailMessages = new List<EmailMessage>();
            nextUniqueIDVal = false;
            uniqueIDValidityVal = 0;
        }

        public Mailbox(Mailbox parent)
        {
            this.Parent = parent;
            this.AllowedUsers = parent.AllowedUsers;
            EmailMessages = new List<EmailMessage>();
            nextUniqueIDVal = false;
            uniqueIDValidityVal = 0;
        }


    }
}
