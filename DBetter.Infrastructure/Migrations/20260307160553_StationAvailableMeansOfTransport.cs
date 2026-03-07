using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StationAvailableMeansOfTransport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AvailableMeansOfTransport_Boats",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AvailableMeansOfTransport_Busses",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AvailableMeansOfTransport_FastTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AvailableMeansOfTransport_HighSpeedTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AvailableMeansOfTransport_RegionalTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AvailableMeansOfTransport_SuburbanTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AvailableMeansOfTransport_Trams",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AvailableMeansOfTransport_UndergroundTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableMeansOfTransport_Boats",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AvailableMeansOfTransport_Busses",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AvailableMeansOfTransport_FastTrains",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AvailableMeansOfTransport_HighSpeedTrains",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AvailableMeansOfTransport_RegionalTrains",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AvailableMeansOfTransport_SuburbanTrains",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AvailableMeansOfTransport_Trams",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AvailableMeansOfTransport_UndergroundTrains",
                table: "Stations");
        }
    }
}
