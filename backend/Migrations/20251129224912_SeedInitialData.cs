using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] {"Id", "Name"},
                values: new object[,]
                {
                    {1, "Currency" },
                    {2, "Metals" },
                    {3, "Crypto" }
                });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "Id", "Name", "Category_id" },
                values: new object[,]
                {
                    {1, "TL", 1 },
                    {2, "EUR", 1 },
                    {3, "USD", 1 },
                    {4, "Altın", 2 },
                    {5, "Gümüş", 2 },
                    {6, "Platin", 2 },
                    {7, "Bitcoin", 3 },
                    {8, "Etherium", 3 },
                    {9, "Ripple", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Category verilerini drop et
            migrationBuilder.DeleteData("Category", "Id" ,1);
            migrationBuilder.DeleteData("Category", "Id", 2);
            migrationBuilder.DeleteData("Category", "Id", 3);

            // Subcategory verilerini drop et
            migrationBuilder.DeleteData("Subcategory", "Id", 1);
            migrationBuilder.DeleteData("Subcategory", "Id", 2);
            migrationBuilder.DeleteData("Subcategory", "Id", 3);
            migrationBuilder.DeleteData("Subcategory", "Id", 4);
            migrationBuilder.DeleteData("Subcategory", "Id", 5);
            migrationBuilder.DeleteData("Subcategory", "Id", 6);
            migrationBuilder.DeleteData("Subcategory", "Id", 7);
            migrationBuilder.DeleteData("Subcategory", "Id", 8);
            migrationBuilder.DeleteData("Subcategory", "Id", 9);
        }
    }
}
