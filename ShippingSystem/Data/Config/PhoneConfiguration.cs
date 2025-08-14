using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.PhoneNumber)
                .HasColumnType("nvarchar")
                .HasMaxLength(15)
                .IsRequired();

            builder.HasOne(p => p.Shipper)
                .WithMany(s => s.Phones)
                .HasForeignKey(p => p.ShipperId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Phones");
        }
    }
}
