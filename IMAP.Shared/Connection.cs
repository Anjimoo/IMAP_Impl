using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IMAP.Shared
{
    public class Connection
    {
        public string Ip { get; set; }

        public NetworkStream Stream { get; set; }

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

        public Connection(string ip)
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

        public void SendToStream(string response)
        {
            Byte[] reply = System.Text.Encoding.UTF8.GetBytes(response);
            Stream.WriteAsync(reply, 0, reply.Length);
            //Log.Logger.Information($"SENT : {response}");
        }

        public async Task<string> ReceiveFromStream()
        {
            string data = null;
            Byte[] bytes = new Byte[256];
            int i = 0;

            while ((i = await Stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
            {
                string hex = BitConverter.ToString(bytes);
                data = Encoding.UTF8.GetString(bytes, 0, i);
                //messageHandler._connections.TryAdd(client, new Connection(client) { Stream = stream });
                //messageHandler.HandleMessage(data, Ip); //, stream);
            }

            return data;
        }

        public void CloseConnection()
        {
            //**TODO: Finish this.
        }

    }
}
