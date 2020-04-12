using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.Models;

namespace LibraryManagement.FactoryMethod
{
    class NonFictionFactory:BookFactory
    {
        public override Book GetBook(string title, string author, DateTime publicationDate)
        {
            BookFactory.LastId++;
            return new NonFiction(BookFactory.LastId,title, author, publicationDate);
        }
    }
}
