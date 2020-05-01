using LibraryManagement.DatabaseEntityFramework;
using LibraryManagement.Models;
using LibraryManagement.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repository
{
    public class UnitOfWork : IDisposable
    {
        private LibraryDatabaseContext LibraryDatabaseContext { get; set; } = new LibraryDatabaseContext();
        private GeneralRepository<BookDatabase> bookRepository;
        private GeneralRepository<UserDatabase> userRepository;

        private bool disposed = false;

        public GeneralRepository<BookDatabase> BookRepository
        {
            get
            {
                if(bookRepository == null)
                {
                    bookRepository = new GeneralRepository<BookDatabase>(LibraryDatabaseContext);
                }
                return bookRepository;
            }
        }
        public GeneralRepository<UserDatabase> UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new GeneralRepository<UserDatabase>(LibraryDatabaseContext);
                }
                return userRepository;
            }
        }

        public void Save()
        {
            LibraryDatabaseContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    LibraryDatabaseContext.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
