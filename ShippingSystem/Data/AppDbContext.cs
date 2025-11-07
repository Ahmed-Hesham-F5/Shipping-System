using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Models;

namespace ShippingSystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPhone> UserPhones { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentStatus> ShipmentStatuses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ShippingSetting> ShippingSettings { get; set; }
        public DbSet<RequestBase> Requests { get; set; }
        public DbSet<PickupRequest> PickupRequests { get; set; }
        public DbSet<PickupRequestShipment> PickupRequestShipments { get; set; }
        public DbSet<ReturnRequest> ReturnRequests { get; set; }
        public DbSet<ReturnRequestShipment> ReturnRequestShipments { get; set; }
        public DbSet<CancellationRequest> CancellationRequests { get; set; }
        public DbSet<CancellationRequestShipment> CancellationRequestShipments { get; set; }
        public DbSet<RescheduleRequest> RescheduleRequests { get; set; }
        public DbSet<Hub> Hubs { get; set; }
        public DbSet<Employee> Employees { get; set; }

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
