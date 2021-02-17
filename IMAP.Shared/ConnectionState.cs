using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace IMAP.Shared
{
    public class ConnectionState
    {
        public string Ip { get; set; }
        public bool Connected { get; set; } //Don't know if we need this (maybe on client, not on server)

        private bool _Authentificated;
        public bool Authentificated 
        {
            get
            {
                return _Authentificated;
                
            } 
            set 
            {
                if (value == false)
                {
                    SelectedMailBox = false;
                }
                _Authentificated = value;
            } 
        }

        public string Username { get; set; } = "ANONYMOUS";
        public bool SelectedMailBox { get; set; }


        public Timer Timer { get; set; }

        public ConnectionState(string ip)
        {
            Ip = ip;
            //Timer = new Timer(10000);
            //Timer.Elapsed += OnTimedEvent;
            //Timer.AutoReset = true;
            //Timer.Enabled = true;
        }

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Logout is executed");
        }

        public void CloseConnection()
        {
            //**TODO: Finish this.
        }
    }
}
