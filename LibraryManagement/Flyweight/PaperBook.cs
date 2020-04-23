using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    public class PaperBook : BookFormat
    {
        public override EBookMaterialType GetBookType()
        {
            return EBookMaterialType.PaperBook;
        }

     
    }
}
