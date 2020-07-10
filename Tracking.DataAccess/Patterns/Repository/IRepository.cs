using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tracking.DataAccess.Patterns.Repository
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> FindAsync(params object[] keyValues);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");
        Task<TEntity> FindByIdAsync(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeProperties = null);
        Task<IEnumerable<TEntity>> AllWithAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> AllWith(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate = null);
        void Insert(TEntity item);
        void InsertBulk(IEnumerable<TEntity> entityToInsert);
        void Update(TEntity item, params Expression<Func<TEntity, object>>[] excludeProperties);
        void Delete(TEntity item);
        void Delete(object id);
        void DeleteBulk(IEnumerable<TEntity> entityToDelete);
    }
}