using System;
using System.Linq;
using System.Linq.Expressions;

namespace TimeIsMoney.DataAccess
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Set { get; }

        T Insert(T entity);

        T Update(T entity);

        void Delete(T entity);

        void DeleteById(int id);

        IQueryable<T> Include(params Expression<Func<T, object>>[] include);
    }
}