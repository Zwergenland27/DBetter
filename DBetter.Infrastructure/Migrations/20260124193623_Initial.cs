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
                    Route_OriginStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Route_MeansOfTransportFirstSection_HighSpeedTrains = table.Column<bool>(type: "boolean", nullable: false),
                    Route_MeansOfTransportFirstSection_FastTrains = table.Column<bool>(type: "boolean", nullable: false),
                    Route_MeansOfTransportFirstSection_RegionalTrains = table.Column<bool>(type: "boolean", nullable: false),
                    Route_MeansOfTransportFirstSection_SuburbanTrains = table.Column<bool>(type: "boolean", nullable: false),
                    Route_MeansOfTransportFirstSection_UndergroundTrains = table.Column<bool>(type: "boolean", nullable: false),
                    Route_MeansOfTransportFirstSection_Trams = table.Column<bool>(type: "boolean", nullable: false),
                    Route_MeansOfTransportFirstSection_Busses = table.Column<bool>(type: "boolean", nullable: false),
                    Route_MeansOfTransportFirstSection_Boats = table.Column<bool>(type: "boolean", nullable: false),
                    Route_FirstStopover_StationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Route_FirstStopover_LengthOfStay = table.Column<int>(type: "integer", nullable: true),
                    Route_FirstStopover_MeansOfTransportNextSection_HighSpeedTrains = table.Column<bool>(type: "boolean", nullable: true),
                    Route_FirstStopover_MeansOfTransportNextSection_FastTrains = table.Column<bool>(type: "boolean", nullable: true),
                    Route_FirstStopover_MeansOfTransportNextSection_RegionalTrains = table.Column<bool>(type: "boolean", nullable: true),
                    Route_FirstStopover_MeansOfTransportNextSection_SuburbanTrains = table.Column<bool>(type: "boolean", nullable: true),
                    Route_FirstStopover_MeansOfTransportNextSection_UndergroundTra = table.Column<bool>(name: "Route_FirstStopover_MeansOfTransportNextSection_UndergroundTra~", type: "boolean", nullable: true),
                    Route_FirstStopover_MeansOfTransportNextSection_Trams = table.Column<bool>(type: "boolean", nullable: true),
                    Route_FirstStopover_MeansOfTransportNextSection_Busses = table.Column<bool>(type: "boolean", nullable: true),
                    Route_FirstStopover_MeansOfTransportNextSection_Boats = table.Column<bool>(type: "boolean", nullable: true),
                    Route_SecondStopover_StationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Route_SecondStopover_LengthOfStay = table.Column<int>(type: "integer", nullable: true),
                    Route_SecondStopover_MeansOfTransportNextSection_HighSpeedTrai = table.Column<bool>(name: "Route_SecondStopover_MeansOfTransportNextSection_HighSpeedTrai~", type: "boolean", nullable: true),
                    Route_SecondStopover_MeansOfTransportNextSection_FastTrains = table.Column<bool>(type: "boolean", nullable: true),
                    Route_SecondStopover_MeansOfTransportNextSection_RegionalTrains = table.Column<bool>(type: "boolean", nullable: true),
                    Route_SecondStopover_MeansOfTransportNextSection_SuburbanTrains = table.Column<bool>(type: "boolean", nullable: true),
                    Route_SecondStopover_MeansOfTransportNextSection_UndergroundTr = table.Column<bool>(name: "Route_SecondStopover_MeansOfTransportNextSection_UndergroundTr~", type: "boolean", nullable: true),
                    Route_SecondStopover_MeansOfTransportNextSection_Trams = table.Column<bool>(type: "boolean", nullable: true),
                    Route_SecondStopover_MeansOfTransportNextSection_Busses = table.Column<bool>(type: "boolean", nullable: true),
                    Route_SecondStopover_MeansOfTransportNextSection_Boats = table.Column<bool>(type: "boolean", nullable: true),
                    Route_DestinationStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Route_MaxTransfers = table.Column<int>(type: "integer", nullable: false),
                    Route_MinTransferTime = table.Column<int>(type: "integer", nullable: false),
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
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EvaNumber = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Location_Latitude = table.Column<float>(type: "real", nullable: true),
                    Location_Longitude = table.Column<float>(type: "real", nullable: true),
                    InfoId = table.Column<string>(type: "text", nullable: true),
                    Ril100 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JourneyId = table.Column<string>(type: "text", nullable: false),
                    Catering_Type = table.Column<int>(type: "integer", nullable: false),
                    Catering_FromStopIndex = table.Column<int>(type: "integer", nullable: true),
                    Catering_ToStopIndex = table.Column<int>(type: "integer", nullable: true),
                    BikeCarriage_Status = table.Column<int>(type: "integer", nullable: false),
                    BikeCarriage_FromStopIndex = table.Column<int>(type: "integer", nullable: true),
                    BikeCarriage_ToStopIndex = table.Column<int>(type: "integer", nullable: true),
                    ServiceInformation_TransportCategory = table.Column<int>(type: "integer", nullable: false),
                    ServiceInformation_ProductClass = table.Column<string>(type: "text", nullable: false),
                    ServiceInformation_LineNumber = table.Column<string>(type: "text", nullable: true),
                    ServiceInformation_ServiceNumber = table.Column<int>(type: "integer", nullable: true)
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
                name: "TrainRunPassengerInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    InformationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStopIndex = table.Column<int>(type: "integer", nullable: false),
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
                name: "IX_Stations_EvaNumber",
                table: "Stations",
                column: "EvaNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainRuns_JourneyId",
                table: "TrainRuns",
                column: "JourneyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionRequestPassengerDiscounts");

            migrationBuilder.DropTable(
                name: "ConnectionRequestSuggestedConnectionIds");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "PassengerInformation");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "TrainRunPassengerInformation");

            migrationBuilder.DropTable(
                name: "ConnectionRequestPassengers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TrainRuns");

            migrationBuilder.DropTable(
                name: "ConnectionRequests");
        }
    }
}
