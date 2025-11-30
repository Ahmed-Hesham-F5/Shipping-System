using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    public partial class SeedingGovernorate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cairo" },
                    { 2, "Giza" },
                    { 3, "Alexandria" },
                    { 4, "Dakahlia" },
                    { 5, "Red Sea" },
                    { 6, "Beheira" },
                    { 7, "Fayoum" },
                    { 8, "Gharbia" },
                    { 9, "Ismailia" },
                    { 10, "Menofia" },
                    { 11, "Minya" },
                    { 12, "Qaliubiya" },
                    { 13, "New Valley" },
                    { 14, "Suez" },
                    { 15, "Aswan" },
                    { 16, "Assiut" },
                    { 17, "Beni Suef" },
                    { 18, "Port Said" },
                    { 19, "Damietta" },
                    { 20, "Sharkia" },
                    { 21, "South Sinai" },
                    { 22, "Kafr El Sheikh" },
                    { 23, "Qena" },
                    { 24, "North Sinai" },
                    { 25, "Sohag" },
                    { 26, "Luxor" },
                    { 27, "Matrouh" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i <= 27; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Governorates",
                    keyColumn: "Id",
                    keyValue: i);
            }
        }
    }
}
