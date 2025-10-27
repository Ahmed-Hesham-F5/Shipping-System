using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class CancellationRequestConfiguration : IEntityTypeConfiguration<CancellationRequest>
    {
        public void Configure(EntityTypeBuilder<CancellationRequest> builder)
        {
            builder.HasMany(cr => cr.CancellationRequestShipments)
           .WithOne(crs => crs.CancellationRequest)
           .HasForeignKey(prs => prs.CancellationRequestId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("CancellationRequests");
        }
    }
}
