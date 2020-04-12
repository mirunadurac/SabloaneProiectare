using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.Models;

namespace LibraryManagement.FactoryMethod
{
    public class Fiction:Book
    {
        public override EBookType Type()
        {
            return EBookType.Fiction;
        }

        public Fiction(int id,string title, string author, DateTime publicationDate) :
           base(id,title, author, publicationDate)
        {

        }
    }
}
