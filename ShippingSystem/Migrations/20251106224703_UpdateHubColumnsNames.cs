using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHubColumnsNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HubAddress_Street",
                table: "Hubs",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "HubAddress_Governorate",
                table: "Hubs",
                newName: "Address_Governorate");

            migrationBuilder.RenameColumn(
                name: "HubAddress_GoogleMapAddressLink",
                table: "Hubs",
                newName: "Address_GoogleMapAddressLink");

            migrationBuilder.RenameColumn(
                name: "HubAddress_Details",
                table: "Hubs",
                newName: "Address_Details");

            migrationBuilder.RenameColumn(
                name: "HubAddress_City",
                table: "Hubs",
                newName: "Address_City");

            migrationBuilder.RenameColumn(
                name: "HubType",
                table: "Hubs",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "HubPhoneNumber",
                table: "Hubs",
                newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Hubs",
                newName: "HubAddress_Street");

            migrationBuilder.RenameColumn(
                name: "Address_Governorate",
                table: "Hubs",
                newName: "HubAddress_Governorate");

            migrationBuilder.RenameColumn(
                name: "Address_GoogleMapAddressLink",
                table: "Hubs",
                newName: "HubAddress_GoogleMapAddressLink");

            migrationBuilder.RenameColumn(
                name: "Address_Details",
                table: "Hubs",
                newName: "HubAddress_Details");

            migrationBuilder.RenameColumn(
                name: "Address_City",
                table: "Hubs",
                newName: "HubAddress_City");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Hubs",
                newName: "HubType");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Hubs",
                newName: "HubPhoneNumber");
        }
    }
}
