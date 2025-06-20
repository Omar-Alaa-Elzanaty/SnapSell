using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapSell.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStoreEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Accounts_SellerId",
                table: "Stores");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Accounts_SellerId",
                table: "Stores",
                column: "SellerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Accounts_SellerId",
                table: "Stores");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Accounts_SellerId",
                table: "Stores",
                column: "SellerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
