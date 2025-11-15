using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AlterReturnTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipperAddress_City",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipperAddress_Details",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipperAddress_GoogleMapAddressLink",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipperAddress_Governorate",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipperAddress_Street",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipperAddress_City",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ShipperAddress_Details",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ShipperAddress_GoogleMapAddressLink",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ShipperAddress_Governorate",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ShipperAddress_Street",
                table: "ReturnRequests");
        }
    }
}
