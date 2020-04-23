using LibraryManagement.ChainOfResponsability;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.State
{
    public class SeeBooksState : State
    {
        public SeeBooksState(User user, Menu menu) : base(user, menu)
        {

        }
        public override bool BorrowBook(int days)
        {
            if (User.CurrentChoose.Count == 0)
            {
                Console.WriteLine("You need to choose some books first");
            }

            DateTime date = DateTime.Now.AddDays(days);
            foreach (var book in User.CurrentChoose)
            {
                BorrowRequest borrowRequest = new BorrowRequest(DateTime.Now, date, book);
                User.ApplyRequest(borrowRequest);
            }

            Menu.SetMenuState(Menu.BorrowBookState);
            return true;
        }

        public override bool ChooseBook()
        {

            Menu.SetMenuState(Menu.ChooseBookState);
            return true;
        }

        public override bool Leave()
        {
            Menu.SetMenuState(Menu.LeaveState);
            return true;

        }

        public override bool Login()
        {
            throw new NotImplementedException();
        }

        public override bool Return()
        {
            if (User.BorrowedBooks.Count==0)
            {
                Console.WriteLine("You don't have any borrowed book");
                return false;
            }
            User.BorrowedBooks.ForEach(book => Console.WriteLine(book));
            Console.WriteLine("Choose the book you want to return");
            int idBook = Convert.ToInt32(Console.ReadLine());
            int indexBook = -1;
            for (int index=0; index< User.BorrowedBooks.Count;index++)
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

        public override bool SeeBooks()
        {
            Menu.SetMenuState(Menu.SeeBooksState);
            return true;
        }
    }
}
