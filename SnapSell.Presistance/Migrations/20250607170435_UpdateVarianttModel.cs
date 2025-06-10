using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapSell.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVarianttModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegularPrice",
                table: "Variants",
                newName: "CoastPrice");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Variants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Variants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsMainImage",
                table: "ProductImage",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Variants");

            migrationBuilder.RenameColumn(
                name: "CoastPrice",
                table: "Variants",
                newName: "RegularPrice");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Variants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "IsMainImage",
                table: "ProductImage",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
