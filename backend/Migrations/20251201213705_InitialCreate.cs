using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BaseCurrency = table.Column<string>(type: "text", nullable: false),
                    TargetCurrency = table.Column<string>(type: "text", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    SavedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    API_LastUpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledTaskLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskName = table.Column<string>(type: "text", nullable: false),
                    LastRunUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledTaskLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategory_Category_Category_id",
                        column: x => x.Category_id,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Currency" },
                    { 2, "Metals" },
                    { 3, "Crypto" }
                });

            migrationBuilder.InsertData(
                table: "Prices",
                columns: new[] { "Id", "API_LastUpdatedAtUtc", "BaseCurrency", "Rate", "SavedAtUtc", "TargetCurrency" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "USD", 0.863m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "EUR" },
                    { 2, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "USD", 42.5054m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "TRY" },
                    { 3, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "USD", 77.73m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "RUB" },
                    { 4, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "USD", 41.98m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "UAH" },
                    { 5, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EUR", 1.1588m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "USD" },
                    { 6, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EUR", 49.2618m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "TRY" },
                    { 7, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EUR", 89.88m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "RUB" },
                    { 8, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EUR", 48.68m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "UAH" },
                    { 9, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "TRY", 0.02353m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "USD" },
                    { 10, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "TRY", 0.02029m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "EUR" },
                    { 11, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "TRY", 1.825m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "RUB" },
                    { 12, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "TRY", 0.9885m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "UAH" },
                    { 13, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "RUB", 0.0129m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "USD" },
                    { 14, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "RUB", 0.01113m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "EUR" },
                    { 15, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "RUB", 0.548m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "TRY" },
                    { 16, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "RUB", 0.5417m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "UAH" },
                    { 17, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "UAH", 0.02382m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "USD" },
                    { 18, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "UAH", 0.02054m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "EUR" },
                    { 19, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "UAH", 1.012m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "TRY" },
                    { 20, new DateTimeOffset(new DateTime(2025, 11, 30, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "UAH", 1.846m, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 15, 31, 849, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "RUB" },
                    { 21, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "USD", 0.8625m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 43, 821, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "EUR" },
                    { 22, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "USD", 42.5085m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 119, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "TRY" },
                    { 23, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "USD", 77.6748m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 207, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "RUB" },
                    { 24, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "USD", 42.2858m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 294, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "UAH" },
                    { 25, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EUR", 1.1594m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 377, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "USD" },
                    { 26, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EUR", 49.2876m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 463, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "TRY" },
                    { 27, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EUR", 90.4284m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 545, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "RUB" },
                    { 28, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EUR", 48.9455m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 628, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "UAH" },
                    { 29, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "TRY", 0.02352m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 715, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "USD" },
                    { 30, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "TRY", 0.02029m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 802, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "EUR" },
                    { 31, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "TRY", 1.8244m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 887, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "RUB" },
                    { 32, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "TRY", 0.9951m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 44, 979, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "UAH" },
                    { 33, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "RUB", 0.01283m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 45, 76, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "USD" },
                    { 34, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "RUB", 0.01111m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 45, 172, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "EUR" },
                    { 35, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "RUB", 0.5481m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 45, 254, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "TRY" },
                    { 36, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "RUB", 0.5423m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 45, 343, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "UAH" },
                    { 37, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "UAH", 0.02365m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 45, 427, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "USD" },
                    { 38, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "UAH", 0.02043m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 45, 505, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "EUR" },
                    { 39, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "UAH", 1.0050m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 45, 592, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "TRY" },
                    { 40, new DateTimeOffset(new DateTime(2025, 12, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "UAH", 1.8440m, new DateTimeOffset(new DateTime(2025, 12, 1, 22, 52, 45, 680, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "RUB" }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "Category_id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, 1, "TL", "Turkish Lira" },
                    { 2, 1, "EUR", "Euro" },
                    { 3, 1, "USD", "US Dollar" },
                    { 4, 1, "UAH", "Ukrainian Hryvnia" },
                    { 5, 1, "RUB", "Russian Ruble" },
                    { 6, 2, "AU", "Gold" },
                    { 7, 2, "AG", "Silver" },
                    { 8, 2, "PT", "Platinum" },
                    { 9, 3, "BTC", "Bitcoin" },
                    { 10, 3, "ETH", "Etherium" },
                    { 11, 3, "XRP", "Ripple" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subcategory_Category_id",
                table: "Subcategory",
                column: "Category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "ScheduledTaskLogs");

            migrationBuilder.DropTable(
                name: "Subcategory");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
