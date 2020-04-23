using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.State
{
    public abstract class State
    {
        public Menu Menu { get; set; }

        public User User { get; set; }

        public State (User user, Menu menu)
        {
            User = user;
            Menu = menu;
        }

        public abstract bool Login();
        public abstract bool SeeBooks();
        public abstract bool ChooseBook();
        public abstract bool BorrowBook();
        public abstract bool Leave();

        public abstract bool Return();

    }
}
