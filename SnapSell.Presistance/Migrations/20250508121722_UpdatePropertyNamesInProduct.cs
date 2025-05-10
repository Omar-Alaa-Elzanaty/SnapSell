using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapSell.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropertyNamesInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "Products",
                newName: "EnglishDescription");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "ArabicDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnglishDescription",
                table: "Products",
                newName: "ShortDescription");

            migrationBuilder.RenameColumn(
                name: "ArabicDescription",
                table: "Products",
                newName: "Description");
        }
    }
}
