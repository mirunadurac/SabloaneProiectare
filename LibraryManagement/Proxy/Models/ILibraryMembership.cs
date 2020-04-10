using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Proxy.Models
{
    interface ILibraryMembership
    {
        void ExtentSubscription(DateTime newDate);
        void CancelSubscription();
        void ShowNumberOfBorrowedBooks();
    }
}
