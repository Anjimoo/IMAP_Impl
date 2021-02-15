using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Client.Services
{
    public class ServerConnection
    {
        private Int32 port;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        public ServerConnection(string ip, Int32 port)
        {
            tcpClient = new TcpClient(ip, port);
            networkStream = tcpClient.GetStream();
        }

        public string SendMessage(String message)
        {
            String response = String.Empty;
            try
            {
                
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                networkStream.Write(data, 0, data.Length);
                Console.WriteLine($"Sent: {message}");

                // Bytes Array to receive Server Response.
                data = new Byte[256];
                

                // Read the Tcp Server Response Bytes.
                Int32 bytes = networkStream.Read(data, 0, data.Length);
                response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", response);
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e}");
            }

            return response;
        }

        public void Disconnect()
        {
            networkStream.Close();
            tcpClient.Close();
        }
    }
}
