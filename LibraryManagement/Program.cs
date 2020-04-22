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
using MySql.Data;

namespace LibraryManagement
{
    class Program
    {
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
            books.ForEach(book => Console.WriteLine(book));

            HeadOffice headOffice = new HeadOffice("Ofiice", "Head", DateTime.Now, null, Utils.Gender.Female);
            Librarian librarian = new Librarian("Librarian", "Last", DateTime.Now, headOffice, Utils.Gender.Female);
            User user = new User("First", "Last", DateTime.Now, librarian, Utils.Gender.Male);
            user.Username = "dummy";
            user.Password = "pass";

            BorrowRequest borrowRequest = new BorrowRequest(DateTime.Now, new DateTime(2020, 6, 11), books.ElementAt(0));
            user.ApplyRequest(borrowRequest);

            var databaseConnection = DatabaseConnection.Instance;
            databaseConnection.OpenConnection();

            var userRepository = new UserRepository<User>(databaseConnection.Connetion);
            userRepository.Add(user);
            userRepository.Add(user);

            var allUsers = userRepository.SelectAll();

            foreach(var queridUser in allUsers)
            {
                Console.WriteLine(queridUser.FirstName + " " + queridUser.LastName);
            }
        }
    }
}
