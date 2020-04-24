using LibraryManagement.ChainOfResponsability;
using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using LibraryManagement.Observer.Logger;
using LibraryManagement.Observer.Logging;
using LibraryManagement.Singleton;
using LibraryManagement.State;
using LibraryManagement.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace LibraryClient
{
    class Client
    {
        static Library Library = Library.Instance;
        IPHostEntry host;
        IPAddress ipAddress;
        IPEndPoint remoteEP;
        Socket sender;

        public Client()
        {
            host = Dns.GetHostEntry("localhost");
            ipAddress = host.AddressList[0];
            remoteEP = new IPEndPoint(ipAddress, 11000);
            sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }


        private int ShowFirstMenu()
        {
            int op;
            Console.WriteLine("1.Register");
            Console.WriteLine("2.Login");
            try
            {
                op = Convert.ToInt32(Console.ReadLine());
                return op;
            }
            catch
            {
                Console.WriteLine("Please choose a valid option");
                return -1;
            }
        }

        private User Register()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("\nPassword");
            string password = Console.ReadLine();
            Console.Write("\nFirst Name");
            string firstname = Console.ReadLine();
            Console.Write("\nLast name");
            string lastname = Console.ReadLine();
            DateTime endTime = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);

            Console.WriteLine("Gender (choose the number)");
            Console.WriteLine("1.Male");
            Console.WriteLine("2.Female");
            Console.WriteLine("3.Unknown");

            int op = Convert.ToInt32(Console.ReadLine());
            Gender gender = (Gender)op;

            User user = new User(firstname, lastname, endTime, gender);
            user.Username = username;
            user.Password = password;
            user.Role = Role.User;
            return user;
        }

        private User Login()
        {
            User user = new User();
            Console.WriteLine("Username");
            user.Username = Console.ReadLine();
            Console.WriteLine("Password");
            user.Password = Console.ReadLine();

            return user;
        }

        private int ShowHome()
        {
            int op;
            Console.WriteLine("1.See Books");
            Console.WriteLine("2.Choose a book");
            Console.WriteLine("3.Borrow Book");
            Console.WriteLine("4.Return Book");
            Console.WriteLine("5.See the books borrowed");
            Console.WriteLine("0.Exit");
            try
            {
                op = Convert.ToInt32(Console.ReadLine());
                return op;
            }
            catch
            {
                Console.WriteLine("Please choose a valid option");
                return -1;
            }
        }

        private int ShowHomeAdmin()
        {
            int op;
            Console.WriteLine("1.See Books");
            Console.WriteLine("2.Add Books");
            Console.WriteLine("0.Exit");
            try
            {
                op = Convert.ToInt32(Console.ReadLine());
                return op;
            }
            catch
            {
                Console.WriteLine("Please choose a valid option");
                return -1;
            }
        }

        private void UserMenu(User userNew, Menu menu)
        {
            byte[] bytes = new byte[1024];
            bool cont = true;
            while (cont)
            {
                int op = ShowHome();

                switch (op)
                {
                    case 0:
                        cont = false;
                        break;
                    case 1:
                        Console.Clear();
                        Library.SeeBooks();
                        menu.UpdateState(EUserOption.SeeBooks);
                        break;
                    case 2:
                        Console.WriteLine("Choose the book you want");
                        byte[] msg = Encoding.ASCII.GetBytes("2");
                        int bytesSent = sender.Send(msg);
                        int idBook = Convert.ToInt32(Console.ReadLine());
                        msg = Encoding.ASCII.GetBytes(idBook.ToString());
                        bytesSent = sender.Send(msg);

                        int bytesRec = sender.Receive(bytes);
                        string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        Book book = JToken.Parse(receivedMessage).ToObject<Fiction>();
                        userNew.CurrentChoose.Add(book);
                        menu.UpdateState(EUserOption.ChooseBooks);

                        break;
                    case 3:
                        Console.Clear();

                        menu.UpdateState(EUserOption.BorrowBooks);
                        break;
                    case 4:
                        Console.Clear();
                        menu.UpdateState(EUserOption.ReturnBook);
                        break;
                    case 5:
                        Console.Clear();
                        userNew.BorrowedBooks.ForEach(book => Console.WriteLine(book.Value));
                        break;
                    case -1:
                        break;

                }
            }
        }


        private Book MakeFiction()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Title ");
                    string title = Console.ReadLine();
                    Console.WriteLine("Author");
                    string author = Console.ReadLine();
                    Console.WriteLine("Publication date (year)");
                    int date = Convert.ToInt32(Console.ReadLine());
                    FictionFactory fictionFactory = new FictionFactory();
                    return fictionFactory.GetBook(title, author, date);
                }
                catch
                {
                    Console.WriteLine("Something wrong");
                }
            }
        }

        private Book MakeNonFiction()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Title ");
                    string title = Console.ReadLine();
                    Console.WriteLine("Author");
                    string author = Console.ReadLine();
                    Console.WriteLine("Publication date (year)");
                    int date = Convert.ToInt32(Console.ReadLine());
                    NonFictionFactory nonFictionFactory = new NonFictionFactory();
                    return nonFictionFactory.GetBook(title, author, date);
                }
                catch
                {
                    Console.WriteLine("Something wrong");
                }
            }
        }

        private void AddBook()
        {
            int op = 0;
            Console.WriteLine("Choose the type");
            Console.WriteLine("1.Fiction");
            Console.WriteLine("2.NonFiction");
            try
            {
                op = Convert.ToInt32(Console.ReadLine());

            }
            catch
            {
                Console.WriteLine("Please choose a valid option");

            }
            switch (op)
            {
                case 1:
                    Library.Books.Add(MakeFiction());
                    break;
                case 2:
                    Library.Books.Add(MakeNonFiction());
                    break;
            }

        }

        private void AdminMenu()
        {
            byte[] bytes = new byte[1024];
            bool cont = true;
            while (cont)
            {
                int op = ShowHomeAdmin();

                switch (op)
                {
                    case 0:
                        cont = false;
                        break;
                    case 1:
                        Console.Clear();
                        Library.SeeBooks();
                        break;
                    case 2:
                        AddBook();
                        break;


                }
            }
        }

        public void StartClient()
        {
            try
            {
                // Connect to Remote EndPoint  
                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                string outputPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var logger = Logger.Instance;
                logger.AddObservers(new List<ILoggingService>()
                            {
                                new ConsoleLogger(),
                                new FileLogger(new FileInfo(Path.Combine(outputPath, "logs.txt")))
                            }
                );

                logger.Log(Status.Info, "Finished generating books");
                //books.ForEach(book => Console.WriteLine(book));

                User userNew = null;
                Menu menu = null;
                byte[] bytes = new byte[1024];
                bool ok = true;
                while (ok)
                {
                    int op = ShowFirstMenu();
                    switch (op)
                    {
                        case 1:
                            userNew = Register();
                            break;
                        case 2:
                            userNew = Login();
                            break;
                        case -1:
                            break;

                    }
                    if (op != -1)
                    {
                        KeyValuePair<int, User> data = new KeyValuePair<int, User>(op, userNew);
                        string serializedObject = JToken.FromObject(data).ToString();
                        byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
                        int bytesSent = sender.Send(msg);
                        int bytesRec = sender.Receive(bytes);
                        string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        try
                        {
                            userNew = JToken.Parse(receivedMessage).ToObject<User>();
                            ok = false;
                            menu = new Menu(userNew);

                            Console.WriteLine("Welcome");
                        }
                        catch
                        {
                            Console.WriteLine(receivedMessage);
                        }

                    }
                }
                if (userNew.Role == Role.User)
                {
                    HeadOffice headOffice = new HeadOffice("Ofiice", "Head", DateTime.Now, null, Gender.Female);
                    Librarian librarian = new Librarian("Librarian", "Last", DateTime.Now, headOffice, Gender.Female);
                    userNew.Supervisor = librarian;
                    Console.Clear();
                    menu.UpdateState(EUserOption.Login);
                    UserMenu(userNew, menu);
                }
                else
                {
                    Console.Clear();
                    AdminMenu();
                }

                // Release the socket.    
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
