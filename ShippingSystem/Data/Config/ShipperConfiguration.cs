using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ShipperConfiguration : IEntityTypeConfiguration<Shipper>
    {
        public void Configure(EntityTypeBuilder<Shipper> builder)
        {
            builder.HasKey(s => s.ShipperId);
            builder.Property(s=>s.ShipperId).ValueGeneratedNever(); 

            builder.Property(s => s.CompanyName)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.CompanyLink)
                .HasColumnType("nvarchar")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(s => s.TypeOfTheProduction)
                .HasColumnType("nvarchar")
                .IsRequired(false)
                .HasMaxLength(255);

            builder.HasOne(s => s.ApplicationUser)
                .WithOne()
                .HasForeignKey<Shipper>(s => s.ShipperId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Phones)
                .WithOne(p => p.Shipper)
                .HasForeignKey(p => p.ShipperId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Addresses)
                .WithOne(a => a.Shipper)
                .HasForeignKey(a => a.ShipperId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Shippers");
        }
    }
}
