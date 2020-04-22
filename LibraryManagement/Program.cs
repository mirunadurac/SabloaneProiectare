using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LibraryManagement.ChainOfResponsability;
using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using LibraryManagement.Observer.Logger;
using LibraryManagement.Observer.Logging;
using LibraryManagement.Singleton;

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

            HeadOffice headOffice = new HeadOffice("Ofiice","Head", DateTime.Now, null);
            Librarian librarian = new Librarian("Librarian","Last", DateTime.Now, headOffice);
            User user = new User("First", "Last", DateTime.Now, librarian);

            BorrowRequest borrowRequest = new BorrowRequest(DateTime.Now, new DateTime(2020, 6, 11), books.ElementAt(0));
            user.ApplyRequest(borrowRequest);
        }
    }
}
