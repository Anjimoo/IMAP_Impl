using IMAP.Shared;
using IMAP.Shared.Models;
using IMAP_Server.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace IMAP_Server
{

    public class Server
    {
        private string ip;
        private int port;
        private TcpListener _server = null;
        private MessageHandler messageHandler;
        private string _response;

        public static Dictionary<string, User> users;

        public static Dictionary<string, Mailbox> mailBoxes;

        public static HashSet<Mailbox> subscriberMailboxes;


        public Server(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            IPAddress localAddress = IPAddress.Parse(ip);
            messageHandler = new MessageHandler();
            DefinePermFlags();
            CreateMailBoxes();
            subscriberMailboxes = new HashSet<Mailbox>();
            _server = new TcpListener(localAddress, port);
            //_server.Start();    
            GenerateUsers();
        }

        public async Task StartListening()
        {
            _server.Start();
            Log.Logger.Information($"Listening on {ip}");

            //var ignored = Task.Run(async () =>
            //{

            while (true)
            {
                try
                {
                    CancellationTokenSource cancellationTokenSourceClient = new CancellationTokenSource();
                    var cancelToken = cancellationTokenSourceClient.Token;
                    var tcpClient = await _server.AcceptTcpClientAsync();
                    _=Task.Run(async () =>
                    {
                        try
                        {
                            using (tcpClient)
                            {
                                await HandleConnection(tcpClient, cancellationTokenSourceClient);
                            }
                            //tcpClient.Dispose();
                        }
                        catch (OperationCanceledException ex)
                        {
                            Log.Logger.Information("Client disconnected.");  //***************Cancelation token doesn't work 
                            //as intended and it might be a problem with the timers. TODO: Fix this when we have time.
                            cancellationTokenSourceClient.Dispose();
                        }
                    }, cancelToken);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex.Message);
                }
            }

            //});
        }



        private async Task HandleConnection(TcpClient tcpClient, CancellationTokenSource token)
        {
            //var stream = tcpClient.GetStream();
            var client = tcpClient.Client.RemoteEndPoint.ToString();

            Log.Logger.Information($"Received incoming connection from {client}");


            Connection con = new Connection(client, tcpClient, token);
                        
            messageHandler._connections.TryAdd(client, con);
            while (!con.token.IsCancellationRequested)
            {
                var command = await con.ReceiveFromStream();
                if (command != null)
                {
                    await messageHandler.HandleMessage(command, client);
                }
            }
            con.token.Dispose();
            messageHandler._connections.Remove(client);
        }
        private void DefinePermFlags()
        {
            PermanentFlags.PermaFlags.Add(Flags.ANSWERED);
            PermanentFlags.PermaFlags.Add(Flags.DELETED);
            PermanentFlags.PermaFlags.Add(Flags.SEEN);
        }
        private void CreateMailBoxes()
        {
            mailBoxes = new Dictionary<string, Mailbox>();
            mailBoxes.Add("Jimoo@gmail.com", new Mailbox() { mailboxName = "Jimoo@gmail.com" });
        }
        private void GenerateUsers()
        {
            users = new Dictionary<string, User>();
            users.Add("Jimoo", new User() { Username = "Jimoo", Password = "123" });
            users.Add("Shiro", new User() { Username = "Shiro", Password = "123" });
            users.Add("Diximango", new User() { Username = "Diximango", Password = "123" });
        }

        //public void StartListeningOld()
        //{
        //    GenerateUsers(); //Just for now. We may add them using another method.

        //    try
        //    {
        //        Log.Logger.Information($"Listening on {ip}");
        //        while (true)
        //        {
        //            TcpClient client = _server.AcceptTcpClient();
        //            Log.Logger.Information($"Received incoming connection.");
        //            try
        //            {
        //                var ignored = Task.Run(async () =>
        //                {
        //                    await HandleConnectionOld(client);
        //                    client.Dispose(); //At the end of the connection by "logout", not here
        //                });
        //            }
        //            catch (Exception e)
        //            {
        //                Log.Logger.Error(e, "Connections is faulted");
        //            }
        //        }

        //    }
        //    catch (SocketException ex)
        //    {
        //        Log.Logger.Error(ex, "Error");
        //        _server.Stop();
        //    }
        //}

        //private async Task HandleConnectionOld(TcpClient tcpClient)
        //{

        //    var stream = tcpClient.GetStream();


        //    string data = null;
        //    Byte[] bytes = new Byte[256];
        //    int i;

        //    try
        //    {
        //        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        //        {
        //            string hex = BitConverter.ToString(bytes);
        //            data = Encoding.UTF8.GetString(bytes, 0, i);
        //            var client = tcpClient.Client.RemoteEndPoint.ToString();

        //            Log.Logger.Information($"{data} received from {client}");

        //            //messageHandler._connections.TryAdd(client, new Connection(client) {Stream=stream});
        //            messageHandler.HandleMessage(data, client);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Exception : {e}");
        //    }
        //}


    }
}
