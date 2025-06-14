using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carpenter.Migrations
{
    /// <inheritdoc />
    public partial class initialall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllProductss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height_inches = table.Column<int>(type: "int", nullable: false),
                    Width_inches = table.Column<int>(type: "int", nullable: false),
                    Length_inches = table.Column<int>(type: "int", nullable: false),
                    Height_mm = table.Column<int>(type: "int", nullable: false),
                    Width_mm = table.Column<int>(type: "int", nullable: false),
                    Length_mm = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllProductss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Earth"),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "/Media/Images/UserProfile/defaultuser.png")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllProductss");

            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
