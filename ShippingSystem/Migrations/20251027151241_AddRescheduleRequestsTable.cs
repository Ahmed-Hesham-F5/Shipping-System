using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddRescheduleRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RescheduleRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ScheduledRequestId = table.Column<int>(type: "int", nullable: false),
                    ScheduledRequestType = table.Column<int>(type: "int", nullable: false),
                    OldRequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OldTimeWindowStart = table.Column<TimeOnly>(type: "time", nullable: false),
                    OldTimeWindowEnd = table.Column<TimeOnly>(type: "time", nullable: false),
                    NewRequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NewTimeWindowStart = table.Column<TimeOnly>(type: "time", nullable: false),
                    NewTimeWindowEnd = table.Column<TimeOnly>(type: "time", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescheduleRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RescheduleRequests_Requests_Id",
                        column: x => x.Id,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RescheduleRequests");
        }
    }
}
