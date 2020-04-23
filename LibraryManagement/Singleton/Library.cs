using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Singleton
{
    public class Library
    {
        private static object padlock = new object();
        private static Library instance;

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
