using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryManagement.Singleton
{
    public class BookGenerator
    {
        private static object padlock = new object();
        private static BookGenerator instance;
        public static BookGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new BookGenerator();
                        }
                    }
                }

                return instance;
            }
        }

        private BookGenerator()
        {

        }

        public List<Book> GenerateBooks(int bookNo = 5)
        {
            Random random = new Random();
            var authorNames = new List<string>()
            {
                "Alexander Dumas", "Amartya Sen", "Bankim Chandra Chatterji", "Bankim Chandra Chatterji",
                "Arundati Roy", "Edward Morgan Forster", "Karl Marx", "Mulk Raj Anand"
            };

            var bookTitles = new List<string>()
            {
                "The Three Musketeers", "The Argumentative Indian","Development as Freedom","River of Smoke",
                "Circle of Reason","Circle of Reason","Clear City of the Day","The Algebra of Infinite Justice"
            };

            var generatedBooks = new List<Book>();

            var fictionFactory = new FictionFactory();
            var nonFictionFactory = new NonFictionFactory();

            for (int index = 0; index < bookNo; ++index)
            {
                generatedBooks.Add(fictionFactory.GetBook(bookTitles[random.Next(0, bookTitles.Count - 1)], authorNames[random.Next(0, authorNames.Count - 1)], random.Next(1800, 1950)));
                generatedBooks.Add(nonFictionFactory.GetBook(bookTitles[random.Next(0, bookTitles.Count - 1)], authorNames[random.Next(0, authorNames.Count - 1)], random.Next(1800, 1950)));
            }

            return generatedBooks;
        }
    }
}
