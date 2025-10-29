using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ReturnRequestConfiguration
    {
        public void Configure(EntityTypeBuilder<ReturnRequest> builder)
        {
            builder.OwnsOne(rr => rr.Address, address =>
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
            
            builder.Property(rr => rr.CustomerContactName)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(rr => rr.CustomerContactPhone) 
                .HasColumnType("varchar")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(rr => rr.CustomerEmail)
                .HasColumnType("nvarchar")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(rr => rr.ReturnDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(rr => rr.WindowStart)
                .HasColumnType("time")
                .IsRequired();

            builder.Property(rr => rr.WindowEnd)
                .HasColumnType("time")
                .IsRequired();

            builder.HasMany(rr => rr.ReturnRequestShipments)
                   .WithOne(rrs => rrs.ReturnRequest)
                   .HasForeignKey(rrs => rrs.ReturnRequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("ReturnRequests");
        }
    }
}
