using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tracking.DataAccess.Patterns.Factory;
using Tracking.DataAccess.Patterns.Repository;

namespace Tracking.DataAccess
{
    public static class DataAccessExtension
    {
        public static IServiceCollection RegisterDataAccess<TDbContext>(this IServiceCollection serviceCollection)
            where TDbContext : BaseDbContext<TDbContext>
        {
            RegisterDataServices<TDbContext>(serviceCollection);
            return serviceCollection;
        }

        private static void RegisterDataServices<TDbContext>(IServiceCollection serviceCollection)
            where TDbContext : BaseDbContext<TDbContext>
        {
            serviceCollection.TryAddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            serviceCollection.TryAddTransient<IDbContext, TDbContext>();
            serviceCollection.TryAddTransient(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}