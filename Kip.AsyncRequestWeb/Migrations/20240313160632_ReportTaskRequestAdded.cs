using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kip.AsyncReport.Migrations
{
    /// <inheritdoc />
    public partial class ReportTaskRequestAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Request_PeriodEnd",
                table: "ReportTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Request_PeriodStart",
                table: "ReportTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Request_UserId",
                table: "ReportTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Request_PeriodEnd",
                table: "ReportTasks");

            migrationBuilder.DropColumn(
                name: "Request_PeriodStart",
                table: "ReportTasks");

            migrationBuilder.DropColumn(
                name: "Request_UserId",
                table: "ReportTasks");
        }
    }
}
