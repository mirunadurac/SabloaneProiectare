using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    public class NewspaperBook : BookFormat
    {
        public override EBookMaterialType GetBookType()
        {
            return EBookMaterialType.Newspaper;
        }
    }
}
