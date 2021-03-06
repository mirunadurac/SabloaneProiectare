﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Proxy.Models
{
    class LibraryMembershipProxy : ILibraryMembership
    {
        private ILibraryMembership RealSubject;

        public LibraryMembershipProxy(DateTime dateTime)
        {
            RealSubject = new LibraryMembership(dateTime);
        }

        public void CancelSubscription()
        {
            RealSubject.CancelSubscription();
        }

        public void ExtentSubscription(DateTime newDate)
        {
            RealSubject.ExtentSubscription(newDate);
        }

        public void ShowNumberOfBorrowedBooks()
        {
            RealSubject.ShowNumberOfBorrowedBooks();
        }
    }
}
