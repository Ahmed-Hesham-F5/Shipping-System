using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class UserPhoneConfiguration : IEntityTypeConfiguration<UserPhone>
    {
        public void Configure(EntityTypeBuilder<UserPhone> builder)
        {
            builder.HasKey(phone => new { phone.UserId, phone.PhoneNumber });

            builder.Property(phone => phone.PhoneNumber)
                .HasColumnType("nvarchar")
                .HasMaxLength(11)
                .IsRequired();

            builder.ToTable("UserPhones");
        }
    }
}
