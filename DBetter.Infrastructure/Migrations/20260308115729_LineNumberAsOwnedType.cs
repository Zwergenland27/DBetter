using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LineNumberAsOwnedType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceInformation_LineNumber",
                table: "TrainCirculations",
                newName: "ServiceInformation_LineNumber_Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceInformation_LineNumber_Number",
                table: "TrainCirculations",
                newName: "ServiceInformation_LineNumber");
        }
    }
}
