using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ShipmentStatusConfiguration : IEntityTypeConfiguration<ShipmentStatus>
    {
        public void Configure(EntityTypeBuilder<ShipmentStatus> builder)
        {
            builder.HasKey(shipmentStatus => shipmentStatus.Id);
            builder.Property(shipmentStatus => shipmentStatus.Id)
                .ValueGeneratedOnAdd();

            builder.Property(shipmentStatus => shipmentStatus.Status)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(shipmentStatus => shipmentStatus.Notes)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.ToTable("ShipmentStatuses");
        }
    }
}
