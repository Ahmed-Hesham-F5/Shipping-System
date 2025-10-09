using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReturnRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ReturnPickupDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReturnPickupWindowStart = table.Column<TimeOnly>(type: "time", nullable: false),
                    ReturnPickupWindowEnd = table.Column<TimeOnly>(type: "time", nullable: false),
                    ReturnPickupAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnPickupAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnPickupAddress_Governorate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnPickupAddress_Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnPickupAddress_GoogleMapAddressLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReturnWindowStart = table.Column<TimeOnly>(type: "time", nullable: false),
                    ReturnWindowEnd = table.Column<TimeOnly>(type: "time", nullable: false),
                    ReturnAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnAddress_Governorate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnAddress_Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnAddress_GoogleMapAddressLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipperContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShipperContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnRequests_Requests_Id",
                        column: x => x.Id,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnRequestShipments",
                columns: table => new
                {
                    ReturnRequestId = table.Column<int>(type: "int", nullable: false),
                    ShipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRequestShipments", x => new { x.ReturnRequestId, x.ShipmentId });
                    table.ForeignKey(
                        name: "FK_ReturnRequestShipments_ReturnRequests_ReturnRequestId",
                        column: x => x.ReturnRequestId,
                        principalTable: "ReturnRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnRequestShipments_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequestShipments_ShipmentId",
                table: "ReturnRequestShipments",
                column: "ShipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnRequestShipments");

            migrationBuilder.DropTable(
                name: "ReturnRequests");
        }
    }
}
