using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTrainRunPassengerInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainRunPassengerInformation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainRunPassengerInformation",
                columns: table => new
                {
                    TrainRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStopIndex = table.Column<int>(type: "integer", nullable: false),
                    PassengerInformationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToStopIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainRunPassengerInformation", x => new { x.TrainRunId, x.Id });
                    table.ForeignKey(
                        name: "FK_TrainRunPassengerInformation_TrainRuns_TrainRunId",
                        column: x => x.TrainRunId,
                        principalTable: "TrainRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainRunPassengerInformation_Id_TrainRunId",
                table: "TrainRunPassengerInformation",
                columns: new[] { "Id", "TrainRunId" },
                unique: true);
        }
    }
}
