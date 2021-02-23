using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMAP.Shared;
using IMAP_Server.Models;

namespace IMAP_Server.Services
{
    public static class IMAP_Search
    { 
        private static Connection _connection;
        /// <summary>
        /// Searches messages in email box by specified criterias
        /// </summary>
        /// <param name="searchCriterias"></param>
        /// <param name="connection"></param>
        /// <returns>Numbers of messages</returns>
        public static List<int> Search(string[] searchCriterias, Connection connection)
        {
            _connection = connection;
            List<int> messages = new List<int>();
            
            switch (Enum.Parse<SearchCriterias>(searchCriterias[0]))
            {
                case SearchCriterias.ALL:
                    foreach (var email in _connection.SelectedMailBox.EmailMessages)
                    {
                        messages.Add(email.MsgNum);
                    }
                    break;
                case SearchCriterias.ANSWERED:
                    messages = CheckFlaggedMessage(Flags.ANSWERED, true);
                    break;
                case SearchCriterias.BCC:
                    messages = CheckInnerText(SearchCriterias.BCC.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.BEFORE:
                    messages = CheckMessageDate(SearchCriterias.BEFORE.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.CC:
                    messages = CheckInnerText(SearchCriterias.CC.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.DELETED:
                    messages = CheckFlaggedMessage(Flags.DELETED, true);
                    break;
                case SearchCriterias.DRAFT:
                    messages = CheckFlaggedMessage(Flags.DRAFT, true);
                    break;
                case SearchCriterias.FLAGGED:
                    messages = CheckFlaggedMessage(Flags.FLAGGED, true);
                    break;
                case SearchCriterias.FROM:
                    messages = CheckInnerText(SearchCriterias.FROM.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.HEADER:
                    CheckInnerText(SearchCriterias.HEADER.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.KEYWORD:
                    break;
                case SearchCriterias.LARGER:
                    messages = CheckSizeOfMessage(searchCriterias[0], Int32.Parse(searchCriterias[1]));
                    break;
                case SearchCriterias.NEW:              
                    var recent = CheckFlaggedMessage(SearchCriterias.RECENT.ToString(), true);
                    var unseen = CheckFlaggedMessage(SearchCriterias.UNSEEN.ToString(), false);
                    var newMails = recent.Intersect(unseen);
                    break;
                case SearchCriterias.NOT:
                    var messagesWithSearchKey = Search(searchCriterias.Skip(1).ToArray(), connection); //check messages with search key
                    List<int> emailsNumbers = new List<int>();
                    foreach(var email in _connection.SelectedMailBox.EmailMessages)
                    {
                        emailsNumbers.Add(email.MsgNum);
                    }
                    var nonIntersect = emailsNumbers.Except(messagesWithSearchKey); //check messages that doest not have search key
                    break;
                case SearchCriterias.OLD:
                    var old = CheckFlaggedMessage(SearchCriterias.RECENT.ToString(), false);
                    break;
                case SearchCriterias.ON:
                    messages = CheckMessageDate(SearchCriterias.ON.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.OR:
                    
                    //TO DO 
                    break;
                case SearchCriterias.RECENT:
                    messages = CheckFlaggedMessage(Flags.RECENT, true);
                    break;
                case SearchCriterias.SEEN:
                    messages = CheckFlaggedMessage(Flags.SEEN, true);
                    break;
                case SearchCriterias.SENTBEFORE:
                    break;
                case SearchCriterias.SENTON:
                    break;
                case SearchCriterias.SENTSINCE:
                    break;
                case SearchCriterias.SINCE:
                    messages = CheckMessageDate(SearchCriterias.SINCE.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.SMALLER:
                    messages = CheckSizeOfMessage(searchCriterias[0], Int32.Parse(searchCriterias[1]));
                    break;
                case SearchCriterias.SUBJECT:
                    messages = CheckInnerText(SearchCriterias.SUBJECT.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.TEXT:
                    messages = CheckInnerText(SearchCriterias.TEXT.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.TO:
                    messages = CheckInnerText(SearchCriterias.TO.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.UID:
                    CheckInnerText(SearchCriterias.UID.ToString(), searchCriterias[1]);
                    break;
                case SearchCriterias.UNANSWERED:
                    messages = CheckFlaggedMessage(Flags.ANSWERED, false);
                    break;
                case SearchCriterias.UNDELETED:
                    messages = CheckFlaggedMessage(Flags.DELETED, false);
                    break;
                case SearchCriterias.UNDRAFT:
                    messages = CheckFlaggedMessage(Flags.DRAFT, false);
                    break;
                case SearchCriterias.UNFLAGGED:
                    messages = CheckFlaggedMessage(SearchCriterias.FLAGGED.ToString(), false);
                    break;
                case SearchCriterias.UNKEYWORD:
                    messages = CheckFlaggedMessage(searchCriterias[1], false);
                    break;
                case SearchCriterias.UNSEEN:
                    messages = CheckFlaggedMessage(Flags.SEEN, false);
                    break;

                default:
                    break;
            }
            return messages;
        }

        /// <summary>
        /// Checks which of messages has or does not specified flag and returns List of these messages
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="flagged"></param>
        /// <returns>Numbers of messages</returns>
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

        /// <summary>
        /// Checks if message is older/newer or received at specific date
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="date"></param>
        /// <returns>Numbers of messages</returns>
        private static List<int> CheckMessageDate(string criteria, string date)
        {
            List<int> messages = new List<int>();
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {      
                foreach (var email in _connection.SelectedMailBox.EmailMessages)
                {
                    if (criteria == SearchCriterias.BEFORE.ToString() && email.Date < parsedDate)
                    {
                        messages.Add(email.MsgNum);
                    }
                    else if (criteria == SearchCriterias.SINCE.ToString() && email.Date > parsedDate)
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
        /// <summary>
        /// Checks if message Content(Body) of message is larger/smaller then specific size in Octets
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="sizeInOctets"></param>
        /// <returns>Numbers of messages</returns>
        private static List<int> CheckSizeOfMessage(string criteria, int sizeInOctets)
        {
            List<int> messages = new List<int>();
            foreach (var email in _connection.SelectedMailBox.EmailMessages)
            {
                var emailBytes = Encoding.UTF8.GetBytes(email.TextBody);
                if (criteria == SearchCriterias.LARGER.ToString() && emailBytes.Length > sizeInOctets)
                {
                    messages.Add(email.MsgNum);
                }
                else if (criteria == SearchCriterias.SMALLER.ToString() && emailBytes.Length < sizeInOctets)
                {
                    messages.Add(email.MsgNum);
                }
            }
            return messages;
        }
        /// <summary>
        /// Checks if message has specified string in it's Content(body)/text/subject/bcc/cc/from
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="subStringToFind"></param>
        /// <returns>Numbers of messages</returns>
        private static List<int> CheckInnerText(string criteria, string subStringToFind)
        {
            List<int> messages = new List<int>();
            foreach (var email in _connection.SelectedMailBox.EmailMessages)
            {
                if (criteria == SearchCriterias.TO.ToString())
                {
                    foreach(var to in email.To)
                    {
                        if (to.Name.Contains(subStringToFind))
                        {
                            messages.Add(email.MsgNum);
                        }
                    }                
                }
                else if (criteria == SearchCriterias.TEXT.ToString() && email.TextBody.Contains(subStringToFind))
                {
                    messages.Add(email.MsgNum);
                }
                else if (criteria == SearchCriterias.SUBJECT.ToString() && email.Subject.Contains(subStringToFind))
                {
                    messages.Add(email.MsgNum);
                }
                //else if (criteria == BODY && (email.Body. .Contains(subStringToFind)))//need to add Header
                //{
                //    messages.Add(count++);
                //}
                else if (criteria == SearchCriterias.CC.ToString())
                {
                    foreach(var cc in email.Cc)
                    {
                        if (cc.Name.Contains(subStringToFind))
                        {
                            messages.Add(email.MsgNum);
                        }
                    }                
                }
                else if (criteria == SearchCriterias.BCC.ToString())
                {
                    foreach (var bcc in email.Bcc)
                    {
                        if (bcc.Name.Contains(subStringToFind))
                        {
                            messages.Add(email.MsgNum);
                        }
                    }
                }
                else if (criteria == SearchCriterias.FROM.ToString())
                {
                    foreach (var from in email.From)
                    {
                        if (from.Name.Contains(subStringToFind))
                        {
                            messages.Add(email.MsgNum);
                        }
                    }                 
                }else if(criteria == SearchCriterias.HEADER.ToString())
                {
                    foreach(var header in email.Headers)
                    {
                        if (header.Value.Contains(subStringToFind))
                        {
                            messages.Add(email.MsgNum);
                        }
                    }
                }else if(criteria == SearchCriterias.UID.ToString())
                {
                    if(email.MessageId == subStringToFind)
                    {
                        messages.Add(email.MsgNum);
                    }
                }
            }
            return messages;
        }
    }
}
