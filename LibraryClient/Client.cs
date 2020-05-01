using LibraryManagement.ChainOfResponsability;
using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using LibraryManagement.Models.DatabaseModels;
using LibraryManagement.Observer.Logger;
using LibraryManagement.Observer.Logging;
using LibraryManagement.State;
using LibraryManagement.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace LibraryClient
{
    public class Client
    {
        private IPHostEntry host;
        private IPAddress ipAddress;
        private IPEndPoint remoteEP;
        private Socket serverSocket;

        private Logger logger;

        public Client(IPHostEntry host, IPAddress ipAddress, IPEndPoint remoteEP, Socket sender)
        {
            this.host = host;
            this.ipAddress = ipAddress;
            this.remoteEP = remoteEP;
            this.serverSocket = sender;

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
            var username = Console.ReadLine();

            Console.Write("\nPassword: ");
            var password = Console.ReadLine();

            Console.Write("\nFirst Name: ");
            var firstname = Console.ReadLine();

            Console.Write("\nLast name: ");
            var lastname = Console.ReadLine();

            DateTime validity = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);

            Console.WriteLine("Gender (choose the number)");
            Console.WriteLine("1.Male");
            Console.WriteLine("2.Female");
            Console.WriteLine("3.Unknown");

            var genderOption = Convert.ToInt32(Console.ReadLine());
            var gender = Gender.Unknown;
            if (Enum.IsDefined(typeof(AdminMenuOptions), genderOption))
            {
                gender = (Gender)genderOption;
            }

            var registeredUser = new UserDatabase
            {
                Username = username,
                Password = password,
                FirstName = firstname,
                LastName = lastname,
                Validity = validity,
                Gender = gender,
                Role = Role.User
            };

            // TO DO 
            // - validare

            return new User(registeredUser);
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
            //SafeUserProxy safeUserProxy = new SafeUserProxy();
            //User user = safeUserProxy.ChangePassword(userNew);
            //if (user != null)
            //{
            //    byte[] msg = Encoding.ASCII.GetBytes("6");
            //    int bytesSent = sender.Send(msg);
            //    User user2 = new User(user.IdUser, user.Username, user.FirstName, user.LastName, user.Password);
            //    string serializedObject = JToken.FromObject(user2).ToString();
            //    msg = Encoding.ASCII.GetBytes(serializedObject);
            //    bytesSent = sender.Send(msg);

            //}
            //else
            //{
            //    Console.WriteLine("Old password fail");
            //}
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
                        SeeBooks(menu, bytes);
                        break;
                    case UserMenuOptions.ChooseBook:
                        ChooseBook(userNew, menu, bytes);
                        break;
                    case UserMenuOptions.BorrowBook:
                        BorrowBook(menu);
                        break;
                    case UserMenuOptions.ReturnBook:
                        ReturnBook(userNew, menu);
                        break;
                    case UserMenuOptions.BorrowedBooks:
                        SeeBorrowedBooks(userNew);
                        break;
                    case UserMenuOptions.ChangePassword:
                        // TO DO
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

        private static void SeeBorrowedBooks(User userNew)
        {
            Console.Clear();
            userNew.BorrowedBooks.ForEach(book => Console.WriteLine(book.Value));
        }

        private void ReturnBook(User userNew, Menu menu)
        {
            Console.Clear();
            userNew.BorrowedBooks.ForEach(book => Console.WriteLine(book.Value));
            Console.WriteLine("ID: ");
            int id;
            int.TryParse(Console.ReadLine(), out id);

            var bookToReturn = userNew.BorrowedBooks.First(book => book.Value.Id == id);

            if (bookToReturn.Value != null)
            {
                userNew.BorrowedBooks.Remove(bookToReturn);
            }

            byte[] option = Encoding.ASCII.GetBytes("4");
            serverSocket.Send(option);

            var dbBook = new BookDatabase
            {
                Author = bookToReturn.Value.Author,
                PublicationDate = new DateTime(bookToReturn.Value.PublicationDate, 1, 1),
                Title = bookToReturn.Value.Title,
                Type = bookToReturn.Value.Type()

            };

            string serializedObject = JToken.FromObject(dbBook).ToString();
            byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
            serverSocket.Send(msg);
            Console.WriteLine("Book returned");

            menu.UpdateState(EUserOption.ReturnBook);
        }

        private static void BorrowBook(Menu menu)
        {
            Console.Clear();
            Console.WriteLine("The choosen books are now borrowed");
            menu.UpdateState(EUserOption.BorrowBooks);
        }

        private void ChooseBook(User userNew, Menu menu, byte[] bytes)
        {
            Console.WriteLine("Choose the book you want");
            byte[] msg = Encoding.ASCII.GetBytes("2");
            serverSocket.Send(msg);

            int idBook = Convert.ToInt32(Console.ReadLine());
            msg = Encoding.ASCII.GetBytes(idBook.ToString());
            serverSocket.Send(msg);

            int bytesRec = serverSocket.Receive(bytes);
            string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            try
            {
                if (JToken.Parse(receivedMessage).ToObject<BookDatabase>() != null)
                {
                    var book = JToken.Parse(receivedMessage).ToObject<BookDatabase>();
                    userNew.CurrentChoose.Add(new Book(book));
                    menu.UpdateState(EUserOption.ChooseBooks);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Book not found");
            }
        }

        private void SeeBooks(Menu menu, byte[] bytes)
        {
            Console.Clear();

            byte[] option = Encoding.ASCII.GetBytes("1");
            serverSocket.Send(option);

            var receiverBooksBytes = serverSocket.Receive(bytes);
            var receiverBooksString = Encoding.ASCII.GetString(bytes, 0, receiverBooksBytes);

            var books = JToken.Parse(receiverBooksString).ToObject<List<BookDatabase>>();
            foreach (var dbBook in books)
            {
                Console.WriteLine($"[Id: {dbBook.Id}, Title: {dbBook.Title}, Author: {dbBook.Author}, Publication Date: {dbBook.PublicationDate}");
            }

            menu.UpdateState(EUserOption.SeeBooks);
        }

        private Book MakeFiction()
        {
            while (true)
            {
                try
                {
                    string title, author;
                    int date;
                    ReadDetailsBook(out title, out author, out date);
                    FictionFactory fictionFactory = new FictionFactory();
                    return fictionFactory.GetBook(title, author, date);
                }
                catch
                {
                    Console.WriteLine("Something wrong");
                }
            }
        }

        private static void ReadDetailsBook(out string title, out string author, out int date)
        {
            Console.Write("Title: ");
            title = Console.ReadLine();
            Console.Write("\nAuthor: ");
            author = Console.ReadLine();
            Console.Write("\nPublication date (year): ");
            date = Convert.ToInt32(Console.ReadLine());
        }

        private Book MakeNonFiction()
        {
            while (true)
            {
                try
                {
                    string title, author;
                    int date;
                    ReadDetailsBook(out title, out author, out date);
                    NonFictionFactory nonFictionFactory = new NonFictionFactory();
                    return nonFictionFactory.GetBook(title, author, date);
                }
                catch
                {
                    Console.WriteLine("Something wrong");
                }
            }
        }

        private Book CreateBook()
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

            Book createdBook = null;
            switch (op)
            {
                case 1:
                    createdBook = MakeFiction();
                    break;
                case 2:
                    createdBook = MakeNonFiction();
                    break;
            }

            return createdBook;

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

                        // TO DO

                        break;
                    case AdminMenuOptions.AddBook:
                        var createdBook = CreateBook();
                        var type = createdBook.Type();
                        byte[] option = Encoding.ASCII.GetBytes("2");
                        serverSocket.Send(option);

                        string serializedObject = JToken.FromObject(new KeyValuePair<Book, EBookType>(createdBook, createdBook.Type())).ToString();
                        byte[] byteBook = Encoding.ASCII.GetBytes(serializedObject);
                        serverSocket.Send(byteBook);
                        Console.WriteLine("Book added");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case AdminMenuOptions.SeeReport:
                        byte[] msg = Encoding.ASCII.GetBytes("3");
                        int bytesSent = serverSocket.Send(msg);
                        int bytesRec = serverSocket.Receive(bytes);
                        string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        Console.WriteLine("Report: \n" + receivedMessage);
                        bytesRec = serverSocket.Receive(bytes);
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
                        var data = new KeyValuePair<int, User>((int)userOption, userNew);
                        string serializedObject = JToken.FromObject(data).ToString();
                        byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
                        int bytesSent = serverSocket.Send(msg);
                        int bytesRec = serverSocket.Receive(bytes);
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
                    HeadOffice headOffice = new HeadOffice("Ofiice", "Head", null);
                    Librarian librarian = new Librarian("Librarian", "Last", headOffice);
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
                serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
