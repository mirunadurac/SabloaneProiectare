using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LibraryManagement.ChainOfResponsability;
using LibraryManagement.Database;
using LibraryManagement.Database.Repository;
using LibraryManagement.Models;
using LibraryManagement.Observer.Logger;
using LibraryManagement.Observer.Logging;
using LibraryManagement.Singleton;
using LibraryManagement.State;
using LibraryManagement.Utils;
using MySql.Data;

namespace LibraryManagement
{
    class Program
    {
        static Library Library = Library.Instance;
        static int ShowFirstMenu()
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

        static User Login(UserRepository<User> userRepository)
        {
            Console.WriteLine("Username");
            string username = Console.ReadLine();
            Console.WriteLine("Password");
            string password = Console.ReadLine();
            User user = userRepository.FindByUsername(username);
            if(user!=null)
                if (user.Password.Equals(password))
                    return user;
            return null;
        }

        static User Register()
        {
            Console.WriteLine("Username");
            string username = Console.ReadLine();
            Console.WriteLine("Password");
            string password = Console.ReadLine();
            Console.WriteLine("First Name");
            string first = Console.ReadLine();
            Console.WriteLine("Last name");
            string last = Console.ReadLine();
            DateTime dt = DateTime.Now;
            DateTime endTime = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);
            Console.WriteLine("Gender (choose the number)");
            Console.WriteLine("1.Male");
            Console.WriteLine("2.Female");
            Console.WriteLine("3.Unknown");
            int op= Convert.ToInt32(Console.ReadLine());
            Gender gender=Gender.Unknown;
            switch (op)
               { 
                case 1:  
                    gender = Gender.Male;
                    break;
                case 2:
                    gender = Gender.Female;
                    break;
                case 3:
                    gender = Gender.Unknown;
                    break;
            }
            User user = new User(first, last, endTime, gender);
            user.Username = username;
            user.Password = password;
            return user;
        }

        static int ShowHome()
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


        static void Main(string[] args)
        {
            var bookGenerator = BookGenerator.Instance;

            string outputPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var logger = Logger.Instance;
            logger.AddObservers(
                new List<ILoggingService>()
                {
                    new ConsoleLogger(),
                    new FileLogger(new FileInfo(Path.Combine(outputPath, "logs.txt")))
                }
            );

            var books = bookGenerator.GenerateBooks();
            logger.Log(Status.Info, "Finished generating books");
            //books.ForEach(book => Console.WriteLine(book));
            
            Library.Books = books;
            var databaseConnection = DatabaseConnection.Instance;
            databaseConnection.OpenConnection();
            var userRepository = new UserRepository<User>(databaseConnection.Connetion);
            User userNew = null;
            Menu menu = null;
            while (true)
            {
                int op = ShowFirstMenu();
                    switch (op)
                    {
                        case 1:
                            userNew = Register();
                            userRepository.Add(userNew);
                            menu = new Menu(userNew);
                            break;
                        case 2:
                            userNew = Login(userRepository);
                            menu = new Menu(userNew);
                            break;
                        case -1:
                            break;
                        
                    }
                if (op!=-1)
                    if (userNew != null)
                    {
                        Console.WriteLine("Welcome");
                        break;
                    }
                    else
                        Console.WriteLine("Wrong username or password");
            }
            HeadOffice headOffice = new HeadOffice("Ofiice", "Head", DateTime.Now, null, Utils.Gender.Female);
            Librarian librarian = new Librarian("Librarian", "Last", DateTime.Now, headOffice, Utils.Gender.Female);
            userNew.Supervisor = librarian;
            menu.UpdateState(EUserOption.Login);
            Console.Clear();
            while (true)
            {
                int op = ShowHome();
                switch (op)
                {
                    case 1:
                        Console.Clear();
                        Library.SeeBooks();
                        menu.UpdateState(EUserOption.SeeBooks);
                        break;
                    case 2:
                        Console.WriteLine("Choose the book you want");
                        int idBook = Convert.ToInt32(Console.ReadLine());
                        userNew.CurrentChoose.Add(books[idBook]);
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

            //HeadOffice headOffice = new HeadOffice("Ofiice", "Head", DateTime.Now, null, Utils.Gender.Female);
            //Librarian librarian = new Librarian("Librarian", "Last", DateTime.Now, headOffice, Utils.Gender.Female);
            //User user = new User("First", "Last", DateTime.Now, librarian, Utils.Gender.Male);
            //user.Username = "dummy";
            //user.Password = "pass";

            //BorrowRequest borrowRequest = new BorrowRequest(DateTime.Now, new DateTime(2020, 6, 11), books.ElementAt(0));
            //user.ApplyRequest(borrowRequest);




            //userRepository.Add(user);
            //userRepository.Add(user);

            //var allUsers = userRepository.SelectAll();

            //foreach(var queridUser in allUsers)
            //{
            //    Console.WriteLine(queridUser.FirstName + " " + queridUser.LastName);
            //}
        }
    }
}
