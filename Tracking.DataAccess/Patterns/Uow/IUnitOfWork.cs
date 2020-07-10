using System;
using System.Threading.Tasks;
using Tracking.DataAccess.Patterns.Repository;
using Tracking.Entities;

namespace Tracking.DataAccess.Patterns.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetEntityRepository<TEntity>()
            where TEntity : class, IEntity,
            new();
        void Commit();
        Task CommitAsync();
    }
}