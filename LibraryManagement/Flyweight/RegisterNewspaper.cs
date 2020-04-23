using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    class RegisterNewspaper : BookRegister
    {
        public override BookFormat CreateNewBook()
        {
            return new NewspaperBook();
        }

        public override bool IsAvailable(double value)
        {
            return false;
        }
    }
}
