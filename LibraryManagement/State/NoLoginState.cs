using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.State
{
   public  class NoLoginState : State
    {
        public NoLoginState(User user, Menu menu) : base(user, menu)
        {

        }
        public override bool BorrowBook(int days)
        {
            throw new NotImplementedException();
        }

        public override bool ChooseBook()
        {
            throw new NotImplementedException();
        }

        public override bool Leave()
        {
            throw new NotImplementedException();
        }

        public override bool Login()
        {
            Menu.SetMenuState(Menu.LoginState);
            return true;
        }

        public override bool Return()
        {
            throw new NotImplementedException();
        }

        public override bool SeeBooks()
        {
            throw new NotImplementedException();
        }
    }
}
