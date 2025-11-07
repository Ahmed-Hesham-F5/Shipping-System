using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddHubStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "HubStatus",
                table: "Hubs",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AlterColumn<byte>(
                name: "AccountStatus",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1,
                oldClrType: typeof(short),
                oldType: "smallint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HubStatus",
                table: "Hubs");

            migrationBuilder.AlterColumn<short>(
                name: "AccountStatus",
                table: "AspNetUsers",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)1);
        }
    }
}
