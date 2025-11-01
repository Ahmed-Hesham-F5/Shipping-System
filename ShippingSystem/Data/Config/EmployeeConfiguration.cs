using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeId);
            builder.Property(e => e.EmployeeId).ValueGeneratedNever();

            builder.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street)
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

                address.Property(a => a.City)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

                address.Property(a => a.Governorate)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

                address.Property(a => a.Details)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired(false);

                address.Property(a => a.GoogleMapAddressLink)
                .HasColumnType("nvarchar")
                .HasMaxLength(2083)
                .IsRequired(false);
            });

            builder.HasOne(e => e.Hub)
                .WithMany(h => h.Employees)
                .HasForeignKey(e => e.HubId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("Employees");
        }
    }
}
