using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JdGarageApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageStorageBikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "Bike");

            migrationBuilder.RenameColumn(
                name: "UrlLocalImage",
                table: "Bike",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Bike",
                newName: "UrlLocalImage");

            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
