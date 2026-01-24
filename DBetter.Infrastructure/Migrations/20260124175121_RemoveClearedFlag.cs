using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClearedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteMessages");

            migrationBuilder.CreateTable(
                name: "RoutePassengerInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    InformationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStopIndex = table.Column<int>(type: "integer", nullable: false),
                    ToStopIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePassengerInformation", x => new { x.RouteId, x.Id });
                    table.ForeignKey(
                        name: "FK_RoutePassengerInformation_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutePassengerInformation");

            migrationBuilder.CreateTable(
                name: "RouteMessages",
                columns: table => new
                {
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStopIndex = table.Column<int>(type: "integer", nullable: false),
                    InformationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCleared = table.Column<bool>(type: "boolean", nullable: false),
                    ToStopIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteMessages", x => new { x.RouteId, x.Id });
                    table.ForeignKey(
                        name: "FK_RouteMessages_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
