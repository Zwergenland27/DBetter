using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TrainFormations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainCompositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainRun = table.Column<Guid>(type: "uuid", nullable: false),
                    Source = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainCompositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormationVehicles",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    TrainCompositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStation = table.Column<Guid>(type: "uuid", nullable: false),
                    ToStation = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormationVehicles", x => new { x.TrainCompositionId, x.Id });
                    table.ForeignKey(
                        name: "FK_FormationVehicles_TrainCompositions_TrainCompositionId",
                        column: x => x.TrainCompositionId,
                        principalTable: "TrainCompositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainCompositions_TrainRun",
                table: "TrainCompositions",
                column: "TrainRun",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormationVehicles");

            migrationBuilder.DropTable(
                name: "TrainCompositions");
        }
    }
}
