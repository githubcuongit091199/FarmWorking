using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmWorking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkBasedPayroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkerWorkDays_WorkerId_WorkDate",
                table: "WorkerWorkDays");

            migrationBuilder.AddColumn<decimal>(
                name: "WorkFraction",
                table: "WorkerWorkDays",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 1m);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkTypeId",
                table: "WorkerWorkDays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.AddColumn<string>(
                name: "WorkTypeName",
                table: "WorkerWorkDays",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "Công việc cũ");

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DailyRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WorkTypes",
                columns: new[] { "Id", "Name", "DailyRate", "IsActive" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Công việc cũ", 0m, false });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWorkDays_WorkerId_WorkDate",
                table: "WorkerWorkDays",
                columns: new[] { "WorkerId", "WorkDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWorkDays_WorkTypeId",
                table: "WorkerWorkDays",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTypes_Name",
                table: "WorkTypes",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerWorkDays_WorkTypes_WorkTypeId",
                table: "WorkerWorkDays",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerWorkDays_WorkTypes_WorkTypeId",
                table: "WorkerWorkDays");

            migrationBuilder.DropTable(
                name: "WorkTypes");

            migrationBuilder.DropIndex(
                name: "IX_WorkerWorkDays_WorkerId_WorkDate",
                table: "WorkerWorkDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkerWorkDays_WorkTypeId",
                table: "WorkerWorkDays");

            migrationBuilder.DropColumn(
                name: "WorkFraction",
                table: "WorkerWorkDays");

            migrationBuilder.DropColumn(
                name: "WorkTypeId",
                table: "WorkerWorkDays");

            migrationBuilder.DropColumn(
                name: "WorkTypeName",
                table: "WorkerWorkDays");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWorkDays_WorkerId_WorkDate",
                table: "WorkerWorkDays",
                columns: new[] { "WorkerId", "WorkDate" },
                unique: true);
        }
    }
}
