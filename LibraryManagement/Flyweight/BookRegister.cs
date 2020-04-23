using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    public abstract class BookRegister
    {
        public Dictionary<EBookMaterialType, int> borrowedBook { get; set; }

        public void CountBorrowedType(EBookMaterialType type)
        {

          
        }

        public abstract BookFormat CreateNewBook();
        public abstract bool IsAvailable(double value);
    }
}
