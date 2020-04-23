using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.State
{
    class ChooseBookState : State
    {
        public ChooseBookState (User user, Menu menu):base(user, menu)
        {

        }
        public override bool BorrowBook()
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
            throw new NotImplementedException();
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
