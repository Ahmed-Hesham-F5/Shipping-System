using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequestsAddressName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "ReturnRequests",
                newName: "CustomerAddress_Street");

            migrationBuilder.RenameColumn(
                name: "Address_Governorate",
                table: "ReturnRequests",
                newName: "CustomerAddress_Governorate");

            migrationBuilder.RenameColumn(
                name: "Address_GoogleMapAddressLink",
                table: "ReturnRequests",
                newName: "CustomerAddress_GoogleMapAddressLink");

            migrationBuilder.RenameColumn(
                name: "Address_Details",
                table: "ReturnRequests",
                newName: "CustomerAddress_Details");

            migrationBuilder.RenameColumn(
                name: "Address_City",
                table: "ReturnRequests",
                newName: "CustomerAddress_City");

            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "PickupRequests",
                newName: "PickupAddress_Street");

            migrationBuilder.RenameColumn(
                name: "Address_Governorate",
                table: "PickupRequests",
                newName: "PickupAddress_Governorate");

            migrationBuilder.RenameColumn(
                name: "Address_GoogleMapAddressLink",
                table: "PickupRequests",
                newName: "PickupAddress_GoogleMapAddressLink");

            migrationBuilder.RenameColumn(
                name: "Address_Details",
                table: "PickupRequests",
                newName: "PickupAddress_Details");

            migrationBuilder.RenameColumn(
                name: "Address_City",
                table: "PickupRequests",
                newName: "PickupAddress_City");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerAddress_Street",
                table: "ReturnRequests",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "CustomerAddress_Governorate",
                table: "ReturnRequests",
                newName: "Address_Governorate");

            migrationBuilder.RenameColumn(
                name: "CustomerAddress_GoogleMapAddressLink",
                table: "ReturnRequests",
                newName: "Address_GoogleMapAddressLink");

            migrationBuilder.RenameColumn(
                name: "CustomerAddress_Details",
                table: "ReturnRequests",
                newName: "Address_Details");

            migrationBuilder.RenameColumn(
                name: "CustomerAddress_City",
                table: "ReturnRequests",
                newName: "Address_City");

            migrationBuilder.RenameColumn(
                name: "PickupAddress_Street",
                table: "PickupRequests",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "PickupAddress_Governorate",
                table: "PickupRequests",
                newName: "Address_Governorate");

            migrationBuilder.RenameColumn(
                name: "PickupAddress_GoogleMapAddressLink",
                table: "PickupRequests",
                newName: "Address_GoogleMapAddressLink");

            migrationBuilder.RenameColumn(
                name: "PickupAddress_Details",
                table: "PickupRequests",
                newName: "Address_Details");

            migrationBuilder.RenameColumn(
                name: "PickupAddress_City",
                table: "PickupRequests",
                newName: "Address_City");
        }
    }
}
