using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.Models;

namespace LibraryManagement.FactoryMethod
{
    class NonFiction:Book
    {
        public override EBookType Type()
        {
            return EBookType.NonFiction;
        }

        public NonFiction(int id,string title, string author, int publicationDate) :
           base(id,title, author, publicationDate)
        {

        }
    }
}
