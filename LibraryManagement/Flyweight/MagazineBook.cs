using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    public class MagazineBook : BookFormat
    {
        public override EBookMaterialType GetBookType()
        {
            return EBookMaterialType.Magazine;
        }
    }
}
