using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalRouteAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Demand_FirstClass",
                table: "RouteStops",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Demand_SecondClass",
                table: "RouteStops",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Platform_Planned",
                table: "RouteStops",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platform_Real",
                table: "RouteStops",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Platform_Type",
                table: "RouteStops",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Demand_FirstClass",
                table: "RouteStops");

            migrationBuilder.DropColumn(
                name: "Demand_SecondClass",
                table: "RouteStops");

            migrationBuilder.DropColumn(
                name: "Platform_Planned",
                table: "RouteStops");

            migrationBuilder.DropColumn(
                name: "Platform_Real",
                table: "RouteStops");

            migrationBuilder.DropColumn(
                name: "Platform_Type",
                table: "RouteStops");
        }
    }
}
