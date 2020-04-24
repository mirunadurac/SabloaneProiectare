using LibraryManagement.Models;
using LibraryManagement.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Singleton
{
    public class Library
    {
        private static object padlock = new object();
        private static Library instance;

        public static List<Book> Books { get; set; }

        public static Library Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Library();
                        }
                    }
                }

                return instance;
            }
        }

        private Library()
        {
            var bookGenerator = BookGenerator.Instance;
            Books = bookGenerator.GenerateBooks();
        }

        public void SeeBooks()
        {
           Books.ForEach(book => Console.WriteLine(book));
        }

    }
}
