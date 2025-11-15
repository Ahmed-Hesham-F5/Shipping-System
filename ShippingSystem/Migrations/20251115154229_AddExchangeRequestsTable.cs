using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddExchangeRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "PickupRequests");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "PickupRequests");

            migrationBuilder.RenameColumn(
                name: "CustomerContactPhone",
                table: "ReturnRequests",
                newName: "CustomerPhone");

            migrationBuilder.RenameColumn(
                name: "CustomerContactName",
                table: "ReturnRequests",
                newName: "CustomerName");

            migrationBuilder.CreateTable(
                name: "ExchangeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PickupAddress_Street = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PickupAddress_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PickupAddress_Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PickupAddress_Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PickupAddress_GoogleMapAddressLink = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true),
                    ShipperName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShipperPhone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    CustomerAddress_Street = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CustomerAddress_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerAddress_Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerAddress_Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustomerAddress_GoogleMapAddressLink = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ExchangeReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRequests_Requests_Id",
                        column: x => x.Id,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRequestShipments",
                columns: table => new
                {
                    ExchangeRequestId = table.Column<int>(type: "int", nullable: false),
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    ExchangeDirection = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRequestShipments", x => new { x.ExchangeRequestId, x.ShipmentId });
                    table.ForeignKey(
                        name: "FK_ExchangeRequestShipments_ExchangeRequests_ExchangeRequestId",
                        column: x => x.ExchangeRequestId,
                        principalTable: "ExchangeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExchangeRequestShipments_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRequestShipments_ShipmentId",
                table: "ExchangeRequestShipments",
                column: "ShipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRequestShipments");

            migrationBuilder.DropTable(
                name: "ExchangeRequests");

            migrationBuilder.RenameColumn(
                name: "CustomerPhone",
                table: "ReturnRequests",
                newName: "CustomerContactPhone");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "ReturnRequests",
                newName: "CustomerContactName");

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "PickupRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "PickupRequests",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");
        }
    }
}
