using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class RequestBaseConfiguration : IEntityTypeConfiguration<RequestBase>
    {
        public void Configure(EntityTypeBuilder<RequestBase> builder)
        {
            builder.HasKey(srb => srb.Id);
            builder.Property(srb => srb.Id)
                .ValueGeneratedOnAdd();

            builder.Property(srb => srb.RequestType)
                .HasConversion<string>()
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(srb => srb.RequestStatus)
                .HasConversion<string>()
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(srb => srb.User)
               .WithMany()
               .HasForeignKey(srb => srb.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Requests");
        }
    }
}
