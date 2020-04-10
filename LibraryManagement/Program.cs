using System;
using LibraryManagement.Models;

namespace LibraryManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = new Book(1, "Gone with the wind", "Margaret Mitchell's", new DateTime(1936, 6, 30));
            Console.WriteLine(book);
        }
    }
}
