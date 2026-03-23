using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConnectionRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ComfortClass = table.Column<int>(type: "integer", nullable: false),
                    PlannedRoute_OriginStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlannedRoute_MeansOfTransportFirstSection_HighSpeedTrains = table.Column<bool>(type: "boolean", nullable: false),
                    PlannedRoute_MeansOfTransportFirstSection_FastTrains = table.Column<bool>(type: "boolean", nullable: false),
                    PlannedRoute_MeansOfTransportFirstSection_RegionalTrains = table.Column<bool>(type: "boolean", nullable: false),
                    PlannedRoute_MeansOfTransportFirstSection_SuburbanTrains = table.Column<bool>(type: "boolean", nullable: false),
                    PlannedRoute_MeansOfTransportFirstSection_UndergroundTrains = table.Column<bool>(type: "boolean", nullable: false),
                    PlannedRoute_MeansOfTransportFirstSection_Trams = table.Column<bool>(type: "boolean", nullable: false),
                    PlannedRoute_MeansOfTransportFirstSection_Busses = table.Column<bool>(type: "boolean", nullable: false),
                    PlannedRoute_MeansOfTransportFirstSection_Boats = table.Column<bool>(type: "boolean", nullable: false),
                    PlannedRoute_FirstStopover_StationId = table.Column<Guid>(type: "uuid", nullable: true),
                    PlannedRoute_FirstStopover_LengthOfStay = table.Column<int>(type: "integer", nullable: true),
                    PlannedRoute_FirstStopover_MeansOfTransportNextSection_HighSpe = table.Column<bool>(name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_HighSpe~", type: "boolean", nullable: true),
                    PlannedRoute_FirstStopover_MeansOfTransportNextSection_FastTra = table.Column<bool>(name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_FastTra~", type: "boolean", nullable: true),
                    PlannedRoute_FirstStopover_MeansOfTransportNextSection_Regiona = table.Column<bool>(name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Regiona~", type: "boolean", nullable: true),
                    PlannedRoute_FirstStopover_MeansOfTransportNextSection_Suburba = table.Column<bool>(name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Suburba~", type: "boolean", nullable: true),
                    PlannedRoute_FirstStopover_MeansOfTransportNextSection_Undergr = table.Column<bool>(name: "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Undergr~", type: "boolean", nullable: true),
                    PlannedRoute_FirstStopover_MeansOfTransportNextSection_Trams = table.Column<bool>(type: "boolean", nullable: true),
                    PlannedRoute_FirstStopover_MeansOfTransportNextSection_Busses = table.Column<bool>(type: "boolean", nullable: true),
                    PlannedRoute_FirstStopover_MeansOfTransportNextSection_Boats = table.Column<bool>(type: "boolean", nullable: true),
                    PlannedRoute_SecondStopover_StationId = table.Column<Guid>(type: "uuid", nullable: true),
                    PlannedRoute_SecondStopover_LengthOfStay = table.Column<int>(type: "integer", nullable: true),
                    PlannedRoute_SecondStopover_MeansOfTransportNextSection_HighSp = table.Column<bool>(name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_HighSp~", type: "boolean", nullable: true),
                    PlannedRoute_SecondStopover_MeansOfTransportNextSection_FastTr = table.Column<bool>(name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_FastTr~", type: "boolean", nullable: true),
                    PlannedRoute_SecondStopover_MeansOfTransportNextSection_Region = table.Column<bool>(name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Region~", type: "boolean", nullable: true),
                    PlannedRoute_SecondStopover_MeansOfTransportNextSection_Suburb = table.Column<bool>(name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Suburb~", type: "boolean", nullable: true),
                    PlannedRoute_SecondStopover_MeansOfTransportNextSection_Underg = table.Column<bool>(name: "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Underg~", type: "boolean", nullable: true),
                    PlannedRoute_SecondStopover_MeansOfTransportNextSection_Trams = table.Column<bool>(type: "boolean", nullable: true),
                    PlannedRoute_SecondStopover_MeansOfTransportNextSection_Busses = table.Column<bool>(type: "boolean", nullable: true),
                    PlannedRoute_SecondStopover_MeansOfTransportNextSection_Boats = table.Column<bool>(type: "boolean", nullable: true),
                    PlannedRoute_DestinationStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlannedRoute_MaxTransfers = table.Column<int>(type: "integer", nullable: false),
                    PlannedRoute_MinTransferTime = table.Column<int>(type: "integer", nullable: false),
                    EarlierReference = table.Column<string>(type: "text", nullable: true),
                    LaterReference = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ContextId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    OccuredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PassengerInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerInformation", x => x.Id);
                });

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
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EvaNumber = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Location_Latitude = table.Column<float>(type: "real", nullable: true),
                    Location_Longitude = table.Column<float>(type: "real", nullable: true),
                    InfoId = table.Column<string>(type: "text", nullable: true),
                    Ril100 = table.Column<string>(type: "text", nullable: true),
                    AvailableMeansOfTransport_HighSpeedTrains = table.Column<bool>(type: "boolean", nullable: false),
                    AvailableMeansOfTransport_FastTrains = table.Column<bool>(type: "boolean", nullable: false),
                    AvailableMeansOfTransport_RegionalTrains = table.Column<bool>(type: "boolean", nullable: false),
                    AvailableMeansOfTransport_SuburbanTrains = table.Column<bool>(type: "boolean", nullable: false),
                    AvailableMeansOfTransport_UndergroundTrains = table.Column<bool>(type: "boolean", nullable: false),
                    AvailableMeansOfTransport_Trams = table.Column<bool>(type: "boolean", nullable: false),
                    AvailableMeansOfTransport_Busses = table.Column<bool>(type: "boolean", nullable: false),
                    AvailableMeansOfTransport_Boats = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainCirculations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    Identifier_OriginStationEva = table.Column<string>(type: "text", nullable: false),
                    Identifier_DepartureTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Identifier_DestinationStationEva = table.Column<string>(type: "text", nullable: false),
                    Identifier_DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    ServiceInformation_TransportCategory = table.Column<int>(type: "integer", nullable: false),
                    ServiceInformation_LineNumber_Number = table.Column<string>(type: "text", nullable: true),
                    ServiceInformation_LineNumber_ProductClass = table.Column<string>(type: "text", nullable: true),
                    ServiceInformation_ServiceNumber = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainCirculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainCompositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainRun = table.Column<Guid>(type: "uuid", nullable: false),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrentUpdateInterval = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainCompositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainCirculationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperatingDay = table.Column<DateOnly>(type: "date", nullable: false),
                    BahnJourneyId = table.Column<string>(type: "text", nullable: false),
                    Catering_Type = table.Column<int>(type: "integer", nullable: false),
                    Catering_FromStopIndex = table.Column<int>(type: "integer", nullable: false),
                    Catering_ToStopIndex = table.Column<int>(type: "integer", nullable: false),
                    BikeCarriage_Status = table.Column<int>(type: "integer", nullable: false),
                    BikeCarriage_FromStopIndex = table.Column<int>(type: "integer", nullable: false),
                    BikeCarriage_ToStopIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Firstname = table.Column<string>(type: "text", nullable: false),
                    Lastname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false),
                    _refreshToken_Token = table.Column<string>(type: "text", nullable: true),
                    _refreshToken_Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    _refreshToken_Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                name: "ConnectionRequestPassengers",
                columns: table => new
                {
                    PassengerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectionRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    Bikes = table.Column<int>(type: "integer", nullable: false),
                    Dogs = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequestPassengers", x => new { x.PassengerId, x.ConnectionRequestId });
                    table.ForeignKey(
                        name: "FK_ConnectionRequestPassengers_ConnectionRequests_ConnectionRe~",
                        column: x => x.ConnectionRequestId,
                        principalTable: "ConnectionRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionRequestSuggestedConnectionIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConnectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectionRequestId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequestSuggestedConnectionIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionRequestSuggestedConnectionIds_ConnectionRequests_~",
                        column: x => x.ConnectionRequestId,
                        principalTable: "ConnectionRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    Demand_FirstClass = table.Column<int>(type: "integer", nullable: false),
                    Demand_SecondClass = table.Column<int>(type: "integer", nullable: false),
                    Platform_Planned = table.Column<string>(type: "text", nullable: true),
                    Platform_Real = table.Column<string>(type: "text", nullable: true),
                    Platform_Type = table.Column<int>(type: "integer", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "TrainRunPassengerInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassengerInformationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStopIndex = table.Column<int>(type: "integer", nullable: false),
                    ToStopIndex = table.Column<int>(type: "integer", nullable: false),
                    TrainRunId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainRunPassengerInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainRunPassengerInformation_TrainRuns_TrainRunId",
                        column: x => x.TrainRunId,
                        principalTable: "TrainRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ComfortClass = table.Column<int>(type: "integer", nullable: false),
                    BoughtAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidUntilUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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

            migrationBuilder.CreateTable(
                name: "ConnectionRequestPassengerDiscounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectionRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ComfortClass = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequestPassengerDiscounts", x => new { x.Id, x.PassengerId, x.ConnectionRequestId });
                    table.ForeignKey(
                        name: "FK_ConnectionRequestPassengerDiscounts_ConnectionRequestPassen~",
                        columns: x => new { x.PassengerId, x.ConnectionRequestId },
                        principalTable: "ConnectionRequestPassengers",
                        principalColumns: new[] { "PassengerId", "ConnectionRequestId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequestPassengerDiscounts_PassengerId_ConnectionR~",
                table: "ConnectionRequestPassengerDiscounts",
                columns: new[] { "PassengerId", "ConnectionRequestId" });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequestPassengers_ConnectionRequestId",
                table: "ConnectionRequestPassengers",
                column: "ConnectionRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequestSuggestedConnectionIds_ConnectionRequestId",
                table: "ConnectionRequestSuggestedConnectionIds",
                column: "ConnectionRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_UserId",
                table: "Discounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerInformation_Text",
                table: "PassengerInformation",
                column: "Text",
                unique: true);

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
                name: "IX_Stations_EvaNumber",
                table: "Stations",
                column: "EvaNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainCirculations_Identifier",
                table: "TrainCirculations",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainCompositions_TrainRun",
                table: "TrainCompositions",
                column: "TrainRun",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainRunPassengerInformation_Id_TrainRunId",
                table: "TrainRunPassengerInformation",
                columns: new[] { "Id", "TrainRunId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainRunPassengerInformation_TrainRunId",
                table: "TrainRunPassengerInformation",
                column: "TrainRunId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainRuns_TrainCirculationId_OperatingDay",
                table: "TrainRuns",
                columns: new[] { "TrainCirculationId", "OperatingDay" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleCoaches_VehicleId",
                table: "VehicleCoaches",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionRequestPassengerDiscounts");

            migrationBuilder.DropTable(
                name: "ConnectionRequestSuggestedConnectionIds");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "FormationVehicles");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "PassengerInformation");

            migrationBuilder.DropTable(
                name: "RouteStops");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "TrainCirculations");

            migrationBuilder.DropTable(
                name: "TrainRunPassengerInformation");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "VehicleCoaches");

            migrationBuilder.DropTable(
                name: "ConnectionRequestPassengers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TrainCompositions");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "TrainRuns");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "ConnectionRequests");
        }
    }
}
