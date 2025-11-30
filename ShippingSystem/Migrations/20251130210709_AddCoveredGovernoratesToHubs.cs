using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCoveredGovernoratesToHubs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_AspNetUsers_UserID",
                table: "UserAddresses");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserAddresses",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_UserID",
                table: "UserAddresses",
                newName: "IX_UserAddresses_UserId");

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryCoveredGovernorates",
                columns: table => new
                {
                    HubId = table.Column<int>(type: "int", nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryCoveredGovernorates", x => new { x.HubId, x.GovernorateId });
                    table.ForeignKey(
                        name: "FK_DeliveryCoveredGovernorates_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryCoveredGovernorates_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickupCoveredGovernorates",
                columns: table => new
                {
                    HubId = table.Column<int>(type: "int", nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    PickupCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupCoveredGovernorates", x => new { x.HubId, x.GovernorateId });
                    table.ForeignKey(
                        name: "FK_PickupCoveredGovernorates_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickupCoveredGovernorates_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCoveredGovernorates_GovernorateId",
                table: "DeliveryCoveredGovernorates",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_Name",
                table: "Governorates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PickupCoveredGovernorates_GovernorateId",
                table: "PickupCoveredGovernorates",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_AspNetUsers_UserId",
                table: "UserAddresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_AspNetUsers_UserId",
                table: "UserAddresses");

            migrationBuilder.DropTable(
                name: "DeliveryCoveredGovernorates");

            migrationBuilder.DropTable(
                name: "PickupCoveredGovernorates");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserAddresses",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_UserId",
                table: "UserAddresses",
                newName: "IX_UserAddresses_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_AspNetUsers_UserID",
                table: "UserAddresses",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
