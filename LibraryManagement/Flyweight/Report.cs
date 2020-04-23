using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    public class Report
    {
        RegisterMagazine RegisterMagazine { get; set; } = new RegisterMagazine();
        RegisterNewspaper RegisterNewspaper { get; set; } = new RegisterNewspaper();
        RegisterPaperBook RegisterPaperBook { get; set; } = new RegisterPaperBook();


        private BookRegister GetBookRegister(EBookMaterialType type)
        {
            BookRegister register = null;

            switch (type)
            {
                case EBookMaterialType.Magazine:
                    register = RegisterMagazine;
                    break;
                case EBookMaterialType.Newspaper:
                    register = RegisterNewspaper;
                    break;
                case EBookMaterialType.PaperBook:
                    register = RegisterPaperBook;
                    break;
            }

            return register;
        }
        public void AddNewBook(EBookMaterialType type)
        {
            GetBookRegister(type).AddABook(type);
        }

        public void SeeReport()
        {
            RegisterNewspaper.SeeReport();
        }
    }
}
