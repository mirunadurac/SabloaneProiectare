﻿using LibraryManagement.FactoryMethod;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LibraryClient
{
    class Program
    {
        static void Main(string[] args)
        {
            StartClient();
        }

        public static void StartClient()
        {

            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.    
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    while (true)
                    {
                        byte[] bytes = new byte[1024];

                        string message = Console.ReadLine();

                        var foo = new Fiction(0, "Title", "Me", 199999);
                        string serializedObject = JToken.FromObject(foo).ToString();

                        // Encode the data string into a byte array.    
                        byte[] msg = Encoding.ASCII.GetBytes(serializedObject);

                        // Send the data through the socket.    
                        int bytesSent = sender.Send(msg);

                        // Receive the response from the remote device.   

                        int bytesRec = sender.Receive(bytes);
                        string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        Console.WriteLine("Echoed test = {0}", receivedMessage);

                        if (receivedMessage.Equals("Stop"))
                        {
                            break;
                        }
                    }
                    // Release the socket.    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
