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
            Client client = new Client();
            client.StartClient();
        }

    }
}
