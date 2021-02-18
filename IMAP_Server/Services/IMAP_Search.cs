using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static List<int> Search(string[] searchCriterias)
        {
            List<int> messages = new List<int>();
            
            switch (searchCriterias[0])
            {
                case ALL:

                    break;
                case ANSWERED:
                    break;
                case BCC:
                    break;
                case BEFORE:
                    break;
                case CC:
                    break;
                case DELETED:
                    break;
                case DRAFT:
                    break;
                case FLAGGED:
                    break;
                case FROM:
                    break;
                case HEADER:
                    break;
                case KEYWORD:
                    break;
                case LARGER:
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
                    break;
                case SEEN:
                    break;
                case SENTBEFORE:
                    break;
                case SENTON:
                    break;
                case SENTSINCE:
                    break;
                case SINCE:
                    break;
                case SMALLER:
                    break;
                case SUBJECT:
                    break;
                case TEXT:
                    foreach (var email in Server.mailBoxes["Jimoo@gmail.com"].EmailMessages)
                    {
                        if (email.Content.Contains(searchCriterias[1]))
                        {
                            messages.Add(email.MsgNum);
                        }
                    }
                    break;
                case TO:
                    break;
                case UID: break;
                case UNANSWERED:
                    break;
                case UNDELETED:
                    break;
                case UNDRAFT:
                    break;
                case UNFLAGGED:
                    break;
                case UNKEYWORD:
                    break;
                case UNSEEN:
                    break;

                default:
                    break;
            }
            return messages;
        }
    }
}
