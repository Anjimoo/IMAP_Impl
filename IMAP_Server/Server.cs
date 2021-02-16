using IMAP.Shared;
using IMAP.Shared.Models;
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
        private TcpListener _server = null;
        private MessageHandler messageHandler;
        private string _response;
        public static Dictionary<string, Mailbox> mailBoxes;
        
        public Server()
        {
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            messageHandler = new MessageHandler();
            CreateMailBoxes();
            _server = new TcpListener(localAddress, 143);
            _server.Start();          
        }

        public void StartListening()
        {
            try
            {
                Log.Logger.Information($"Listening on 127.0.0.1:143");
                while (true)
                {                   
                    TcpClient client = _server.AcceptTcpClient();
                    Log.Logger.Information($"Received incoming connection.");
                    try
                    {
                        var ignored = Task.Run(async () =>
                        {
                            await HandleConnection(client);
                            client.Dispose();
                        });
                    }
                    catch(Exception e)
                    {
                        Log.Logger.Error(e, "Connections is faulted");
                    }                   
                }

            }catch(SocketException ex)
            {
                Log.Logger.Error(ex, "Error");
                _server.Stop();
            }
        }

        private async Task HandleConnection(TcpClient tcpClient)
        {
            
            var stream = tcpClient.GetStream();

            
            string data = null;
            Byte[] bytes = new Byte[256];
            int i;

            try
            {
                while((i = stream.Read(bytes, 0 , bytes.Length)) != 0)
                {
                    string hex = BitConverter.ToString(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    var client = tcpClient.Client.RemoteEndPoint;

                    Log.Logger.Information($"{data} received from {client}");

                    messageHandler._connections.TryAdd(client.ToString(), new ConnectionState(client.ToString()));
                    messageHandler.HandleMessage(data, client.ToString(), stream);
                }
            }catch(Exception e)
            {
                Console.WriteLine($"Exception : {e}");
            }
        }

        private void CreateMailBoxes()
        {
            mailBoxes = new Dictionary<string, Mailbox>();
            mailBoxes.Add("Jimoo@gmail.com", new Mailbox() { mailboxName = "Jimoo@gmail.com" });
        }
    }
}
