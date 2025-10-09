using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class PickupRequestShipmentConfiguration : IEntityTypeConfiguration<PickupRequestShipment>
    {
        public void Configure(EntityTypeBuilder<PickupRequestShipment> builder)
        {
            builder.HasKey(prs => new { prs.PickupRequestId, prs.ShipmentId });

            builder.HasOne(prs => prs.Shipment)
                   .WithMany()
                   .HasForeignKey(prs => prs.ShipmentId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("PickupRequestShipments");
        }
    }
}
