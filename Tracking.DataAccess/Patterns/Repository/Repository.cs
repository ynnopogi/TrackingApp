using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tracking.Entities;

namespace Tracking.DataAccess.Patterns.Repository
{
    public sealed class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity,
        new()
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext) => _dbContext = dbContext;

        /// <summary>
        /// All Async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate != null) return await _dbContext.Set<TEntity>().Where(predicate).ToArrayAsync();
            else return await _dbContext.Set<TEntity>().ToArrayAsync();
        }
        /// <summary>
        /// All With
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> AllWith(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties){
            IQueryable<TEntity> query;
            if (predicate != null)
                query = _dbContext.Set<TEntity>().Where(predicate);
            else
                query = _dbContext.Set<TEntity>();

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
            return query.ToArray();
        }
        /// <summary>
        /// All With Async
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> AllWithAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query;
            if (predicate != null)
                query = _dbContext.Set<TEntity>().Where(predicate);
            else
                query = _dbContext.Set<TEntity>();

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return await query.ToArrayAsync();
        }
        /// <summary>
        /// Delete by Entity
        /// </summary>
        /// <param name="item"></param>
        public void Delete(TEntity item) => _dbContext.Entry(item).State = EntityState.Deleted;
        /// <summary>
        /// Delete by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(object id)
        {
            var item = _dbContext.Set<TEntity>().Find(id);
            if (item != null) _dbContext.Set<TEntity>().Remove(item);
        }
        /// <summary>
        /// Delete Bulk
        /// </summary>
        /// <param name="entityToDelete"></param>
        public void DeleteBulk(IEnumerable<TEntity> entityToDelete) => _dbContext.Set<TEntity>().RemoveRange(entityToDelete);
        /// <summary>
        /// Find Async
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync(params object[] keyValues) => await _dbContext.Set<TEntity>().FindAsync(keyValues);
        /// <summary>
        /// Find By
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public TEntity FindBy(Expression<Func<TEntity, bool>> predicate, string includeProperties = "") => Task.Run(async () => await FindByAsync(predicate, includeProperties)).Result;
        /// <summary>
        /// Find By Async
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "")
        {
            {
                IQueryable<TEntity> query = _dbContext.Set<TEntity>();
                foreach (string includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                return query.Where(predicate).FirstOrDefaultAsync();
            }
        }
        /// <summary>
        /// Find By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<TEntity> FindByIdAsync(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeProperties = null)
        {
            if (id == null)
                return null;
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="item"></param>
        public void Insert(TEntity item) => _dbContext.Set<TEntity>().Add(item);
        /// <summary>
        /// Insert Bulk
        /// </summary>
        /// <param name="entityToInsert"></param>
        public void InsertBulk(IEnumerable<TEntity> entityToInsert) => _dbContext.Set<TEntity>().AddRange(entityToInsert);
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="item"></param>
        /// <param name="excludeProperties"></param>
        public void Update(TEntity item, params Expression<Func<TEntity, object>>[] excludeProperties)
        {
            _dbContext.Set<TEntity>().Attach(item);

            var entry = _dbContext.Entry(item);
            entry.State = EntityState.Modified;

            excludeProperties.ToList().ForEach(excludeProperty =>
            {
                entry.Property(excludeProperty).IsModified = false;
            });
        }
    }
}