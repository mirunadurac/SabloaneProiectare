﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Proxy.Models
{
    class LibraryMembership : ILibraryMembership
    {

        public DateTime EndDateValidity { get; set; }
        public int NumberOfBorrowedBooks { get; set; }
        public bool IsValid { get; set; }
        public void CancelSubscription()
        {
            IsValid = false;
        }

        public void ShowNumberOfBorrowedBooks()
        {
            Console.WriteLine($"You have {NumberOfBorrowedBooks} borrowd book/s");
        }

        public void ExtentSubscription(DateTime newDate)
        {
            if(newDate > EndDateValidity)
            {
                Console.WriteLine($"You're membership has been extended with {(newDate - EndDateValidity).Days} days");
                EndDateValidity = newDate;
            }
            else
            {
                Console.WriteLine("Invalid Date");
            }
        }
    }
}