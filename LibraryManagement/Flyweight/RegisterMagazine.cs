using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    class RegisterMagazine : BookRegister
    {
        public override BookFormat CreateNewBook()
        {
            return new MagazineBook();
        }

        public override bool IsAvailable(double value)
        {
            return false;
        }
    }
}
