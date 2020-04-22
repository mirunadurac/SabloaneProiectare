using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.ChainOfResponsability
{
     class Librarian:User
    {
        public Librarian(string firstName, string lastName, DateTime dateTime, User supervisor):
            base(firstName,lastName,dateTime, supervisor)
        {

        }

        public override int GetMaxDayCanAprove()
        {
            return 30;
        }

        public override string ToString()
        {
            return "Librarian";
        }
    }
}
