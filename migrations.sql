CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "ConnectionRequests" (
        "Id" uuid NOT NULL,
        "OwnerId" uuid,
        "DepartureTime" timestamp with time zone,
        "ArrivalTime" timestamp with time zone,
        "ComfortClass" integer NOT NULL,
        "PlannedRoute_OriginStationId" uuid NOT NULL,
        "PlannedRoute_MeansOfTransportFirstSection_HighSpeedTrains" boolean NOT NULL,
        "PlannedRoute_MeansOfTransportFirstSection_FastTrains" boolean NOT NULL,
        "PlannedRoute_MeansOfTransportFirstSection_RegionalTrains" boolean NOT NULL,
        "PlannedRoute_MeansOfTransportFirstSection_SuburbanTrains" boolean NOT NULL,
        "PlannedRoute_MeansOfTransportFirstSection_UndergroundTrains" boolean NOT NULL,
        "PlannedRoute_MeansOfTransportFirstSection_Trams" boolean NOT NULL,
        "PlannedRoute_MeansOfTransportFirstSection_Busses" boolean NOT NULL,
        "PlannedRoute_MeansOfTransportFirstSection_Boats" boolean NOT NULL,
        "PlannedRoute_FirstStopover_StationId" uuid,
        "PlannedRoute_FirstStopover_LengthOfStay" integer,
        "PlannedRoute_FirstStopover_MeansOfTransportNextSection_HighSpe~" boolean,
        "PlannedRoute_FirstStopover_MeansOfTransportNextSection_FastTra~" boolean,
        "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Regiona~" boolean,
        "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Suburba~" boolean,
        "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Undergr~" boolean,
        "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Trams" boolean,
        "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Busses" boolean,
        "PlannedRoute_FirstStopover_MeansOfTransportNextSection_Boats" boolean,
        "PlannedRoute_SecondStopover_StationId" uuid,
        "PlannedRoute_SecondStopover_LengthOfStay" integer,
        "PlannedRoute_SecondStopover_MeansOfTransportNextSection_HighSp~" boolean,
        "PlannedRoute_SecondStopover_MeansOfTransportNextSection_FastTr~" boolean,
        "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Region~" boolean,
        "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Suburb~" boolean,
        "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Underg~" boolean,
        "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Trams" boolean,
        "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Busses" boolean,
        "PlannedRoute_SecondStopover_MeansOfTransportNextSection_Boats" boolean,
        "PlannedRoute_DestinationStationId" uuid NOT NULL,
        "PlannedRoute_MaxTransfers" integer NOT NULL,
        "PlannedRoute_MinTransferTime" integer NOT NULL,
        "EarlierReference" text,
        "LaterReference" text,
        CONSTRAINT "PK_ConnectionRequests" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "Connections" (
        "Id" uuid NOT NULL,
        "ConnectionDate" date NOT NULL,
        "ContextId" text NOT NULL,
        CONSTRAINT "PK_Connections" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "OutboxMessages" (
        "Id" uuid NOT NULL,
        "Type" text NOT NULL,
        "Payload" text NOT NULL,
        "OccuredAt" timestamp with time zone NOT NULL,
        "ProcessedAt" timestamp with time zone,
        CONSTRAINT "PK_OutboxMessages" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "PassengerInformation" (
        "Id" uuid NOT NULL,
        "Type" integer NOT NULL,
        "Priority" integer NOT NULL,
        "Text" text NOT NULL,
        "Code" text NOT NULL,
        CONSTRAINT "PK_PassengerInformation" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "Routes" (
        "Id" uuid NOT NULL,
        "TrainRunId" uuid NOT NULL,
        "Source" integer NOT NULL,
        "LastUpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Routes" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "Stations" (
        "Id" uuid NOT NULL,
        "EvaNumber" text NOT NULL,
        "Name" text NOT NULL,
        "Location_Latitude" real,
        "Location_Longitude" real,
        "InfoId" text,
        "Ril100" text,
        "AvailableMeansOfTransport_HighSpeedTrains" boolean NOT NULL,
        "AvailableMeansOfTransport_FastTrains" boolean NOT NULL,
        "AvailableMeansOfTransport_RegionalTrains" boolean NOT NULL,
        "AvailableMeansOfTransport_SuburbanTrains" boolean NOT NULL,
        "AvailableMeansOfTransport_UndergroundTrains" boolean NOT NULL,
        "AvailableMeansOfTransport_Trams" boolean NOT NULL,
        "AvailableMeansOfTransport_Busses" boolean NOT NULL,
        "AvailableMeansOfTransport_Boats" boolean NOT NULL,
        CONSTRAINT "PK_Stations" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "TrainCirculations" (
        "Id" uuid NOT NULL,
        "Identifier" text NOT NULL,
        "Identifier_OriginStationEva" text NOT NULL,
        "Identifier_DepartureTime" time without time zone NOT NULL,
        "Identifier_DestinationStationEva" text NOT NULL,
        "Identifier_DurationMinutes" integer NOT NULL,
        "ServiceInformation_TransportCategory" integer NOT NULL,
        "ServiceInformation_LineNumber_Number" text,
        "ServiceInformation_LineNumber_ProductClass" text,
        "ServiceInformation_ServiceNumber" integer,
        CONSTRAINT "PK_TrainCirculations" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "TrainCompositions" (
        "Id" uuid NOT NULL,
        "TrainRun" uuid NOT NULL,
        "Source" integer NOT NULL,
        "DepartureTime" timestamp with time zone NOT NULL,
        "LastUpdate" timestamp with time zone NOT NULL,
        "CurrentUpdateInterval" integer,
        CONSTRAINT "PK_TrainCompositions" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "TrainRuns" (
        "Id" uuid NOT NULL,
        "TrainCirculationId" uuid NOT NULL,
        "OperatingDay" date NOT NULL,
        "BahnJourneyId" text NOT NULL,
        "Catering_Type" integer NOT NULL,
        "Catering_FromStopIndex" integer NOT NULL,
        "Catering_ToStopIndex" integer NOT NULL,
        "BikeCarriage_Status" integer NOT NULL,
        "BikeCarriage_FromStopIndex" integer NOT NULL,
        "BikeCarriage_ToStopIndex" integer NOT NULL,
        CONSTRAINT "PK_TrainRuns" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL,
        "Firstname" text NOT NULL,
        "Lastname" text NOT NULL,
        "Email" text NOT NULL,
        "Birthday" timestamp with time zone NOT NULL,
        "PasswordHash" text NOT NULL,
        "PasswordSalt" text NOT NULL,
        "_refreshToken_Token" text,
        "_refreshToken_Created" timestamp with time zone,
        "_refreshToken_Expires" timestamp with time zone,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "Vehicles" (
        "Id" uuid NOT NULL,
        "Identifier" text NOT NULL,
        "MaxAllowedCouplings" integer NOT NULL,
        CONSTRAINT "PK_Vehicles" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "ConnectionRequestPassengers" (
        "PassengerId" uuid NOT NULL,
        "ConnectionRequestId" uuid NOT NULL,
        "UserId" uuid,
        "Name" text,
        "Birthday" timestamp with time zone,
        "Age" integer,
        "Bikes" integer NOT NULL,
        "Dogs" integer NOT NULL,
        CONSTRAINT "PK_ConnectionRequestPassengers" PRIMARY KEY ("PassengerId", "ConnectionRequestId"),
        CONSTRAINT "FK_ConnectionRequestPassengers_ConnectionRequests_ConnectionRe~" FOREIGN KEY ("ConnectionRequestId") REFERENCES "ConnectionRequests" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "ConnectionRequestSuggestedConnectionIds" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "ConnectionId" uuid NOT NULL,
        "ConnectionRequestId" uuid NOT NULL,
        CONSTRAINT "PK_ConnectionRequestSuggestedConnectionIds" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ConnectionRequestSuggestedConnectionIds_ConnectionRequests_~" FOREIGN KEY ("ConnectionRequestId") REFERENCES "ConnectionRequests" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "Transfers" (
        "Id" smallint NOT NULL,
        "ConnectionId" uuid NOT NULL,
        "PreviousSubConnection_FromStationId" uuid NOT NULL,
        "PreviousSubConnection_DepartureTime" timestamp with time zone NOT NULL,
        "PreviousSubConnection_ToStationId" uuid NOT NULL,
        "PreviousSubConnection_ArrivalTime" timestamp with time zone NOT NULL,
        "PreviousSubConnection_OriginalMeansOfTransport_HighSpeedTrains" boolean NOT NULL,
        "PreviousSubConnection_OriginalMeansOfTransport_FastTrains" boolean NOT NULL,
        "PreviousSubConnection_OriginalMeansOfTransport_RegionalTrains" boolean NOT NULL,
        "PreviousSubConnection_OriginalMeansOfTransport_SuburbanTrains" boolean NOT NULL,
        "PreviousSubConnection_OriginalMeansOfTransport_UndergroundTrai~" boolean NOT NULL,
        "PreviousSubConnection_OriginalMeansOfTransport_Trams" boolean NOT NULL,
        "PreviousSubConnection_OriginalMeansOfTransport_Busses" boolean NOT NULL,
        "PreviousSubConnection_OriginalMeansOfTransport_Boats" boolean NOT NULL,
        "FollowingSubConnection_FromStationId" uuid NOT NULL,
        "FollowingSubConnection_DepartureTime" timestamp with time zone NOT NULL,
        "FollowingSubConnection_ToStationId" uuid NOT NULL,
        "FollowingSubConnection_ArrivalTime" timestamp with time zone NOT NULL,
        "FollowingSubConnection_OriginalMeansOfTransport_HighSpeedTrains" boolean NOT NULL,
        "FollowingSubConnection_OriginalMeansOfTransport_FastTrains" boolean NOT NULL,
        "FollowingSubConnection_OriginalMeansOfTransport_RegionalTrains" boolean NOT NULL,
        "FollowingSubConnection_OriginalMeansOfTransport_SuburbanTrains" boolean NOT NULL,
        "FollowingSubConnection_OriginalMeansOfTransport_UndergroundTra~" boolean NOT NULL,
        "FollowingSubConnection_OriginalMeansOfTransport_Trams" boolean NOT NULL,
        "FollowingSubConnection_OriginalMeansOfTransport_Busses" boolean NOT NULL,
        "FollowingSubConnection_OriginalMeansOfTransport_Boats" boolean NOT NULL,
        CONSTRAINT "PK_Transfers" PRIMARY KEY ("ConnectionId", "Id"),
        CONSTRAINT "FK_Transfers_Connections_ConnectionId" FOREIGN KEY ("ConnectionId") REFERENCES "Connections" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "RouteStops" (
        "Id" smallint NOT NULL,
        "RouteId" uuid NOT NULL,
        "RouteIndex" integer NOT NULL,
        "StationId" uuid NOT NULL,
        "DepartureTime_Planned" timestamp with time zone,
        "DepartureTime_Real" timestamp with time zone,
        "ArrivalTime_Planned" timestamp with time zone,
        "ArrivalTime_Real" timestamp with time zone,
        "Demand_FirstClass" integer NOT NULL,
        "Demand_SecondClass" integer NOT NULL,
        "Platform_Planned" text,
        "Platform_Real" text,
        "Platform_Type" integer,
        "Attributes_IsAdditional" boolean NOT NULL,
        "Attributes_IsCancelled" boolean NOT NULL,
        "Attributes_IsExitOnly" boolean NOT NULL,
        "Attributes_IsEntryOnly" boolean NOT NULL,
        "Attributes_IsRequestStop" boolean NOT NULL,
        CONSTRAINT "PK_RouteStops" PRIMARY KEY ("Id", "RouteId"),
        CONSTRAINT "FK_RouteStops_Routes_RouteId" FOREIGN KEY ("RouteId") REFERENCES "Routes" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "FormationVehicles" (
        "Id" smallint NOT NULL,
        "TrainCompositionId" uuid NOT NULL,
        "VehicleId" uuid NOT NULL,
        "FromStation" uuid NOT NULL,
        "ToStation" uuid NOT NULL,
        CONSTRAINT "PK_FormationVehicles" PRIMARY KEY ("TrainCompositionId", "Id"),
        CONSTRAINT "FK_FormationVehicles_TrainCompositions_TrainCompositionId" FOREIGN KEY ("TrainCompositionId") REFERENCES "TrainCompositions" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "TrainRunPassengerInformation" (
        "Id" uuid NOT NULL,
        "PassengerInformationId" uuid NOT NULL,
        "FromStopIndex" integer NOT NULL,
        "ToStopIndex" integer NOT NULL,
        "TrainRunId" uuid NOT NULL,
        CONSTRAINT "PK_TrainRunPassengerInformation" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_TrainRunPassengerInformation_TrainRuns_TrainRunId" FOREIGN KEY ("TrainRunId") REFERENCES "TrainRuns" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "Discounts" (
        "Id" uuid NOT NULL,
        "Type" integer NOT NULL,
        "ComfortClass" integer NOT NULL,
        "BoughtAtUtc" timestamp with time zone NOT NULL,
        "ValidUntilUtc" timestamp with time zone,
        "UserId" uuid NOT NULL,
        CONSTRAINT "PK_Discounts" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Discounts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "VehicleCoaches" (
        "Id" smallint NOT NULL,
        "VehicleId" uuid NOT NULL,
        "ConstructionType" text,
        "CoachType" text,
        CONSTRAINT "PK_VehicleCoaches" PRIMARY KEY ("Id", "VehicleId"),
        CONSTRAINT "FK_VehicleCoaches_Vehicles_VehicleId" FOREIGN KEY ("VehicleId") REFERENCES "Vehicles" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE TABLE "ConnectionRequestPassengerDiscounts" (
        "Id" uuid NOT NULL,
        "PassengerId" uuid NOT NULL,
        "ConnectionRequestId" uuid NOT NULL,
        "Type" integer NOT NULL,
        "ComfortClass" integer NOT NULL,
        CONSTRAINT "PK_ConnectionRequestPassengerDiscounts" PRIMARY KEY ("Id", "PassengerId", "ConnectionRequestId"),
        CONSTRAINT "FK_ConnectionRequestPassengerDiscounts_ConnectionRequestPassen~" FOREIGN KEY ("PassengerId", "ConnectionRequestId") REFERENCES "ConnectionRequestPassengers" ("PassengerId", "ConnectionRequestId") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE INDEX "IX_ConnectionRequestPassengerDiscounts_PassengerId_ConnectionR~" ON "ConnectionRequestPassengerDiscounts" ("PassengerId", "ConnectionRequestId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE INDEX "IX_ConnectionRequestPassengers_ConnectionRequestId" ON "ConnectionRequestPassengers" ("ConnectionRequestId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE INDEX "IX_ConnectionRequestSuggestedConnectionIds_ConnectionRequestId" ON "ConnectionRequestSuggestedConnectionIds" ("ConnectionRequestId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE INDEX "IX_Discounts_UserId" ON "Discounts" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE UNIQUE INDEX "IX_PassengerInformation_Text" ON "PassengerInformation" ("Text");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE UNIQUE INDEX "IX_Routes_TrainRunId" ON "Routes" ("TrainRunId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE INDEX "IX_RouteStops_RouteId" ON "RouteStops" ("RouteId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE UNIQUE INDEX "IX_Stations_EvaNumber" ON "Stations" ("EvaNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE UNIQUE INDEX "IX_TrainCirculations_Identifier" ON "TrainCirculations" ("Identifier");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE UNIQUE INDEX "IX_TrainCompositions_TrainRun" ON "TrainCompositions" ("TrainRun");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE UNIQUE INDEX "IX_TrainRunPassengerInformation_Id_TrainRunId" ON "TrainRunPassengerInformation" ("Id", "TrainRunId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE INDEX "IX_TrainRunPassengerInformation_TrainRunId" ON "TrainRunPassengerInformation" ("TrainRunId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE UNIQUE INDEX "IX_TrainRuns_TrainCirculationId_OperatingDay" ON "TrainRuns" ("TrainCirculationId", "OperatingDay");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    CREATE INDEX "IX_VehicleCoaches_VehicleId" ON "VehicleCoaches" ("VehicleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322170023_Initial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260322170023_Initial', '9.0.1');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322200451_NullableMeansOfTransportForStations') THEN
    ALTER TABLE "Stations" ALTER COLUMN "AvailableMeansOfTransport_UndergroundTrains" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322200451_NullableMeansOfTransportForStations') THEN
    ALTER TABLE "Stations" ALTER COLUMN "AvailableMeansOfTransport_Trams" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322200451_NullableMeansOfTransportForStations') THEN
    ALTER TABLE "Stations" ALTER COLUMN "AvailableMeansOfTransport_SuburbanTrains" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322200451_NullableMeansOfTransportForStations') THEN
    ALTER TABLE "Stations" ALTER COLUMN "AvailableMeansOfTransport_RegionalTrains" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322200451_NullableMeansOfTransportForStations') THEN
    ALTER TABLE "Stations" ALTER COLUMN "AvailableMeansOfTransport_HighSpeedTrains" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322200451_NullableMeansOfTransportForStations') THEN
    ALTER TABLE "Stations" ALTER COLUMN "AvailableMeansOfTransport_FastTrains" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322200451_NullableMeansOfTransportForStations') THEN
    ALTER TABLE "Stations" ALTER COLUMN "AvailableMeansOfTransport_Busses" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322200451_NullableMeansOfTransportForStations') THEN
    ALTER TABLE "Stations" ALTER COLUMN "AvailableMeansOfTransport_Boats" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260322200451_NullableMeansOfTransportForStations') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260322200451_NullableMeansOfTransportForStations', '9.0.1');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325165031_TrainCompositionPersistenceDto') THEN
    ALTER TABLE "TrainCompositions" RENAME COLUMN "TrainRun" TO "TrainRunId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325165031_TrainCompositionPersistenceDto') THEN
    ALTER INDEX "IX_TrainCompositions_TrainRun" RENAME TO "IX_TrainCompositions_TrainRunId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325165031_TrainCompositionPersistenceDto') THEN
    ALTER TABLE "FormationVehicles" RENAME COLUMN "ToStation" TO "ToStationId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325165031_TrainCompositionPersistenceDto') THEN
    ALTER TABLE "FormationVehicles" RENAME COLUMN "FromStation" TO "FromStationId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325165031_TrainCompositionPersistenceDto') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260325165031_TrainCompositionPersistenceDto', '9.0.1');
    END IF;
END $EF$;
COMMIT;

