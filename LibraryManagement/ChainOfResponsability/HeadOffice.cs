using LibraryManagement.Models;
using LibraryManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.ChainOfResponsability
{
    public class HeadOffice : User
    {
        public HeadOffice(string firstName, string lastName, User supervisor) :
            base(firstName, lastName, supervisor)
        {

        }

        public override int GetMaxDayCanAprove()
        {
            return 60;
        }

        public override string ToString()
        {
            return "Head Office";
        }
    }
}
