using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.Models;

namespace LibraryManagement.FactoryMethod
{
    public abstract class BookFactory
    {
        protected static int LastId { get; set; }
        public abstract Book GetBook(string title, string author, DateTime publicationDate);
    }
}
