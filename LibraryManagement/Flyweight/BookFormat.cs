using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Flyweight
{
    public abstract class BookFormat
    {
        public int totalBook { get; set; } = 0;
        public Book book;
        public abstract EBookMaterialType GetBookType();
    }
}
