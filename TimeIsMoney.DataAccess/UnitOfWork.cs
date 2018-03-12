using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace TimeIsMoney.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TimeInMoneyContext _context;
        private bool _disposed;
        private Dictionary<string, object> _repositories;

        public UnitOfWork(TimeInMoneyContext context)
        {
            _context = context;
        }

        public IDbTransaction BeginTransaction()
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            return new EfDbTransaction(transaction);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (Repository<T>)_repositories[type];
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        internal sealed class EfDbTransaction : IDbTransaction
        {
            private IDbContextTransaction _transaction;

            internal EfDbTransaction(IDbContextTransaction transaction)
            {
                _transaction = transaction;
            }

            public void Commit() => _transaction.Commit();

            public void Rollback() => _transaction.Rollback();

            public void Dispose() => _transaction.Dispose();
        }
    }
}