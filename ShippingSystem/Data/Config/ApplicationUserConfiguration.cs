using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Enums;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(appUser => appUser.FirstName)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(appUser => appUser.LastName)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(appUser => appUser.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(appUser => appUser.Role)
                .HasConversion<string>()
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(appUser => appUser.AccountStatus)
                .HasConversion<byte>()
                .HasDefaultValue(AccountStatus.Active)
                .IsRequired();

            builder.Property(appUser => appUser.MustChangePassword)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasMany(appUser => appUser.Addresses)
                .WithOne(address => address.User)
                .HasForeignKey(address => address.UserID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(appUser => appUser.Phones)
                .WithOne(phone => phone.User)
                .HasForeignKey(phone => phone.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
