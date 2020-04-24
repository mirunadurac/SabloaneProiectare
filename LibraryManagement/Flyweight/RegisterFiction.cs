using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    class RegisterFiction : BookRegister
    {
        public override Book CreateNewBook()
        {
            return new Fiction();
        }

        public override int NumberOfBook()
        {
            if (borrowedBook.ContainsKey(EBookType.Fiction))
                return borrowedBook[EBookType.Fiction];
            return 0;
        }
    }
}
