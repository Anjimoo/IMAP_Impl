using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Server.Services
{
    public class IMAP_Fetch
    {
        private const string ALL = "ALL";
        private const string FAST = "FAST";
        private const string FULL = "FULL";
        private static Connection _connection;
        public static void TryFetch(string[] fetchCriterias, Connection connection)
        {
            _connection = connection;
            var sequenceSet = GetSequenceSet(fetchCriterias[0]);

            if (fetchCriterias[1].Contains(ALL))
            {

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
    }
}
