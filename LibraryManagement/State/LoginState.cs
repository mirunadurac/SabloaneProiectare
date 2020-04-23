using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.State
{
    public class LoginState : State
    {
        public LoginState(User user, Menu menu) : base(user, menu)
        {

        }
        public override bool BorrowBook(int days)
        {
            Console.WriteLine("You need to choose a book first");
            return false;
        }

        public override bool ChooseBook()
        {
            Console.WriteLine("You need to see the books first");
            return false;
        }

        public override bool Leave()
        {
            Menu.SetMenuState(Menu.LeaveState);
            return true;
        }

        public override bool Login()
        {
            if (User.BorrowedBooks.Count == 0)
            {
                Console.WriteLine("You don't have any borrowed book");
                return false;
            }
            User.BorrowedBooks.ForEach(book => Console.WriteLine(book));
            Console.WriteLine("Choose the book you want to return");
            int idBook = Convert.ToInt32(Console.ReadLine());
            int indexBook = -1;
            for (int index = 0; index < User.BorrowedBooks.Count; index++)
            {
                if (User.BorrowedBooks[index].Value.Id == idBook)
                    indexBook = index;
            }
            if (indexBook != -1)
            {
                User.BorrowedBooks.RemoveAt(indexBook);
                Menu.SetMenuState(Menu.ReturnState);

            }

            return true;
        }

        public override bool Return()
        {
            throw new NotImplementedException();
        }

        public override bool SeeBooks()
        {
            Menu.SetMenuState(Menu.SeeBooksState);
            return true;
        }
    }
}
