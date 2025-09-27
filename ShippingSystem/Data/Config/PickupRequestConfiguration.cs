using Microsoft.EntityFrameworkCore;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class PickupRequestConfiguration : IEntityTypeConfiguration<PickupRequest>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PickupRequest> builder)
        {
            builder.HasKey(pr => pr.Id);
            builder.Property(pr => pr.Id).ValueGeneratedOnAdd();

            builder.HasOne(pr => pr.Shipper)
               .WithMany()
               .HasForeignKey(pr => pr.ShipperId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pr => pr.Street)
                  .HasColumnType("nvarchar")
                  .HasMaxLength(256)
                  .IsRequired();

            builder.Property(pr => pr.City)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(pr => pr.Governorate)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(pr => pr.Details)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(pr => pr.ContactName)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(pr => pr.ContactPhone)
                .HasColumnType("varchar")
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
