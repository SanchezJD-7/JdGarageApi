using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JdGarageApi.Migrations
{
    /// <inheritdoc />
    public partial class UploadBikeImageSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlLocalImage",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlLocalImage",
                table: "Bike");
        }
    }
}
