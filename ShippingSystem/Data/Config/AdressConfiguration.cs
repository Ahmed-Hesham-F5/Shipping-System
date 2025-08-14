using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class AdressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            builder.Property(a => a.Street)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.City)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Country)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Egypt");

            builder.Property(a => a.Details)
                .HasColumnType("nvarchar")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.HasOne(a => a.Shipper)
                .WithMany(s => s.Addresses)
                .HasForeignKey(a => a.ShipperId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Addresses");
        }
    }
}
