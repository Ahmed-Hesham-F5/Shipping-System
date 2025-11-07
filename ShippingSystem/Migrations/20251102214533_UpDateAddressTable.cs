using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpDateAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipperAddresses");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address_Details",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address_GoogleMapAddressLink",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address_Governorate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                table: "Employees");

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

            migrationBuilder.CreateTable(
                name: "UserAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GoogleMapAddressLink = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddresses_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_UserID",
                table: "UserAddresses",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAddresses");

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

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Details",
                table: "Employees",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_GoogleMapAddressLink",
                table: "Employees",
                type: "nvarchar(2083)",
                maxLength: 2083,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Governorate",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "Employees",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShipperAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipperId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipperAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipperAddresses_Shippers_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "Shippers",
                        principalColumn: "ShipperId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShipperAddresses_ShipperId",
                table: "ShipperAddresses",
                column: "ShipperId");
        }
    }
}
