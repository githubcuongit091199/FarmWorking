using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmWorking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteProcesses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "WorkProcesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WorkProcesses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_WorkProcesses_IsDeleted",
                table: "WorkProcesses",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkProcesses_IsDeleted",
                table: "WorkProcesses");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "WorkProcesses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WorkProcesses");
        }
    }
}
