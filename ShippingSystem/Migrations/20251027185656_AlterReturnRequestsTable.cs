using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AlterReturnRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnAddress_City",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ReturnAddress_Details",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ReturnAddress_GoogleMapAddressLink",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ReturnAddress_Governorate",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ReturnAddress_Street",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ReturnWindowEnd",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ReturnWindowStart",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ShipperContactName",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "ShipperContactPhone",
                table: "ReturnRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReturnAddress_City",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReturnAddress_Details",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnAddress_GoogleMapAddressLink",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnAddress_Governorate",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReturnAddress_Street",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReturnDate",
                table: "ReturnRequests",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ReturnWindowEnd",
                table: "ReturnRequests",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ReturnWindowStart",
                table: "ReturnRequests",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "ShipperContactName",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipperContactPhone",
                table: "ReturnRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
