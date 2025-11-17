using Microsoft.EntityFrameworkCore;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ShipmentRequestConfiguration : IEntityTypeConfiguration<ShipmentRequest>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ShipmentRequest> builder)
        {
            builder.HasKey(sr => new { sr.ShipmentId, sr.RequestId });

            builder.HasOne(sr => sr.Shipment)
                   .WithMany(s => s.ShipmentRequests)
                   .HasForeignKey(sr => sr.ShipmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sr => sr.Request)
                   .WithMany()
                   .HasForeignKey(sr => sr.RequestId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("ShipmentRequests");
        }
    }
}
