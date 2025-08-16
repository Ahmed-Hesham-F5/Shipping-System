using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ShipperAddressConfiguration : IEntityTypeConfiguration<ShipperAddress>
    {
        public void Configure(EntityTypeBuilder<ShipperAddress> builder)
        {
            builder.HasKey(shipperAddress => shipperAddress.Id);
            builder.Property(shipperAddress => shipperAddress.Id).ValueGeneratedOnAdd();

            builder.Property(shipperAddress => shipperAddress.Street)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(shipperAddress => shipperAddress.City)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(shipperAddress => shipperAddress.Country)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Egypt");

            builder.Property(shipperAddress => shipperAddress.Details)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasOne(shipperAddress => shipperAddress.Shipper)
                .WithMany(shipper => shipper.Addresses)
                .HasForeignKey(shipperAddress => shipperAddress.ShipperId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("ShipperAddresses");
        }
    }
}
