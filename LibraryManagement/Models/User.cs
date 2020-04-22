using LibraryManagement.ChainOfResponsability;
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
        public List<KeyValuePair<DateTime,Book>> BorrowedBooks { get; set; }
        public LibraryMembership LibraryMembership { get; set; }

        User Supervisor { get; set; }

        public User(string firstName, string lastName, DateTime dateTime, User supervisor)
        {
            FirstName = firstName;
            LastName = lastName;
            IdUser = ++Id;
            BorrowedBooks = new List<KeyValuePair<DateTime, Book>>();
            LibraryMembership = new LibraryMembership(dateTime);
            Supervisor = supervisor;
        }

        public void ApplyRequest(BorrowRequest request)
        {
            if (ApproveRequest(request))
            {
                Console.WriteLine(request.RequestNumber + " approved by " + Supervisor.ToString()+ " for " + request.GetNumberOfDays() + "days");
                BorrowedBooks.Add(new KeyValuePair<DateTime,Book> (request.EndDay, request.BorrowedBook));
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

            if (request.GetNumberOfDays() <=Supervisor.GetMaxDayCanAprove())
                return true;
            else
                return false;
        }
    }
}
