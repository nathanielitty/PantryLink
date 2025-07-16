using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDonationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodDonations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PantryId = table.Column<int>(type: "int", nullable: false),
                    DonorName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DonorEmail = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DonorPhone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreferredPickupTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodDonations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodDonations_Pantries_PantryId",
                        column: x => x.PantryId,
                        principalTable: "Pantries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MonetaryDonations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PantryId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DonorName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DonorEmail = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DonorPhone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentMethod = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRecurring = table.Column<bool>(type: "bit(1)", nullable: false),
                    Notes = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TransactionId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonetaryDonations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonetaryDonations_Pantries_PantryId",
                        column: x => x.PantryId,
                        principalTable: "Pantries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DonationItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FoodDonationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonationItems_FoodDonations_FoodDonationId",
                        column: x => x.FoodDonationId,
                        principalTable: "FoodDonations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Pantries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(4304), new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(4304) });

            migrationBuilder.UpdateData(
                table: "Pantries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(4320), new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(4320) });

            migrationBuilder.UpdateData(
                table: "ZipDistances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(5577));

            migrationBuilder.UpdateData(
                table: "ZipDistances",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(5580));

            migrationBuilder.UpdateData(
                table: "ZipDistances",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdated",
                value: new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(5581));

            migrationBuilder.CreateIndex(
                name: "IX_DonationItems_FoodDonationId",
                table: "DonationItems",
                column: "FoodDonationId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodDonations_PantryId",
                table: "FoodDonations",
                column: "PantryId");

            migrationBuilder.CreateIndex(
                name: "IX_MonetaryDonations_PantryId",
                table: "MonetaryDonations",
                column: "PantryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonationItems");

            migrationBuilder.DropTable(
                name: "MonetaryDonations");

            migrationBuilder.DropTable(
                name: "FoodDonations");

            migrationBuilder.UpdateData(
                table: "Pantries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(964), new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(965) });

            migrationBuilder.UpdateData(
                table: "Pantries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(981), new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(981) });

            migrationBuilder.UpdateData(
                table: "ZipDistances",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(2089));

            migrationBuilder.UpdateData(
                table: "ZipDistances",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(2091));

            migrationBuilder.UpdateData(
                table: "ZipDistances",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdated",
                value: new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(2092));
        }
    }
}
