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
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(shipperAddress => shipperAddress.City)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(shipperAddress => shipperAddress.Governorate)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(shipperAddress => shipperAddress.Details)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.ToTable("ShipperAddresses");
        }
    }
}
