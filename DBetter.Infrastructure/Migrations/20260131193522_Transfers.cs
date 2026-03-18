using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Transfers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    ConnectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviousSubConnection_FromStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviousSubConnection_DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PreviousSubConnection_ToStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviousSubConnection_ArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PreviousSubConnection_OriginalMeansOfTransport_HighSpeedTrains = table.Column<bool>(type: "boolean", nullable: false),
                    PreviousSubConnection_OriginalMeansOfTransport_FastTrains = table.Column<bool>(type: "boolean", nullable: false),
                    PreviousSubConnection_OriginalMeansOfTransport_RegionalTrains = table.Column<bool>(type: "boolean", nullable: false),
                    PreviousSubConnection_OriginalMeansOfTransport_SuburbanTrains = table.Column<bool>(type: "boolean", nullable: false),
                    PreviousSubConnection_OriginalMeansOfTransport_UndergroundTrai = table.Column<bool>(name: "PreviousSubConnection_OriginalMeansOfTransport_UndergroundTrai~", type: "boolean", nullable: false),
                    PreviousSubConnection_OriginalMeansOfTransport_Trams = table.Column<bool>(type: "boolean", nullable: false),
                    PreviousSubConnection_OriginalMeansOfTransport_Busses = table.Column<bool>(type: "boolean", nullable: false),
                    PreviousSubConnection_OriginalMeansOfTransport_Boats = table.Column<bool>(type: "boolean", nullable: false),
                    FollowingSubConnection_FromStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowingSubConnection_DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FollowingSubConnection_ToStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowingSubConnection_ArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FollowingSubConnection_OriginalMeansOfTransport_HighSpeedTrains = table.Column<bool>(type: "boolean", nullable: false),
                    FollowingSubConnection_OriginalMeansOfTransport_FastTrains = table.Column<bool>(type: "boolean", nullable: false),
                    FollowingSubConnection_OriginalMeansOfTransport_RegionalTrains = table.Column<bool>(type: "boolean", nullable: false),
                    FollowingSubConnection_OriginalMeansOfTransport_SuburbanTrains = table.Column<bool>(type: "boolean", nullable: false),
                    FollowingSubConnection_OriginalMeansOfTransport_UndergroundTra = table.Column<bool>(name: "FollowingSubConnection_OriginalMeansOfTransport_UndergroundTra~", type: "boolean", nullable: false),
                    FollowingSubConnection_OriginalMeansOfTransport_Trams = table.Column<bool>(type: "boolean", nullable: false),
                    FollowingSubConnection_OriginalMeansOfTransport_Busses = table.Column<bool>(type: "boolean", nullable: false),
                    FollowingSubConnection_OriginalMeansOfTransport_Boats = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => new { x.ConnectionId, x.Id });
                    table.ForeignKey(
                        name: "FK_Transfers_Connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "Connections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
