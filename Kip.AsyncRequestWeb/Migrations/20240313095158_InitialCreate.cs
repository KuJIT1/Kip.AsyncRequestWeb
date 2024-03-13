using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kip.AsyncReport.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountSignIn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportTasks_ReportResult_ReportResultId",
                        column: x => x.ReportResultId,
                        principalTable: "ReportResult",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportTasks_ReportResultId",
                table: "ReportTasks",
                column: "ReportResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportTasks");

            migrationBuilder.DropTable(
                name: "ReportResult");
        }
    }
}
