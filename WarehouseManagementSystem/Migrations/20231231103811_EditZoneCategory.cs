using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditZoneCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZoneCategories_ProductCategories_PreviousCategoryId",
                table: "ZoneCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_ZoneCategories_ZoneCategories_PreviousCategoryId",
                table: "ZoneCategories",
                column: "PreviousCategoryId",
                principalTable: "ZoneCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZoneCategories_ZoneCategories_PreviousCategoryId",
                table: "ZoneCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_ZoneCategories_ProductCategories_PreviousCategoryId",
                table: "ZoneCategories",
                column: "PreviousCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }
    }
}
