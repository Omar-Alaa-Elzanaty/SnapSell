using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapSell.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoUrlToBrands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Brands");
        }
    }
}
