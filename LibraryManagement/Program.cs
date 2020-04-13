using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        }
    }
}
