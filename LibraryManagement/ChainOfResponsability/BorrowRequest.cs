using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.ChainOfResponsability
{
    public class BorrowRequest
    {
        private static int LastRequestNumber;
        public int RequestNumber { get; set; }

        public DateTime StartDay { get; set; }

        public DateTime EndDay { get; set; }

        public Book BorrowedBook { get; set; }


        public BorrowRequest(DateTime start, DateTime end, Book book)
        {
            StartDay = start;
            EndDay = end;
            LastRequestNumber++;
            RequestNumber = LastRequestNumber;
            BorrowedBook = book;
        }

        public int GetNumberOfDays()
        {
            return (EndDay - StartDay).Days;
        }
    }
}
