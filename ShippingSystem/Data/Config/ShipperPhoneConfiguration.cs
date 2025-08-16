using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ShipperPhoneConfiguration : IEntityTypeConfiguration<ShipperPhone>
    {
        public void Configure(EntityTypeBuilder<ShipperPhone> builder)
        {
            builder.HasKey(phone => new { phone.ShipperId, phone.PhoneNumber });

            builder.Property(phone => phone.PhoneNumber)
                .HasColumnType("nvarchar")
                .HasMaxLength(11)
                .IsRequired();

            builder.ToTable("ShipperPhones");
        }
    }
}
