using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingSystem.Enums;

namespace ShippingSystem.Data.Config
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                 new IdentityRole { Id=((int)RolesEnum.Shipper).ToString(), Name = RolesEnum.Shipper.ToString(), NormalizedName = RolesEnum.Shipper.ToString().ToUpper() },
                 new IdentityRole { Id=((int)RolesEnum.Courier).ToString(), Name = RolesEnum.Courier.ToString(), NormalizedName = RolesEnum.Courier.ToString().ToUpper() },
                 new IdentityRole { Id=((int)RolesEnum.Storekeeper).ToString(), Name = RolesEnum.Storekeeper.ToString(), NormalizedName = RolesEnum.Storekeeper.ToString().ToUpper() },
                 new IdentityRole { Id=((int)RolesEnum.TechnicalSupport).ToString(), Name = RolesEnum.TechnicalSupport.ToString(), NormalizedName = RolesEnum.TechnicalSupport.ToString().ToUpper() },
                 new IdentityRole { Id=((int)RolesEnum.WarehouseManager).ToString(), Name = RolesEnum.WarehouseManager.ToString(), NormalizedName = RolesEnum.WarehouseManager.ToString().ToUpper() },
                 new IdentityRole { Id=((int)RolesEnum.Accountant).ToString(), Name = RolesEnum.Accountant.ToString(), NormalizedName = RolesEnum.Accountant.ToString().ToUpper() },
                 new IdentityRole { Id=((int)RolesEnum.Admin).ToString(), Name = RolesEnum.Admin.ToString(), NormalizedName = RolesEnum.Admin.ToString().ToUpper() },
                 new IdentityRole { Id=((int)RolesEnum.MainAdmin).ToString(), Name = RolesEnum.MainAdmin.ToString(), NormalizedName = RolesEnum.MainAdmin.ToString().ToUpper() }
                
                 );

        }
    }
}
