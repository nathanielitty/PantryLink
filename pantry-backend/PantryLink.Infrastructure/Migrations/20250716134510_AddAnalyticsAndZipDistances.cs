using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PantryLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalyticsAndZipDistances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PantryAnalytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PantryId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalItemsCount = table.Column<int>(type: "int", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    ExpiringItemsCount = table.Column<int>(type: "int", nullable: false),
                    VisitorCount = table.Column<int>(type: "int", nullable: false),
                    InventoryUpdatesCount = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "double", precision: 3, scale: 2, nullable: false),
                    MostPopularCategory = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItemsDistributedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PantryAnalytics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PantryAnalytics_Pantries_PantryId",
                        column: x => x.PantryId,
                        principalTable: "Pantries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SystemAnalytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalActivePantries = table.Column<int>(type: "int", nullable: false),
                    TotalUsers = table.Column<int>(type: "int", nullable: false),
                    TotalInventoryItems = table.Column<int>(type: "int", nullable: false),
                    TotalSearches = table.Column<int>(type: "int", nullable: false),
                    TotalRecommendations = table.Column<int>(type: "int", nullable: false),
                    MostSearchedZipCode = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalItemsDistributed = table.Column<int>(type: "int", nullable: false),
                    AverageSystemRating = table.Column<double>(type: "double", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAnalytics", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ZipDistances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FromZipCode = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ToZipCode = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DistanceMiles = table.Column<double>(type: "double", precision: 8, scale: 2, nullable: false),
                    EstimatedTravelTimeMinutes = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipDistances", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.InsertData(
                table: "ZipDistances",
                columns: new[] { "Id", "DistanceMiles", "EstimatedTravelTimeMinutes", "FromZipCode", "IsVerified", "LastUpdated", "ToZipCode" },
                values: new object[,]
                {
                    { 1, 2.5, 8, "12345", true, new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(2089), "12346" },
                    { 2, 5.0999999999999996, 15, "12345", true, new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(2091), "12347" },
                    { 3, 2.5, 8, "12346", true, new DateTime(2025, 7, 16, 13, 45, 9, 964, DateTimeKind.Utc).AddTicks(2092), "12345" }
                });

            migrationBuilder.CreateIndex(
                name: "idx_pantry_date",
                table: "PantryAnalytics",
                columns: new[] { "PantryId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_system_date",
                table: "SystemAnalytics",
                column: "Date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_zip_pair",
                table: "ZipDistances",
                columns: new[] { "FromZipCode", "ToZipCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PantryAnalytics");

            migrationBuilder.DropTable(
                name: "SystemAnalytics");

            migrationBuilder.DropTable(
                name: "ZipDistances");

            migrationBuilder.UpdateData(
                table: "Pantries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 11, 33, 525, DateTimeKind.Utc).AddTicks(3426), new DateTime(2025, 7, 16, 13, 11, 33, 525, DateTimeKind.Utc).AddTicks(3426) });

            migrationBuilder.UpdateData(
                table: "Pantries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 11, 33, 525, DateTimeKind.Utc).AddTicks(3444), new DateTime(2025, 7, 16, 13, 11, 33, 525, DateTimeKind.Utc).AddTicks(3444) });
        }
    }
}
