using IMAP.Shared;
using IMAP_Server.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Server.CommandModels
{
    public static class SelectStateCommands
    {
        public static void Check(string[] command, Connection connectionState)
        {
            
        }
        public static void Close(string[] command, Connection connectionState)
        {
            
        }
        public static void Copy(string[] command, Connection connectionState)
        {
            
        }
        public static void Expunge(string[] command, Connection connectionState)
        {
            
        }
        public static void Fetch(string[] command, Connection connectionState)
        {
            
        }
        public static void Search(string[] command, Connection connectionState)
        {
            if(command.Length > 2)
            {
                if (command.Contains("OR"))
                {
                    //need to do search 2 times?
                }else if (command.Contains("AND"))
                {
                    //need to do search 2 times and filter
                }
                
                IMAP_Search.Search(command.Take(2).ToArray());
            }
            else
            {
                var str = "BAD - no search criteria specified";
            }
            
        }
        public static void Store(string[] command, Connection connectionState)
        {

        }
        public static void UID(string[] command, Connection connectionState)
        {

        }
    }
}
