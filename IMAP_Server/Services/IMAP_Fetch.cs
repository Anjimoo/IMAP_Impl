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
        public static void TryFetch(string command, Connection connection)
        {
            var fetchTokens = ParseFetchCommand(command);
            _connection = connection;
            var sequenceSet = GetSequenceSet(fetchTokens.First());
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
