using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.FactoryMethod;
using LibraryManagement.Models.DatabaseModels;

namespace LibraryManagement.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationDate { get; set; }

        public EBookType EBookType { get; set; }

        public Book(string title, string author, int publicationdDate)
        {
            Title = title;
            Author = author;
            PublicationDate = publicationdDate;
        }

        public Book(BookDatabase bookDatabase)
        {
            Id = bookDatabase.Id;
            Title = bookDatabase.Title; ;
            Author = bookDatabase.Author;
            PublicationDate = bookDatabase.PublicationDate.Year;

        }

        public Book(int id, string title, string author, int publicationDate)
        {
            Id = id;
            Title = title;
            Author = author;
            PublicationDate = publicationDate;
        }

        public Book() { }

        public override string ToString()
        {
            return $"[Id: {Id}, Title: {Title}, Author: {Author}, Publication Date: {PublicationDate}]";
        }

        public virtual EBookType Type()
        {
            return EBookType;
        }
    }
}
