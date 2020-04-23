using LibraryManagement.Models;
using LibraryManagement.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Singleton
{
    public class Library
    {
        private static object padlock = new object();
        private static Library instance;

        public List<Book> Books { get; set; }

        public static Library Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Library();
                        }
                    }
                }

                return instance;
            }
        }

        private Library()
        {

        }

    }
}
