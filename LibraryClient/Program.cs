using LibraryManagement.FactoryMethod;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LibraryManagement;
using LibraryManagement.Singleton;
using System.IO;
using System.Reflection;
using LibraryManagement.Observer.Logging;
using System.Collections.Generic;
using LibraryManagement.Observer.Logger;
using LibraryManagement.Models;
using LibraryManagement.State;
using LibraryManagement.Utils;
using LibraryManagement.ChainOfResponsability;

namespace LibraryClient
{
    class Program
    {
        
        static void Main(string[] args)
        {

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Client client = new Client(host, ipAddress, remoteEP, sender);
            client.StartClient();
        }

    }
}
