using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using IMAP_Server.Models;
using MimeKit;

namespace IMAP_Server.Services
{
    class JsonParser
    {

        public static Dictionary<string, User> ReadUsers()
        {        
            var users = new Dictionary<string, User>();
            string jsonString = File.ReadAllText("Users.json");          
            users =JsonSerializer.Deserialize< Dictionary<string, User>> (jsonString);
            return users;
        }

        public static List<EmailMessage> ParseEmails()
        {
            string jsonString = File.ReadAllText("Emails.json");
            var emails = JsonSerializer.Deserialize<List<EmailMessage>>(jsonString);
            return emails;
        }

        public static List<EmailMessage> CreateEmails()
        {
            List<EmailMessage> emailMessages = new List<EmailMessage>();

            var emails = JsonSerializer.Deserialize<List<EmailModel>>(File.ReadAllText("Emails.json"));
            int count = 1;
            foreach(var email in emails)
            {
                var message = new EmailMessage();
                message.Bcc.Add(new MailboxAddress(email.Bcc_Name, email.Bcc_Address));
                message.Cc.Add(new MailboxAddress(email.Cc_Name, email.Cc_Address));
                message.From.Add(new MailboxAddress(email.From_Name, email.From_Address));
                message.To.Add(new MailboxAddress(email.To_Name, email.To_Address));
                message.Subject = email.Subject;
                message.Body = new TextPart()
                {
                    Text = email.TextBody
                };
                message.MsgNum = count++;
                emailMessages.Add(message);     
            }
            return emailMessages;
        }

    }
}