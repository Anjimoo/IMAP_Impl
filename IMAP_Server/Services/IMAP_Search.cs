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
        private static string _searchParameter;
        /// <summary>
        /// Searches messages in email box by specified criterias
        /// </summary>
        /// <param name="searchCriterias"></param>
        /// <param name="connection"></param>
        /// <returns>Numbers of messages</returns>
        public static List<int> Search(string[] searchCriterias, Connection connection)
        {
            _connection = connection;
            /*messages numbers that will be returned to client*/
            List<int> messages = new List<int>(); 
            /*after parsing to enum search criteria  will be placed here as tokens*/
            List<SearchCriterias> searchTokens = new List<SearchCriterias>(); 

            foreach (var criteria in searchCriterias)
            {
                /*try parse each criterias to Enum and add it to search tokens*/
                if (Enum.TryParse<SearchCriterias>(criteria, false, out var parsedCriteria))
                {
                    searchTokens.Add(parsedCriteria);
                }
                else
                {
                    _searchParameter = criteria;
                    messages.Add(-1);
                }
            }
            foreach (var token in searchTokens)
            {
                switch (token)
                {
                   
                    case SearchCriterias.ALL:
                    /*
                    *  ALL
                    *    All messages in the mailbox; the default initial key for ANDing.
                    */
                        foreach (var email in _connection.SelectedMailBox.EmailMessages)
                        {
                            messages.Add(email.MsgNum);
                        }
                        break;
                    case SearchCriterias.ANSWERED:
                        /*
                         * Messages with the \Answered flag set.
                         */
                        messages = CheckFlaggedMessage(Flags.ANSWERED, true);
                        break;
                    case SearchCriterias.BCC:
                        /*
                         * Messages that contain the specified string in the envelope structure’s BCC field.
                         */
                        messages = CheckInnerText(SearchCriterias.BCC.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.BEFORE:
                        /*
                         * Messages whose internal date (disregarding time and timezone) is earlier than the specified date.
                         */
                        messages = CheckMessageDate(SearchCriterias.BEFORE.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.CC:
                        /*
                         * Messages that contain the specified string in the envelope structure’s CC field.
                         */
                        messages = CheckInnerText(SearchCriterias.CC.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.DELETED:
                        /*
                         * Messages with the \Deleted flag set.
                         */
                        messages = CheckFlaggedMessage(Flags.DELETED, true);
                        break;
                    case SearchCriterias.DRAFT:
                        /*
                         * Messages with the \Draft flag set.
                         */
                        messages = CheckFlaggedMessage(Flags.DRAFT, true);
                        break;
                    case SearchCriterias.FLAGGED:
                        /*
                         * Messages with the \Flagged flag set.
                         */
                        messages = CheckFlaggedMessage(Flags.FLAGGED, true);
                        break;
                    case SearchCriterias.FROM:
                        /*
                        * Messages that contain the specified string in the envelope structure’s FROM field.
                        */
                        messages = CheckInnerText(SearchCriterias.FROM.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.HEADER:
                        /*
                        * Messages that have a header with the specified field-name (as
                          defined in [RFC-2822]) and that contains the specified string
                          in the text of the header (what comes after the colon). If the
                          string to search is zero-length, this matches all messages that
                          have a header line with the specified field-name regardless of the contents.
                        */
                        CheckInnerText(SearchCriterias.HEADER.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.LARGER:
                        /*
                        * Messages with an [RFC-2822] size larger than the specified number of octets.
                        */
                        messages = CheckSizeOfMessage(searchCriterias[0], Int32.Parse(_searchParameter));
                        break;
                    case SearchCriterias.NEW:
                        /*
                        * Messages that have the \Recent flag set but not the \Seen flag. 
                        * This is functionally equivalent to "(RECENT UNSEEN)".
                        */
                        var recent = CheckFlaggedMessage(SearchCriterias.RECENT.ToString(), true);
                        var unseen = CheckFlaggedMessage(SearchCriterias.UNSEEN.ToString(), false);
                        var newMails = recent.Intersect(unseen);
                        break;
                    case SearchCriterias.NOT:
                        /*
                        * Messages that do not match the specified search key.
                        */
                        var messagesWithSearchKey = Search(searchCriterias.Skip(1).ToArray(), connection); //check messages with search key
                        List<int> emailsNumbers = new List<int>();
                        foreach (var email in _connection.SelectedMailBox.EmailMessages)
                        {
                            emailsNumbers.Add(email.MsgNum);
                        }
                        var nonIntersect = emailsNumbers.Except(messagesWithSearchKey); //check messages that doest not have search key
                        break;
                    case SearchCriterias.OLD:
                        /*
                       * Messages that do not have the \Recent flag set. This is
                       * functionally equivalent to "NOT RECENT" (as opposed to "NOT NEW").
                       */
                        var old = CheckFlaggedMessage(SearchCriterias.RECENT.ToString(), false);
                        break;
                    case SearchCriterias.ON:
                        /*
                        * Messages whose internal date (disregarding time and timezone)
                        * is within the specified date.
                        */
                        messages = CheckMessageDate(SearchCriterias.ON.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.RECENT:
                        /*
                        * Messages that have the \Recent flag set.
                        */
                        messages = CheckFlaggedMessage(Flags.RECENT, true);
                        break;
                    case SearchCriterias.SEEN:
                        /*
                        * Messages that have the \Seen flag set.
                        */
                        messages = CheckFlaggedMessage(Flags.SEEN, true);
                        break;
                    case SearchCriterias.SINCE:
                        /*
                         * Messages whose internal date (disregarding time and timezone)
                         * is within or later than the specified date.
                         */
                        messages = CheckMessageDate(SearchCriterias.SINCE.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.SMALLER:
                        /*
                         * Messages with an [RFC-2822] size smaller than the specified number of octets.
                         */
                        messages = CheckSizeOfMessage(searchCriterias[0], Int32.Parse(_searchParameter));
                        break;
                    case SearchCriterias.SUBJECT:
                        /*
                         * Messages that contain the specified string in the envelope structure’s SUBJECT field.
                         */
                        messages = CheckInnerText(SearchCriterias.SUBJECT.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.TEXT:
                        /*
                         * Messages that contain the specified string in the header or body of the message.
                         */
                        messages = CheckInnerText(SearchCriterias.TEXT.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.TO:
                        /*
                         * Messages that contain the specified string in the envelope structure’s TO field.
                         */
                        messages = CheckInnerText(SearchCriterias.TO.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.UID:
                        /*
                         *Messages with unique identifiers corresponding to the specified
                         *unique identifier set. Sequence set ranges are permitted.
                         */
                        CheckInnerText(SearchCriterias.UID.ToString(), _searchParameter);
                        break;
                    case SearchCriterias.UNANSWERED:
                        /*
                         *Messages that do not have the \Answered flag set.
                         */
                        messages = CheckFlaggedMessage(Flags.ANSWERED, false);
                        break;
                    case SearchCriterias.UNDELETED:
                        /*
                         * Messages that do not have the \Deleted flag set.
                         */
                        messages = CheckFlaggedMessage(Flags.DELETED, false);
                        break;
                    case SearchCriterias.UNDRAFT:
                        /*
                         * Messages that do not have the \Draft flag set.
                         */
                        messages = CheckFlaggedMessage(Flags.DRAFT, false);
                        break;
                    case SearchCriterias.UNFLAGGED:
                        /*
                         * Messages that do not have the \Flagged flag set.
                         */
                        messages = CheckFlaggedMessage(SearchCriterias.FLAGGED.ToString(), false);
                        break;
                    case SearchCriterias.UNKEYWORD:
                        /*
                         * Messages that do not have the specified keyword flag set.
                         */
                        messages = CheckFlaggedMessage(searchCriterias[1], false);
                        break;
                    case SearchCriterias.UNSEEN:
                        /*
                         * Messages that do not have the \Seen flag set.
                         */
                        messages = CheckFlaggedMessage(Flags.SEEN, false);
                        break;

                    default:
                        break;
                }
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
                    foreach (var to in email.To)
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
                else if (criteria == SearchCriterias.CC.ToString())
                {
                    foreach (var cc in email.Cc)
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
                }
                else if (criteria == SearchCriterias.HEADER.ToString())
                {
                    foreach (var header in email.Headers)
                    {
                        if (header.Value.Contains(subStringToFind))
                        {
                            messages.Add(email.MsgNum);
                        }
                    }
                }
                else if (criteria == SearchCriterias.UID.ToString())
                {
                    if (email.MessageId == subStringToFind)
                    {
                        messages.Add(email.MsgNum);
                    }
                }
            }
            return messages;
        }
    }
}
