using LibraryManagement.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.DatabaseEntityFramework
{
    public class LibraryDatabaseContext : DbContext
    {
        public LibraryDatabaseContext()
        {

        }

        public DbSet<UserDatabase> Users { get; set; }
        public DbSet<BookDatabase> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Library;Trusted_Connection=True;");
        }
    }
}
