using IMAP_Client.UpdateEvents;
using Prism.Events;
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

        public async Task<string> SendMessage(String message, IEventAggregator _eventAggregator = null)
        {
            String response = "";// String.Empty;

            try
            {
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

                // Send the message to the connected TcpServer. 
                await networkStream.WriteAsync(data, 0, data.Length);

                // Bytes Array to receive Server Response.
                data = new Byte[512];


                //while (!(response.Contains("OK") || response.Contains("NO") || response.Contains("BAD") || response.Contains("BYE")))
                while(!response.Contains(message.Split(' ')[0])) //Could also do while contains * but we sent it as a tag in connect.
                {
                    // Read the Tcp Server Response Bytes.
                    try
                    {
                        Int32 bytes = await networkStream.ReadAsync(data, 0, data.Length);
                        response = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                        _eventAggregator?.GetEvent<UpdateUserConsole>().Publish(response);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
                return $"{e}";
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
