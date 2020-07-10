using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Tracking.DataAccess.Patterns.Repository;
using Tracking.Entities;

namespace Tracking.DataAccess.Patterns.Uow
{
    public class UnitOfWork : BaseUnitOfWork<DbContext>, IUnitOfWork
    {
        protected bool _disposed = false;
        //private static readonly Dictionary<int, string> _sqlErrorTextDict =
        //new Dictionary<int, string> {
        //    { 547, "This operation failed because another data entry uses this entry."},
        //    { 2601, "One of the properties is marked as Unique index and there is already an entry with that value."}
        //};

        public UnitOfWork(DbContext dbContext,
            IServiceProvider serviceProvider) :
            base(dbContext, serviceProvider)
        { }

        public IRepository<TEntity> GetEntityRepository<TEntity>()
            where TEntity : class, IEntity,
            new()
        {
            Repository<TEntity> repository = new Repository<TEntity>(_dbContext);
            if (repository == null)
                throw new ArgumentException("Repository not found.");
            return repository;
        }

        public void Commit() => _dbContext.SaveChanges();

        public async Task CommitAsync()
        {
            if (CheckDisposed())
                throw new ObjectDisposedException(nameof(UnitOfWork));

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Check if the UnitOfWork has been disposed.
        /// </summary>
        /// <returns>True when <see cref="Dispose()"/> as been called</returns>
        protected virtual bool CheckDisposed() => _disposed;

        ~UnitOfWork()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}