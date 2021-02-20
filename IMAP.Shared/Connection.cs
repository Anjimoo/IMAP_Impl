using IMAP.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace IMAP.Shared
{
    public class Connection
    {
        public string Ip { get; set; }

        private TcpClient connection;

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
                    SelectedState = false;
                }
                _Authentificated = value;
            }
        }

        public string Username { get; set; } = "ANONYMOUS";
        public bool SelectedState { get; set; }
        public Mailbox SelectedMailBox { get; set; }

        public CancellationTokenSource token;

        public System.Timers.Timer Timer { get; set; } //FOR NOW, **TODO: try this later on.
        private const double timeout = 60000;

        public Connection(string ip, TcpClient conn, CancellationTokenSource token = null)
        {
            Ip = ip;
            connection = conn;
            Stream = conn.GetStream();

            if (token != null)
            {
                this.token = token;
            }
            StartTimeout();
        }


        private void StartTimeout()
        {
            Timer = new System.Timers.Timer(timeout);
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = false;
            Timer.Start();
        }

        public void ResetTimeout()
        {
            Timer.Stop();
            Timer.Interval = timeout;
            Timer.Start();
        }

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            SendToStream("* BYE - Connection timed out.");
            Console.WriteLine($"Timeout Logout for {Ip}/{Username} is executed.");

            CloseConnection();
        }

        public void SendToStream(string response)
        {
            try
            {
                Byte[] reply = System.Text.Encoding.UTF8.GetBytes(response);
                Stream.WriteAsync(reply, 0, reply.Length);
                //Log.Logger.Information($"SENT : {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Timer.Dispose();
            }
        }

        public async Task<string> ReceiveFromStream()
        {
            ResetTimeout();

            string data = null;
            Byte[] bytes = new Byte[512];
            int i = 0;
            try
            {
                i = await Stream.ReadAsync(bytes, 0, bytes.Length);
                string hex = BitConverter.ToString(bytes);
                data = Encoding.UTF8.GetString(bytes, 0, i);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Either client logged out, net problem or a Timeout: " + ex.Message);
                Timer.Dispose();
                CloseConnection();
            }

            return data;
        }

        public void CloseConnection()
        {
            try
            {
                token.Cancel();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Stream.Dispose();
            connection.Close();
        }

    }
}
