using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CETHotelProject_CY.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalDays",
                table: "Reservation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPaid",
                table: "Reservation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "TotalPaid",
                table: "Reservation");
        }
    }
}
