using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddHubAndEmployeeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipperPhones");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "ShippingSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyLink",
                table: "Shippers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ShipmentStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "Shipments",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerAdditionalPhone",
                table: "Shipments",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HubId",
                table: "Shipments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestType",
                table: "Requests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "RequestStatus",
                table: "Requests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "PickupRequests",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserPhones",
                columns: table => new
                {
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhones", x => new { x.UserId, x.PhoneNumber });
                    table.ForeignKey(
                        name: "FK_UserPhones_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Address_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address_GoogleMapAddressLink = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true),
                    HubId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hubs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HubType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address_GoogleMapAddressLink = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true),
                    HubPhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    AreaInSquareMeters = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hubs_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.Sql("DELETE FROM [AspNetRoles]");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0", null, "Shipper", "SHIPPER" },
                    { "1", null, "Courier", "COURIER" },
                    { "2", null, "Storekeeper", "STOREKEEPER" },
                    { "3", null, "TechnicalSupport", "TECHNICALSUPPORT" },
                    { "4", null, "OperationsAgent", "OPERATIONSAGENT" },
                    { "5", null, "HubManager", "HUBMANAGER" },
                    { "6", null, "Accountant", "ACCOUNTANT" },
                    { "7", null, "Admin", "ADMIN" },
                    { "8", null, "MainAdmin", "MAINADMIN" }
                });


            migrationBuilder.CreateIndex(
                name: "IX_Shipments_HubId",
                table: "Shipments",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_HubId",
                table: "Employees",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_ManagerId",
                table: "Hubs",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Hubs_HubId",
                table: "Shipments",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Hubs_HubId",
                table: "Employees",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Hubs_HubId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Hubs_HubId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "UserPhones");

            migrationBuilder.DropTable(
                name: "Hubs");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_HubId",
                table: "Shipments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DropColumn(
                name: "HubId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "ShippingSettings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyLink",
                table: "Shippers",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ShipmentStatuses",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "Shipments",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerAdditionalPhone",
                table: "Shipments",
                type: "varchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestType",
                table: "Requests",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "RequestStatus",
                table: "Requests",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "PickupRequests",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.CreateTable(
                name: "ShipperPhones",
                columns: table => new
                {
                    ShipperId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipperPhones", x => new { x.ShipperId, x.PhoneNumber });
                    table.ForeignKey(
                        name: "FK_ShipperPhones_Shippers_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "Shippers",
                        principalColumn: "ShipperId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Shipper", "SHIPPER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Courier", "COURIER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Storekeeper", "STOREKEEPER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "TechnicalSupport", "TECHNICALSUPPORT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "128", null, "MainAdmin", "MAINADMIN" },
                    { "16", null, "WarehouseManager", "WAREHOUSEMANAGER" },
                    { "32", null, "Accountant", "ACCOUNTANT" },
                    { "64", null, "Admin", "ADMIN" }
                });
        }
    }
}
