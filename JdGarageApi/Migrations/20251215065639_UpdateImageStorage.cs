using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JdGarageApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "UrlLocalImage",
                table: "Brands",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Brands",
                newName: "UrlLocalImage");

            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
