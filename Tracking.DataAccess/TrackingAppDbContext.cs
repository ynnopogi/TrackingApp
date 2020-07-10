using Microsoft.EntityFrameworkCore;
using Tracking.Entities.Models;

namespace Tracking.DataAccess
{
    public class TrackingAppDbContext : BaseDbContext<TrackingAppDbContext>
    {
        public TrackingAppDbContext(DbContextOptions<TrackingAppDbContext> contextOptions) : base(contextOptions)
        { }

        public virtual DbSet<UserIdentity> UserIdentities { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //TODO:
        //    //
        //    // uncomment when doing the migration process from package manager console.
        //    //   
        //    const string conn = "Server=(LocalDB)\\MSSQLLocalDB;Database=QA4S;Trusted_Connection=True;";
        //    optionsBuilder.UseSqlServer(conn, builder =>
        //    {
        //        builder.EnableRetryOnFailure(5, System.TimeSpan.FromSeconds(10), null);
        //    });
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}