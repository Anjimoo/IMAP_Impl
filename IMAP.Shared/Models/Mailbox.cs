﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP.Shared.Models
{
    /// <summary>
    /// Represents a mailbox folder.
    /// </summary>
    public class Mailbox
    {
        private string mailboxName { get; set; } //Should use UTF-7 string instead since IMAP supports that only? Not sure how 
        private int mailboxSize { get; set; }
        private bool nextUniqueIDVal { get; set; } //MUST NOT change unless new messages are added to the mailbox; and second, the next unique identifier value MUST change whenever new messages are added to the mailbox, even if those new messages are subsequently expunged.
        private long uniqueIDValidityVal { get; } //containing the UIDVALIDITY of the currently selected folder, or 0 if no folder is selected. (The client needs to check this value for each folder every session)
    }
}