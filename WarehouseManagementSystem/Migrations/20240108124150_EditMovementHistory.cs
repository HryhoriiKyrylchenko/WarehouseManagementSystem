using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditMovementHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "MovementHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "MovementHistories");
        }
    }
}
