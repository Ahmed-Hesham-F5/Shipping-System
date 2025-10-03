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
        public DbSet<ShippingSetting> ShippingSettings { get; set; }
        public DbSet<RequestBase> Requests { get; set; }
        public DbSet<PickupRequest> PickupRequests { get; set; }
        public DbSet<PickupRequestShipment> PickupRequestShipments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
