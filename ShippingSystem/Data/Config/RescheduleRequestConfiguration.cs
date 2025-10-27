using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class RescheduleRequestConfiguration : IEntityTypeConfiguration<RescheduleRequest>
    {
        public void Configure(EntityTypeBuilder<RescheduleRequest> builder)
        {
            builder.Property(r => r.OldRequestDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(r => r.OldTimeWindowStart)
                .HasColumnType("time")
                .IsRequired();

            builder.Property(r => r.OldTimeWindowEnd)
                .HasColumnType("time")
                .IsRequired();

            builder.Property(r => r.NewRequestDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(r => r.NewTimeWindowStart)
                .HasColumnType("time")
                .IsRequired();

            builder.Property(r => r.NewTimeWindowEnd)
                .HasColumnType("time")
                .IsRequired();

            builder.Property(r => r.Reason)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.ToTable("RescheduleRequests");
        }
    }
}
