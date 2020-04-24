using LibraryManagement.Database;
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
            var databaseConnection = DatabaseConnection.Instance;
            databaseConnection.OpenConnection();

             StartServer(databaseConnection);

        }

        public static void StartServer(DatabaseConnection  databaseConnection)
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            try
            {
                Socket handler = null;
                // Create a Socket that will use Tcp protocol      
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method  
                listener.Bind(localEndPoint);

                try
                {
                   
                    // Specify how many requests a Socket can listen before it gives Server busy response.  
                    // We will listen 10 requests at a time  
                    listener.Listen(10);

                    Console.WriteLine("Waiting for a connection...");
                    while (true)
                    {
                        handler = listener.Accept();
                        ServerThread server = new ServerThread(databaseConnection);
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
            catch(Exception e)
                {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
