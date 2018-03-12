using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace TimeIsMoney.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TimeInMoneyContext _dbContext;
        private readonly DbSet<T> _entities;

        public Repository(TimeInMoneyContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<T>();
        }

        public IQueryable<T> Set { get { return _entities; } }

        public T GetById(object id)
        {
            return this._entities.Find(id);
        }

        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _entities.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _entities.Update(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _entities.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void DeleteById(int id)
        {
            T entity = this._entities.Find(id);

            if (entity != null)
            {
                _entities.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] include)
        {
            return include.Aggregate((IQueryable<T>)_entities, (current, inc) => current.Include(inc));
        }
    }
}