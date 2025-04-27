using DBetter.Application.Connections;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BackgroundJobs;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.Routes.Entities;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using JourneyId = DBetter.Infrastructure.BahnDe.Routes.DTOs.JourneyId;
using TravelTime = DBetter.Domain.Routes.ValueObjects.TravelTime;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionsQueryRepository(
    DBetterContext context,
    ConnectionService connectionService) : IConnectionsQueryRepository
{
    public async Task<ConnectionSuggestionsDto> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page)
    {
        var requestStationEvas = await context.Stations
            .AsNoTracking()
            .Where(s => request.GetStops().Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.EvaNumber);

        var bahnConnectionRequest = request.ToRequest(requestStationEvas, page);
        var bahnConnectionRequestEntity = new BahnConnectionRequestEntity(request.Id, bahnConnectionRequest);
        
        var fahrplan = await connectionService.GetSuggestionsAsync(bahnConnectionRequest);

        var journeyIds = fahrplan.GetJourneyIds();
        var stopEvas = fahrplan.GetEvaNumbers(journeyIds);

        var existingRoutes = await context.Routes
            .AsNoTracking()
            .Where(tr => journeyIds.Contains(tr.JourneyId))
            .ToDictionaryAsync(tr => tr.JourneyId, tr => tr);
        
        var existingStations = await context.Stations
            .AsNoTracking()
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionaryAsync(s => s.EvaNumber, s => s);

        var connectionFactory = ConnectionFactory
            .CreateFrom(fahrplan)
            .WithRequestId(request.Id)
            .WithExistingRoutes(existingRoutes)
            .WithExistingStations(existingStations);
        
        RouteScraperJob.AddRoutes(connectionFactory.RoutesToCreate
            .Where(tr => tr.ScrapingRequired)
            .Select(tr => tr.Id)
            .ToList());
        
        await context.BahnConnectionRequests.AddAsync(bahnConnectionRequestEntity);
        await context.Stations.AddRangeAsync(connectionFactory.StationsToCreate);
        await context.Routes.AddRangeAsync(connectionFactory.RoutesToCreate);
        await context.Connections.AddRangeAsync(connectionFactory.ConnectionsToCreate);
        
        return connectionFactory.SuggestionsDto;
    }
    
    public async Task<ConnectionDto?> GetConnectionWithIncreasedTransferTime(
        ConnectionId id,
        StationId fixedStartStationId,
        DateTime fixedStartTime,
        StationId fixedEndStationId,
        DateTime fixedEndTime)
    {
        var originalConnection = await context.Connections
            .FirstOrDefaultAsync(c => c.Id == id);

        if (originalConnection is null) return null;
        
        var bahnRequest = await context.BahnConnectionRequests
            .FirstOrDefaultAsync(r => r.Id == originalConnection.RequestId);

        var stationIds = await context.Stations
            .AsNoTracking()
            .Where(s => s.Id == fixedStartStationId || s.Id == fixedEndStationId)
            .ToDictionaryAsync(s => s.Id, s => s.EvaNumber);
        
        if (bahnRequest is null) return null;

        var request = bahnRequest.Request.ToRequest(
            originalConnection.ContextId,
            stationIds[fixedStartStationId],
            fixedStartTime,
            stationIds[fixedEndStationId],
            fixedEndTime);

        var teilstrecke = await connectionService.GetSuggestionsWithIncreasedTransferTimeAsync(request);

        var journeyIds = teilstrecke.Verbindung.GetJourneyIds();

        var stopEvas = teilstrecke.Verbindung.GetEvaNumbers();

        var existingRoutes = await context.Routes
            .Where(tr => journeyIds.Contains(tr.JourneyId))
            .ToDictionaryAsync(tr => tr.JourneyId, tr => tr);
        
        var existingStations = await context.Stations
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionaryAsync(s => s.EvaNumber, s => s);
        
        var connectionFactory = ConnectionFactory
            .CreateFrom(teilstrecke)
            .WithRequestId(originalConnection.RequestId)
            .WithExistingRoutes(existingRoutes)
            .WithExistingStations(existingStations);
        
        RouteScraperJob.AddRoutes(connectionFactory.RoutesToCreate
            .Where(tr => tr.ScrapingRequired)
            .Select(tr => tr.Id)
            .ToList());
        
        await context.Stations.AddRangeAsync(connectionFactory.StationsToCreate);
        await context.Routes.AddRangeAsync(connectionFactory.RoutesToCreate);
        await context.Connections.AddRangeAsync(connectionFactory.ConnectionsToCreate);
        
        return connectionFactory.ConnectionDto;
    }
}