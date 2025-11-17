using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasKey(shipment => shipment.Id);
            builder.Property(shipment => shipment.Id)
                .ValueGeneratedOnAdd();

            builder.Property(shipment => shipment.CustomerName)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(shipment => shipment.CustomerPhone)
                .HasColumnType("nvarchar")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(shipment => shipment.CustomerAdditionalPhone)
                .HasColumnType("nvarchar")
                .HasMaxLength(11)
                .IsRequired(false);

            builder.OwnsOne(shipment => shipment.CustomerAddress, address =>
            {
                address.Property(a => a.Street)
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

                address.Property(a => a.City)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

                address.Property(a => a.Governorate)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

                address.Property(a => a.Details)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired(false);

                address.Property(a => a.GoogleMapAddressLink)
                .HasColumnType("nvarchar")
                .HasMaxLength(2083)
                .IsRequired(false);
            });

            builder.Property(shipment => shipment.CustomerEmail)
                .HasColumnType("nvarchar")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(shipment => shipment.ShipmentDescription)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(shipment => shipment.ShipmentTrackingNumber)
                .HasColumnType("nvarchar")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(shipment => shipment.ShipmentNotes)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasMany(shipment => shipment.ShipmentStatuses)
                .WithOne(status => status.Shipment)
                .HasForeignKey(status => status.ShipmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(shipment => shipment.ShipmentTrackingNumber)
                .IsUnique();

            builder.ToTable("Shipments");
        }
    }
}
