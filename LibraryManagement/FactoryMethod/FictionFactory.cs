using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.Models;

namespace LibraryManagement.FactoryMethod
{
    public class FictionFactory:BookFactory
    {
        public override Book GetBook(string title, string author, DateTime publicationDate)
        {
            BookFactory.LastId++;
            return new Fiction(BookFactory.LastId,title, author, publicationDate);
        }
    }
}
