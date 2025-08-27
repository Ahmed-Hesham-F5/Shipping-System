using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;

namespace ShippingSystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<ShipperAddress> ShipperAddresses { get; set; }
        public DbSet<ShipperPhone> ShipperPhones { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentStatus> ShipmentStatuses { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }  

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

            var now = DateTime.UtcNow;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc);

            foreach (var entry in entries)
            {
                var entity = (ITimeStamped)entry.Entity;
                var createdProp = entry.Property(nameof(ITimeStamped.CreatedAt));
                var updatedProp = entry.Property(nameof(ITimeStamped.UpdatedAt));

                if (entry.State == EntityState.Added)
                {
                    createdProp.CurrentValue = now;
                    updatedProp.CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    updatedProp.CurrentValue = now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
