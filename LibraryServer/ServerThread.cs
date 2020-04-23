using LibraryManagement.FactoryMethod;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LibraryServer
{
    public class ServerThread
    {
        private Socket clientSocket;
        public void StartServerThread(Socket inClientSocket)
        {
            clientSocket = inClientSocket;
            Thread ctThread = new Thread(Execute);
            ctThread.Start();
        }
        private void Execute()
        {
            // Incoming data from the client. 
            try
            {
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = clientSocket.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    var fooReceived = JToken.Parse(data).ToObject<Fiction>();
                    Console.WriteLine("Text received : {0}", data);

                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    clientSocket.Send(msg);

                    if (data.IndexOf("Stop") > -1)
                    {
                        break;
                    }
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
    }
}
