using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class PickupCoveredGovernorateConfiguration : IEntityTypeConfiguration<PickupCoveredGovernorate>
    {
        public void Configure(EntityTypeBuilder<PickupCoveredGovernorate> builder)
        {
            builder.HasKey(x => new { x.HubId, x.GovernorateId });

            builder.HasOne(x => x.Hub)
                .WithMany(h => h.PickupCoveredGovernorates)
                .HasForeignKey(x => x.HubId);

            builder.HasOne(x => x.Governorate)
                .WithMany()
                .HasForeignKey(x => x.GovernorateId);

            builder.Property(x => x.PickupCost)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.ToTable("PickupCoveredGovernorates");
        }
    }
}
