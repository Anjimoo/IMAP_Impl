using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using IMAP_Server.Models;

namespace IMAP_Server.Services
{
    class JsonParser
    {
        //private static string AppPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        //private static DirectoryInfo myDir = new DirectoryInfo(AppPath);
        //public static string dataDir = myDir.Parent.FullName.ToString();


        public static Dictionary<string, User> ReadUsers()
        {
            //string path = Path.Combine(dataDir,"Users.json"); 
            var users = new Dictionary<string, User>();
            string jsonString = File.ReadAllText("Users.json");
            //string jsonString = File.ReadAllText(path);
            users =JsonSerializer.Deserialize< Dictionary<string, User>> (jsonString);
            return users;
        }
    }
}