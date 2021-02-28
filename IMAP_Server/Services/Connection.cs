using IMAP_Server.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace IMAP_Server
{
    /// <summary>
    /// An object representing a connection to a client. When a connection ends (the connection recognizes it by catching
    /// an exception in the stream reading), it will be removed from the dictionary that's holding the active connections.
    /// </summary>
    public class Connection
    {
        public string Ip { get; set; } //Ip:Port of the client.

        private TcpClient connection; //The tcp socket for the client.

        public NetworkStream Stream { get; set; } //Network stream used to send and get messages.

        public bool Connected { get; set; } //Don't know if we need this (maybe on client, not on server)

        private int _emptyString = 0;
        public int EmptyStringsNumber
        {
            get
            {
                return _emptyString;
            }
            set
            {
                if (value >2)
                {
                    Log.Logger.Information($"Client has sent 3 empty strings in a row. Ending session with {Ip}/{Username}");
                    CloseConnection();
                }
                _emptyString = value;
            }
        }

        //Booleans to check what the client is able to do..
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

        public string Username { get; set; } = "ANONYMOUS"; //As soon as the client logs in, this is filled.
        public bool SelectedState { get; set; }
        public Mailbox SelectedMailBox { get; set; } //If a mailbox is selected, which one.

        public CancellationTokenSource token; //A token source that is used as a cancelable token.

        public System.Timers.Timer Timer { get; set; } //A timer that ticks until the timeout function.
        private const double timeout = 600000; //The time the client has until a timeout.

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

        //Upon getting a message, or a noop, reset the timeout countdown.
        public void ResetTimeout()
        {
            Timer.Stop();
            Timer.Interval = timeout;
            Timer.Start();
        }

        //What will happen on a timeout.
        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            SendToStream("* BYE - Connection timed out.");
            Console.WriteLine($"Timeout Logout for {Ip}/{Username} is executed.");

            CloseConnection();
        }


        //A function used to send messages to the client.
        public void SendToStream(string response)
        {
            if (response != "")
            {
                response += Environment.NewLine;
                EmptyStringsNumber = 0;
            }
            else
            {
                EmptyStringsNumber++;
            }

            if (EmptyStringsNumber < 3)
            {
                try
                {
                    Log.Logger.Information($"SENT RESPONSE : {response}");
                    Byte[] reply = System.Text.Encoding.UTF8.GetBytes(response);
                    if (Stream.CanWrite)
                    {
                        Stream.WriteAsync(reply, 0, reply.Length);
                        Stream.FlushAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    CloseConnection();
                }
            }
        }

        //A function used to receive messages form the client (used in a loop).
        public async Task<string> ReceiveFromStream(MessageHandler messageHandler, string client)
        {
            ResetTimeout();

            string data = null;
            Byte[] bytes = new Byte[4096];
            int i = 0;
            try
            {
                while((i = await Stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    string hex = BitConverter.ToString(bytes);
                    data = Encoding.UTF8.GetString(bytes, 0, i);
                    Log.Logger.Information($"RECEIVED : {data}");
                    if (data != null)
                    {                     
                        await messageHandler.HandleMessage(data, client);
                    }
                }
                token.Cancel();           
            }
            catch (Exception ex)
            {
                Log.Logger.Information("Either client logged out, net problem or a Timeout, disconnecting. ");// + ex.Message);
                try
                {
                }
                catch { }
                CloseConnection();
            }

            return data;
        }

        //What to do when the connection is closed. This is called on either forced connection closure, timeout,
        //or a "graceful" logout.
        public void CloseConnection()
        {
            try
            {
                token.Cancel();

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }

            Timer.Dispose();
            Stream.Dispose();
            connection.Close();

        }

    }
}
