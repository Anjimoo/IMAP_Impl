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
        public Server()
        {
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            messageHandler = new MessageHandler();
            _server = new TcpListener(localAddress, 143);
            _server.Start();
           
        }

        public void StartListening()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = _server.AcceptTcpClient();
                    Console.WriteLine($"Connected");

                    Thread thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                    thread.Start(client);
                }

            }catch(SocketException ex)
            {
                Console.WriteLine($"SocketException : {ex}");
                _server.Stop();
            }
        }

        private void HandleConnection(object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            string imei = String.Empty;

            string data = null;
            Byte[] bytes = new Byte[256];
            int i;

            try
            {
                while((i = stream.Read(bytes, 0 , bytes.Length)) != 0)
                {
                    string hex = BitConverter.ToString(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine($"{data} received : {Thread.CurrentThread.ManagedThreadId}");

                    _response = messageHandler.HandleMessage(data);

                    
                    Byte[] reply = System.Text.Encoding.ASCII.GetBytes(_response);
                    stream.Write(reply, 0, reply.Length);
                    Console.WriteLine($"{_response} sent : {Thread.CurrentThread.ManagedThreadId}");
                }
            }catch(Exception e)
            {
                Console.WriteLine($"Exception : {e}");
                client.Close();
            }
        }
    }
}
