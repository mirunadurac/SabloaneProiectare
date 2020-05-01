using LibraryManagement.ChainOfResponsability;
using LibraryManagement.Proxy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.Utils;
using System.Dynamic;
using LibraryManagement.Models.DatabaseModels;

namespace LibraryManagement.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public List<KeyValuePair<DateTime, Book>> BorrowedBooks { get; set; }
        public List<Book> CurrentChoose { get; set; } = new List<Book>();
        public LibraryMembership LibraryMembership { get; set; }
        public Role Role { get; set; }

        public User Supervisor { get; set; }

        public User(UserDatabase userDatabase, User supervisor = null)
        {
            FirstName = userDatabase.FirstName;
            LastName = userDatabase.LastName;
            LastName = userDatabase.LastName;
            Username = userDatabase.Username;
            Password = userDatabase.Password;
            IdUser = userDatabase.Id;
            Role = userDatabase.Role;
            BorrowedBooks = new List<KeyValuePair<DateTime, Book>>();
            LibraryMembership = new LibraryMembership(userDatabase.Validity);
            PopulateBooks(userDatabase.Books);
            Supervisor = supervisor;
            Gender = userDatabase.Gender;
        }

        private void PopulateBooks(ICollection<BookDatabase> books)
        {
            foreach(var dbBook in books)
            {
                BorrowedBooks.Add(new KeyValuePair<DateTime, Book>(DateTime.Now, new Book(dbBook)));
            }
        }

        public User(string firstName, string lastName, User supervisor = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Supervisor = supervisor;
            BorrowedBooks = new List<KeyValuePair<DateTime, Book>>();
        }

        public User()
        {
            BorrowedBooks = new List<KeyValuePair<DateTime, Book>>();
        }


        public void ApplyRequest(BorrowRequest request)
        {
            if (ApproveRequest(request))
            {
                Console.WriteLine(request.RequestNumber + " approved by " + Supervisor.ToString() + " for " + request.GetNumberOfDays() + "days");
                BorrowedBooks.Add(new KeyValuePair<DateTime, Book>(request.EndDay, request.BorrowedBook));
            }
            else
            {
                this.Supervisor = Supervisor.Supervisor;
                if (Supervisor == null)
                {
                    Console.WriteLine("You cannot borrow a book for more than 60 days");
                }
                else
                {
                    ApplyRequest(request);
                }
            }
        }

        public virtual int GetMaxDayCanAprove()
        {
            return 0;
        }
        public bool ApproveRequest(BorrowRequest request)
        {

            if (request.GetNumberOfDays() <= Supervisor.GetMaxDayCanAprove())
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return Username + " " + FirstName + " " + LastName;
        }
    }
}
