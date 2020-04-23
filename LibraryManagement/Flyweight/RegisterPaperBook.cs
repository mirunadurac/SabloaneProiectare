using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    class RegisterPaperBook : BookRegister
    {
        public override BookFormat CreateNewBook()
        {
            return new PaperBook();
        }

        public override bool IsAvailable(double value)
        {
            return false;
        }
    }
}
