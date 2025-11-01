using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class PickupRequestConfiguration : IEntityTypeConfiguration<PickupRequest>
    {
        public void Configure(EntityTypeBuilder<PickupRequest> builder)
        {
            builder.OwnsOne(pr => pr.Address, address =>
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

            builder.Property(pr => pr.ContactName)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(pr => pr.ContactPhone)
                .HasColumnType("nvarchar")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(pr => pr.PickupDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(pr => pr.WindowStart)
                .HasColumnType("time")
                .IsRequired();

            builder.Property(pr => pr.WindowEnd)
                .HasColumnType("time")
                .IsRequired();

            builder.HasMany(pr => pr.PickupRequestShipments)
                   .WithOne(prs => prs.PickupRequest)
                   .HasForeignKey(prs => prs.PickupRequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("PickupRequests");
        }
    }
}
