using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMAP.Shared;
using IMAP.Shared.Models;

namespace IMAP_Server.Services
{
    public static class IMAP_Search
    {
        public const string ALL = "ALL";
        public const string ANSWERED = "ANSWERED";
        public const string BCC = "BCC";
        public const string BEFORE = "BEFORE";
        public const string BODY = "BODY";
        public const string CC = "CC";
        public const string DELETED = "DELETED";
        public const string DRAFT = "DRAFT";
        public const string FLAGGED = "FLAGGED";
        public const string FROM = "FROM";
        public const string HEADER = "HEADER";
        public const string KEYWORD = "KETWORD";
        public const string LARGER = "LARGER";
        public const string NEW = "NEW";
        public const string NOT = "NOT";
        public const string OLD = "OLD";
        public const string ON = "ON";
        public const string OR = "OR";
        public const string RECENT = "RECENT";
        public const string SEEN = "SEEN";
        public const string SENTBEFORE = "SENTBEFORE";
        public const string SENTON = "SENTON";
        public const string SENTSINCE = "SENTSINCE";
        public const string SINCE = "SINCE";
        public const string SMALLER = "SMALLER";
        public const string SUBJECT = "SUBJECT";
        public const string TEXT = "TEXT";
        public const string TO = "TO";
        public const string UID = "UID";
        public const string UNANSWERED = "UNANSWERED";
        public const string UNDELETED = "UNDELETED";
        public const string UNDRAFT = "UNDRAFT";
        public const string UNFLAGGED = "UNFLAGGED";
        public const string UNKEYWORD = "UNKEYWORD";
        public const string UNSEEN = "UNSEEN";
        private static Connection _connection;
        public static List<int> Search(string[] searchCriterias, Connection connection)
        {
            _connection = connection;
            List<int> messages = new List<int>();

            switch (searchCriterias[0])
            {
                case ALL:
                    foreach (var email in _connection.SelectedMailBox.EmailMessages)
                    {
                        messages.Add(email.MsgNum);
                    }
                    break;
                case ANSWERED:
                    messages = CheckFlaggedMessage(Flags.ANSWERED, true);
                    break;
                case BCC:
                    messages = CheckInnerText(BCC, searchCriterias[1]);
                    break;
                case BEFORE:
                    messages = CheckMessageDate(BEFORE, searchCriterias[1]);
                    break;
                case CC:
                    messages = CheckInnerText(CC, searchCriterias[1]);
                    break;
                case DELETED:
                    messages = CheckFlaggedMessage(Flags.DELETED, true);
                    break;
                case DRAFT:
                    messages = CheckFlaggedMessage(Flags.DRAFT, true);
                    break;
                case FLAGGED:
                    messages = CheckFlaggedMessage(Flags.FLAGGED, true);
                    break;
                case FROM:
                    messages = CheckInnerText(FROM, searchCriterias[1]);
                    break;
                case HEADER:
                    break;
                case KEYWORD:
                    break;
                case LARGER:
                    messages = CheckSizeOfMessage(searchCriterias[0], Int32.Parse(searchCriterias[1]));
                    break;
                case NEW:              
                    var recent = CheckFlaggedMessage(RECENT, true);
                    var unseen = CheckFlaggedMessage(UNSEEN, false);
                    var newMails = recent.Intersect(unseen);
                    break;
                case NOT:
                    var messagesWithSearchKey = Search(searchCriterias.Skip(1).ToArray(), connection); //check messages with search key
                    List<int> emailsNumbers = new List<int>();
                    foreach(var email in _connection.SelectedMailBox.EmailMessages)
                    {
                        emailsNumbers.Add(email.MsgNum);
                    }
                    var nonIntersect = emailsNumbers.Except(messagesWithSearchKey); //check messages that doest not have search key
                    break;
                case OLD:
                    var old = CheckFlaggedMessage(RECENT, false);
                    break;
                case ON:
                    messages = CheckMessageDate(ON, searchCriterias[1]);
                    break;
                case OR:
                    
                    //TO DO 
                    break;
                case RECENT:
                    messages = CheckFlaggedMessage(Flags.RECENT, true);
                    break;
                case SEEN:
                    messages = CheckFlaggedMessage(Flags.SEEN, true);
                    break;
                case SENTBEFORE:
                    break;
                case SENTON:
                    break;
                case SENTSINCE:
                    break;
                case SINCE:
                    messages = CheckMessageDate(SINCE, searchCriterias[1]);
                    break;
                case SMALLER:
                    messages = CheckSizeOfMessage(searchCriterias[0], Int32.Parse(searchCriterias[1]));
                    break;
                case SUBJECT:
                    messages = CheckInnerText(SUBJECT, searchCriterias[1]);
                    break;
                case TEXT:
                    messages = CheckInnerText(TEXT, searchCriterias[1]);
                    break;
                case TO:
                    messages = CheckInnerText(TO, searchCriterias[1]);
                    break;
                case UID: break;
                case UNANSWERED:
                    messages = CheckFlaggedMessage(Flags.ANSWERED, false);
                    break;
                case UNDELETED:
                    messages = CheckFlaggedMessage(Flags.DELETED, false);
                    break;
                case UNDRAFT:
                    messages = CheckFlaggedMessage(Flags.DRAFT, false);
                    break;
                case UNFLAGGED:
                    messages = CheckFlaggedMessage(FLAGGED, false);
                    break;
                case UNKEYWORD:
                    messages = CheckFlaggedMessage(searchCriterias[1], false);
                    break;
                case UNSEEN:
                    messages = CheckFlaggedMessage(Flags.SEEN, false);
                    break;

                default:
                    break;
            }
            return messages;
        }

        private static List<int> CheckFlaggedMessage(string flag, bool flagged)
        {
            List<int> messages = new List<int>();
            foreach (var email in _connection.SelectedMailBox.EmailMessages)
            {
                if (email.Flags[flag] == flagged)
                {
                    messages.Add(email.MsgNum);
                }
            }
            return messages;
        }

        private static List<int> CheckMessageDate(string criteria, string date)
        {
            List<int> messages = new List<int>();
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                foreach (var email in _connection.SelectedMailBox.EmailMessages)
                {
                    if (criteria == BEFORE && email.Date < parsedDate)
                    {
                        messages.Add(email.MsgNum);
                    }
                    else if (criteria == SINCE && email.Date > parsedDate)
                    {
                        messages.Add(email.MsgNum);
                    }
                }
            }
            else
            {
                _connection.SendToStream("BAD - command unknown or arguments invalid");
            }
            return messages;
        }

        private static List<int> CheckSizeOfMessage(string criteria, int sizeInOctets)
        {
            List<int> messages = new List<int>();
            foreach (var email in _connection.SelectedMailBox.EmailMessages)
            {
                var emailBytes = Encoding.UTF8.GetBytes(email.Content);
                if (criteria == LARGER && emailBytes.Length > sizeInOctets)
                {
                    messages.Add(email.MsgNum);
                }
                else if (criteria == SMALLER && emailBytes.Length < sizeInOctets)
                {
                    messages.Add(email.MsgNum);
                }
            }
            return messages;
        }

        private static List<int> CheckInnerText(string criteria, string subStringToFind)
        {
            List<int> messages = new List<int>();
            foreach (var email in _connection.SelectedMailBox.EmailMessages)
            {
                if (criteria == TO && email.To.Contains(subStringToFind))
                {
                    messages.Add(email.MsgNum);
                }
                else if (criteria == TEXT && email.Content.Contains(subStringToFind))
                {
                    messages.Add(email.MsgNum);
                }
                else if (criteria == SUBJECT && email.Subject.Contains(subStringToFind))
                {
                    messages.Add(email.MsgNum);
                }
                else if (criteria == BODY && (email.Content.Contains(subStringToFind)))//need to add Header
                {
                    messages.Add(email.MsgNum);
                }
                else if (criteria == CC && email.CC.Contains(subStringToFind))
                {
                    messages.Add(email.MsgNum);
                }
                else if (criteria == BCC && email.BCC.Contains(subStringToFind))
                {
                    messages.Add(email.MsgNum);
                }
                else if (criteria == FROM && email.From.Contains(subStringToFind))
                {
                    messages.Add(email.MsgNum);
                }
            }
            return messages;
        }
    }
}
