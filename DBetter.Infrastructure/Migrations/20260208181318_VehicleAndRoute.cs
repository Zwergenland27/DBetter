using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VehicleAndRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_StationId",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_StationId");

            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_MeansOfTransportNextSection_UndergroundTr~",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Underg~");

            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_MeansOfTransportNextSection_Trams",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Trams");

            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_MeansOfTransportNextSection_SuburbanTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Suburb~");

            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_MeansOfTransportNextSection_RegionalTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Region~");

            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_MeansOfTransportNextSection_HighSpeedTrai~",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_HighSp~");

            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_MeansOfTransportNextSection_FastTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_FastTr~");

            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_MeansOfTransportNextSection_Busses",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Busses");

            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_MeansOfTransportNextSection_Boats",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Boats");

            migrationBuilder.RenameColumn(
                name: "Route_SecondStopover_LengthOfStay",
                table: "ConnectionRequests",
                newName: "PlannedRoute_SecondStopover_LengthOfStay");

            migrationBuilder.RenameColumn(
                name: "Route_OriginStationId",
                table: "ConnectionRequests",
                newName: "PlannedRoute_OriginStationId");

            migrationBuilder.RenameColumn(
                name: "Route_MinTransferTime",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MinTransferTime");

            migrationBuilder.RenameColumn(
                name: "Route_MeansOfTransportFirstSection_UndergroundTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MeansOfTransportFirstSection_UndergroundTrains");

            migrationBuilder.RenameColumn(
                name: "Route_MeansOfTransportFirstSection_Trams",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MeansOfTransportFirstSection_Trams");

            migrationBuilder.RenameColumn(
                name: "Route_MeansOfTransportFirstSection_SuburbanTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MeansOfTransportFirstSection_SuburbanTrains");

            migrationBuilder.RenameColumn(
                name: "Route_MeansOfTransportFirstSection_RegionalTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MeansOfTransportFirstSection_RegionalTrains");

            migrationBuilder.RenameColumn(
                name: "Route_MeansOfTransportFirstSection_HighSpeedTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MeansOfTransportFirstSection_HighSpeedTrains");

            migrationBuilder.RenameColumn(
                name: "Route_MeansOfTransportFirstSection_FastTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MeansOfTransportFirstSection_FastTrains");

            migrationBuilder.RenameColumn(
                name: "Route_MeansOfTransportFirstSection_Busses",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MeansOfTransportFirstSection_Busses");

            migrationBuilder.RenameColumn(
                name: "Route_MeansOfTransportFirstSection_Boats",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MeansOfTransportFirstSection_Boats");

            migrationBuilder.RenameColumn(
                name: "Route_MaxTransfers",
                table: "ConnectionRequests",
                newName: "PlannedRoute_MaxTransfers");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_StationId",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_StationId");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_MeansOfTransportNextSection_UndergroundTra~",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Undergr~");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_MeansOfTransportNextSection_Trams",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Trams");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_MeansOfTransportNextSection_SuburbanTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Suburba~");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_MeansOfTransportNextSection_RegionalTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Regiona~");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_MeansOfTransportNextSection_HighSpeedTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_HighSpe~");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_MeansOfTransportNextSection_FastTrains",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_FastTra~");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_MeansOfTransportNextSection_Busses",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Busses");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_MeansOfTransportNextSection_Boats",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Boats");

            migrationBuilder.RenameColumn(
                name: "Route_FirstStopover_LengthOfStay",
                table: "ConnectionRequests",
                newName: "PlannedRoute_FirstStopover_LengthOfStay");

            migrationBuilder.RenameColumn(
                name: "Route_DestinationStationId",
                table: "ConnectionRequests",
                newName: "PlannedRoute_DestinationStationId");

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    MaxAllowedCouplings = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteStops",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteIndex = table.Column<int>(type: "integer", nullable: false),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartureTime_Planned = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartureTime_Real = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArrivalTime_Planned = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArrivalTime_Real = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Attributes_IsAdditional = table.Column<bool>(type: "boolean", nullable: false),
                    Attributes_IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    Attributes_IsExitOnly = table.Column<bool>(type: "boolean", nullable: false),
                    Attributes_IsEntryOnly = table.Column<bool>(type: "boolean", nullable: false),
                    Attributes_IsRequestStop = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStops", x => new { x.Id, x.RouteId });
                    table.ForeignKey(
                        name: "FK_RouteStops_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleCoaches",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConstructionType = table.Column<string>(type: "text", nullable: true),
                    CoachType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleCoaches", x => new { x.Id, x.VehicleId });
                    table.ForeignKey(
                        name: "FK_VehicleCoaches_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TrainRunId",
                table: "Routes",
                column: "TrainRunId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_RouteId",
                table: "RouteStops",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleCoaches_VehicleId",
                table: "VehicleCoaches",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteStops");

            migrationBuilder.DropTable(
                name: "VehicleCoaches");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_StationId",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_StationId");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Underg~",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_MeansOfTransportNextSection_UndergroundTr~");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Trams",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_MeansOfTransportNextSection_Trams");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Suburb~",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_MeansOfTransportNextSection_SuburbanTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Region~",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_MeansOfTransportNextSection_RegionalTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_HighSp~",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_MeansOfTransportNextSection_HighSpeedTrai~");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_FastTr~",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_MeansOfTransportNextSection_FastTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Busses",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_MeansOfTransportNextSection_Busses");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Boats",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_MeansOfTransportNextSection_Boats");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_SecondStopover_LengthOfStay",
                table: "ConnectionRequests",
                newName: "Route_SecondStopover_LengthOfStay");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_OriginStationId",
                table: "ConnectionRequests",
                newName: "Route_OriginStationId");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MinTransferTime",
                table: "ConnectionRequests",
                newName: "Route_MinTransferTime");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MeansOfTransportFirstSection_UndergroundTrains",
                table: "ConnectionRequests",
                newName: "Route_MeansOfTransportFirstSection_UndergroundTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MeansOfTransportFirstSection_Trams",
                table: "ConnectionRequests",
                newName: "Route_MeansOfTransportFirstSection_Trams");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MeansOfTransportFirstSection_SuburbanTrains",
                table: "ConnectionRequests",
                newName: "Route_MeansOfTransportFirstSection_SuburbanTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MeansOfTransportFirstSection_RegionalTrains",
                table: "ConnectionRequests",
                newName: "Route_MeansOfTransportFirstSection_RegionalTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MeansOfTransportFirstSection_HighSpeedTrains",
                table: "ConnectionRequests",
                newName: "Route_MeansOfTransportFirstSection_HighSpeedTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MeansOfTransportFirstSection_FastTrains",
                table: "ConnectionRequests",
                newName: "Route_MeansOfTransportFirstSection_FastTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MeansOfTransportFirstSection_Busses",
                table: "ConnectionRequests",
                newName: "Route_MeansOfTransportFirstSection_Busses");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MeansOfTransportFirstSection_Boats",
                table: "ConnectionRequests",
                newName: "Route_MeansOfTransportFirstSection_Boats");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_MaxTransfers",
                table: "ConnectionRequests",
                newName: "Route_MaxTransfers");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_StationId",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_StationId");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Undergr~",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_MeansOfTransportNextSection_UndergroundTra~");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Trams",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_MeansOfTransportNextSection_Trams");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Suburba~",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_MeansOfTransportNextSection_SuburbanTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Regiona~",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_MeansOfTransportNextSection_RegionalTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_HighSpe~",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_MeansOfTransportNextSection_HighSpeedTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_FastTra~",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_MeansOfTransportNextSection_FastTrains");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Busses",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_MeansOfTransportNextSection_Busses");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Boats",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_MeansOfTransportNextSection_Boats");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_FirstStopover_LengthOfStay",
                table: "ConnectionRequests",
                newName: "Route_FirstStopover_LengthOfStay");

            migrationBuilder.RenameColumn(
                name: "PlannedRoute_DestinationStationId",
                table: "ConnectionRequests",
                newName: "Route_DestinationStationId");
        }
    }
}
