using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ShipperConfiguration : IEntityTypeConfiguration<Shipper>
    {
        public void Configure(EntityTypeBuilder<Shipper> builder)
        {
            builder.HasKey(shipper => shipper.ShipperId);
            builder.Property(shipper => shipper.ShipperId).ValueGeneratedNever();   

            builder.Property(shipper => shipper.CompanyName)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(shipper => shipper.CompanyLink)
                .HasColumnType("nvarchar")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(shipper => shipper.TypeOfProduction)
                .HasColumnType("nvarchar")
                .IsRequired(false)
                .HasMaxLength(255);

            builder.HasOne(shipper => shipper.User)
                .WithOne()
                .HasForeignKey<Shipper>(shipper => shipper.ShipperId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(shipper => shipper.Shipments)
                .WithOne(shipment => shipment.Shipper)
                .HasForeignKey(shipment => shipment.ShipperId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Shippers");
        }
    }
}
