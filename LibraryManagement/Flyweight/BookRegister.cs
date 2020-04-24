using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    public abstract class BookRegister
    {
        public Dictionary<EBookType, int> borrowedBook { get; set; }

        public BookRegister()
        {
            borrowedBook = new Dictionary<EBookType, int>();
        }

        public void AddABook(EBookType type)
        {
            if (borrowedBook.ContainsKey(type))
                borrowedBook[type]++;
            else borrowedBook.Add(type, 1);
        }

        public string SeeReport()
        {
            string str = "";
            foreach (var it in borrowedBook)
            {
                str += it.Key + " " + it.Value.ToString() + "\n";
            }
            return str;
        }


        public abstract Book CreateNewBook();
        public abstract int NumberOfBook();
    }
}
