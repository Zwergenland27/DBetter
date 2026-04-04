using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TrainRunPassengerInformationKeyTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainRunPassengerInformation",
                table: "TrainRunPassengerInformation");

            migrationBuilder.DropIndex(
                name: "IX_TrainRunPassengerInformation_TrainRunId",
                table: "TrainRunPassengerInformation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainRunPassengerInformation",
                table: "TrainRunPassengerInformation",
                columns: new[] { "TrainRunId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainRunPassengerInformation",
                table: "TrainRunPassengerInformation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainRunPassengerInformation",
                table: "TrainRunPassengerInformation",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrainRunPassengerInformation_TrainRunId",
                table: "TrainRunPassengerInformation",
                column: "TrainRunId");
        }
    }
}
