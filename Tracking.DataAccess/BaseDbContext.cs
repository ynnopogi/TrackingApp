using Microsoft.EntityFrameworkCore;

namespace Tracking.DataAccess
{
    public class BaseDbContext<TContext> : DbContext, IDbContext where TContext : DbContext
    {
        public BaseDbContext(DbContextOptions<TContext> contextOptions) : base(contextOptions) { }
    }
}