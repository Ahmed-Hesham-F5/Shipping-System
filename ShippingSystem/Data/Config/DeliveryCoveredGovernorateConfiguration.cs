using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class DeliveryCoveredGovernorateConfiguration : IEntityTypeConfiguration<DeliveryCoveredGovernorate>
    {
        public void Configure(EntityTypeBuilder<DeliveryCoveredGovernorate> builder)
        {
            builder.HasKey(x => new { x.HubId, x.GovernorateId });

            builder.HasOne(x => x.Hub)
                .WithMany(h => h.DeliveryCoveredGovernorates)
                .HasForeignKey(x => x.HubId);

            builder.HasOne(x => x.Governorate)
                .WithMany()
                .HasForeignKey(x => x.GovernorateId);

            builder.Property(x => x.DeliveryCost)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.ToTable("DeliveryCoveredGovernorates");
        }
    }
}
