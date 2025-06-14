using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carpenter.Migrations
{
    /// <inheritdoc />
    public partial class nextone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Height_inches",
                table: "AllProductss",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Height_inches",
                table: "AllProductss",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
