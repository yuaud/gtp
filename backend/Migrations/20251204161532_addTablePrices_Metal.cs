using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class addTablePrices_Metal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prices_Metal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Basecurrency = table.Column<string>(type: "text", nullable: false),
                    XAG = table.Column<decimal>(type: "numeric", nullable: false),
                    XAU = table.Column<decimal>(type: "numeric", nullable: false),
                    XPT = table.Column<decimal>(type: "numeric", nullable: false),
                    USDXAG = table.Column<decimal>(type: "numeric", nullable: false),
                    USDXAU = table.Column<decimal>(type: "numeric", nullable: false),
                    USDXPT = table.Column<decimal>(type: "numeric", nullable: false),
                    Saved_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    API_LastUpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices_Metal", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: "TRY");

            migrationBuilder.UpdateData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: "XAU");

            migrationBuilder.UpdateData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: "XAG");

            migrationBuilder.UpdateData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 8,
                column: "Code",
                value: "XPT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices_Metal");

            migrationBuilder.UpdateData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: "TL");

            migrationBuilder.UpdateData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: "AU");

            migrationBuilder.UpdateData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: "AG");

            migrationBuilder.UpdateData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 8,
                column: "Code",
                value: "PT");
        }
    }
}
