using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    class RegisterNonFiction : BookRegister
    {
        public override Book CreateNewBook()
        {
            return new NonFiction();
        }

        public override int NumberOfBook()
        {
            if (borrowedBook.ContainsKey(EBookType.NonFiction))
                return borrowedBook[EBookType.NonFiction];
            return 0;
        }
    }
}
