using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    public abstract class BookRegister
    {
        public Dictionary<EBookMaterialType, int> borrowedBook { get; set; }

        //public void CountBorrowedType(EBookMaterialType type)
        //{


        //}

        public void AddABook(EBookMaterialType type)
        {
            if (borrowedBook.ContainsKey(type))
                borrowedBook[type]++;
            else borrowedBook.Add(type, 1);
        }

        public void SeeReport()
        {
            foreach (var it in borrowedBook)
            {
                Console.WriteLine(it.Key + " " + it.Value.ToString());
            }
        }


        public abstract BookFormat CreateNewBook();
        public abstract bool IsAvailable(double value);
    }
}
