using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LibraryServer
{
    class Program
    {
        static void Main(string[] args)
        {         
            StartServer();
        }

        public static void StartServer()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            try
            {
                Socket handler = null;
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);

                try
                {
                    listener.Listen(10);

                    Console.WriteLine("Waiting for a connection...");
                    while (true)
                    {
                        handler = listener.Accept();
                        ServerThread server = new ServerThread();
                        server.StartServerThread(handler);

                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
