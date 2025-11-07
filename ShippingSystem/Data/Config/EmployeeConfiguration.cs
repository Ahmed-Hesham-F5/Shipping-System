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

            builder.Property(e => e.FirstLogin)
                .HasDefaultValue(true);

            builder.Property(e => e.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.HasOne(e => e.Hub)
                .WithMany(h => h.Employees)
                .HasForeignKey(e => e.HubId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("Employees");
        }
    }
}
