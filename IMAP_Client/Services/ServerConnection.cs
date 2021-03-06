﻿using IMAP_Client.UpdateEvents;
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

        public string outgoingTag = ""; //The last tag sent.
        public string outgoingCommand = ""; //The last command sent.
        string wholeServerResponse = ""; //The whole response for a command as one block of string.

        public async Task SendMessage(String message, IEventAggregator _eventAggregator = null)
        {
            try
            {
                message += Environment.NewLine;
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                await Task.Delay(1000);
                // Send the message to the connected TcpServer. 
                await networkStream.WriteAsync(data, 0, data.Length);

                outgoingTag = message.Split(' ')[0];
                outgoingCommand = message.Split(' ')[1]; 
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public void GetMessages(IEventAggregator _eventAggregator)
        {
            ResponseHandler._eventAggregator = _eventAggregator;
            String response = "";

            // Bytes Array to receive Server Response.
            Byte[] data = new Byte[512];

            Task.Run(async () =>
            {

                // Read the Tcp Server Response Bytes.
                try
                {
                    Int32 bytes;
                    while ((bytes = await networkStream.ReadAsync(data, 0, data.Length)) != 0)
                    {
                        //Int32 bytes = await networkStream.ReadAsync(data, 0, data.Length);
                        response = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                        _eventAggregator?.GetEvent<UpdateUserConsole>().Publish(response);

                        if (response.Contains("BYE"))
                        {
                            ResponseHandler.Bye();
                            Disconnect();
                            break;
                        }

                        var responseTokens = response.Split(Environment.NewLine);
                        responseTokens = responseTokens.Where(i => i != "").ToArray();


                        if (responseTokens.Last().Split(' ')[0] == outgoingTag)
                        {
                            //Here we can handle a server result (it means that the command execution is finished
                            //on server side and we can start handle its result on the server).
                            wholeServerResponse += response;
                            ResponseHandler.HandleResponse(outgoingCommand, response);
                            wholeServerResponse += "";
                        }
                        else if (response.Split(' ')[0] == "*" || response.Split(' ')[0] == "+")
                        {
                            //Over here we can add a condition for a currently occuring command
                            //for an instance, sending encryption keys if the server response is "+",
                            //or when we want to read the information a bit by a bit.
                            wholeServerResponse += response;
                        }
                        else
                        {
                            //Illegal server response. Should usually not be the case.
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("You are now disconnected from the server.");
                    Disconnect();
                    ResponseHandler.Bye();
                    //System.Windows.MessageBox.Show(ex.Message);
                    //break;
                }


            });
        }


        public void Disconnect()
        {
            networkStream.Close();
            networkStream.Dispose();
            tcpClient.Close();
        }
    }
}
