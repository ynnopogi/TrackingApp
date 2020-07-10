using Microsoft.EntityFrameworkCore;
using System;
using Tracking.DataAccess.Patterns.Uow;

namespace Tracking.DataAccess.Patterns.Factory
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public IUnitOfWork Create(bool trackChanges = true, bool enableLogging = false)
        {
            DbContext dbContext = (DbContext)_serviceProvider.GetService(typeof(IDbContext));
            if (!trackChanges)
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return new UnitOfWork(dbContext, _serviceProvider);
        }
    }
}