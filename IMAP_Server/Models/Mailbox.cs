﻿using System;
using System.Collections.Generic;
using System.Text;
using IMAP_Server.Models;
using IMAP_Server.Services;

namespace IMAP_Server.Models
{
    /// <summary>
    /// Represents a mailbox folder.
    /// </summary>
    public class Mailbox
    {
        public List<string> AllowedUsers { get; set; }
        public string mailboxName { get; set; } //Should use UTF-7 string instead since IMAP supports that only? Not sure how 
        public int mailboxSize { get; set; }
        public long nextUniqueIDVal { get; set; } //MUST NOT change unless new messages are added to the mailbox; and second, the next unique identifier value MUST change whenever new messages are added to the mailbox, even if those new messages are subsequently expunged.
        public long uniqueIDValidityVal { get; set; } //containing the UIDVALIDITY of the currently selected folder, or 0 if no folder is selected. (The client needs to check this value for each folder every session)
        public List<EmailMessage> EmailMessages { get; set; }
        public string Path { get; set; } //can be null or have a mailbox - it's the hierarchy property
        public List<string> supportedFlags;

        public Mailbox()
        {
            AllowedUsers = new List<string>();
            EmailMessages = JsonParser.CreateEmails();
            supportedFlags = new List<string>();
            supportedFlags.AddRange(PermanentFlags.PermaFlags);
            supportedFlags.Add(Flags.DRAFT);
            supportedFlags.Add(Flags.FLAGGED);
            nextUniqueIDVal = 0;
            uniqueIDValidityVal = 0;
        }

        public Mailbox(string path)
        {
            AllowedUsers = new List<string>();
            EmailMessages = new List<EmailMessage>();
            nextUniqueIDVal = 0;
            uniqueIDValidityVal = 0;
        }

        public int GetAllRecents()
        {
            int c = 0;
            foreach(EmailMessage em in EmailMessages)
            {
                if(em.Flags.TryGetValue(@"\Recent",out var isRecent))
                {
                    if (isRecent)
                        c++;
                }
            }
            return c;
        }
        /// <summary>
        /// returns all unseen mail messages
        /// </summary>
        /// <returns></returns>
        public int GetAllUnseen()
        {
            int c = 0;
            foreach (EmailMessage em in EmailMessages)
            {
                if (em.Flags.TryGetValue(@"\Seen", out var isSeen))
                {
                    if (!isSeen)
                        c++;
                }
            }
            return c;

        }
    }
}
