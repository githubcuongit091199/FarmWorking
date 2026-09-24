using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmWorking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProcessFarmManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkProcesses_Farms_FarmId",
                table: "WorkProcesses");

            migrationBuilder.DropIndex(
                name: "IX_WorkProcesses_FarmId",
                table: "WorkProcesses");

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

            migrationBuilder.Sql("""
                INSERT INTO [FarmWorkProcesses] ([FarmId], [WorkProcessId])
                SELECT [FarmId], [Id]
                FROM [WorkProcesses]
                WHERE [FarmId] IS NOT NULL;
                """);

            migrationBuilder.DropColumn(
                name: "FarmId",
                table: "WorkProcesses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmWorkProcesses");

            migrationBuilder.AddColumn<Guid>(
                name: "FarmId",
                table: "WorkProcesses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkProcesses_FarmId",
                table: "WorkProcesses",
                column: "FarmId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProcesses_Farms_FarmId",
                table: "WorkProcesses",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
