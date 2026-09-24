using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmWorking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFarmProcessRuns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FarmProcessRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FarmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SeasonName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmProcessRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmProcessRuns_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmProcessRuns_WorkProcesses_WorkProcessId",
                        column: x => x.WorkProcessId,
                        principalTable: "WorkProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FarmProcessRunSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FarmProcessRunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DayOffset = table.Column<int>(type: "int", nullable: false),
                    IsFarmExtra = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmProcessRunSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmProcessRunSteps_FarmProcessRuns_FarmProcessRunId",
                        column: x => x.FarmProcessRunId,
                        principalTable: "FarmProcessRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmProcessRuns_FarmId_StartDate",
                table: "FarmProcessRuns",
                columns: new[] { "FarmId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FarmProcessRuns_WorkProcessId",
                table: "FarmProcessRuns",
                column: "WorkProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmProcessRunSteps_FarmProcessRunId_Order",
                table: "FarmProcessRunSteps",
                columns: new[] { "FarmProcessRunId", "Order" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmProcessRunSteps");

            migrationBuilder.DropTable(
                name: "FarmProcessRuns");
        }
    }
}
