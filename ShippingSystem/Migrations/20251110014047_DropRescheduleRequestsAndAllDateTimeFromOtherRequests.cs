using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Migrations
{
    /// <inheritdoc />
    public partial class DropRescheduleRequestsAndAllDateTimeFromOtherRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RescheduleRequests");

            migrationBuilder.DropColumn(
                name: "PickupDate",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "WindowEnd",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "WindowStart",
                table: "ReturnRequests");

            migrationBuilder.DropColumn(
                name: "PickupDate",
                table: "PickupRequests");

            migrationBuilder.DropColumn(
                name: "WindowEnd",
                table: "PickupRequests");

            migrationBuilder.DropColumn(
                name: "WindowStart",
                table: "PickupRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "PickupDate",
                table: "ReturnRequests",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "WindowEnd",
                table: "ReturnRequests",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "WindowStart",
                table: "ReturnRequests",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<DateOnly>(
                name: "PickupDate",
                table: "PickupRequests",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "WindowEnd",
                table: "PickupRequests",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "WindowStart",
                table: "PickupRequests",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateTable(
                name: "RescheduleRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NewRequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NewTimeWindowEnd = table.Column<TimeOnly>(type: "time", nullable: false),
                    NewTimeWindowStart = table.Column<TimeOnly>(type: "time", nullable: false),
                    OldRequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OldTimeWindowEnd = table.Column<TimeOnly>(type: "time", nullable: false),
                    OldTimeWindowStart = table.Column<TimeOnly>(type: "time", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ScheduledRequestId = table.Column<int>(type: "int", nullable: false),
                    ScheduledRequestType = table.Column<int>(type: "int", nullable: false)
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
    }
}
