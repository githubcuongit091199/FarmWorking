using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmWorking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceFarmPricingWithInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmSupplies");

            migrationBuilder.CreateTable(
                name: "SupplyPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyPrices_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FarmSupplyEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FarmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplyPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UsedQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmSupplyEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmSupplyEntries_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmSupplyEntries_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FarmSupplyEntries_SupplyPrices_SupplyPriceId",
                        column: x => x.SupplyPriceId,
                        principalTable: "SupplyPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmSupplyEntries_FarmId_SupplyId_ReceivedAt",
                table: "FarmSupplyEntries",
                columns: new[] { "FarmId", "SupplyId", "ReceivedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FarmSupplyEntries_SupplyId",
                table: "FarmSupplyEntries",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmSupplyEntries_SupplyPriceId",
                table: "FarmSupplyEntries",
                column: "SupplyPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyPrices_SupplyId_Price",
                table: "SupplyPrices",
                columns: new[] { "SupplyId", "Price" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmSupplyEntries");

            migrationBuilder.DropTable(
                name: "SupplyPrices");

            migrationBuilder.CreateTable(
                name: "FarmSupplies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FarmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmSupplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmSupplies_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmSupplies_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmSupplies_FarmId_SupplyId",
                table: "FarmSupplies",
                columns: new[] { "FarmId", "SupplyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FarmSupplies_SupplyId",
                table: "FarmSupplies",
                column: "SupplyId");
        }
    }
}
