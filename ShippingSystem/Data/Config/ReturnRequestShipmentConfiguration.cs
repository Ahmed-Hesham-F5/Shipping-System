using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ReturnRequestShipmentConfiguration : IEntityTypeConfiguration<ReturnRequestShipment>
    {
        public void Configure(EntityTypeBuilder<ReturnRequestShipment> builder)
        {
            builder.HasKey(rrs => new { rrs.ReturnRequestId, rrs.ShipmentId });

            builder.HasOne(rrs => rrs.Shipment)
                   .WithMany()
                   .HasForeignKey(rrs => rrs.ShipmentId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("ReturnRequestShipments");
        }
    }
}
