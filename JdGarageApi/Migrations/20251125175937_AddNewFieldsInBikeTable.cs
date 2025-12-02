using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JdGarageApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsInBikeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Armor",
                table: "Bike",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Engine",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extras",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mileage",
                table: "Bike",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfOwners",
                table: "Bike",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerContact",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlateLastDigit",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Power",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Bike",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Sinister",
                table: "Bike",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SoatDate",
                table: "Bike",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TechnicalInspection",
                table: "Bike",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Torque",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransmissionType",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "Bike",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Armor",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Engine",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Extras",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "NumberOfOwners",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "OwnerContact",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "PlateLastDigit",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Sinister",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "SoatDate",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "TechnicalInspection",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Torque",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "Bike");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Bike");
        }
    }
}
