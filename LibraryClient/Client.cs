﻿using LibraryManagement.ChainOfResponsability;
using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using LibraryManagement.Observer.Logger;
using LibraryManagement.Observer.Logging;
using LibraryManagement.Proxy.ChangePassword;
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
    public class Client
    {
        private static Library Library = Library.Instance;
        private IPHostEntry host;
        private IPAddress ipAddress;
        private IPEndPoint remoteEP;
        private Socket sender;

        private Logger logger;

        public Client(IPHostEntry host, IPAddress ipAddress, IPEndPoint remoteEP, Socket sender)
        {
            this.host = host;
            this.ipAddress = ipAddress;
            this.remoteEP = remoteEP;
            this.sender = sender;

            // Connect to Remote EndPoint  
            sender.Connect(remoteEP);
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            InitializeLogger();
        }

        private void InitializeLogger()
        {
            string outputPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            logger = Logger.Instance;
            logger.AddObservers(new List<ILoggingService>()
                            {
                                new ConsoleLogger(),
                                new FileLogger(new FileInfo(Path.Combine(outputPath, "logs.txt")))
                            }
            );
        }

        private AuthenticationOption ShowFirstMenu()
        {
            int userOption;
            Console.WriteLine("1.Register");
            Console.WriteLine("2.Login");
            try
            {
                userOption = Convert.ToInt32(Console.ReadLine());
                if (Enum.IsDefined(typeof(AuthenticationOption), userOption))
                {
                    return (AuthenticationOption)userOption;
                }

                return AuthenticationOption.Invalid;
            }
            catch
            {
                Console.WriteLine("Please choose a valid option");
                return AuthenticationOption.Invalid;
            }
        }

        private User Register()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("\nPassword: ");
            string password = Console.ReadLine();
            Console.Write("\nFirst Name: ");
            string firstname = Console.ReadLine();
            Console.Write("\nLast name: ");
            string lastname = Console.ReadLine();
            DateTime endTime = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);

            Console.WriteLine("Gender (choose the number)");
            Console.WriteLine("1.Male");
            Console.WriteLine("2.Female");
            Console.WriteLine("3.Unknown");

            int genderOption = Convert.ToInt32(Console.ReadLine());
            Gender gender = (Gender)genderOption;

            User user = new User(firstname, lastname, endTime, gender);
            user.Username = username;
            user.Password = password;
            user.Role = Role.User;
            return user;
        }

        private User Login()
        {
            User user = new User();
            Console.Write("Username: ");
            user.Username = Console.ReadLine();
            Console.Write("\nPassword: ");
            user.Password = Console.ReadLine();

            return user;
        }

        private UserMenuOptions ShowHome()
        {
            int userOption;
            Console.WriteLine("1.See Books");
            Console.WriteLine("2.Choose a book");
            Console.WriteLine("3.Borrow Book");
            Console.WriteLine("4.Return Book");
            Console.WriteLine("5.See the books borrowed");
            Console.WriteLine("6.Change Password");
            Console.WriteLine("0.Exit");
            try
            {
                userOption = Convert.ToInt32(Console.ReadLine());
                if (Enum.IsDefined(typeof(UserMenuOptions), userOption))
                {
                    return (UserMenuOptions)userOption;
                }

                return UserMenuOptions.Invalid;
            }
            catch
            {
                Console.WriteLine("Please choose a valid option");
                return UserMenuOptions.Invalid;
            }
        }


        private AdminMenuOptions ShowHomeAdmin()
        {
            int userOption;
            Console.WriteLine("1.See Books");
            Console.WriteLine("2.Add Books");
            Console.WriteLine("3.See Report");
            Console.WriteLine("0.Exit");
            try
            {
                userOption = Convert.ToInt32(Console.ReadLine());
                if (Enum.IsDefined(typeof(AdminMenuOptions), userOption))
                {
                    return (AdminMenuOptions)userOption;
                }

                return AdminMenuOptions.Invalid;
            }
            catch
            {
                Console.WriteLine("Please choose a valid option");
                return AdminMenuOptions.Invalid;
            }
        }

        private void ChangePassword(User userNew)
        {
            SafeUserProxy safeUserProxy = new SafeUserProxy();
            User user = safeUserProxy.ChangePassword(userNew);
            if (user != null)
            {
                byte[] msg = Encoding.ASCII.GetBytes("6");
                int bytesSent = sender.Send(msg);
                User user2 = new User(user.IdUser, user.Username, user.FirstName, user.LastName, user.Password);
                string serializedObject = JToken.FromObject(user2).ToString();
                msg = Encoding.ASCII.GetBytes(serializedObject);
                bytesSent = sender.Send(msg);

            }
            else
            {
                Console.WriteLine("Old password fail");
            }
        }

        private void UserMenu(User userNew, Menu menu)
        {
            byte[] bytes = new byte[1024];
            bool cont = true;
            while (cont)
            {
                var userOption = ShowHome();

                switch (userOption)
                {
                    case UserMenuOptions.Exit:
                        cont = false; 
                        break;
                    case UserMenuOptions.SeeBooks:
                        Console.Clear();
                        Library.SeeBooks();
                        menu.UpdateState(EUserOption.SeeBooks);
                        break;
                    case UserMenuOptions.ChooseBook:
                        Console.WriteLine("Choose the book you want");
                        byte[] msg = Encoding.ASCII.GetBytes("2");
                        int bytesSent = sender.Send(msg);

                        int idBook = Convert.ToInt32(Console.ReadLine());
                        msg = Encoding.ASCII.GetBytes(idBook.ToString());
                        bytesSent = sender.Send(msg);

                        int bytesRec = sender.Receive(bytes);
                        string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        Book book = null;
                        try
                        {
                            if (JToken.Parse(receivedMessage).ToObject<Fiction>() != null)
                            {
                                book = JToken.Parse(receivedMessage).ToObject<Fiction>();
                                userNew.CurrentChoose.Add(book);
                                menu.UpdateState(EUserOption.ChooseBooks);
                            }
                        }
                        catch(Exception)
                        {
                            Console.WriteLine("Book not found");
                        }
                                                                
                        break;
                    case UserMenuOptions.BorrowBook:
                        Console.Clear();

                        menu.UpdateState(EUserOption.BorrowBooks);
                        break;
                    case UserMenuOptions.ReturnBook:
                        Console.Clear();
                        menu.UpdateState(EUserOption.ReturnBook);
                        break;
                    case UserMenuOptions.BorrowedBooks:
                        Console.Clear();
                        userNew.BorrowedBooks.ForEach(book => Console.WriteLine(book.Value));
                        break;
                    case UserMenuOptions.ChangePassword:
                        Console.Clear();
                        ChangePassword(userNew);

                        Console.Clear();
                        break;
                    case UserMenuOptions.Invalid:
                        break;
                    default:
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
                var adminOption = ShowHomeAdmin();

                switch (adminOption)
                {
                    case AdminMenuOptions.Exit:
                        cont = false;
                        break;
                    case AdminMenuOptions.SeeBooks:
                        Console.Clear();
                        Library.SeeBooks();
                        break;
                    case AdminMenuOptions.AdBook:
                        AddBook();
                        break;
                    case AdminMenuOptions.SeeReport:
                        byte[] msg = Encoding.ASCII.GetBytes("3");
                        int bytesSent = sender.Send(msg);
                        int bytesRec = sender.Receive(bytes);
                        string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        Console.WriteLine("Report: \n" + receivedMessage);
                        bytesRec = sender.Receive(bytes);
                        receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        Console.WriteLine("Total borrow books:" + receivedMessage);

                        break;

                }
            }
        }

        public void StartClient()
        {
            try
            {
                User userNew = null;
                Menu menu = null;

                logger.Log(Status.Info, "Finished generating books");
                //books.ForEach(book => Console.WriteLine(book));


                byte[] bytes = new byte[1024];
                bool isActive = true;
                while (isActive)
                {
                    var userOption = ShowFirstMenu();
                    switch (userOption)
                    {
                        case AuthenticationOption.Register:
                            userNew = Register();
                            break;
                        case AuthenticationOption.Login:
                            userNew = Login();
                            break;
                        case AuthenticationOption.Invalid:
                            Console.WriteLine("Invalid Option");
                            break;
                        default:
                            break;

                    }
                    if (userOption != AuthenticationOption.Invalid)
                    {
                        KeyValuePair<int, User> data = new KeyValuePair<int, User>((int)userOption, userNew);
                        string serializedObject = JToken.FromObject(data).ToString();
                        byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
                        int bytesSent = sender.Send(msg);
                        int bytesRec = sender.Receive(bytes);
                        string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        try
                        {
                            userNew = JToken.Parse(receivedMessage).ToObject<User>();
                            isActive = false;
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
