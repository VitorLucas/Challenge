using Challenge.Domain;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Api.Infrastructure.Context
{
    public class SecurityContext : DbContext
    {
        public SecurityContext(DbContextOptions<SecurityContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "SecurityDb");
        }

        public DbSet<IsinModel> Isin { get; set; }
    }
}
