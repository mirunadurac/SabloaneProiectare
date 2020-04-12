using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.FactoryMethod;

namespace LibraryManagement.Models
{
    public abstract class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublicationDate { get; set; }

        public EBookType EBookType { get; set; }

        public Book(int id, string title, string author, DateTime publicationdDate)
        {
            Id = id;
            Title = title;
            Author = author;
            PublicationDate = publicationdDate;
        }

        public Book(string title, string author, DateTime publicationdDate)
        {
            Title = title;
            Author = author;
            PublicationDate = publicationdDate;

        }


        public override string ToString()
        {
            return $"[Id: {Id}, Title: {Title}, Autho: {Author}, Publication Date: {PublicationDate}]";
        }

        public abstract EBookType Type();
    }
}
