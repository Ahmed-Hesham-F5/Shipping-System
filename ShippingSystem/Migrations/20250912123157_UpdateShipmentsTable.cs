using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShipmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdditionalWeightCost",
                table: "Shipments",
                newName: "AdditionalWeightCostPrtKg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdditionalWeightCostPrtKg",
                table: "Shipments",
                newName: "AdditionalWeightCost");
        }
    }
}
