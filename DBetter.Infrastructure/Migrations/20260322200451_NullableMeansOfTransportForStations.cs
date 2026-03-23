using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NullableMeansOfTransportForStations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_UndergroundTrains",
                table: "Stations",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_Trams",
                table: "Stations",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_SuburbanTrains",
                table: "Stations",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_RegionalTrains",
                table: "Stations",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_HighSpeedTrains",
                table: "Stations",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_FastTrains",
                table: "Stations",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_Busses",
                table: "Stations",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_Boats",
                table: "Stations",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_UndergroundTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_Trams",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_SuburbanTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_RegionalTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_HighSpeedTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_FastTrains",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_Busses",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AvailableMeansOfTransport_Boats",
                table: "Stations",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);
        }
    }
}
