using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace IMAP.Shared
{
    public class ConnectionState
    {
        public bool Connected { get; set; }
        public bool Authentificated { get; set; }
        public bool SelectedMail { get; set; }
        public Timer Timer { get; set; }

        public ConnectionState()
        {
            Timer = new Timer(10000);
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Logout is executed");
        }
    }
}
