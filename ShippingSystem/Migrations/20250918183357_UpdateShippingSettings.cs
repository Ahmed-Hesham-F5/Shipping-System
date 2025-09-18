using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShippingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdditionalWeightCost",
                table: "Shipments",
                newName: "AdditionalWeightCostPrtKg");

            migrationBuilder.UpdateData(
                table: "ShippingSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "Key",
                value: "AdditionalWeightCostPrtKg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdditionalWeightCostPrtKg",
                table: "Shipments",
                newName: "AdditionalWeightCost");

            migrationBuilder.UpdateData(
                table: "ShippingSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "Key",
                value: "AdditionalWeightCost");
        }
    }
}
