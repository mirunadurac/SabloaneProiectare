using LibraryManagement.FactoryMethod;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    public class Report
    {
        RegisterFiction RegisterFiction { get; set; } = new RegisterFiction();
        RegisterNonFiction RegisterNonFiction { get; set; } = new RegisterNonFiction();


        private BookRegister GetBookRegister(EBookType type)
        {
            BookRegister register = null;

            switch (type)
            {
                case EBookType.Fiction:
                    register = RegisterFiction;
                    break;
                case EBookType.NonFiction:
                    register = RegisterNonFiction;
                    break;

            }

            return register;
        }
        public void AddNewBook(EBookType type)
        {
            GetBookRegister(type).AddABook(type);
        }

        public string TotalBorrowBook()
        {
            int total = RegisterNonFiction.NumberOfBook() + RegisterFiction.NumberOfBook();
            return total.ToString();
        }

        public string SeeReport()
        {
            return RegisterFiction.SeeReport();
        }
    }
}
