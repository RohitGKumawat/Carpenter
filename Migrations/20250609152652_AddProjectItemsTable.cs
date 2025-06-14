using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carpenter.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height_inches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width_inches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Length_inches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height_mm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width_mm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Length_mm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorkProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectItems_WorkProjects_WorkProjectId",
                        column: x => x.WorkProjectId,
                        principalTable: "WorkProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectItems_WorkProjectId",
                table: "ProjectItems",
                column: "WorkProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectItems");
        }
    }
}
