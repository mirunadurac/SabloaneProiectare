using LibraryManagement.DatabaseEntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repository
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class
    {

        public LibraryDatabaseContext LibraryDatabaseContext { get; private set; }
        public DbSet<T> DbSet { get; private set; }


        public GeneralRepository(LibraryDatabaseContext libraryDatabaseContext)
        {
            LibraryDatabaseContext = libraryDatabaseContext;
            DbSet = LibraryDatabaseContext.Set<T>();
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (LibraryDatabaseContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual T GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Insert(T entitiy)
        {
            DbSet.Add(entitiy);
        }

        public virtual void RemoveAll()
        {
            DbSet.RemoveRange(DbSet);
        }

        public virtual void Update(T entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            LibraryDatabaseContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
