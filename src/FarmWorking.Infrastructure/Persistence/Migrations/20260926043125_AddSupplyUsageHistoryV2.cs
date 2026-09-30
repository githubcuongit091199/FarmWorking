using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmWorking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplyUsageHistoryV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseTransactionId",
                table: "FarmSupplyEntries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FarmSupplyUsages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FarmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplyPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Worker = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmSupplyUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmSupplyUsages_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmSupplyUsages_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FarmSupplyUsages_SupplyPrices_SupplyPriceId",
                        column: x => x.SupplyPriceId,
                        principalTable: "SupplyPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmSupplyEntries_ExpenseTransactionId",
                table: "FarmSupplyEntries",
                column: "ExpenseTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmSupplyUsages_FarmId_UsedAt",
                table: "FarmSupplyUsages",
                columns: new[] { "FarmId", "UsedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FarmSupplyUsages_SupplyId",
                table: "FarmSupplyUsages",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmSupplyUsages_SupplyPriceId",
                table: "FarmSupplyUsages",
                column: "SupplyPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmSupplyEntries_FarmTransactions_ExpenseTransactionId",
                table: "FarmSupplyEntries",
                column: "ExpenseTransactionId",
                principalTable: "FarmTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FarmSupplyEntries_FarmTransactions_ExpenseTransactionId",
                table: "FarmSupplyEntries");

            migrationBuilder.DropTable(
                name: "FarmSupplyUsages");

            migrationBuilder.DropIndex(
                name: "IX_FarmSupplyEntries_ExpenseTransactionId",
                table: "FarmSupplyEntries");

            migrationBuilder.DropColumn(
                name: "ExpenseTransactionId",
                table: "FarmSupplyEntries");
        }
    }
}
