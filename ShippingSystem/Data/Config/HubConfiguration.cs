using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Enums;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class HubConfiguration : IEntityTypeConfiguration<Hub>
    {
        public void Configure(EntityTypeBuilder<Hub> builder)
        {

            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id)
                .ValueGeneratedOnAdd();

            builder.Property(h => h.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.AreaInSquareMeters)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(h => h.HubStatus)
                .HasConversion<byte>()
                .HasDefaultValue(HubStatusEnum.Active)
                .IsRequired();

            builder.OwnsOne(h => h.Address, address =>
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

            builder.Property(h => h.PhoneNumber)
                .HasColumnType("nvarchar")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(h => h.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.HasMany(h => h.Shipments)
                .WithOne(s => s.Hub)
                .HasForeignKey(s => s.HubId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(h => h.Manager)
                .WithMany()
                .HasForeignKey(h => h.ManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(h => h.Employees)
                .WithOne(e => e.Hub)
                .HasForeignKey(e => e.HubId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("Hubs");
        }
    }
}
