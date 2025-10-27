using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class CancellationRequestShipmentConfiguration : IEntityTypeConfiguration<CancellationRequestShipment>
    {
        public void Configure(EntityTypeBuilder<CancellationRequestShipment> builder)
        {
            builder.HasKey(crs => new { crs.CancellationRequestId, crs.ShipmentId });

            builder.HasOne(crs => crs.Shipment)
                   .WithMany()
                   .HasForeignKey(prs => prs.ShipmentId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("CancellationRequestShipments");
        }
    }
}
