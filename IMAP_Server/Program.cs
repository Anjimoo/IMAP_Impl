using System;
using System.Threading.Tasks;
using IMAP_Server.CommandModels;
using Serilog;
using Serilog.Sinks.SystemConsole;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;

namespace IMAP_Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = log;

            Server server;


            //Show the user a list of IPs to choose from.
            List<string> ipAdressesList = new List<string>(); //List of IPs on the machine.
            string chosenIP = "127.0.0.1"; //Default value is localhost.
            int chosenPort = 143; //Default value
            ipAdressesList.Add(chosenIP); //First default IP is localhost
            ipAdressesList.Add("0.0.0.0"); //Second default IP is 0.0.0.0

            //Letting the user choose the server's IP.
            Log.Logger.Information("Displaying IP addresses..." + Environment.NewLine);
            Log.Logger.Information("The available IP addresses for this pc:" + Environment.NewLine);
            Log.Logger.Information("0) " + chosenIP + " (localhost)");
            Log.Logger.Information("1) " + ipAdressesList.Last());



            //Displaying the availble IP addresses that are not default values..
            int i = 2;
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString())
                {
                    ipAdressesList.Add(ip.ToString());
                    Log.Logger.Information(i.ToString() + ") " + ip.ToString());
                    i++;
                }
            }

            //Choosing the right address.
            while (true)
            {
                //User picks the desired IP address.                
                try
                {
                    Console.Write(">");
                    chosenIP = ipAdressesList[Convert.ToInt32(Console.ReadLine())];
                    Log.Logger.Information("IP Address chosen: " + chosenIP);
                    Console.WriteLine();
                    break;
                }

                catch
                {
                    Log.Logger.Error("Invalid address.");
                }
            }

            //Creating the TcpListener
            while (true)
            {
                try
                {
                    Log.Logger.Information("Please choose a port for the server to run on.");
                    Console.Write(">");
                    chosenPort = Convert.ToInt32(Console.ReadLine());
                    server = new Server(chosenIP, chosenPort);
                    break;
                }
                catch 
                {
                    Log.Logger.Error("Port is either taken, or not a valid port at all.");
                }
            }

            try
            {
                await server.StartListening();
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error: " + ex.Message);
            }




        }
    }
}
