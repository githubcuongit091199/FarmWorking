using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmWorking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AssignWorkersToFarms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FarmId",
                table: "Workers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_FarmId",
                table: "Workers",
                column: "FarmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Farms_FarmId",
                table: "Workers",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Farms_FarmId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_FarmId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "FarmId",
                table: "Workers");
        }
    }
}
