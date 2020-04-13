using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Models
{
    class User
    {
        protected static int Id;
        int IdUser;
        public string FirstName;
        public string LastName;
        public List<Book> BorrowedBooks { get; set; }

        public User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Id++;
            IdUser = Id;
            BorrowedBooks = new List<Book>();
        }
    }
}
