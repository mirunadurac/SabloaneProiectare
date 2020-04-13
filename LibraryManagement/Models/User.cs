using LibraryManagement.Proxy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Models
{
    class User
    {
        protected static int Id { get; set; } = 0;
        public int IdUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Book> BorrowedBooks { get; set; }
        public LibraryMembership LibraryMembership { get; set; }

        public User(string firstName, string lastName, DateTime dateTime)
        {
            FirstName = firstName;
            LastName = lastName;
            IdUser = ++Id;
            BorrowedBooks = new List<Book>();
            LibraryMembership = new LibraryMembership(dateTime);
        }
    }
}
