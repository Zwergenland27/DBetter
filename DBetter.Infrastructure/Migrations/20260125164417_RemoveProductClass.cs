using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceInformation_ProductClass",
                table: "TrainCirculations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceInformation_ProductClass",
                table: "TrainCirculations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
