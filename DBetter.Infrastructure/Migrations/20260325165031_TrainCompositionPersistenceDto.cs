using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TrainCompositionPersistenceDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainRun",
                table: "TrainCompositions",
                newName: "TrainRunId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainCompositions_TrainRun",
                table: "TrainCompositions",
                newName: "IX_TrainCompositions_TrainRunId");

            migrationBuilder.RenameColumn(
                name: "ToStation",
                table: "FormationVehicles",
                newName: "ToStationId");

            migrationBuilder.RenameColumn(
                name: "FromStation",
                table: "FormationVehicles",
                newName: "FromStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainRunId",
                table: "TrainCompositions",
                newName: "TrainRun");

            migrationBuilder.RenameIndex(
                name: "IX_TrainCompositions_TrainRunId",
                table: "TrainCompositions",
                newName: "IX_TrainCompositions_TrainRun");

            migrationBuilder.RenameColumn(
                name: "ToStationId",
                table: "FormationVehicles",
                newName: "ToStation");

            migrationBuilder.RenameColumn(
                name: "FromStationId",
                table: "FormationVehicles",
                newName: "FromStation");
        }
    }
}
