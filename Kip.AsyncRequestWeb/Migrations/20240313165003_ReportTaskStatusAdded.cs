using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kip.AsyncReport.Migrations
{
    /// <inheritdoc />
    public partial class ReportTaskStatusAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Percent",
                table: "ReportTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ReportTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percent",
                table: "ReportTasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReportTasks");
        }
    }
}
