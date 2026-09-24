using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmWorking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FarmSingleProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkProcessId",
                table: "Farms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE f
                SET [WorkProcessId] = assignments.[WorkProcessId]
                FROM [Farms] f
                INNER JOIN (
                    SELECT [FarmId], MIN([WorkProcessId]) AS [WorkProcessId]
                    FROM [FarmWorkProcesses]
                    GROUP BY [FarmId]
                ) assignments ON assignments.[FarmId] = f.[Id];
                """);

            migrationBuilder.DropTable(
                name: "FarmWorkProcesses");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_WorkProcessId",
                table: "Farms",
                column: "WorkProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_WorkProcesses_WorkProcessId",
                table: "Farms",
                column: "WorkProcessId",
                principalTable: "WorkProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farms_WorkProcesses_WorkProcessId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_WorkProcessId",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "WorkProcessId",
                table: "Farms");

            migrationBuilder.CreateTable(
                name: "FarmWorkProcesses",
                columns: table => new
                {
                    FarmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmWorkProcesses", x => new { x.FarmId, x.WorkProcessId });
                    table.ForeignKey(
                        name: "FK_FarmWorkProcesses_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmWorkProcesses_WorkProcesses_WorkProcessId",
                        column: x => x.WorkProcessId,
                        principalTable: "WorkProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmWorkProcesses_WorkProcessId",
                table: "FarmWorkProcesses",
                column: "WorkProcessId");
        }
    }
}
