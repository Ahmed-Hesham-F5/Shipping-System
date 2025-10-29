using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AlterReturnRequestsColumnsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReturnPickupWindowStart",
                table: "ReturnRequests",
                newName: "WindowStart");

            migrationBuilder.RenameColumn(
                name: "ReturnPickupWindowEnd",
                table: "ReturnRequests",
                newName: "WindowEnd");

            migrationBuilder.RenameColumn(
                name: "ReturnPickupDate",
                table: "ReturnRequests",
                newName: "ReturnDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WindowStart",
                table: "ReturnRequests",
                newName: "ReturnPickupWindowStart");

            migrationBuilder.RenameColumn(
                name: "WindowEnd",
                table: "ReturnRequests",
                newName: "ReturnPickupWindowEnd");

            migrationBuilder.RenameColumn(
                name: "ReturnDate",
                table: "ReturnRequests",
                newName: "ReturnPickupDate");
        }
    }
}
