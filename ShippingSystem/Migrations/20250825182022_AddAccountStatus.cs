using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CashOnDeliveryEnabled",
                table: "Shipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExpressDeliveryEnabled",
                table: "Shipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OpenPackageOnDeliveryEnabled",
                table: "Shipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverAdditionalPhone",
                table: "Shipments",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverAddress_GoogleMapAddressLink",
                table: "Shipments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "AccountStatus",
                table: "AspNetUsers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashOnDeliveryEnabled",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ExpressDeliveryEnabled",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "OpenPackageOnDeliveryEnabled",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ReceiverAdditionalPhone",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ReceiverAddress_GoogleMapAddressLink",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "AspNetUsers");
        }
    }
}
