using IMAP.Shared;
using IMAP_Server.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using IMAP_Server.Services;
using System.Threading.Tasks;
using MimeKit;

namespace IMAP_Server
{

    public class Server
    {
        private string ip;
        private int port;
        private TcpListener _server = null;
        private MessageHandler messageHandler;

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
            GenerateUsers();
        }

        /// <summary>
        /// How the listening goes on the server. The server will accept connections asynchronously - and only act upon getting a connection. 
        /// </summary>
        public async Task StartListening()
        {
            _server.Start();
            Log.Logger.Information($"Listening on {ip}:{port}");
                        
            while (true)
            {
                try
                {
                    CancellationTokenSource cancellationTokenSourceClient = new CancellationTokenSource();
                    var cancelToken = cancellationTokenSourceClient.Token;
                    var tcpClient = await _server.AcceptTcpClientAsync();
                    //var tcpClient = await _server.AcceptTcpClientAsync();
                    _ =Task.Run(async () =>
                    {
                        try
                        {
                            using (tcpClient) //Will be disposed at the end of the "using".
                            {
                                await HandleConnection(tcpClient, cancellationTokenSourceClient);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.Error(ex.Message);
                        }
                    }, cancelToken);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex.Message);
                    break;
                }
            }

        }


        /// <summary>
        /// How the server handles each of the connections. It will be added to the connections dicionary in the message handler
        /// and wait for this connection to send commands. Both the client and the server will always wait for eachother to send something,
        /// while considering that the client is the that preempts the commands. The only time a server will send something on it own accord
        /// is upon a timeout. The server will always finish handling 1 command before moving on to the next.
        /// </summary>
        private async Task HandleConnection(TcpClient tcpClient, CancellationTokenSource token)
        {
            var client = tcpClient.Client.RemoteEndPoint.ToString();

            Log.Logger.Information($"Received incoming connection from {client}");


            Connection con = new Connection(client, tcpClient, token);
            
            //con.SendToStream($"* OK greetings.");


            messageHandler._connections.TryAdd(client, con);
            while (true)
            {
                var command = await con.ReceiveFromStream();
                if (command != null)
                {
                    //if (command.Split(' ').Length == 1)
                    //{
                    //    //Here would go user input that consists of only 1 block of string. This is possible in the 
                    //    //case of authentication key - a command that requires back-and-forth communication by the server
                    //    //and the client. Should we have enough time, we might implement this.

                    //    //Should it not answer the correct format for handling, a Default should be returned as a reponse.
                    //}
                    //else
                    
                    await messageHandler.HandleMessage(command, client);
                    
                }
            }
            con.token.Dispose();
            messageHandler._connections.Remove(client);
            Log.Logger.Information($"Client {con.Ip}/{con.Username} is now disconnected.");
        }



        //Hard coded things, needs to be changed to some DB or text file to hold these.
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
            mailBoxes.Add("hod@gmail.com", new Mailbox() { mailboxName = "hod@gmail.com" });
        }
        private void GenerateUsers()
        {
            users = JsonParser.ReadUsers();

            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Joey", "joey@friends.com"));
            //message.To.Add(new MailboxAddress("Alice", "alice@wonderland.com"));
            //message.Subject = "How you doin?";
            
            //message.Body = new TextPart("plain")
            //{
            //    Text = @"Hey Alice,

            //             What are you up to this weekend? Monica is throwing one of her parties on
            //             Saturday and I was hoping you could make it.

            //             Will you be my +1?

            //             =-- Joey"
            //};
        }   
    }
}
