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
    }
}
