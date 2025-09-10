using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddShipmentCostAndPriceProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReceiverAddress_GoogleMapAddressLink",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalWeight",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalWeightCost",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CollectionAmount",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CollectionFee",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalWeight",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "AdditionalWeightCost",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "CollectionAmount",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "CollectionFee",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Shipments");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverAddress_GoogleMapAddressLink",
                table: "Shipments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
