using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapSell.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class AuditableToFaviritCleint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddedDate",
                table: "ClientCategoryFavorites",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "AddedDate",
                table: "ClientBrandFavorites",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ClientCategoryFavorites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ClientCategoryFavorites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "ClientCategoryFavorites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "ClientCategoryFavorites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ClientBrandFavorites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ClientBrandFavorites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "ClientBrandFavorites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "ClientBrandFavorites",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ClientCategoryFavorites");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ClientCategoryFavorites");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "ClientCategoryFavorites");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "ClientCategoryFavorites");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ClientBrandFavorites");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ClientBrandFavorites");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "ClientBrandFavorites");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "ClientBrandFavorites");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ClientCategoryFavorites",
                newName: "AddedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ClientBrandFavorites",
                newName: "AddedDate");
        }
    }
}
