using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repository
{
    interface IGeneralRepository<T> where T: class
    {
        void Delete(object id);
        void Delete(T entityToDelete);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        T GetById(object id);
        void Insert(T entitiy);
        void Update(T entityToUpdate);
        void RemoveAll();
    }
}
