using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapSell.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class AddNewProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainVideoUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainVideoUrl",
                table: "Products");
        }
    }
}
