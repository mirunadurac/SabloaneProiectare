using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using LibraryManagement.Singleton;

namespace LibraryManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = new Fiction(1, "Gone with the wind", "Margaret Mitchell's", 1936);
            Console.WriteLine(book);


            var bookGenerator = BookGenerator.Instance;

            var books = bookGenerator.GenerateBooks();

            books.ForEach(book => Console.WriteLine(book));
        }
    }
}
