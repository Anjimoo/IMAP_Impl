using IMAP.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMAP_Server.Services
{
    public class IMAP_Fetch
    {
        private static Connection _connection;
        /// <summary>
        /// Trying to fetch sequence of specified fields from email messages
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        public static bool TryFetch(string command, Connection connection)
        {
            var fetchTokens = ParseFetchCommand(command).ToArray();
            _connection = connection;
            var sequenceSet = GetSequenceSet(fetchTokens.First());
            string response = "";  
            if (Enum.TryParse<MessageAttributes>(fetchTokens[1], out var fetchCriterion))
            {
                foreach (var message in _connection.SelectedMailBox.EmailMessages)
                {     
                    response = $"* {message.MsgNum} FETCH {command}{Environment.NewLine}";
                    switch (fetchCriterion)
                    {
                        case MessageAttributes.FLAGS:
                            foreach(var flag in message.Flags)
                            {
                                if(flag.Value == true)
                                {
                                    response += $"{flag.Key}{Environment.NewLine}";
                                }                              
                            }
                            _connection.SendToStream(response);
                            break;
                        case MessageAttributes.BODY:

                            if (fetchTokens[2] == "HEADER.FIELDS" && fetchTokens.Length > 3)
                            {
                                for (int i = 3; i < fetchTokens.Length; i++)
                                {
                                    switch (Enum.Parse<SearchCriterias>(fetchTokens[i]))
                                    {
                                        case SearchCriterias.BCC:
                                            foreach(var bcc in message.Bcc)
                                            {
                                                response += $"BCC: {bcc.Name}{Environment.NewLine}";
                                            }
                                            break;
                                        case SearchCriterias.BODY:
                                            response += $"Body: {message.Body}{Environment.NewLine}";
                                            break;
                                        case SearchCriterias.CC:
                                            foreach (var cc in message.Cc)
                                            {
                                                response += $"CC: {cc.Name}{Environment.NewLine}";
                                            }
                                            break;
                                        case SearchCriterias.FROM:
                                            foreach (var from in message.From)
                                            {
                                                response += $"From: {from.Name}{Environment.NewLine}";
                                            }
                                            break;
                                        case SearchCriterias.SUBJECT:
                                            response += $"Subject: {message.Subject}{Environment.NewLine}";
                                            break;
                                        case SearchCriterias.TEXT:
                                            response += $"Text: {message.TextBody}{Environment.NewLine}";
                                            break;
                                        case SearchCriterias.TO:
                                            foreach (var to in message.To)
                                            {
                                                response += $"To: {to.Name}{Environment.NewLine}";
                                            }
                                            break;
                                        case SearchCriterias.UID:
                                            response += $"UID: {message.MessageId}{Environment.NewLine}";
                                            break;
                                        case SearchCriterias.DATE:
                                            response += $"Date: {message.Date.ToLocalTime()}{Environment.NewLine}";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                _connection.SendToStream(response);
                            }
                            break;
                        default:
                            break;
                    }
                }
                return true;
            }else
            {
                return false;
            }
        }
        /// <summary>
        /// Receieves sequence of numbers "(1:5)" in string and parses it to list of message's numbers
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static List<int> GetSequenceSet(string sequence)
        {
            string[] sequenceSet = sequence.Split(':');
            List<int> sequenceList = new List<int>();
            if (sequenceSet.Length >= 2)
            {
                int secondElement;
                if (sequenceSet[1] == "*")
                {
                    secondElement = _connection.SelectedMailBox.EmailMessages.Count + 1;
                }
                else
                {
                    secondElement = Int32.Parse(sequenceSet[1]);
                }
                int firstElement = Int32.Parse(sequenceSet[0]);

                int numberOfElements = secondElement - firstElement;

                for (int i = 0; i <= numberOfElements; i++)
                {
                    sequenceList.Add(firstElement++);
                }
            }
            return sequenceList;
        }
        /// <summary>
        /// Parses command into a list of strings and returns this list.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static List<string> ParseFetchCommand(string command)
        {
            string[] tempCommands = command.Split(' ');
            List<string> fetchTokens = new List<string>();
            if (tempCommands.Length > 2)
            {
                for (int i = 2; i < tempCommands.Length; i++)
                {
                    var token = tempCommands[i].Replace('[', ' ').Replace(']', ' ').Replace('(', ' ').Replace(')', ' ');
                    var splittedTokens = token.Split(' ');
                    if (splittedTokens.Length > 1)
                    {
                        foreach (var sToken in splittedTokens)
                        {
                            if (sToken != "")
                            {
                                fetchTokens.Add(sToken);
                            }
                        }
                    }
                    else
                    {
                        fetchTokens.Add(token);
                    }
                }
            }
            return fetchTokens;
        }
    }
}
