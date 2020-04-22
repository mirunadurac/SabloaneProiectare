using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Database.Repository
{
    interface IRepository<T>
    {
        List<T> SelectAll();
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T FindById(int Id);
    }
}
