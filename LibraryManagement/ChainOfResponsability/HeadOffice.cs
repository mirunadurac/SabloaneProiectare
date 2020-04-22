using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.ChainOfResponsability
{
    class HeadOffice:User
    {
        public HeadOffice(string firstName, string lastName, DateTime dateTime, User supervisor) :
            base(firstName, lastName, dateTime, supervisor)
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
