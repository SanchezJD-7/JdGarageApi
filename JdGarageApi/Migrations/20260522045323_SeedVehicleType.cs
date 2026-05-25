using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JdGarageApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedVehicleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Vehicles] SET [VehicleType] = N'Bike' WHERE [VehicleType] = N''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Vehicles] SET [VehicleType] = N'' WHERE [VehicleType] = N'Bike'");
        }
    }
}
