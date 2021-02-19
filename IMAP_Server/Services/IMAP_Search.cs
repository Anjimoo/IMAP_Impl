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
                    CheckFlaggedMessage(messages, Flags.ANSWERED, true);
                    break;
                case BCC:
                    break;
                case BEFORE:
                    CheckMessageDate(messages, BEFORE, searchCriterias[1]);
                    break;
                case CC:
                    break;
                case DELETED:
                    CheckFlaggedMessage(messages, Flags.DELETED, true);
                    break;
                case DRAFT:
                    CheckFlaggedMessage(messages, Flags.DRAFT, true);
                    break;
                case FLAGGED:
                    CheckFlaggedMessage(messages, Flags.FLAGGED, true);
                    break;
                case FROM:
                    break;
                case HEADER:
                    break;
                case KEYWORD:
                    break;
                case LARGER:
                    CheckSizeOfMessage(messages, searchCriterias[0], Int32.Parse(searchCriterias[1]));
                    break;
                case NEW:
                    break;
                case NOT:
                    break;
                case OLD:
                    break;
                case ON:
                    break;
                case OR:
                    break;
                case RECENT:
                    CheckFlaggedMessage(messages, Flags.RECENT, true);
                    break;
                case SEEN:
                    CheckFlaggedMessage(messages, Flags.SEEN, true);
                    break;
                case SENTBEFORE:
                    break;
                case SENTON:
                    break;
                case SENTSINCE:
                    break;
                case SINCE:
                    CheckMessageDate(messages, SINCE, searchCriterias[1]);
                    break;
                case SMALLER:
                    CheckSizeOfMessage(messages, searchCriterias[0], Int32.Parse(searchCriterias[1]));
                    break;
                case SUBJECT:
                    CheckInnerText(messages, SUBJECT, searchCriterias[1]);
                    break;
                case TEXT:
                    CheckInnerText(messages, TEXT, searchCriterias[1]);
                    break;
                case TO:
                    CheckInnerText(messages, TO, searchCriterias[1]);
                    break;
                case UID: break;
                case UNANSWERED:
                    CheckFlaggedMessage(messages, Flags.ANSWERED, false);
                    break;
                case UNDELETED:
                    CheckFlaggedMessage(messages, Flags.DELETED, false);
                    break;
                case UNDRAFT:
                    CheckFlaggedMessage(messages, Flags.DRAFT, false);
                    break;
                case UNFLAGGED:
                    CheckFlaggedMessage(messages, Flags.FLAGGED, false);
                    break;
                case UNKEYWORD:
                    break;
                case UNSEEN:
                    CheckFlaggedMessage(messages, Flags.SEEN, false);
                    break;

                default:
                    break;
            }
            return messages;
        }

        private static void CheckFlaggedMessage(List<int> messages, string flag, bool flagged)
        {
            foreach (var email in _connection.SelectedMailBox.EmailMessages)
            {
                if (email.Flags[flag] == flagged)
                {
                    messages.Add(email.MsgNum);
                }
            }
        }

        private static void CheckMessageDate(List<int> messages, string criteria, string date)
        {
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
        }

        private static void CheckSizeOfMessage(List<int> messages, string criteria, int sizeInOctets)
        {
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
        }

        private static void CheckInnerText(List<int> messages, string criteria, string subStringToFind)
        {
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
            }
        }
    }
}
