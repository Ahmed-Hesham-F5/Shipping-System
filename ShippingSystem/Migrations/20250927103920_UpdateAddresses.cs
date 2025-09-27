using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "ShipperAddresses");

            migrationBuilder.RenameColumn(
                name: "ReceiverAddress_Country",
                table: "Shipments",
                newName: "ReceiverAddress_Governorate");

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "ShipperAddresses",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "ShipperAddresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverAddress_Street",
                table: "Shipments",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverAddress_GoogleMapAddressLink",
                table: "Shipments",
                type: "nvarchar(2083)",
                maxLength: 2083,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "ShipperAddresses");

            migrationBuilder.RenameColumn(
                name: "ReceiverAddress_Governorate",
                table: "Shipments",
                newName: "ReceiverAddress_Country");

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "ShipperAddresses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ShipperAddresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Egypt");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverAddress_Street",
                table: "Shipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverAddress_GoogleMapAddressLink",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2083)",
                oldMaxLength: 2083,
                oldNullable: true);
        }
    }
}
