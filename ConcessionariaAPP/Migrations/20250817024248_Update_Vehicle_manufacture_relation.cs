using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcessionariaAPP.Migrations
{
    /// <inheritdoc />
    public partial class Update_Vehicle_manufacture_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManufacturersVehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ManufacturerId",
                table: "Vehicles",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Manufacturers_ManufacturerId",
                table: "Vehicles",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "ManufacturerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Manufacturers_ManufacturerId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ManufacturerId",
                table: "Vehicles");

            migrationBuilder.CreateTable(
                name: "ManufacturersVehicles",
                columns: table => new
                {
                    ManufacturersManufacturerId = table.Column<int>(type: "int", nullable: false),
                    VehiclesVehicleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturersVehicles", x => new { x.ManufacturersManufacturerId, x.VehiclesVehicleId });
                    table.ForeignKey(
                        name: "FK_ManufacturersVehicles_Manufacturers_ManufacturersManufacturerId",
                        column: x => x.ManufacturersManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "ManufacturerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManufacturersVehicles_Vehicles_VehiclesVehicleId",
                        column: x => x.VehiclesVehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturersVehicles_VehiclesVehicleId",
                table: "ManufacturersVehicles",
                column: "VehiclesVehicleId");
        }
    }
}
