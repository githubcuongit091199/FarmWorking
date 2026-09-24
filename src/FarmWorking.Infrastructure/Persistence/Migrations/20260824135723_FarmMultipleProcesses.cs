using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmWorking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FarmMultipleProcesses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farms_WorkProcesses_WorkProcessId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_WorkProcessId",
                table: "Farms");

            migrationBuilder.CreateTable(
                name: "FarmWorkProcesses",
                columns: table => new
                {
                    FarmsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkProcessesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmWorkProcesses", x => new { x.FarmsId, x.WorkProcessesId });
                    table.ForeignKey(
                        name: "FK_FarmWorkProcesses_Farms_FarmsId",
                        column: x => x.FarmsId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmWorkProcesses_WorkProcesses_WorkProcessesId",
                        column: x => x.WorkProcessesId,
                        principalTable: "WorkProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmWorkProcesses_WorkProcessesId",
                table: "FarmWorkProcesses",
                column: "WorkProcessesId");

            migrationBuilder.Sql("""
                INSERT INTO [FarmWorkProcesses] ([FarmsId], [WorkProcessesId])
                SELECT [Id], [WorkProcessId]
                FROM [Farms]
                WHERE [WorkProcessId] IS NOT NULL;
                """);

            migrationBuilder.DropColumn(
                name: "WorkProcessId",
                table: "Farms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkProcessId",
                table: "Farms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE f
                SET [WorkProcessId] = assignments.[WorkProcessesId]
                FROM [Farms] f
                INNER JOIN (
                    SELECT [FarmsId], MIN([WorkProcessesId]) AS [WorkProcessesId]
                    FROM [FarmWorkProcesses]
                    GROUP BY [FarmsId]
                ) assignments ON assignments.[FarmsId] = f.[Id];
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
    }
}
