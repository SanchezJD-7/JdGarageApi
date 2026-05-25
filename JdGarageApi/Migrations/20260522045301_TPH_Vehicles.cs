using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JdGarageApi.Migrations
{
    /// <inheritdoc />
    public partial class TPH_Vehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bike_BikeCategory_BikeCategoryId",
                table: "Bike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bike",
                table: "Bike");

            migrationBuilder.RenameTable(
                name: "Bike",
                newName: "Vehicles");

            migrationBuilder.RenameIndex(
                name: "IX_Bike_BikeCategoryId",
                table: "Vehicles",
                newName: "IX_Vehicles_BikeCategoryId");

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "Vehicles",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_BikeCategory_BikeCategoryId",
                table: "Vehicles",
                column: "BikeCategoryId",
                principalTable: "BikeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_BikeCategory_BikeCategoryId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Vehicles");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "Bike");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_BikeCategoryId",
                table: "Bike",
                newName: "IX_Bike_BikeCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bike",
                table: "Bike",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bike_BikeCategory_BikeCategoryId",
                table: "Bike",
                column: "BikeCategoryId",
                principalTable: "BikeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
