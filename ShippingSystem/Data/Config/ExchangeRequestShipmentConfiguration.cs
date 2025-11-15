using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ExchangeRequestShipmentConfiguration : IEntityTypeConfiguration<ExchangeRequestShipment>
    {
        public void Configure(EntityTypeBuilder<ExchangeRequestShipment> builder)
        {
            builder.HasKey(ers => new { ers.ExchangeRequestId, ers.ShipmentId });

            builder.HasOne(rrs => rrs.Shipment)
                   .WithMany()
                   .HasForeignKey(rrs => rrs.ShipmentId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(ers => ers.ExchangeDirection)
                .HasConversion<string>()
                .IsRequired();

            builder.ToTable("ExchangeRequestShipments");
        }
    }
}
