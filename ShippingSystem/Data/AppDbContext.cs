using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;

namespace ShippingSystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<ShipperAddress> Addresses { get; set; }
        public DbSet<ShipperPhone> Phones { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is ITimeStamped &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (ITimeStamped)entry.Entity;
                var createdProp = entry.Property(nameof(ITimeStamped.CreatedAt));
                var updatedProp = entry.Property(nameof(ITimeStamped.UpdatedAt));

                if (entry.State == EntityState.Added)
                {
                    createdProp.CurrentValue = DateTime.UtcNow;
                    updatedProp.CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    updatedProp.CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
