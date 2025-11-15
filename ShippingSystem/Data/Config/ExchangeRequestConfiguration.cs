using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ExchangeRequestConfiguration : IEntityTypeConfiguration<ExchangeRequest>
    {
        public void Configure(EntityTypeBuilder<ExchangeRequest> builder)
        {
            builder.OwnsOne(e => e.PickupAddress, address =>
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

            builder.OwnsOne(e => e.CustomerAddress, address =>
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

            builder.Property(e => e.CustomerName)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.CustomerPhone)
                .HasColumnType("nvarchar")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(e => e.CustomerEmail)
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired(false);

            builder.Property(e => e.ExchangeReason)
                .HasColumnType("nvarchar")
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.HasMany(e => e.ExchangeRequestShipments)
                .WithOne(ers => ers.ExchangeRequest)
                .HasForeignKey(ers => ers.ExchangeRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("ExchangeRequests");
        }
    }
}
