using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Enums;
using ShippingSystem.Models;

namespace ShippingSystem.Data.Config
{
    public class ShippingSettingConfiguration : IEntityTypeConfiguration<ShippingSetting>
    {
        public void Configure(EntityTypeBuilder<ShippingSetting> builder)
        {
            builder.HasKey(ss => ss.Id);
            builder.Property(ss => ss.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(ss => ss.Key)
                .HasConversion<string>()
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(ss => ss.Value)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasData(
                new ShippingSetting { Id = 1, Key = ShippingSettingKeys.AdditionalWeightCostPrtKg, Value = "5" },
                new ShippingSetting { Id = 2, Key = ShippingSettingKeys.CollectionFeePercentage, Value = "0.01" },
                new ShippingSetting { Id = 3, Key = ShippingSettingKeys.CollectionFeeThreshold, Value = "3000" }
            );

            builder.HasIndex(ss => ss.Key).IsUnique();

            builder.ToTable("ShippingSettings");
        }
    }
}
