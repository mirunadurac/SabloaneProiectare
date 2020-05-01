using LibraryManagement.Models;
using LibraryManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.ChainOfResponsability
{
    public class Librarian : User
    {
        public Librarian(string firstName, string lastName, User supervisor) :
            base(firstName, lastName, supervisor)
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
