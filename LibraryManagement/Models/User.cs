using LibraryManagement.ChainOfResponsability;
using LibraryManagement.Proxy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.Utils;
using System.Dynamic;

namespace LibraryManagement.Models
{
     public class User
    {
        protected static int Id { get; set; } = 0;
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public List<KeyValuePair<DateTime,Book>> BorrowedBooks { get; set; }
        public List<Book> CurrentChoose { get; set; } = new List<Book>();
        public LibraryMembership LibraryMembership { get; set; }
        public Role Role { get; set; }

        public User Supervisor { get; set; }

        public User(string firstName, string lastName, DateTime dateTime, User supervisor, Gender gender)
        {
            FirstName = firstName;
            LastName = lastName;
            IdUser = ++Id;
            BorrowedBooks = new List<KeyValuePair<DateTime, Book>>();
            LibraryMembership = new LibraryMembership(dateTime);
            Supervisor = supervisor;
            Gender = gender;
        }

        public User(int id, string username, string firstName, string lastName, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            IdUser = id;
            Password = password;
            Username = username;

        }

        public User(string firstName, string lastName, DateTime dateTime, Gender gender)
        {
            FirstName = firstName;
            LastName = lastName;
            IdUser = ++Id;
            BorrowedBooks = new List<KeyValuePair<DateTime, Book>>();
            LibraryMembership = new LibraryMembership(dateTime);
            Gender = gender;
        }

        public User()
        {
            BorrowedBooks = new List<KeyValuePair<DateTime, Book>>();
        }

        public User(int id,string username, string password,string firstName, string lastName, DateTime dateTime, Gender gender, Role role)
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            IdUser = id;
            BorrowedBooks = new List<KeyValuePair<DateTime, Book>>();
            LibraryMembership = new LibraryMembership(dateTime);
            Gender = gender;
            Role = role;
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

        public override string ToString()
        {
            return Username + " " + FirstName + " " + LastName;
        }
    }
}
