using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{

    public class RefreshTokenConfiguration:IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(o => new { o.UserId, o.Id });

            builder.Property(o=>o.Id).ValueGeneratedOnAdd();

            builder.HasIndex(o => o.Token).IsUnique();

        }
    }
}
