using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP_Server.Models
{
    public class EmailMessage : MimeMessage
    {
        public int MsgNum { get; set; }       
        public Dictionary<string, bool> Flags {get;set;}  = new Dictionary<string, bool>();

        public EmailMessage()
        {
            InitFlags();
        }

        private void InitFlags()
        {
            Flags.Add(Models.Flags.ANSWERED, false);
            Flags.Add(Models.Flags.FLAGGED, false);
            Flags.Add(Models.Flags.DELETED, false);
            Flags.Add(Models.Flags.SEEN, false);
            Flags.Add(Models.Flags.DRAFT, false);
            Flags.Add(Models.Flags.RECENT, false);
        }

        public void RaiseFlag(string flagName)
        {
            Flags[flagName] = true;
        }

        public void LowerFlag(string flagName)
        {
            Flags[flagName] = false;
        }

        public EmailMessage GetCopy(int messageNumber)
        {
            var newEmail = new EmailMessage();
            newEmail.MsgNum = messageNumber;
            foreach (var bcc in Bcc)
            {
                newEmail.Bcc.Add(bcc.Clone());
            }
            foreach (var cc in Cc)
            {
                newEmail.Cc.Add(cc.Clone());
            }
            foreach (var from in From)
            {
                newEmail.From.Add(from.Clone());
            }
            foreach (var to in To)
            {
                newEmail.To.Add(to.Clone());
            }
            newEmail.Subject = Subject;
            newEmail.Body = new TextPart()
            {
                Text = TextBody
            };

            return newEmail;
        }
    }
}
